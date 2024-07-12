using Microsoft.EntityFrameworkCore;
using TheRaven.MVC.Areas.Identity.Data;
using TheRaven.MVC.Services.Abstract;
using TheRaven.Shared.Dto;
using TheRaven.Shared.Entity;
using TheRaven.Shared.Response;

namespace TheRaven.MVC.Services.Concrete;

public class FeatureService : IFeatureService
{
    private readonly TheRavenMVCContext _context;
    private readonly IProductService _productService;

    public FeatureService(TheRavenMVCContext context, IProductService productService)
    {
        _context = context;
        _productService = productService;
    }

    public async Task<ServiceResponse<FeatureDto>> CreateFeatureForProduct(FeatureDto featureDTO, int productId)
    {
        var result = await _productService.GetAsync(productId);
        ServiceResponse<FeatureDto> _response = new ServiceResponse<FeatureDto>();
        if (result != null)
        {
            Feature feature = new Feature();
            feature.ProductId = featureDTO.ProductId;
            feature.ManufacturerNumber = featureDTO.ManufacturerNumber;
            feature.Manufacturer = featureDTO.Manufacturer;
            feature.OurCode = featureDTO.OurCode;

            _context.Features.Add(feature);
            if (await _context.SaveChangesAsync() > 0)
            {
                _response.Success = true;
                _response.Message = "Operation Successfull";
                return _response;
            }
        }
        return null;
    }

    public async Task<ServiceResponse<FeatureDto>> UpdateAsync(int productId, FeatureDto featureDto)
    {
        ServiceResponse<FeatureDto> _response = new ServiceResponse<FeatureDto>();

        var _object = await _context.Features.FirstOrDefaultAsync(x => x.ProductId == productId);
        //.FindAsync(x=> x.ProductId);
        if (_object != null)
        {
            _object.Manufacturer = featureDto.Manufacturer;
            _object.ManufacturerNumber = featureDto.ManufacturerNumber;
            _object.OurCode = featureDto.OurCode;
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
