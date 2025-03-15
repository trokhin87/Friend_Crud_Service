using Bussines.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

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

// Настройка HttpClient
builder.Services.AddHttpClient<Service>(client =>
{
    var baseUrl = builder.Configuration["ProxyMicroservice:BaseUrl"];
    client.BaseAddress = new Uri(baseUrl);
});

var app = builder.Build();

// Настройка middleware
app.UseSwagger();
app.UseSwaggerUI();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthorization();

// Маршруты
app.MapOpenApi();
app.MapControllers();

app.Run();