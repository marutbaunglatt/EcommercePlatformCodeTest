using EcommercePlatformCodeTest.Data;
using EcommercePlatformCodeTest.Infterfaces;
using EcommercePlatformCodeTest.Interfaces;
using EcommercePlatformCodeTest.Repositories;
using EcommercePlatformCodeTest.Repositoriies;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromHours(12);
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/login";
        option.AccessDeniedPath = "/denied";
    });

builder.Services.AddScoped<IProduct, ProductRepo>();
builder.Services.AddScoped<IUser, UserRepo>();
builder.Services.AddScoped<ICart, CartRepo>();


var app = builder.Build();
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
