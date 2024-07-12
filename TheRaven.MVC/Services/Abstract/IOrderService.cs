using TheRaven.Shared.Dto;
using TheRaven.Shared.Entity;
using TheRaven.Shared.Response;
namespace TheRaven.MVC.Services.Abstract;

public interface IOrderService
{
    Task<ServiceResponse<Order>> CreateOrder(OrderDto model, List<Product> _product);
    Task<ServiceResponse<List<Order>>> ListOrders();
    Task<ServiceResponse<Order>> GetOrderById(int id);
    Task<ServiceResponse<List<Order>>> MyOrders(string userId);
    Task<bool> IsMyProduct(string userId, int productId);
}
