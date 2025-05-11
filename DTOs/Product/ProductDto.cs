using MarketApp.Models;
using MarketApp.Utils;

namespace MarketApp.DTOs.Product;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public float Quantity { get; set; }
    public string Units { get; set; } = null!;
    public string CategoryName { get; set; } = null!;
}