namespace TheRaven.Shared.Dto;

public class ProductDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public int Quantity { get; set; } = 0;
    public decimal Price { get; set; }
    public decimal DealerPrice { get; set; }
    public int CategoryId { get; set; }
    public string? ImageUrl { get; set; }
}
