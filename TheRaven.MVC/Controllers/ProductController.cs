using Iyzipay.Model.V2.Subscription;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using TheRaven.MVC.Areas.Identity.Data;
using TheRaven.MVC.Models;
using TheRaven.MVC.Services.Abstract;
using TheRaven.Shared.Dto;
using TheRaven.Shared.Entity;
using static NuGet.Packaging.PackagingConstants;

namespace TheRaven.MVC.Controllers;
[Authorize(Roles = "Admin")]
public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly TheRavenMVCContext _context;
    private readonly IFeatureService _featureService;


    public ProductController(IProductService productService, ICategoryService categoryService, IWebHostEnvironment webHostEnvironment, TheRavenMVCContext context, IFeatureService featureService)
    {
        _productService = productService;
        _categoryService = categoryService;
        _webHostEnvironment = webHostEnvironment;
        _context = context;
        _featureService = featureService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _productService.ListAsync();
        if (result.Data is not null)
        {
            return View(result.Data);
        }
        return RedirectToAction("Create", "Product");
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ProductModel model = new ProductModel();
        LoadCategorySelectDataView();
        ViewData["Title"] = "Create Product";
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                ProductDto product = new ProductDto();
                product.Description = model.Description;
                product.Name = model.Name;
                product.Price = model.Price;
                product.DealerPrice = model.DealerPrice;
                product.CategoryId = model.CategoryId;
                product.Quantity = model.Quantity;

                string folder = "Images\\";

                if (model.CoverPhoto != null)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.CoverPhoto.FileName;
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder, uniqueFileName);

                    using (var stream = new FileStream(serverFolder, FileMode.Create))
                    {
                        await model.CoverPhoto.CopyToAsync(stream);
                    }

                    product.ImageUrl = uniqueFileName;
                }
                else
                {
                    string defaultImageFileName = "nopic.png";
                    string defaultImagePath = Path.Combine(_webHostEnvironment.WebRootPath, folder, defaultImageFileName);

                    product.ImageUrl = defaultImageFileName;
                }

                var result = await _productService.CreateAsync(product);


                if (result.Success == true)
                {

                    FeatureDto featureDto = new FeatureDto();
                    featureDto.Manufacturer = model.Feature.Manufacturer;
                    featureDto.ManufacturerNumber = model.Feature.ManufacturerNumber;
                    featureDto.OurCode = model.Feature.OurCode;
                    featureDto.ProductId = int.Parse(result.Message);

                    var resultFeature = await _featureService.CreateFeatureForProduct(featureDto, featureDto.ProductId);
                    if (resultFeature.Success == true)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    ModelState.AddModelError("", "Product creation failed.");
                }
                else
                {
                    ModelState.AddModelError("", "Product creation failed.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred during product creation.");
            }
        }
        return View();
    }

    [HttpGet("Update/{productId}")]
    public async Task<IActionResult> Update([FromRoute] int productId)
    {

        var product = await _productService.GetAsync(productId);
        ProductUpdateModel _model = new ProductUpdateModel();
        _model.ProductId = productId;
        _model.Name = product.Data.Name;
        _model.Description = product.Data.Description;
        _model.Price = product.Data.Price;
        _model.DealerPrice = product.Data.DealerPrice;
        _model.Quantity = product.Data.Quantity;
        _model.ImageUrl = product.Data.ImageUrl;
        _model.CategoryId = product.Data.CategoryId;

        //_model.Feature = new FeatureModel
        //{
        //    ManufacturerNumber = product.Data.Features.ManufacturerNumber,
        //    OurCode = product.Data.Features.OurCode,
        //    Manufacturer = product.Data.Features.Manufacturer
        //};

        _model.Feature.ManufacturerNumber = product.Data.Features.ManufacturerNumber;
        _model.Feature.OurCode = product.Data.Features.OurCode;
        _model.Feature.Manufacturer = product.Data.Features.Manufacturer;

        LoadCategorySelectDataView();
        if (product != null)
        {
            return View(_model);
        }
        else
        {
            return BadRequest(product.Message);
        }

    }

    [HttpPost("Update/{productId}")]
    public async Task<IActionResult> Update([FromRoute] int productId, ProductUpdateModel model)
    {
        if (ModelState.IsValid)
        {
            ProductDto productDTO = new ProductDto();
            var productForPhoto = await _productService.GetAsync(productId);
            productDTO.Description = model.Description;
            productDTO.Name = model.Name;
            productDTO.Price = model.Price;
            productDTO.ImageUrl = productForPhoto.Data.ImageUrl;
            productDTO.CategoryId = model.CategoryId;
            productDTO.DealerPrice = model.DealerPrice;
            productDTO.Quantity = model.Quantity;

            FeatureDto featureDTO = new FeatureDto();
            featureDTO.Manufacturer = model.Feature.Manufacturer;
            featureDTO.ManufacturerNumber = model.Feature.ManufacturerNumber;
            featureDTO.OurCode = model.Feature.OurCode;

            if (model.CoverPhoto != null)
            {
                string folder = "Images\\";
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.CoverPhoto.FileName;
                string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder, uniqueFileName);

                using (var stream = new FileStream(serverFolder, FileMode.Create))
                {
                    await model.CoverPhoto.CopyToAsync(stream);
                }
                productDTO.ImageUrl = uniqueFileName;
            }

            var result = await _productService.UpdateAsync(productId, productDTO);
            if (result.Success)
            {
                var resultFeature = await _featureService.UpdateAsync(productId, featureDTO);
                if (resultFeature.Success)
                {
                    return RedirectToAction(nameof(Index));
                }
                return BadRequest(result.Message);
            }
            return BadRequest(result.Message);
        }
        return BadRequest();
    }

    [HttpPost("Delete/{productId}")]
    public async Task<IActionResult> Delete([FromRoute] int productId)
    {
        var result = await _productService.DeleteAsync(productId);
        if (result.Success == true)
        {
            return RedirectToAction(nameof(Index));
        }
        return BadRequest("One error occured");
    }

    private void LoadCategorySelectDataView()
    {
        List<Category> categories = _categoryService.GetCategories();
        List<SelectListItem> selectListItem = categories.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();
        ViewData["categories"] = selectListItem;
    }
}
