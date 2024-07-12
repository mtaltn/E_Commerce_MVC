using TheRaven.Shared.Dto;
using TheRaven.Shared.Entity;
using TheRaven.Shared.Response;

namespace TheRaven.MVC.Services.Abstract;

public interface ICategoryService
{
    Task<ServiceResponse<CategoryDto>> CreateAsync(CategoryDto request);
    Task<ServiceResponse<bool>> DeleteAsync(int categoryId);
    Task<ServiceResponse<CategoryDto>> GetAsync(int categoryId);
    Task<ServiceResponse<CategoryDto>> UpdateAsync(int categoryId, CategoryDto categoryDto);
    Task<ServiceResponse<List<Category>>> ListAsync();
    List<Category> GetCategories();
    Task<Category> GetCategoryByNameAsync(string categoryName);
}