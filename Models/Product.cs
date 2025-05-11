using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketApp.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public float Quantity { get; set; }
    public UnitType Units { get; set; } = UnitType.Piece;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}

public enum UnitType
{
    [Description("шт")]
    Piece,
    
    [Description("кг")]
    Kilogram
}