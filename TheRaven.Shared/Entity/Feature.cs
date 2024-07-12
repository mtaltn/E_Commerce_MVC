using System.ComponentModel.DataAnnotations;

namespace TheRaven.Shared.Entity;

public class Feature
{
    [Key]
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public string? OurCode { get; set; }
    public string? ManufacturerNumber { get; set; }
    public string? Manufacturer { get; set; }
}
