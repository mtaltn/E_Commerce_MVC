using TheRaven.Shared.Dto;
using TheRaven.Shared.Entity;
using TheRaven.Shared.Response;
namespace TheRaven.MVC.Services.Abstract;

public interface IProductService
{

    Task<ServiceResponse<ProductDto>> CreateAsync(ProductDto model);
    Task<ServiceResponse<bool>> DeleteAsync(int productId);
    Task<ServiceResponse<Product>> GetAsync(int productId);
    Task<ServiceResponse<ProductDto>> UpdateAsync(int productId, ProductDto product);
    Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(int categoryId);
    Task<ServiceResponse<List<Product>>> ListAsync();
    Task<ServiceResponse<ProductSearchResponse>> GetProductsAsync(int page);
}
