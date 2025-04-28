using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PTemp_Ochoa.Services;

namespace PTemp_Ochoa.Controllers
{
    public class ReclamoController : Controller
    {
        private readonly ReclamoService _reclamoService;

        public ReclamoController(ReclamoService reclamoService)
        {
            _reclamoService = reclamoService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(ReclamoViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool resultado = await _reclamoService.CrearReclamo(model);
            if (!resultado)
            {
                ModelState.AddModelError("", "Ocurrió un error al registrar el reclamo. Intente nuevamente.");
                return View(model);
            }

            return RedirectToAction("Confirmacion");
        }

        [AllowAnonymous]
        public IActionResult Confirmacion()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Index(int page = 1)
        {
            var model = await _reclamoService.ObtenerReclamos(page, 7);
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Detalle(int id)
        {
            var reclamo = await _reclamoService.ObtenerReclamoPorId(id);
            if (reclamo == null)
                return NotFound();

            return View(reclamo);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Clasificar(int id)
        {
            var reclamo = await _reclamoService.ObtenerReclamoPorId(id);
            if (reclamo == null)
                return NotFound();

            var estados = await _reclamoService.ObtenerEstados();

            var model = new ReclamoClasificarViewModel
            {
                IdReclamo = reclamo.IdReclamo,
                NombreProveedor = reclamo.NombreProveedor,
                DireccionProveedor = reclamo.DireccionProveedor,
                DetalleReclamo = reclamo.DetalleReclamo,
                TelefonoProveedor = reclamo.TelefonoProveedor,
                MontoReclamo = reclamo.MontoReclamo,
                NombreConsumidor = reclamo.IdConsumidorNavigation.NombreConsumidor,
                ApellidoConsumidor = reclamo.IdConsumidorNavigation.ApellidoConsumidor,
                CorreoElectronico = reclamo.IdConsumidorNavigation.CorreoElectronico,
                DuiConsumidor = reclamo.IdConsumidorNavigation.DuiConsumidor,
                Estados = estados
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Clasificar(ReclamoClasificarViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool result = await _reclamoService.ClasificarReclamo(model);
            if (!result)
            {
                ModelState.AddModelError("", "No se pudo clasificar el reclamo. Intente nuevamente.");
                return View(model);
            }

            return RedirectToAction("Index");
        }
    }
}
