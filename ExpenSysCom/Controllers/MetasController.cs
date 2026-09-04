using ExpenSysCom.Data;
using ExpenSysCom.Models;
using ExpenSysCom.Services;
using ExpenSysCom.ViewModels;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenSysCom.Enums;

namespace ExpenSysCom.Controllers
{
    public class MetasController : Controller
    {
        private readonly ExpenSysDBContext _context;

        public MetasController(ExpenSysDBContext context)
        {
            _context = context;
        }


        //Index 
        [HttpGet]
        public async Task<IActionResult> Index(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var userId = ObtenerIdUsuarioLogueado();
            if (userId == null) return RedirectToAction("Login", "Account");

            var query = _context.Metas
                .Include(m => m.IdAhorroNavigation)
                .Where(m => m.IdUsuario == userId)
                .AsQueryable();

            if (fechaInicio.HasValue)
            {
                query = query.Where(m => m.FechaInicio >= DateOnly.FromDateTime(fechaInicio.Value));
                ViewBag.FechaInicio = fechaInicio.Value.ToString("yyyy-MM-dd");
            }

            if (fechaFin.HasValue)
            {
                query = query.Where(m => m.FechaFin <= DateOnly.FromDateTime(fechaFin.Value));
                ViewBag.FechaFin = fechaFin.Value.ToString("yyyy-MM-dd");
            }

            var metas = await query.ToListAsync();
            return View(metas);
        }



        // ======================= DETAILS =======================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var meta = await _context.Metas
                .Include(m => m.IdAhorroNavigation)
                .FirstOrDefaultAsync(m => m.IdMeta == id);

            if (meta == null) return NotFound();
            if (meta.IdUsuario != ObtenerIdUsuarioLogueado()) return Forbid();

            var viewModel = new MetaViewModel
            {
                IdMeta = meta.IdMeta,
                Nombre = meta.Nombre,
                Descripcion = meta.Descripcion,
                Valor = meta.Valor,
                FechaInicio = meta.FechaInicio,
                FechaFin = meta.FechaFin,
                IdAhorro = meta.IdAhorro,
                Estado = meta.Estado,
                NombreAhorro = meta.IdAhorroNavigation?.Nombre,
            };

            return View(viewModel);
        }

        // ======================= CREATE (GET) =======================
        public async Task<IActionResult> Create()
        {
            var userId = ObtenerIdUsuarioLogueado();

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var viewModel = new MetaViewModel
            {
                FechaInicio = DateOnly.FromDateTime(DateTime.Today),
            };

            await CargarSelectLists (viewModel, userId);

            return View(viewModel);
        }

        // ======================= CREATE (POST) =======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MetaViewModel viewModel)
        {
            var userId = ObtenerIdUsuarioLogueado();

            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (viewModel.FechaFin <= viewModel.FechaInicio)
                ModelState.AddModelError("FechaFin", "La fecha de fin debe ser posterior a la fecha de inicio.");

            if (viewModel.FechaInicio < DateOnly.FromDateTime(DateTime.Today))
                ModelState.AddModelError("FechaInicio", "La fecha de inicio no puede ser anterior a hoy.");

            var ahorro = await _context.Ahorros
                .Include(a => a.AportesAhorros)
                .FirstOrDefaultAsync(a => a.IdAhorro == viewModel.IdAhorro && a.IdUsuario == userId);

            if (ahorro == null)
                ModelState.AddModelError("IdAhorro", "El ahorro seleccionado no es válido.");
            else
            {
                var valorActual = ahorro.AportesAhorros?.Sum(a => a.Valor) ?? 0;
                if (viewModel.Valor <= valorActual)
                    ModelState.AddModelError("Valor", "El valor de la meta debe ser mayor al valor actual de tu ahorro.");
            }

            // Validar máximo de 5 metas activas
            int metasActivas = _context.Metas.Count(m => m.IdUsuario == userId && m.IdEstado == 1);
            if (metasActivas >= 5)
                ModelState.AddModelError("", "No puedes tener más de 5 metas activas.");

            if (ModelState.IsValid)
            {
                var meta = new Meta
                {
                    Nombre = viewModel.Nombre,
                    Descripcion = viewModel.Descripcion,
                    Valor = viewModel.Valor,
                    FechaInicio = viewModel.FechaInicio,
                    FechaFin = viewModel.FechaFin,
                    IdAhorro = viewModel.IdAhorro,
                    IdUsuario = userId,
                    Estado = Estado.Activo,
                };

                _context.Add(meta);

                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }

            await CargarSelectLists(viewModel, userId);
            
            return View(viewModel);
        }

        // ======================= EDIT (GET) =======================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var meta = await _context.Metas.FindAsync(id);
            if (meta == null) return NotFound();
            var userId = ObtenerIdUsuarioLogueado();
            if (meta.IdUsuario != userId) return Forbid();

            var viewModel = new MetaViewModel
            {
                IdMeta = meta.IdMeta,
                Nombre = meta.Nombre,
                Descripcion = meta.Descripcion,
                Valor = meta.Valor,
                FechaInicio = meta.FechaInicio,
                FechaFin = meta.FechaFin,
                IdAhorro = meta.IdAhorro,
                Estado = meta.Estado
            };

