using Microsoft.EntityFrameworkCore;
using CocinaManager.Infrastructure.Data;
using CocinaManager.Application.Interfaces;
using CocinaManager.Infrastructure.Repositories;
using CocinaManager.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Conexión a base de datos
builder.Services.AddDbContext<CocinaDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Repositorio
builder.Services.AddScoped<IPersonalRepository, PersonalRepository>();

// Add services to the container.
builder.Services.AddScoped<IPersonalService, PersonalService>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
