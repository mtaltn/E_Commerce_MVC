using TheRaven.Shared.Dto;
using TheRaven.Shared.Response;

namespace TheRaven.MVC.Services.Abstract;

public interface IStarService
{
    Task<ServiceResponse<string>> GiveStarProduct(StarDto model);
}
