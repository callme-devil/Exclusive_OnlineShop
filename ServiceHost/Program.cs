using System.Text.Encodings.Web;
using System.Text.Unicode;
using _0_Framework.Application;
using _0_Framework.Application.Sms;
using _0_Framework.Application.ZarinPal;
using AccountManagement.Infrastructure.Configuration;
using BlogManagement.Infrastructure.Configuration;
using CommentManagement.Infrastructure.Configuration;
using DiscountManagement.Configuration;
using InventoryManagement.Infrastructure.Configuration;
using InventoryManagement.Presentation.Api;
using Microsoft.AspNetCore.Authentication.Cookies;
using ServiceHost.Controllers;
using ServiceHost.Extension;
using ShopManagement.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//! Encode to Persian
builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Arabic));


//!WiredUp ShopManagementBootstrapper Dependencies
var connectionString = builder.Configuration.GetConnectionString("Exclusive_OnlineShopDb");
ShopManagementBootstrapper.Configure(builder.Services, connectionString);
DiscountManagementBootstrapper.Configure(builder.Services , connectionString);
InventoryManagementBootstrapper.Configure(builder.Services , connectionString);
BlogManagementBootstrapper.Configure(builder.Services, connectionString);
CommentManagementBootstrapper.Configure(builder.Services, connectionString);
AccountManagementBootstrapper.Configure(builder.Services, connectionString);
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddTransient<IFileUploader, FileUploader>();
builder.Services.AddTransient<IAuthHelper , AuthHelper>();
builder.Services.AddTransient<IViewRenderService, RenderViewToString>();
builder.Services.AddTransient<IZarinPalFactory , ZarinPalFactory>();
builder.Services.AddTransient<ISmsService , SmsService>();



builder.Services.AddHttpContextAccessor();


//! CORS = Cross Origin Resource Sharing
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORS-Policy" , builder =>
    {
        builder.WithOrigins("https://localhost:7018")
            .AllowAnyHeader()
            .AllowAnyMethod();
        //builder.AllowAnyOrigin();
    });
});


//! Cookie Section
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    //? Will Cause TempData
    //options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
});

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
//    {
//        options.LoginPath = new PathString("/Login");
//        options.LogoutPath = new PathString("/Login/Logout");
//        options.AccessDeniedPath = new PathString("/AccessDenied");
//        //!Option not necessary
//        options.ExpireTimeSpan = TimeSpan.FromDays(1);
//    });

#region Authentication

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = new PathString("/Login");
    options.LogoutPath = new PathString("/Logout");
    options.AccessDeniedPath = new PathString("/AccessDenied");
    options.ExpireTimeSpan = TimeSpan.FromMinutes(43200); //? Around a Month

});

#endregion


//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminArea", policy => policy.RequireRole(new List<string>{Roles.Administrator , Roles.Manager}));

//    options.AddPolicy("Shop", policy => policy.RequireRole(new List<string>{Roles.Administrator}));

//    options.AddPolicy("Discounts", policy => policy.RequireRole(new List<string> { Roles.Administrator }));
    
//    options.AddPolicy("Account", policy => policy.RequireRole(new List<string> { Roles.Administrator }));

//});

//builder.Services.AddRazorPages()
//    .AddRazorPagesOptions(options =>
//    {
//        options.Conventions.AuthorizeAreaFolder("Administration" , "/" ,  "AdminArea");

//        options.Conventions.AuthorizeAreaFolder("Administration", "/Shop", "Shop");
        
//        options.Conventions.AuthorizeAreaFolder("Administration", "/Discounts", "Discounts");

//        options.Conventions.AuthorizeAreaFolder("Administration", "/Accounts", "Account");

//    });

builder.Services.AddRazorPages()
    .AddApplicationPart(typeof(ProductController).Assembly)
    .AddApplicationPart(typeof(InventoryController).Assembly)
    .AddNewtonsoftJson();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCookiePolicy();

app.UseRouting();

app.UseAuthorization();

app.UseCors("CORS-Policy");

app.MapRazorPages();

app.MapControllers();

app.MapDefaultControllerRoute();

app.Run();
