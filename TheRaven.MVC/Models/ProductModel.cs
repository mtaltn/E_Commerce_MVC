using System.ComponentModel.DataAnnotations;

namespace TheRaven.MVC.Models;

public class ProductModel
{
    public string? Name { get; set; }
    [MaxLength(250)]
    public string Description { get; set; }
    public int Quantity { get; set; } = 0;
    public decimal Price { get; set; }
    public decimal DealerPrice { get; set; }
    public string? ImageUrl { get; set; }
    public IFormFile? CoverPhoto { get; set; }
    public int CategoryId { get; set; }
    public FeatureModel? Feature { get; set; }
}
