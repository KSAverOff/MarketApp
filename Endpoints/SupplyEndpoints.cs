using MarketApp.Data;
using MarketApp.DTOs.Supply;
using MarketApp.Models;
using Microsoft.EntityFrameworkCore;
using MarketApp.Utils;

namespace MarketApp.Endpoints;

public static class SupplyEndpoints
{
    public static void MapSupplyEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/supplies", async (AppDbContext db, SupplyCreateDto dto) =>
        {
            foreach (var item in dto.Items)
            {
                var product = await db.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                if (product == null)
                    return Results.BadRequest($"Product with ID {item.ProductId} not found.");
            }

            var supply = new Supply
            {
                Date = DateTime.UtcNow,
                Items = new List<SupplyItem>()
            };

            foreach (var item in dto.Items)
            {
                var supplyItem = new SupplyItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                supply.Items.Add(supplyItem);

                var product = await db.Products.FirstAsync(p => p.Id == item.ProductId);
                product.Quantity += item.Quantity;
            }

            db.Supplies.Add(supply);
            await db.SaveChangesAsync();

            return Results.Created($"/supplies/{supply.Id}", new { supply.Id });
        });

        app.MapGet("/supplies", async (AppDbContext db) =>
        {
            var supplies = await db.Supplies
                .Include(s => s.Items)
                .ThenInclude(si => si.Product)
                .ToListAsync();

            var result = supplies.Select(s => new
            {
                s.Id,
                Date = s.Date.ToLocalTime(),
                Items = s.Items.Select(si => new
                {
                    Product = si.Product.Name,
                    si.Quantity,
                    Units = si.Product.Units.GetDescription()
                })
            });

            return Results.Ok(result);
        });

        app.MapGet("/supplies/{id:int}", async (int id, AppDbContext db) =>
        {
            var supply = await db.Supplies
                .Include(s => s.Items)
                .ThenInclude(si => si.Product)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (supply == null) return Results.NotFound();

            var result = new
            {
                supply.Id,
                Date = supply.Date.ToLocalTime(),
                Items = supply.Items.Select(si => new
                {
                    Product = si.Product.Name,
                    si.Quantity,
                    Units = si.Product.Units.GetDescription()
                })
            };

            return Results.Ok(result);
        });
    }
}
