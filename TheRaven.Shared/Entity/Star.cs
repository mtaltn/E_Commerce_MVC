using System.ComponentModel.DataAnnotations;

namespace TheRaven.Shared.Entity;

public class Star
{
    [Key]
    public int Id { get; set; }
    public int? ProductId { get; set; }
    public string? UserId { get; set; }
    public double? RateStar { get; set; }
}
