using TheRaven.Shared.Dto;

namespace TheRaven.MVC.Models;

public class SearchViewPageModel
{
    public List<ProductElasticDto> _listProductModel { get; set; }
    public ProductElasticDto productElasticDTO { get; set; }
}
