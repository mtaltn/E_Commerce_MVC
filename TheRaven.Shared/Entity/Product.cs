using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheRaven.Shared.Entity;

public class Product
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    public int Quantity { get; set; } = 0;
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }[Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal DealerPrice { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } 
    public string? ImageUrl { get; set; }
    public double? RateAvg { get; set; }

    public Feature? Features { get; set; } = new Feature();
    public List<Order> Orders { get; set; } = new List<Order>();
}
