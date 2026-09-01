using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenSysCom.Data;
using ExpenSysCom.Modelos;
using System.Security.Claims;

namespace ExpenSysCom.Controllers
{
    public class AportesAhorroController : Controller
    {
        private readonly ExpenSysDBContext _context;

        public AportesAhorroController(ExpenSysDBContext context)
        {
            _context = context;
        }

        private string ObtenerIdUsuarioLogueado()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        // GET: index
        public async Task<IActionResult> Index()
        {
            var userId = ObtenerIdUsuarioLogueado();

            var aportesUsuario = _context.AportesAhorros
                .Include(a => a.IdAhorroNavigation)
                .Include(a => a.IdIngresoNavigation)
                .Where(a =>
                    a.IdAhorroNavigation.IdUsuario == userId &&
                    a.IdIngresoNavigation.IdUsuario == userId
                );

          
            var metaActiva = await _context.Metas
                .Include(m => m.IdAhorroNavigation)
                .FirstOrDefaultAsync(m =>
                    m.IdUsuario == userId &&
                    (m.IdEstado == 1 || m.IdEstado == 2) 
                );

            decimal totalAportado = 0;
            decimal porcentaje = 0;
            decimal valorMeta = 0;

            if (metaActiva != null)
            {
                valorMeta = metaActiva.Valor;

                totalAportado = await _context.AportesAhorros
                    .Where(a => a.IdAhorro == metaActiva.IdAhorro)
                    .SumAsync(a => a.Valor);

                porcentaje = valorMeta > 0 ? (totalAportado / valorMeta) * 100 : 0;

              
                if (totalAportado >= valorMeta && metaActiva.IdEstado != 3)
                {
                    metaActiva.IdEstado = 3;
                    _context.Update(metaActiva);
                    await _context.SaveChangesAsync();

                    TempData["MetaFinalizada"] = $"¡Felicidades! La meta \"{metaActiva.Nombre}\" ha sido finalizada.";
                }
            }

            ViewBag.TotalAportado = totalAportado;
            ViewBag.Porcentaje = porcentaje;
            ViewBag.ValorMeta = valorMeta;
            ViewBag.MetaActivaNombre = metaActiva?.Nombre ?? "Sin meta activa o pausada";

            return View(await aportesUsuario.ToListAsync());
        }

