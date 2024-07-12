using System.Text.Json.Serialization;

namespace TheRaven.Shared.Dto;

public class ProductElasticDto
{
    [JsonPropertyName("_id")]
    public string Id { get; set; } = null!;
    [JsonPropertyName("productid")]

    public int ProductId { get; set; }
    [JsonPropertyName("product_name")]

    public string ProductName { get; set; } = null!;
    [JsonPropertyName("product_description")]

    public string ProductDescription { get; set; } = null!;
    [JsonPropertyName("price")]

    public double Price { get; set; }
    [JsonPropertyName("categoryid")]

    public int CategoryId { get; set; }
    [JsonPropertyName("_image_url")]

    public string? ImageUrl { get; set; } = null!;

    public double? LtePrice { get; set; }
}
