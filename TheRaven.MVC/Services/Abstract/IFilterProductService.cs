using TheRaven.Shared.Dto;

namespace TheRaven.MVC.Services.Abstract;

public interface IFilterProductService
{
    Task<List<ProductElasticDto>> SearchAsync(ProductElasticDto productViewModel);
}
