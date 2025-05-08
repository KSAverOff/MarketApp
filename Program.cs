using MarketApp.Data;
using MarketApp.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Добавляем строку подключения
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.Run();