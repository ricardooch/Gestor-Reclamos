using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PTemp_Ochoa.Models;
using PTemp_Ochoa.Services;

var builder = WebApplication.CreateBuilder(args);

// Servicios a utilizar
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ReclamoService>();
builder.Services.AddHttpContextAccessor();


// Config de bd
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

// Config de sesiones
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; 
        options.AccessDeniedPath = "/Auth/AccessDenied"; 
    });

var app = builder.Build();

app.UseSession(); // Sesiones

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  
app.UseAuthorization();

// Ruta predeterminada
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
