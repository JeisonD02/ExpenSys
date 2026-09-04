using ExpenSysCom.Data;
using ExpenSysCom.Enums;
using ExpenSysCom.Models;
using ExpenSysCom.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExpenSysCom.Controllers
{
    public class AhorrosController : Controller
    {
        private readonly ExpenSysDBContext _context;

        public AhorrosController(ExpenSysDBContext context)
        {
            _context = context; 
        }

        private string ObtenerIdUsuarioLogueado()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim?.Value ?? string.Empty;
        }


        // =========== INDEX ====================
        public async Task<IActionResult> Index()
        {
            var userId = ObtenerIdUsuarioLogueado();
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");

            var ahorros = await _context.Ahorros
                .Where(a => a.IdUsuario == userId)
                .ToListAsync();

            var ahorrosVM = ahorros.Select(a => new AhorroViewModel
            {
                IdAhorro = a.IdAhorro,
                Nombre = a.Nombre,
                Descripcion = a.Descripcion,
                Porcentaje = a.Porcentaje,
                
                Estado = a.Estado,
                Intensidad = a.Intensidad
            });

            return View(ahorrosVM);
        }



        // ======================= DETAILS =======================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ahorro = await _context.Ahorros
                .FirstOrDefaultAsync(a => a.IdAhorro == id);

            if (ahorro == null) return NotFound();

            if (ahorro.IdUsuario != ObtenerIdUsuarioLogueado()) return Forbid();

            var viewModel = new AhorroViewModel
            {
                IdAhorro = ahorro.IdAhorro,
                Nombre = ahorro.Nombre,
                Descripcion = ahorro.Descripcion,
                Porcentaje = ahorro.Porcentaje,

                Estado = ahorro.Estado,
                Intensidad = ahorro.Intensidad,
            };

            return View(viewModel);
        }



        // ======================= CREATE (GET) =======================
        public IActionResult Create()
        {
            var viewModel = new AhorroViewModel
            {
            };

            return View(viewModel); 
        }

        // ======================= CREATE (POST) =======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AhorroViewModel viewModel)
        {
            var userId = ObtenerIdUsuarioLogueado();

            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("", "No se pudo obtener el ID del usuario.");

                return View(viewModel);
            }


            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }


            var ahorro = new Ahorro
            {
                Nombre = viewModel.Nombre,
                Descripcion = viewModel.Descripcion,
                Porcentaje = viewModel.Porcentaje,
                Intensidad = viewModel.Intensidad,
                Estado = Estado.Activo,
                IdUsuario = userId
            };

            try
            {
                _context.Ahorros.Add(ahorro);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al guardar el ahorro.");

                return View(viewModel);
            }

            return RedirectToAction(nameof(Index));
        }



        // ======================= EDIT (GET) =======================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ahorro = await _context.Ahorros.FindAsync(id);
        

            if (ahorro == null) 
                return NotFound();

            if (ahorro.IdUsuario != ObtenerIdUsuarioLogueado()) 
                return Forbid();

            var viewModel = new AhorroViewModel
            {
                IdAhorro = ahorro.IdAhorro,
                Nombre = ahorro.Nombre,
                Descripcion = ahorro.Descripcion,
                Porcentaje = ahorro.Porcentaje,
                Intensidad = ahorro.Intensidad,
                Estado = ahorro.Estado,
            };

            return View(viewModel);
        }

        // ======================= EDIT (POST) =======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AhorroViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var ahorro = await _context.Ahorros.FindAsync(model.IdAhorro);
            
            if (ahorro == null) return NotFound();

            if (ahorro.IdUsuario != ObtenerIdUsuarioLogueado()) return Forbid();


            if (ahorro.IdEstado == 3)
            {
                TempData["Error"] = "No se puede editar un ahorro finalizado.";

                return RedirectToAction("Index");
            }

            
            ahorro.Nombre = model.Nombre;
            ahorro.Descripcion = model.Descripcion;
            ahorro.Porcentaje = model.Porcentaje;
            ahorro.Intensidad = model.Intensidad;
            ahorro.Estado = model.Estado;

            try
            {
                _context.Update(ahorro);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Ahorros.Any(a => a.IdAhorro == model.IdAhorro))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction("Index");
        }



        // ======================= DELETE (GET) =======================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var ahorro = await _context.Ahorros
                .FirstOrDefaultAsync(a => a.IdAhorro == id);

            if (ahorro == null) return NotFound();
            if (ahorro.IdUsuario != ObtenerIdUsuarioLogueado()) return Forbid();

            var viewModel = new AhorroViewModel
            {
                IdAhorro = ahorro.IdAhorro,
                Nombre = ahorro.Nombre,
                Descripcion = ahorro.Descripcion,
                Porcentaje = ahorro.Porcentaje,
                Intensidad = ahorro.Intensidad,
                Estado = ahorro.Estado,
            };

            return View(viewModel);
        }

        // ======================= DELETE (POST) =======================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ahorro = await _context.Ahorros.FindAsync(id);
            if (ahorro != null && ahorro.IdUsuario == ObtenerIdUsuarioLogueado())
            {
                _context.Ahorros.Remove(ahorro);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
