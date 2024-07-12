namespace TheRaven.MVC.Models;

public class ProductBasketModel
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public string? OurCode { get; set; }
    public string? ManufacturerNumber { get; set; }
    public string? Manufacturer { get; set; }
    public decimal Price { get; set; }
    public decimal DealerPrice { get; set; }
    public string CategoryName { get; set; }
    public string? ImageUrl { get; set; }
}
