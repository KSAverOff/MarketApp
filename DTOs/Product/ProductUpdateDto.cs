using MarketApp.Models;

namespace MarketApp.DTOs.Product;

public class ProductUpdateDto
{
    public string Name { get; set; } = null!;
    public float Quantity { get; set; }
    public UnitType Units { get; set; }
    public int CategoryId { get; set; }
}