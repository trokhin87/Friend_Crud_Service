using System.Reflection;
using Bussines.Services;
using Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebLevel.Middleware.Validators;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using WebLevel.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Настройка логирования
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();


string dbProxy = String.Empty;
if (builder.Environment.IsDevelopment())
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(5015);  
    });
    // для обычного запуска
    var configuration = builder.Configuration;
    dbProxy = configuration["ProxyMicroservice:BaseUrl"] ?? throw new Exception("DbProxy is missing");
}
else
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(8080);  
    });
    //докерок
    dbProxy = Environment.GetEnvironmentVariable("BaseUrl") ?? throw new Exception("DbProxy is missing"); 
    if (!Uri.IsWellFormedUriString(dbProxy, UriKind.Absolute))
    {
       throw new Exception($"DbProxy is not valid: {dbProxy}");
    }  
}

builder.Services.AddHttpClient("ProxyApiClient", client =>
{
    client.BaseAddress = new Uri(dbProxy); 
});

builder.Services.AddScoped<IFriendService>(provider =>
{
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient("ProxyApiClient"); 

    return new FriendService(httpClient);
});



// // Регистрация HttpClient (фикс ошибки)
// builder.Services.AddHttpClient(); 
// Если сервис использует HttpClient напрямую в конструкторе




// Добавление сервисов
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Регистрация бизнес-логики
builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Friend Microservice API",
        Version = "v1",
        Description = "API для управления друзьями",
    });
    options.EnableAnnotations();
    options.ExampleFilters();
});
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
