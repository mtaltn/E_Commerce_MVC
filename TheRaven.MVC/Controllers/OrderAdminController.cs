using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheRaven.MVC.Areas.Identity.Data;
using TheRaven.MVC.Services.Abstract;

namespace TheRaven.MVC.Controllers;

[Authorize(Roles = "Admin")]
public class OrderAdminController : Controller
{
    private readonly IOrderService _orderService;
    private readonly TheRavenMVCContext _context;

    public OrderAdminController(IOrderService orderService, TheRavenMVCContext context)
    {
        _orderService = orderService;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _orderService.ListOrders();
        if (result != null)
        {
            return View(result.Data);
        }
        return View(result);
    }

    [HttpPost("ChangeStatus/{orderId}")]
    public async Task<IActionResult> StatusCreate([FromRoute] int orderId, string orderStatus)
    {
        var order = await _orderService.GetOrderById(orderId);
        if (order != null)
        {
            order.Data.Status = orderStatus;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "OrderAdmin");

        }
        return BadRequest(order.Message);
    }
}

