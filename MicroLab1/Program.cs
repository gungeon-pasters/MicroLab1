using Microsoft.EntityFrameworkCore;
using MicroLab1.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddOpenApi();
builder.Services.AddDbContext<TrainingContext>(opt =>
    opt.UseInMemoryDatabase("Training"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MicroLab1",
        Version = "v1"
    });
});



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TrainingContext>();

    context.Training.AddRange(
        new Training
        {
            Id = 1,
            Title = "Утренняя пробежка",
            Type = "Кардио",
            DurationMinutes = 30,
            TrainingDate = DateTime.Today,
            Approaches = 1,
            Weights = 0
        },
        new Training
        {
            Id = 2,
            Title = "Силовая тренировка",
            Type = "Силовая",
            DurationMinutes = 60,
            TrainingDate = DateTime.Today.AddDays(1),
            Approaches = 4,
            Weights = 40
        },
        new Training
        {
            Id = 3,
            Title = "Растяжка",
            Type = "Гибкость",
            DurationMinutes = 20,
            TrainingDate = DateTime.Today.AddDays(2),
            Approaches = 2,
            Weights = 0
        }
    );

    context.SaveChanges();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MicroLab1 v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
