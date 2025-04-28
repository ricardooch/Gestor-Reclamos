using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PTemp_Ochoa.Models;
using System.Security.Claims;

namespace PTemp_Ochoa.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> AuthenticateUser(string usuario, string clave)
        {
            var empleado = await _context.CEmpleados.FirstOrDefaultAsync(e => e.Usuario == usuario);

            /* 
                Debería utilizarse encriptación en la clave, como con Bcrypt, en este caso no se utilizó para comodidad de revisión
                en la base de datos de las claves por cualquier persona que revise el proyecto y por la falta de un módulo de registro
            */
            if (empleado == null || empleado.Clave != clave) 
            {
                return false;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, empleado.Usuario),
                new Claim(ClaimTypes.NameIdentifier, empleado.IdEmpleado.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return true;
        }

        public async Task Logout()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
