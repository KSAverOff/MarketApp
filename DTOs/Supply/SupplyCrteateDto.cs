namespace MarketApp.DTOs.Supply;

public class SupplyCreateDto
{
    public List<SupplyItemCreateDto> Items { get; set; } = new();
}