using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheRaven.MVC.Areas.Identity.Data;
using TheRaven.MVC.Services.Abstract;
using TheRaven.Shared.Dto;
using TheRaven.Shared.Entity;
using TheRaven.Shared.Response;

namespace TheRaven.MVC.Services.Concrete;

public class StarService : IStarService
{
    private TheRavenMVCContext _context;
    private readonly IMapper _mapper;

    public StarService(TheRavenMVCContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<string>> GiveStarProduct(StarDto model)
    {
        ServiceResponse<string> _response = new ServiceResponse<string>();
        int count = 0;
        double? calc = 0;
        var product = await _context.Products.FindAsync(model.ProductId);
        var objDTO = _mapper.Map<Star>(model);
        _context.Stars.Add(objDTO);
        await _context.SaveChangesAsync();
        var calculateStar = await _context.Stars.Where(x => x.ProductId == model.ProductId).ToListAsync();
        foreach (var item in calculateStar)
        {
            count++;
            calc = calc + item.RateStar;
        }
        product.RateAvg = calc / count;
        await _context.SaveChangesAsync();
        _response.Success = true;
        _response.Message = "Your star is sended";
        return _response;
    }
}
