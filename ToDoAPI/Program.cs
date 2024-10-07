using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ToDoAPI.Models;
using ToDoAPI.Models.ApplicationDbContext;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add Db to Application

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(configuration.GetConnectionString("DefaultConnection"))
);

// Add Identity to implement users module

builder.Services.AddIdentity<Users, UserRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add services to the container.

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
