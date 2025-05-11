using MarketApp.Data;
using MarketApp.DTOs.Category;
using MarketApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketApp.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/categories", async (AppDbContext db, CategoryCreateDto dto) =>
        {
            var category = new Category
            {
                Name = dto.Name
            };

            db.Categories.Add(category);
            await db.SaveChangesAsync();

            return Results.Created($"/categories/{category.Id}", new { category.Id });
        });

        app.MapPut("/categories/{id:int}", async (int id, AppDbContext db, CategoryUpdateDto dto) =>
        {
            var category = await db.Categories.FindAsync(id);
            if (category == null) return Results.NotFound();

            category.Name = dto.Name;

            await db.SaveChangesAsync();
            return Results.Ok(new { category.Id });
        });

        
        app.MapGet("/categories", async (AppDbContext db) =>
        {
            var categories = await db.Categories.ToListAsync();
            var result = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            });

            return Results.Ok(result);
        });

        app.MapGet("/categories/{id:int}", async (AppDbContext db, int id) =>
        {
            var category = await db.Categories.FindAsync(id);
            if (category == null) return Results.NotFound();

            var result = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };

            return Results.Ok(result);
        });

        app.MapDelete("/categories/{id:int}", async (AppDbContext db, int id) =>
        {
            var category = await db.Categories.FindAsync(id);
            if (category == null) return Results.NotFound();

            db.Categories.Remove(category);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}