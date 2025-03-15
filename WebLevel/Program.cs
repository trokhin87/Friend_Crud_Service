using Bussines.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<Service>(client =>
{
    var baseUrl = builder.Configuration["ProxyMicroservice:BaseUrl"];
    client.BaseAddress = new Uri(baseUrl); // Устанавливаем базовый URL для клиента
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapOpenApi();
app.MapControllers();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();