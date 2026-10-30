using MicroLab1.Models;
using MicroLab1.Services;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Конфигурация MongoDB
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDB"));

// Регистрируем сервис как Singleton (MongoClient переиспользуется)
builder.Services.AddSingleton<MongoDbService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHttpMetrics();

app.UseAuthorization();

app.MapControllers();
app.MapMetrics();

app.Run();