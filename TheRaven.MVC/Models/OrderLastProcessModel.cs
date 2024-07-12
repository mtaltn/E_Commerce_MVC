using TheRaven.Shared.Dto;
using TheRaven.Shared.Entity;

namespace TheRaven.MVC.Models;

public class OrderLastProcessModel
{
    public OrderDto _orderDTO { get; set; } = new OrderDto();
    public List<Product> _product { get; set; } = new List<Product>();
}
