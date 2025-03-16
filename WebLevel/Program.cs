using Bussines.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebLevel.Middleware.Validators;
using Serilog;
using WebLevel.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Настройка логирования
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

// Добавление сервисов
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

// Регистрация HttpClient (фикс ошибки)
builder.Services.AddHttpClient(); 
// Если сервис использует HttpClient напрямую в конструкторе
builder.Services.AddHttpClient<Service>(); 

// Регистрация бизнес-логики
builder.Services.AddScoped<Service>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthorization();

// Подключаем middleware
app.UseMiddleware<ExceptionHandlingMiddleware>(); 
app.UseMiddleware<ValidationMiddleware>();

// Подключаем маршруты контроллеров
app.MapControllers();

app.Run();