using E_Commerce_MVC.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TheRaven.MVC.Areas.Identity.Data;
using TheRaven.MVC.Models;
using TheRaven.MVC.Services.Abstract;

namespace TheRaven.MVC.Controllers;

public class BasketController : Controller
{
    private readonly IProductService _productService;
    private readonly TheRavenMVCContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public BasketController(IProductService productService, TheRavenMVCContext context, UserManager<ApplicationUser> userManager)
    {
        _productService = productService;
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var basket = HttpContext.Session.GetString(Key.Basket_Items);
        if (string.IsNullOrEmpty(basket))
        {
            return View(new List<ProductBasketModel>());
        }
        else
        {
            var basket_items = JsonConvert.DeserializeObject<List<ProductBasketModel>>(basket);
            return View(basket_items);
        }
    }

    [HttpPost("AddProductToBasket/{productId}")]
    public async Task<IActionResult> AddBasketItem([FromRoute] int productId)
    {
        List<ProductBasketModel> _products = new List<ProductBasketModel>();
        ProductBasketModel _model = new ProductBasketModel();
        var userEmail = HttpContext.User.Identity.Name;
        var product = await _productService.GetAsync(productId);
        var category = await _context.Categories.FindAsync(product.Data.CategoryId);
        _model.Manufacturer = product.Data.Features.Manufacturer;
        _model.ManufacturerNumber = product.Data.Features.ManufacturerNumber;
        _model.OurCode = product.Data.Features.OurCode;
        _model.ProductName = product.Data.Name;
        _model.Price = product.Data.Price;
        _model.ImageUrl = product.Data.ImageUrl;
        _model.CategoryName = category.Name;
        _model.Id = product.Data.Id;
        _model.DealerPrice = product.Data.DealerPrice;
        var basket = HttpContext.Session.GetString(Key.Basket_Items);
        if (basket != null)
        {
            var basket_items = JsonConvert.DeserializeObject<List<ProductBasketModel>>(basket);
            foreach (var item in basket_items)
            {
                _products.Add(item);
            }
            _products.Add(_model);
        }
        else
        {
            _products.Add(_model);
        }

        var user = await _userManager.FindByNameAsync(userEmail);
        string serializeProduct = JsonConvert.SerializeObject(_products);
        string serializeUser = JsonConvert.SerializeObject(user);
        HttpContext.Session.SetString(Key.Basket_Items, serializeProduct);
        HttpContext.Session.SetString(Key.Users, serializeUser);
        return RedirectToAction("Index", "ProductClient");
    }

    [HttpPost("DeleteItem/{productId}")]
    public async Task<IActionResult> DeleteCartItem([FromRoute] int productId)
    {
        var basket = HttpContext.Session.GetString(Key.Basket_Items);
        var basket_items = JsonConvert.DeserializeObject<List<ProductBasketModel>>(basket);
        foreach (var item in basket_items)
        {
            if (item.Id == productId)
            {
                basket_items.Remove(item);
                string serializeProduct = JsonConvert.SerializeObject(basket_items);
                HttpContext.Session.SetString(Key.Basket_Items, serializeProduct);
                return RedirectToAction("Index", "Basket");
            }
        }
        return RedirectToAction("Index", "Basket");
    }
}
