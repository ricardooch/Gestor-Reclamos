using Microsoft.EntityFrameworkCore;
using PTemp_Ochoa.Models;
using System.Security.Claims;

namespace PTemp_Ochoa.Services
{
    public class ReclamoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReclamoService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> CrearReclamo(ReclamoViewModel model)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(); 
            try
            {
                /*
                    Busca si existe el consumidor, sino, lo crea. 
                    También podría validarse ingresar el mismo nombre y apellido según DUI
                */
                var consumidor = await _context.CConsumidors
                    .FirstOrDefaultAsync(c => c.DuiConsumidor == model.DuiConsumidor);

                if (consumidor == null)
                {
                    consumidor = new CConsumidor
                    {
                        NombreConsumidor = model.NombreConsumidor,
                        ApellidoConsumidor = model.ApellidoConsumidor,
                        Direccion = "No especificado",
                        CorreoElectronico = model.CorreoElectronico,
                        DuiConsumidor = model.DuiConsumidor,
                        Activo = true
                    };
                    _context.CConsumidors.Add(consumidor);
                    await _context.SaveChangesAsync();
                }

                // crea el reclamo
                var reclamo = new TReclamo
                {
                    NombreProveedor = model.NombreProveedor,
                    DireccionProveedor = model.DireccionProveedor,
                    TelefonoProveedor = model.TelefonoProveedor,
                    MontoReclamo = model.MontoReclamo,
                    DetalleReclamo = model.DetalleReclamo,
                    FechaIngreso = DateTime.Now,
                    IdConsumidor = consumidor.IdConsumidor,
                    IdEstado = 1, // estado inicial: "Pendiente"
                    Activo = true
                };

                _context.TReclamos.Add(reclamo);
                await _context.SaveChangesAsync();

                // confirma la transacción
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> ClasificarReclamo(ReclamoClasificarViewModel model)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var reclamo = await _context.TReclamos.FindAsync(model.IdReclamo);
                if (reclamo == null)
                    return false;

                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return false;

                reclamo.IdEstado = model.IdEstado;
                reclamo.FechaRevision = DateTime.Now;
                reclamo.IdEmpleado = int.Parse(userId);

                if (model.IdEstado == 3) // Asesoría
                {
                    var asesoria = new TAsesorium
                    {
                        IdReclamo = reclamo.IdReclamo,
                        MotivoAsesoria = model.MotivoAsesoria,
                        RespuestaAsesoria = model.RespuestaAsesoria,
                        FechaIngreso = DateTime.Now
                    };
                    _context.TAsesoria.Add(asesoria);
                }
                else if (model.IdEstado == 4) // Aviso
                {
                    var aviso = new TAviso
                    {
                        IdReclamo = reclamo.IdReclamo,
                        DetalleAviso = model.DetalleAviso,
                        FechaIngreso = DateTime.Now
                    };
                    _context.TAvisos.Add(aviso);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<ReclamosViewModel> ObtenerReclamos(int page, int pageSize)
        {
            var totalReclamos = await _context.TReclamos.CountAsync();
            var reclamos = await _context.TReclamos
                .Include(r => r.IdConsumidorNavigation)
                .Include(r => r.IdEstadoNavigation)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new ReclamosViewModel
            {
                Reclamos = reclamos,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalReclamos / (double)pageSize)
            };
        }

        public async Task<TReclamo> ObtenerReclamoPorId(int id)
        {
            return await _context.TReclamos
                .Include(r => r.IdConsumidorNavigation)
                .Include(r => r.IdEstadoNavigation)
                .Include(r => r.IdEmpleadoNavigation)
                .Include(r => r.TAsesoria)
                .Include(r => r.TAviso)
                .FirstOrDefaultAsync(r => r.IdReclamo == id);
        }

        public async Task<List<CEstado>> ObtenerEstados()
        {
            return await _context.CEstados.ToListAsync();
        }
    }
}
