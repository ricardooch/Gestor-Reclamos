using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PTemp_Ochoa.Services;
using DotNetEnv;
using PTemp_Ochoa.Models;

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

// Cargar archivo .env solo en desarrollo
if (env == "Development")
{
    DotNetEnv.Env.Load(); 
}

var builder = WebApplication.CreateBuilder(args);

// 🔐 Obtener el connection string desde variable de entorno
var connectionString = Environment.GetEnvironmentVariable("RECLAMOSDT_CONNECTION_STRING");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("La variable de entorno RECLAMOSDT_CONNECTION_STRING no está definida.");
}

// 🌐 Mostrar la cadena de conexión actual para depuración
Console.WriteLine("🔎 Conexión actual: " + connectionString);

// 📦 Registrar DbContext con la conexión
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 🧩 Registrar servicios propios
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ReclamoService>();

// ⚙️ Servicios adicionales
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });

var app = builder.Build();

// 🌐 Middleware
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// 🧭 Rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 🛠️ Manejo de errores solo en producción
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.Run();
