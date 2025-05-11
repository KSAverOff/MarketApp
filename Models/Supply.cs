namespace MarketApp.Models;

public class Supply
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public List<SupplyItem> Items { get; set; } = new();
}