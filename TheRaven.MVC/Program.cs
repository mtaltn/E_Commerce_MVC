using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheRaven.MVC.Areas.Identity.Data;
using TheRaven.MVC.Core.ForEmail;
using TheRaven.MVC.Extensions;
using TheRaven.MVC.Services.Abstract;
using TheRaven.MVC.Services.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("TheRavenMVCContextConnection") ?? throw new InvalidOperationException("Connection string 'TheRavenMVCContextConnection' not found.");
builder.Services.AddDbContext<TheRavenMVCContext>(options =>
    options.UseSqlServer(connectionString));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<TheRavenMVCContext>();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "E_CommerceWith_EDT_Automotive";
    options.IdleTimeout = TimeSpan.FromDays(5);
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddDefaultTokenProviders().AddDefaultUI().AddEntityFrameworkStores<TheRavenMVCContext>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IFeatureService, FeatureService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IFilterProductService, FilterProductService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IStarService, StarService>();

builder.Services.AddElastic(builder.Configuration);


builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ProductClient}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
