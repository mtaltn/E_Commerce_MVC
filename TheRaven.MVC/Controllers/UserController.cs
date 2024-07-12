using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheRaven.MVC.Areas.Identity.Data;

namespace TheRaven.MVC.Controllers;
//[Authorize(Roles = "Admin")]
public class UserController : Controller
{
    private readonly TheRavenMVCContext _context;

    public UserController(TheRavenMVCContext context)
    {
        _context = context;
    }

    [HttpGet("VerificationEmail/{concurrencyStamp}")]
    public async Task<IActionResult> Index([FromRoute] string concurrencyStamp)
    {
        var user = await _context.Users.Where(x => x.ConcurrencyStamp == concurrencyStamp).FirstOrDefaultAsync();
        return View(user);
    }

    [HttpPost("RegisterConfirmation/{concurrencyStamp}")]
    public async Task<IActionResult> RegisterVerify([FromRoute] string concurrencyStamp)
    {
        var userDetail = _context.Users.Where(x => x.ConcurrencyStamp == concurrencyStamp).FirstOrDefault();
        userDetail.IsRegisterVerification = true;
        await _context.SaveChangesAsync();
        return View();
    }
}
