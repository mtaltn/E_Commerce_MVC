using Microsoft.AspNetCore.Mvc;
using TheRaven.MVC.Models;
using TheRaven.MVC.Services.Abstract;
using TheRaven.Shared.Dto;

namespace TheRaven.MVC.Controllers;

public class FilterProductController : Controller
{
    private readonly IFilterProductService _filterProductService;

    public FilterProductController(IFilterProductService filterProductService)
    {
        _filterProductService = filterProductService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("QueryProducts")]
    public async Task<IActionResult> Search([FromQuery] ProductElasticDto productElasticDTO)
    {
        SearchViewPageModel _pageModel = new SearchViewPageModel();
        var eCommerceList = await _filterProductService.SearchAsync(productElasticDTO);
        _pageModel._listProductModel = eCommerceList;
        return View(_pageModel);
    }
}
