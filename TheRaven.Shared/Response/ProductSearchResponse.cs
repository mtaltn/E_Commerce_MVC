using TheRaven.Shared.Entity;

namespace TheRaven.Shared.Response;

public class ProductSearchResponse
{
    public List<Product> Products { get; set; } = new List<Product>();
    public int Pages { get; set; }
    public int CurrentPage { get; set; }
}
