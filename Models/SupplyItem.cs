namespace MarketApp.Models;

public class SupplyItem
{
    public int Id { get; set; }

    public int SupplyId { get; set; }
    public Supply Supply { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public float Quantity { get; set; }
}