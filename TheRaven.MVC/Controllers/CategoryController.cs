using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheRaven.MVC.Services.Abstract;
using TheRaven.Shared.Dto;

namespace TheRaven.MVC.Controllers;
[Authorize(Roles = "Admin")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _categoryService.ListAsync();        
        return View(result.Data);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryDto model)
    {
        var result = await _categoryService.CreateAsync(model);
        if (result.Success is true)
        {
            return RedirectToAction("Index", "Category");
        }
        return View();
    }

    [HttpGet("CategoryUpdate/{categoryId}")]
    public async Task<IActionResult> Update([FromRoute] int categoryId)
    {
        var category = await _categoryService.GetAsync(categoryId);
        if (category is null)
        {
            return View();
        }
        return View(category.Data);
    }

    [HttpPost("CategoryUpdate/{categoryId}")]
    public async Task<IActionResult> Update([FromRoute] int categoryId, CategoryDto model)
    {
        var result = await _categoryService.UpdateAsync(categoryId, model);
        if (result.Success is true)
        {
            return RedirectToAction("Index", "Category");
        }
        return View();
    }

    [HttpPost("{categoryId}")]
    public async Task<IActionResult> Delete([FromRoute] int categoryId)
    {
        var result = await _categoryService.DeleteAsync(categoryId);
        if (result.Success == true)
        {
            return RedirectToAction("Index", "Category");
        }
        return BadRequest("Hata oluştu");
    }
}
