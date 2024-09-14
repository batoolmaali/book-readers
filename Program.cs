using BookReaders.Areas.AccountUser.Models;
using BookReaders.Data;
using BookReaders.Repository.Base;
using BookReaders.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var conncectionString = configuration.GetConnectionString("BookConnectionString");

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(conncectionString));


builder.Services.AddTransient(typeof(IRepository<>), typeof(MainRepository<>));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaims>();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/AccountUser/Account/Login";
});

/*builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret =builder.Configuration["Authentication:Google:ClientSecret"];
});*/


builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 512 * 1024 * 1024;
});
builder.Services.Configure<FormOptions>(options =>
{
    /* options.MultipartBodyLengthLimit = 104857600;*/
    options.MultipartBodyLengthLimit = 512 * 1024 * 1024; // Set to 2GB or any desired value
});


var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
