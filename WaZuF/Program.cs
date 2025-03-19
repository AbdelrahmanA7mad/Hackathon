using Microsoft.EntityFrameworkCore;
using WaZuF.Data;
using Microsoft.AspNetCore.Identity;
using WaZuF.Models;
using System.Net.Http;  // Add this
using static WaZuF.Data.AppDbContext;
using WaZuF.Services;
using WaZuF.EmpServices;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();





builder.Services.AddScoped<IJobRequestService, JobRequestService>();

builder.Services.AddScoped<IEmpService, EmpService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();




builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("PersonOnly", policy =>
        policy.RequireClaim("UserType", "Person")); // Remove extra space

    options.AddPolicy("CompanyOnly", policy =>
        policy.RequireClaim("UserType", "Company"));
});



builder.Services.AddHttpClient<IEmpService, EmpService>();

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
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
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
