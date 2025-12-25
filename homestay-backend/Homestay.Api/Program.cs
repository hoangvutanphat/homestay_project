using Homestay.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Homestay.Api.Infrastructure.Repositories;
using Homestay.Api.Application.Services;
using Homestay.Api.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HomestayDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    .LogTo(Console.WriteLine, LogLevel.Information)
);
builder.Services.AddControllers();
builder.Services.AddScoped<HomestayRepository>();
builder.Services.AddScoped<IHomestayService, HomestayService>();
var app = builder.Build();

app.MapControllers();

app.Run();

