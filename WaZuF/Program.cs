using Microsoft.EntityFrameworkCore;
using WaZuF.Data;
using Microsoft.AspNetCore.Identity;
using WaZuF.Models;
using System.Net.Http;  // Add this
using static WaZuF.Data.AppDbContext;
using WaZuF.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();





builder.Services.AddScoped<IJobRequestService, JobRequestService>();


builder.Services.AddAuthorization(options =>

{
    options.AddPolicy("PersonOnly", policy =>
        policy.RequireClaim(" UserType", "Person"));

    options.AddPolicy("CompanyOnly", policy =>
        policy.RequireClaim("UserType", "Company"));
});





// Database Configuration
var connectionString = builder.Configuration.GetConnectionString("MyConnection")
                       ?? throw new InvalidOperationException("Connection string 'MyConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// ✅ Register HttpClient for GeminiService
builder.Services.AddHttpClient<IGeminiService, GeminiService>();

// ✅ Keep other service registrations
builder.Services.AddScoped<IJopService, JopService>();

// Identity Configuration
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultUI()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages(); // ????? ???? Identity UI

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
