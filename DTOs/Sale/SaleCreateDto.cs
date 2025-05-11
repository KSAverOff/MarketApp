namespace MarketApp.DTOs.Sale;

public class SaleCreateDto
{
    public List<SaleItemCreateDto> Items { get; set; } = new();
}