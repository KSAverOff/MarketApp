using MarketApp.Data;
using MarketApp.DTOs.Sale;
using MarketApp.Models;
using Microsoft.EntityFrameworkCore;
using MarketApp.Utils;

namespace MarketApp.Endpoints;

public static class SaleEndpoints
{
    public static void MapSaleProducts(this IEndpointRouteBuilder app)
    {
        app.MapPost("/sales", async (AppDbContext db, SaleCreateDto dto) =>
        {
            foreach (var item in dto.Items)
            {
                var product = await db.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                if (product == null) return Results.BadRequest($"Product with ID {item.ProductId} not found.");
                if (product.Quantity < item.Quantity) return Results.BadRequest($"Not enough stock for '{product.Name}'.");
            }

            var sale = new Sale
            {
                Date = DateTime.UtcNow,
                Items = new List<SaleItem>()
            };

            foreach (var item in dto.Items)
            {
                var saleItem = new SaleItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                sale.Items.Add(saleItem);

                var product = await db.Products.FirstAsync(p => p.Id == item.ProductId);
                product.Quantity -= item.Quantity;
            }

            db.Sales.Add(sale);
            await db.SaveChangesAsync();

            return Results.Created($"/sales/{sale.Id}", new { sale.Id });
        });

        app.MapGet("/sales", async (AppDbContext db) =>
        {
            var sales = await db.Sales
                .Include(s => s.Items)
                .ThenInclude(si => si.Product)
                .ToListAsync();

            var result = sales.Select(s => new
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

        app.MapGet("/sales/{id:int}", async (int id, AppDbContext db) =>
        {
            var sale = await db.Sales
                .Include(s => s.Items)
                .ThenInclude(si => si.Product)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sale == null) return Results.NotFound();

            var result = new
            {
                sale.Id,
                Date = sale.Date.ToLocalTime(),
                Items = sale.Items.Select(si => new
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
