using MarketApp.Data;
using MarketApp.DTOs.Product;
using MarketApp.Models;
using MarketApp.Utils;
using Microsoft.EntityFrameworkCore;

namespace MarketApp.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (AppDbContext db, ProductCreateDto dto) =>
        {
            var category = await db.Categories.FindAsync(dto.CategoryId);
            if (category == null) return Results.BadRequest("Category not found");

            var product = new Product
            {
                Name = dto.Name,
                Quantity = dto.Quantity,
                Units = dto.Units,
                CategoryId = dto.CategoryId
            };

            db.Products.Add(product);
            await db.SaveChangesAsync();

            return Results.Created($"/products/{product.Id}", new { product.Id });
        });
        
        app.MapPut("/products/{id:int}", async (int id, AppDbContext db, ProductUpdateDto dto) =>
        {
            var product = await db.Products.FindAsync(id);
            if (product == null) return Results.NotFound();

            var category = await db.Categories.FindAsync(dto.CategoryId);
            if (category == null) return Results.BadRequest("Category not found");

            product.Name = dto.Name;
            product.Quantity = dto.Quantity;
            product.Units = dto.Units;
            product.CategoryId = dto.CategoryId;

            await db.SaveChangesAsync();
            return Results.Ok(new { product.Id });
        });

        
        app.MapGet("/products", async (AppDbContext db) =>
        {
            var products = await db.Products.Include(p => p.Category).ToListAsync();
            var result = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Quantity = p.Quantity,
                Units = p.Units.GetDescription(),
                CategoryName = p.Category?.Name ?? "Без категории"
            });

            return Results.Ok(result);
        });

        app.MapGet("/products/{id:int}", async (int id, AppDbContext db) =>
        {
            var product = await db.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return Results.NotFound();

            var result = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Quantity = product.Quantity,
                Units = product.Units.GetDescription(),
                CategoryName = product.Category?.Name ?? "Без категории"
            };

            return Results.Ok(result);
        });

        app.MapDelete("/products/{id:int}", async (int id, AppDbContext db) =>
        {
            var product = await db.Products.FindAsync(id);
            if (product == null) return Results.NotFound();

            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}
