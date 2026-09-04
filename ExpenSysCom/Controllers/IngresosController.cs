using ExpenSysCom.Data;
using ExpenSysCom.Models;
using ExpenSysCom.Services;
using ExpenSysCom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExpenSysCom.Controllers
{
    [Authorize]
    public class IngresosController : Controller
    {
        private readonly ExpenSysDBContext _context;

        public IngresosController(ExpenSysDBContext context)
        {
            _context = context;
        }

        // ======================= INDEX =======================
        [HttpGet]
        public async Task<IActionResult> Index(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = _context.Ingresos
                .Where(i => i.IdUsuario == userId)
                .AsQueryable();

            if (fechaInicio.HasValue)
            {
                query = query.Where(i => i.Fecha >= fechaInicio.Value);
                ViewBag.FechaInicio = fechaInicio.Value.ToString("yyyy-MM-dd");
            }

            if (fechaFin.HasValue)
            {
                query = query.Where(i => i.Fecha <= fechaFin.Value);
                ViewBag.FechaFin = fechaFin.Value.ToString("yyyy-MM-dd");
            }

            var ingresos = await query.ToListAsync();
            return View(ingresos);
        }


        // GET: Ingresos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingreso = await _context.Ingresos
                .Include(i => i.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdIngreso == id && m.IdUsuario == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (ingreso == null)
            {
                return NotFound();
            }

            return View(ingreso);
        }



        // GET: Ingresos/Create
        public IActionResult Create()
        {
            var viewModel = new IngresoViewModel();
            
            return View(viewModel);
        }

        // POST: Ingresos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IngresoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var ingreso = new Ingreso
                {
                    Nombre = viewModel.Nombre,
                    Descripcion = viewModel.Descripcion,
                    Valor = viewModel.Valor,
                    Fecha = viewModel.Fecha,
                    CategoriaIngreso = viewModel.CategoriaIngreso,
                    IdUsuario = userId
                };

                _context.Add(ingreso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Ingresos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingreso = await _context.Ingresos.FirstOrDefaultAsync(m => m.IdIngreso == id && m.IdUsuario == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (ingreso == null)
            {
                return NotFound();
            }

            var viewModel = new IngresoViewModel
            {
                IdIngreso = ingreso.IdIngreso,
                Nombre = ingreso.Nombre,
                Descripcion = ingreso.Descripcion,
                Valor = ingreso.Valor,
                Fecha = ingreso.Fecha,
                CategoriaIngreso = ingreso.CategoriaIngreso
            };

            return View(viewModel);
        }

        // POST: Ingresos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IngresoViewModel viewModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                try
                {
                    var ingresoToUpdate = await _context.Ingresos.FirstOrDefaultAsync(i => i.IdIngreso == viewModel.IdIngreso && i.IdUsuario == userId);

                    if (ingresoToUpdate == null)
                    {
                        return NotFound();
                    }

                    ingresoToUpdate.Nombre = viewModel.Nombre;
                    ingresoToUpdate.Descripcion = viewModel.Descripcion;
                    ingresoToUpdate.Valor = viewModel.Valor;
                    ingresoToUpdate.Fecha = viewModel.Fecha;
                    ingresoToUpdate.CategoriaIngreso = viewModel.CategoriaIngreso;

                    _context.Update(ingresoToUpdate);

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngresoExists(viewModel.IdIngreso))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(viewModel);
        }

        // GET: Ingresos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingreso = await _context.Ingresos
                .Include(i => i.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdIngreso == id && m.IdUsuario == User
                .FindFirstValue(ClaimTypes.NameIdentifier));

            if (ingreso == null)
            {
                return NotFound();
            }

            return View(ingreso);
        }

        // POST: Ingresos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ingreso = await _context.Ingresos.FindAsync(id);

            if (ingreso != null && ingreso.IdUsuario == User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                _context.Ingresos.Remove(ingreso);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool IngresoExists(int id)
        {
            return _context.Ingresos.Any(e => e.IdIngreso == id);
        }


        //Generar pdf
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExportarPdf(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = _context.Ingresos
                .Where(i => i.IdUsuario == userId)
                .AsQueryable();

            if (fechaInicio.HasValue)
                query = query.Where(i => i.Fecha >= fechaInicio.Value);

            if (fechaFin.HasValue)
                query = query.Where(i => i.Fecha <= fechaFin.Value);

            var ingresos = await query.ToListAsync();

            var datosReporte = ingresos.Select(i => new DatoReporte
            {
                Nombre = i.Nombre,
                Categoria = i.CategoriaIngreso.ToString(),
                Descripcion = i.Descripcion,
                Valor = i.Valor,
                Fecha = i.Fecha
            }).ToList();

            var reportePdf = new ReportePdf();
            byte[] archivoPdf = reportePdf.CrearPdf(
                tipoReporte: "Ingresos",
                nombreEmpresa: "Tu Empresa",
                usuario: User.Identity.Name,
                items: datosReporte,
                rutaLogo: "wwwroot/img/Logo_ExpenSys.jpg", 
                desde: fechaInicio?.ToString("yyyy-MM-dd") ?? "Inicio",
                hasta: fechaFin?.ToString("yyyy-MM-dd") ?? "Fin"
            );

            return File(archivoPdf, "application/pdf", "ReporteIngresos.pdf");
        }

    }
}
