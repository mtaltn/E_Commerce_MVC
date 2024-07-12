using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheRaven.Shared.Entity;
using TheRaven.Shared.Dto;
using Microsoft.AspNetCore.Http.Features;

namespace TheRaven.MVC.Areas.Identity.Data;

public class TheRavenMVCContext : IdentityDbContext<ApplicationUser>
{
    public TheRavenMVCContext(DbContextOptions<TheRavenMVCContext> options)
        : base(options)
    {
    }

    //protected override void OnModelCreating(ModelBuilder builder)
    //{
    //    base.OnModelCreating(builder);
    //    // Customize the ASP.NET Identity model and override the defaults if needed.
    //    // For example, you can rename the ASP.NET Identity table names and more.
    //    // Add your customizations after calling base.OnModelCreating(builder);
    //}
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Feature> Features { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Star> Stars { get; set; }
}
