using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheRaven.MVC.Areas.Identity.Data;
using TheRaven.MVC.Services.Abstract;
using TheRaven.MVC.Services.Concrete;
using TheRaven.Shared.Dto;

namespace TheRaven.MVC.Controllers;

public class ProductClientController : Controller
{
    private readonly IProductService _productService;
    private readonly IStarService _starService;
    private readonly TheRavenMVCContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    //private readonly IFavouriteService _favouriteService;

    public ProductClientController(IProductService productService, TheRavenMVCContext context, UserManager<ApplicationUser> userManager, IStarService starService)
    {
        _productService = productService;
        _context = context;
        _userManager = userManager;
        _starService = starService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _productService.ListAsync();
        if (result != null)
        {
            return View(result.Data);
        }
        return View("Error");
    }

    [HttpGet("productById/{productId}")]
    public async Task<IActionResult> GetProductById([FromRoute] int productId)
    {
        var result = await _productService.GetAsync(productId);
        if (result.Data != null)
        {
            return View(result.Data);
        }
        return View("Error");
    }

    [HttpGet("GetProducts/{page}")]
    public async Task<IActionResult> GetAll([FromRoute] int page)
    {
        var result = await _productService.GetProductsAsync(page);
        if (result.Data != null)
        {
            return View(result.Data);
        }
        return View("Error");
    }

    [HttpGet("GetProductsByCategory/{categoryId}")]
    public async Task<IActionResult> GetProductByCategory([FromRoute] int categoryId)
    {
        var result = await _productService.GetProductsByCategoryAsync(categoryId);
        if (result != null)
        {
            return View(result.Data);
        }
        return View("Error");
    }

    [HttpPost("SendRate/{productId}")]
    public async Task<IActionResult> GiveStar(string star, [FromRoute] int productId)
    {
        var userEmail = HttpContext.User.Identity.Name;
        var user = await _userManager.FindByEmailAsync(userEmail);
        StarDto _star = new StarDto();
        double starValue = 0;
        _star.RateStar = double.Parse(star);
        _star.ProductId = productId;
        _star.UserId = user.Id;
        var IsSuccess = await _starService.GiveStarProduct(_star);
        return View(IsSuccess.Message);
    }
}
