using Microsoft.EntityFrameworkCore;
using Scheduly.WebApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//add db context
builder.Services.AddDbContext<SchedulyContext>(options => options.UseSqlServer("name:ConnectionStrings;DefaultConnection"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
