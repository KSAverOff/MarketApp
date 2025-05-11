using MarketApp.Data;
using MarketApp.Endpoints;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// Добавляем строку подключения
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Подключаем эндпоинты (будут позже)
app.MapProductEndpoints();
app.MapCategoryEndpoints();
app.MapSaleProducts();
app.MapSupplyEndpoints();

app.Run("http://localhost:5000");