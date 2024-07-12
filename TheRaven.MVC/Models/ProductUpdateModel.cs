using System.ComponentModel.DataAnnotations;

namespace TheRaven.MVC.Models;

public class ProductUpdateModel
{
    public int ProductId { get; set; }
    public string? Name { get; set; }
    [MaxLength(300)]
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; } = 0;
    public decimal DealerPrice { get; set; }
    public string? ImageUrl { get; set; }
    public IFormFile? CoverPhoto { get; set; }
    public int CategoryId { get; set; }
    public FeatureModel? Feature { get; set; } = new FeatureModel();
}
