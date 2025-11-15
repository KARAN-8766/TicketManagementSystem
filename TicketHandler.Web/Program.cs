using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TicketHandler.Infrastructure.Data;
using TicketHandler.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserDbContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("TicketRaise"), 
sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 5,              // Retry 5 times
            maxRetryDelay: TimeSpan.FromSeconds(10),  // Wait up to 10 sec between retries
            errorNumbersToAdd: null)));

builder.Services.AddDistributedMemoryCache();

builder.Services.AddScoped<TicketService>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout= TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    //pattern: "{controller=Home}/{action=Index}/{id?}");
    pattern: "{controller=Userdatas}/{action=login}/");

app.Run();
