using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTemp_Ochoa.Models;
using PTemp_Ochoa.Services;

namespace PTemp_Ochoa.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect(returnUrl ?? "/");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool isAuthenticated = await _authService.AuthenticateUser(model.Usuario, model.Clave);

            if (!isAuthenticated)
            {
                ModelState.AddModelError("", "Usuario o contraseña incorrectos");
                return View(model);
            }

            return RedirectToAction("Index", "Reclamo");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}
