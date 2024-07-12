using Microsoft.EntityFrameworkCore;
using TheRaven.MVC.Areas.Identity.Data;
using TheRaven.MVC.Services.Abstract;
using TheRaven.Shared.Dto;
using TheRaven.Shared.Entity;
using TheRaven.Shared.Response;

namespace TheRaven.MVC.Services.Concrete;

public class CategoryService : ICategoryService
{
    private readonly TheRavenMVCContext _context;

    public CategoryService(TheRavenMVCContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<CategoryDto>> CreateAsync(CategoryDto request)
    {
        Category category = new Category()
        {
            Name = request.Name,
        };
        var result = _context.Categories.Add(category);

        if (await _context.SaveChangesAsync() > 0)
        {
            return new ServiceResponse<CategoryDto>
            {
                Message = "Operation success",
                Success = true,
            };
        }
        else
        {
            return new ServiceResponse<CategoryDto>
            {
                Message = "Operation is failed",
                Success = false,
            };
        }
    }

    public async Task<ServiceResponse<bool>> DeleteAsync(int categoryId)
    {
        var result = await _context.Categories.FindAsync(categoryId);
        if (result is null)
        {
            return new ServiceResponse<bool>
            {
                Message = "Category is not found",
                Success = false,
            };
        }
        else
        {
            _context.Categories.Remove(result);
            if (await _context.SaveChangesAsync() > 0)
            {
                return new ServiceResponse<bool>
                {
                    Success = true,
                    Message = "Success",
                };
            }
            else
            {
                return new ServiceResponse<bool>
                {
                    Message = "While data is save to db,create event fail",
                };
            }
        }
    }

    public List<Category> GetCategories()
    {
        var result = _context.Categories.ToList();
        return result ?? null;
    }

    public async Task<ServiceResponse<CategoryDto>> GetAsync(int categoryId)
    {
        var result = await _context.Categories.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == categoryId);
        CategoryDto modelDto = new()
        {
            Name = result.Name,
            Id = result.Id,
        };
        if (result is null)
        {
            return new ServiceResponse<CategoryDto>
            {
                Success = true,
                Message = "This category has not product",
            };
        }
        else
        {
            return new ServiceResponse<CategoryDto> { Success = true, Data = modelDto };
        }
    }

    public async Task<Category> GetCategoryByNameAsync(string categoryName)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Name == categoryName);
        return category ?? null;
    }

    public async Task<ServiceResponse<List<Category>>> ListAsync()
    {
        ServiceResponse<List<Category>> serviceResponse = new ServiceResponse<List<Category>>();
        var result = await _context.Categories.ToListAsync();
        if (result.Count is 0)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = "Categories are not found";
            return serviceResponse;
        }
        else
        {
            serviceResponse.Success = true;
            serviceResponse.Message = "Categories listed";
            serviceResponse.Data = result;
            return serviceResponse;
        }
    }

    public async Task<ServiceResponse<CategoryDto>> UpdateAsync(int categoryId, CategoryDto categoryDto)
    {
        var result = await _context.Categories.FindAsync(categoryId);
        if (result is null)
        {
            return new ServiceResponse<CategoryDto>
            {
                Message = "Ooops! category is not found",
                Success = false,
            };
        }
        else
        {
            result.Name = categoryDto.Name;
            await _context.SaveChangesAsync();
            return new ServiceResponse<CategoryDto>
            {
                Message = "your process is successfull",
                Success = true,
            };
        }
    }
}