        // GET: Create
        public IActionResult Create()
        {
            var userId = ObtenerIdUsuarioLogueado();

            ViewData["IdAhorro"] = new SelectList(
                _context.Ahorros.Where(a => a.IdUsuario == userId),
                "IdAhorro", "Nombre"
            );

            ViewData["IdIngreso"] = new SelectList(
                _context.Ingresos.Where(i => i.IdUsuario == userId),
                "IdIngreso", "Nombre"
            );

            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAporteAhorro,Valor,Fecha,IdAhorro,IdIngreso")] AportesAhorro aportesAhorro)
        {
            var userId = ObtenerIdUsuarioLogueado();

            aportesAhorro.Fecha = aportesAhorro.Fecha.Date;

            
            if (aportesAhorro.Valor <= 0)
            {
                ModelState.AddModelError("Valor", "El valor del aporte debe ser mayor a cero.");
            }

            if (aportesAhorro.Fecha > DateTime.Today)
            {
                ModelState.AddModelError("Fecha", "La fecha del aporte no puede ser en el futuro.");
            }

            aportesAhorro.IdAhorroNavigation = await _context.Ahorros
                .FirstOrDefaultAsync(a => a.IdAhorro == aportesAhorro.IdAhorro && a.IdUsuario == userId);

            aportesAhorro.IdIngresoNavigation = await _context.Ingresos
                .FirstOrDefaultAsync(i => i.IdIngreso == aportesAhorro.IdIngreso && i.IdUsuario == userId);

            if (aportesAhorro.IdAhorroNavigation == null)
            {
                ModelState.AddModelError("IdAhorro", "El ahorro seleccionado no es válido.");
            }

            if (aportesAhorro.IdIngresoNavigation == null)
            {
                ModelState.AddModelError("IdIngreso", "El ingreso seleccionado no es válido.");
            }

            // Validar que el valor no supere el disponible del ingreso
            if (aportesAhorro.IdIngresoNavigation != null)
            {
                var totalAportadoDesdeIngreso = await _context.AportesAhorros
                    .Where(a => a.IdIngreso == aportesAhorro.IdIngreso)
                    .SumAsync(a => (decimal?)a.Valor) ?? 0;

                decimal disponible = aportesAhorro.IdIngresoNavigation.Valor - totalAportadoDesdeIngreso;

                if (aportesAhorro.Valor > disponible)
                {
                    ModelState.AddModelError("Valor", $"El aporte excede el valor disponible del ingreso (disponible: {disponible:C}).");
                }
            }

            
            var metaRelacionada = await _context.Metas
                .FirstOrDefaultAsync(m =>
                    m.IdAhorro == aportesAhorro.IdAhorro &&
                    m.IdUsuario == userId &&
                    (m.IdEstado == 1 || m.IdEstado == 2) 
                );

            if (metaRelacionada == null)
            {
                ModelState.AddModelError("IdAhorro", "Debe existir una meta activa o pausada asociada a este ahorro para poder aportar.");
            }

            if (!ModelState.IsValid)
            {
                ViewData["IdAhorro"] = new SelectList(
                    _context.Ahorros.Where(a => a.IdUsuario == userId),
                    "IdAhorro", "Nombre", aportesAhorro.IdAhorro
                );

                ViewData["IdIngreso"] = new SelectList(
                    _context.Ingresos.Where(i => i.IdUsuario == userId),
                    "IdIngreso", "Nombre", aportesAhorro.IdIngreso
                );

                return View(aportesAhorro);
            }

            _context.Add(aportesAhorro);
            await _context.SaveChangesAsync();
        
            if (metaRelacionada != null && metaRelacionada.IdEstado != 3)
            {
                var totalAportado = await _context.AportesAhorros
                    .Where(a => a.IdAhorro == metaRelacionada.IdAhorro)
                    .SumAsync(a => a.Valor);

                if (totalAportado >= metaRelacionada.Valor)
                {
                    metaRelacionada.IdEstado = 3; 
                    _context.Update(metaRelacionada);
                    await _context.SaveChangesAsync();

                    TempData["MetaFinalizada"] = $"¡Felicidades! La meta \"{metaRelacionada.Nombre}\" ha sido finalizada.";
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var aportesAhorro = await _context.AportesAhorros
                .Include(a => a.IdAhorroNavigation)
                .Include(a => a.IdIngresoNavigation)
                .FirstOrDefaultAsync(m => m.IdAporteAhorro == id);

            if (aportesAhorro == null) return NotFound();

            return View(aportesAhorro);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var aportesAhorro = await _context.AportesAhorros.FindAsync(id);
            if (aportesAhorro == null) return NotFound();

            var userId = ObtenerIdUsuarioLogueado();

            ViewData["IdAhorro"] = new SelectList(
                _context.Ahorros.Where(a => a.IdUsuario == userId),
                "IdAhorro", "Nombre", aportesAhorro.IdAhorro
            );

            ViewData["IdIngreso"] = new SelectList(
                _context.Ingresos.Where(i => i.IdUsuario == userId),
                "IdIngreso", "Nombre", aportesAhorro.IdIngreso
            );

            return View(aportesAhorro);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAporteAhorro,Valor,Fecha,IdAhorro,IdIngreso")] AportesAhorro aportesAhorro)
        {
            if (id != aportesAhorro.IdAporteAhorro) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = ObtenerIdUsuarioLogueado();

                    aportesAhorro.IdAhorroNavigation = await _context.Ahorros
                                    .FirstOrDefaultAsync(a => a.IdAhorro == aportesAhorro.IdAhorro && a.IdUsuario == userId);

                    aportesAhorro.IdIngresoNavigation = await _context.Ingresos
                        .FirstOrDefaultAsync(i => i.IdIngreso == aportesAhorro.IdIngreso && i.IdUsuario == userId);

                    if (aportesAhorro.IdAhorroNavigation == null)
                    {
                        ModelState.AddModelError("IdAhorro", "El ahorro seleccionado no es válido.");
                        throw new Exception("Validación inválida");
                    }

                    if (aportesAhorro.IdIngresoNavigation == null)
                    {
                        ModelState.AddModelError("IdIngreso", "El ingreso seleccionado no es válido.");
                        throw new Exception("Validación inválida");
                    }

                    _context.Update(aportesAhorro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AportesAhorroExists(aportesAhorro.IdAporteAhorro))
                        return NotFound();
                    else
                        throw;
                }
                catch
                {
                    var userId = ObtenerIdUsuarioLogueado();
                    ViewData["IdAhorro"] = new SelectList(
                        _context.Ahorros.Where(a => a.IdUsuario == userId),
                        "IdAhorro", "Nombre", aportesAhorro.IdAhorro
                    );

                    ViewData["IdIngreso"] = new SelectList(
                        _context.Ingresos.Where(i => i.IdUsuario == userId),
                        "IdIngreso", "Nombre", aportesAhorro.IdIngreso
                    );

                    return View(aportesAhorro);
                }

                return RedirectToAction(nameof(Index));
            }

            var userIdEdit = ObtenerIdUsuarioLogueado();

            ViewData["IdAhorro"] = new SelectList(
                _context.Ahorros.Where(a => a.IdUsuario == userIdEdit),
                "IdAhorro", "Nombre", aportesAhorro.IdAhorro
            );

            ViewData["IdIngreso"] = new SelectList(
                _context.Ingresos.Where(i => i.IdUsuario == userIdEdit),
                "IdIngreso", "Nombre", aportesAhorro.IdIngreso
            );

            return View(aportesAhorro);
        }

        // GET: Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var aportesAhorro = await _context.AportesAhorros
                .Include(a => a.IdAhorroNavigation)
                .Include(a => a.IdIngresoNavigation)
                .FirstOrDefaultAsync(m => m.IdAporteAhorro == id);

            if (aportesAhorro == null) return NotFound();

            return View(aportesAhorro);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aportesAhorro = await _context.AportesAhorros.FindAsync(id);
            if (aportesAhorro != null)
            {
                _context.AportesAhorros.Remove(aportesAhorro);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AportesAhorroExists(int id)
        {
            return _context.AportesAhorros.Any(e => e.IdAporteAhorro == id);
        }
    }
}


