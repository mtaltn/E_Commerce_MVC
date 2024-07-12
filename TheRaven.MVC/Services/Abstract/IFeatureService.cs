using TheRaven.Shared.Dto;
using TheRaven.Shared.Entity;
using TheRaven.Shared.Response;

namespace TheRaven.MVC.Services.Abstract;

public interface IFeatureService
{
    Task<ServiceResponse<FeatureDto>> CreateFeatureForProduct(FeatureDto featureDTO, int productId);
    Task<ServiceResponse<FeatureDto>> UpdateAsync(int productId, FeatureDto featureDto);
    //Task<ServiceResponse<Feature>> GetAsync(int productId);
}
