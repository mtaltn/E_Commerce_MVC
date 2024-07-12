using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using TheRaven.MVC.Areas.Identity.Data;
using TheRaven.MVC.Services.Abstract;
using TheRaven.Shared.Dto;
using TheRaven.Shared.Entity;
using TheRaven.Shared.Response;

namespace TheRaven.MVC.Services.Concrete;

public class ProductService : IProductService
{
    private readonly TheRavenMVCContext _context;
    private readonly ElasticsearchClient _elasticSearch;
    private const string indexName = "the_raven_mvc";

    public ProductService(TheRavenMVCContext context, ElasticsearchClient elasticSearch)
    {
        _context = context;
        _elasticSearch = elasticSearch;
    }

    public async Task<ServiceResponse<ProductDto>> CreateAsync(ProductDto model)
    {
        ServiceResponse<ProductDto> _response = new ServiceResponse<ProductDto>();
        ProductElasticDto _productElastic = new ProductElasticDto();

        Product _product = new()
        {
            CategoryId = model.CategoryId,
            ImageUrl = model.ImageUrl,
            Price = model.Price,
            Description = model.Description,
            Name = model.Name,
            Quantity = model.Quantity,
            DealerPrice = model.DealerPrice,
        };

        _context.Products.Add(_product);
        if (await _context.SaveChangesAsync() > 0)
        {
            var inserted = _context.Products.Single(x => x.Id == _product.Id);
            _productElastic.ImageUrl = _product.ImageUrl;
            _productElastic.Price = (double)_product.Price;
            _productElastic.ProductDescription = _product.Description;
            _productElastic.ProductName = _product.Name;
            _productElastic.CategoryId = _product.CategoryId;
            _productElastic.ProductId = _product.Id;
            var elasticResponse = await _elasticSearch.IndexAsync(_productElastic, x => x.Index(indexName));

            if (!elasticResponse.IsValidResponse)
            {
                return null;
            }

            _response.Success = true;
            _response.Message = _product.Id.ToString();            
            return _response;
        }
        else
        {
            _response.Success = false;
            _response.Message = "Product is not created";
            return _response;
        }
    }

    public async Task<ServiceResponse<bool>> DeleteAsync(int productId)
    {
        var product = await _context.Products.FindAsync(productId);
        ServiceResponse<bool> _response = new ServiceResponse<bool>();
        if (product is not null)
        {
            _context.Products.Remove(product);
            if (await _context.SaveChangesAsync() > 0)
            {
                _response.Success = true;
                _response.Message = "Product is deleted";
                return _response;
            }
        }
        _response.Success = false;
        _response.Message = "Operation failed";
        return _response;
    }

    public async Task<ServiceResponse<Product>> GetAsync(int productId)
    {
        var result = await _context.Products.Include(x=>x.Category).Include(x=>x.Features).FirstOrDefaultAsync(x=>x.Id == productId);
            ServiceResponse<Product> _response = new ServiceResponse<Product>();
            if (result != null)
            {
                _response.Data = result;
                return _response;
            }
            _response.Success = false;
            return _response; 
    }

    public async Task<ServiceResponse<ProductSearchResponse>> GetProductsAsync(int page)
    {
        var pageResult = 3f;
        var result = await _context.Products.ToListAsync();
        var pageCount = Math.Ceiling((result).Count / pageResult);
        var products = await _context.Products.Skip((page - 1) * (int)pageResult).Take((int)pageResult).ToListAsync();
        var response = new ServiceResponse<ProductSearchResponse>()
        {
            Data = new ProductSearchResponse
            {
                Products = products,
                CurrentPage = page,
                Pages = (int)pageCount,
            }
        };
        return response;
    }

    public async Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(int categoryId)
    {
        var result = await _context.Products.Where(x => x.CategoryId == categoryId).ToListAsync();
        ServiceResponse<List<Product>> _response = new ServiceResponse<List<Product>>();
        if (result != null)
        {
            _response.Data = result;
            _response.Success = true;
            _response.Message = "Process is success";
            return _response;
        }
        else
        {
            _response.Success = false;
            _response.Message = "Process is fail";
            return _response;
        }
    }

    public async Task<ServiceResponse<List<Product>>> ListAsync()
    {
        var result = await _context.Products.Include(x => x.Features).ToListAsync();
        ServiceResponse<List<Product>> _response = new ServiceResponse<List<Product>>();
        if (result is not null)
        {
            _response.Data = result;
            _response.Success = true;
            _response.Message = "Process is success";
            return _response;
        }
        else
        {
            _response.Success = false;
            _response.Message = "Process is fail";
            return _response;
        }
    }

    public async Task<ServiceResponse<ProductDto>> UpdateAsync(int productId, ProductDto product)
    {
        ServiceResponse<ProductDto> _response = new ServiceResponse<ProductDto>();
        var _object = await _context.Products.FindAsync(productId);
        if (_object != null)
        {
            _object.Description = product.Description;
            _object.Name = product.Name;
            _object.Price = product.Price;
            _object.DealerPrice = product.DealerPrice;
            _object.Quantity = product.Quantity;
            _object.CategoryId = product.CategoryId;
            _object.ImageUrl = product.ImageUrl;

            await _context.SaveChangesAsync();
            _response.Success = true;
            _response.Message = "Update process is success";
            return _response;
        }
        _response.Success = false;
        _response.Message = "Process is fail";
        return _response;
    }
}