            await CargarSelectLists(viewModel, userId);
            return View(viewModel);
        }

        // ======================= EDIT (POST) =======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MetaViewModel viewModel)
        {
            if (id != viewModel.IdMeta) return NotFound();
            var userId = ObtenerIdUsuarioLogueado();

            if (viewModel.FechaFin <= viewModel.FechaInicio)
                ModelState.AddModelError("FechaFin", "La fecha de fin debe ser posterior a la fecha de inicio.");

            if (viewModel.FechaInicio < DateOnly.FromDateTime(DateTime.Today))
                ModelState.AddModelError("FechaInicio", "La fecha de inicio no puede ser anterior a hoy.");

            // Validar máximo de 5 metas activas
            int metasActivas = _context.Metas.Count(m => m.IdUsuario == userId && 
                                                    m.Estado == Estado.Activo && 
                                                    m.IdMeta != viewModel.IdMeta);
            
            if (metasActivas >= 5 && viewModel.Estado == Estado.Activo)
                ModelState.AddModelError("", "No puedes tener más de 5 metas activas.");


            if (ModelState.IsValid)
            {
                var meta = await _context.Metas.FindAsync(viewModel.IdMeta);

                if (meta == null) return NotFound();
                
                if (meta.IdUsuario != userId) return Forbid();

                if (meta.IdEstado == 3)
                {
                    TempData["Error"] = "No se puede editar una meta finalizada.";
                    return RedirectToAction(nameof(Index));
                }

                meta.Nombre = viewModel.Nombre;
                meta.Descripcion = viewModel.Descripcion;
                meta.Valor = viewModel.Valor;
                meta.FechaInicio = viewModel.FechaInicio;
                meta.FechaFin = viewModel.FechaFin;
                meta.IdAhorro = viewModel.IdAhorro;
                meta.Estado = viewModel.Estado;

                _context.Update(meta);

                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }

            await CargarSelectLists(viewModel, userId);
            return View(viewModel);
        }



        // ======================= DELETE (GET) =======================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var meta = await _context.Metas
                .Include(m => m.IdAhorroNavigation)
                .FirstOrDefaultAsync(m => m.IdMeta == id);

            if (meta == null) return NotFound();
            if (meta.IdUsuario != ObtenerIdUsuarioLogueado()) return Forbid();

            var viewModel = new MetaViewModel
            {
                IdMeta = meta.IdMeta,
                Nombre = meta.Nombre,
                Descripcion = meta.Descripcion,
                Valor = meta.Valor,
                FechaInicio = meta.FechaInicio,
                FechaFin = meta.FechaFin,
                IdAhorro = meta.IdAhorro,
                Estado = meta.Estado,
                NombreAhorro = meta.IdAhorroNavigation?.Nombre,

            };

            return View(viewModel);
        }

        // ======================= DELETE (POST) =======================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var meta = await _context.Metas.FindAsync(id);
            if (meta != null && meta.IdUsuario == ObtenerIdUsuarioLogueado())
            {
                // Opcional: no eliminar metas finalizadas
                if (meta.IdEstado == 3)
                {
                    TempData["Error"] = "No se puede eliminar una meta finalizada.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Metas.Remove(meta);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }



        // ======================= MÉTODOS PRIVADOS =======================
        private string? ObtenerIdUsuarioLogueado()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private async Task CargarSelectLists(MetaViewModel viewModel, string userId)
        {
            viewModel.Ahorros = new SelectList(await _context.Ahorros
                .Where(a => a.IdUsuario == userId)
                .ToListAsync(), "IdAhorro", "Nombre", viewModel.IdAhorro);

        }



        //pdf
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExportarPdf(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var userId = ObtenerIdUsuarioLogueado();

            var query = _context.Metas
                .Include(m => m.IdAhorroNavigation)
                .Where(m => m.IdUsuario == userId)
                .AsQueryable();

            if (fechaInicio.HasValue)
                query = query.Where(m => m.FechaInicio >= DateOnly.FromDateTime(fechaInicio.Value));

            if (fechaFin.HasValue)
                query = query.Where(m => m.FechaFin <= DateOnly.FromDateTime(fechaFin.Value));

            var metas = await query.ToListAsync();

            var datosReporte = metas.Select(m => new DatoReporte
            {
                Nombre = m.Nombre,
                Estado = m.Estado.ToString(),
                Descripcion = m.Descripcion,
                Valor = m.Valor,
                Fecha = m.FechaInicio.ToDateTime(TimeOnly.MinValue) 
            }).ToList();

            var reportePdf = new ReportePdf();
            byte[] archivoPdf = reportePdf.CrearPdf(
                tipoReporte: "Metas",
                nombreEmpresa: "Tu Empresa",
                usuario: User.Identity?.Name ?? "Usuario",
                items: datosReporte,
                rutaLogo: "wwwroot/img/Logo_ExpenSys.jpg", 
                desde: fechaInicio?.ToString("yyyy-MM-dd") ?? "Inicio",
                hasta: fechaFin?.ToString("yyyy-MM-dd") ?? "Fin"
            );

            return File(archivoPdf, "application/pdf", "ReporteMetas.pdf");
        }

    }
}
