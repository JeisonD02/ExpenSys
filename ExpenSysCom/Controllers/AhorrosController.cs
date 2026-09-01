using ExpenSysCom.Data;
using ExpenSysCom.Modelos;
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
            _context = context; // Inyección del contexto de la BD
        }

        // ======================= AUXILIAR =======================
        // Obtiene el ID del usuario logueado
        private string ObtenerIdUsuarioLogueado()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim?.Value ?? string.Empty;
        }

        public async Task<IActionResult> Index()
        {
            var userId = ObtenerIdUsuarioLogueado();
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");

            var ahorros = await _context.Ahorros
                .Include(a => a.IdEstadoNavigation)
                .Include(a => a.IdIntensidadNavigation)
                .Where(a => a.IdUsuario == userId)
                .ToListAsync();

            var ahorrosVM = ahorros.Select(a => new AhorroViewModel
            {
                IdAhorro = a.IdAhorro,
                Nombre = a.Nombre,
                Descripcion = a.Descripcion,
                Porcentaje = a.Porcentaje,
                IdEstado = a.IdEstado,
                IdIntensidad = a.IdIntensidad,
                NombreEstado = a.IdEstadoNavigation?.Nombre,
                NombreIntensidad = a.IdIntensidadNavigation?.Nombre
            });

            return View(ahorrosVM);
        }

        // ======================= DETAILS =======================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var ahorro = await _context.Ahorros
                .Include(a => a.IdEstadoNavigation)
                .Include(a => a.IdIntensidadNavigation)
                .FirstOrDefaultAsync(a => a.IdAhorro == id);

            if (ahorro == null) return NotFound();

            if (ahorro.IdUsuario != ObtenerIdUsuarioLogueado()) return Forbid();

            var viewModel = new AhorroViewModel
            {
                IdAhorro = ahorro.IdAhorro,
                Nombre = ahorro.Nombre,
                Descripcion = ahorro.Descripcion,
                Porcentaje = ahorro.Porcentaje,
                IdEstado = ahorro.IdEstado,
                IdIntensidad = ahorro.IdIntensidad,
                NombreEstado = ahorro.IdEstadoNavigation?.Nombre,
                NombreIntensidad = ahorro.IdIntensidadNavigation?.Nombre
            };

            return View(viewModel);
        }

        // ======================= CREATE (GET) =======================
        public IActionResult Create()
        {
            // 🔹 Creamos el ViewModel con estado Activo por defecto
            var viewModel = new AhorroViewModel
            {
                IdEstado = 1, // Siempre Activo al crear
                Intensidades = new SelectList(_context.Intensidades, "IdIntensidad", "Nombre"), // Cargamos las intensidades desde BD
                Estados = new SelectList(_context.Estados, "IdEstado", "Nombre", 1) // Por si en futuro quieres mostrar estados
            };

            return View(viewModel); // Retornamos la vista con el ViewModel
        }

        // ======================= CREATE (POST) =======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AhorroViewModel viewModel)
        {
            // 🔹 Obtenemos el ID del usuario logueado
            var userId = ObtenerIdUsuarioLogueado();
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("", "No se pudo obtener el ID del usuario.");
                // 🔹 Recargamos SelectList para que el combo no quede vacío
                viewModel.Intensidades = new SelectList(_context.Intensidades, "IdIntensidad", "Nombre", viewModel.IdIntensidad);
                viewModel.Estados = new SelectList(_context.Estados, "IdEstado", "Nombre", viewModel.IdEstado);
                return View(viewModel);
            }

            // 🔹 Validación del modelo
            if (!ModelState.IsValid)
            {
                // 🔹 Recargamos SelectList en caso de error para que los combos se mantengan
                viewModel.Intensidades = new SelectList(_context.Intensidades, "IdIntensidad", "Nombre", viewModel.IdIntensidad);
                viewModel.Estados = new SelectList(_context.Estados, "IdEstado", "Nombre", viewModel.IdEstado);
                return View(viewModel);
            }

            // 🔹 Creamos el objeto Ahorro a partir del ViewModel
            var ahorro = new Ahorro
            {
                Nombre = viewModel.Nombre,
                Descripcion = viewModel.Descripcion,
                Porcentaje = viewModel.Porcentaje,
                IdIntensidad = viewModel.IdIntensidad,
                IdEstado = 1, // Siempre activo al crear
                IdUsuario = userId
            };

            try
            {
                // 🔹 Guardamos el ahorro en la base de datos
                _context.Ahorros.Add(ahorro);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // 🔹 Captura de errores al guardar
                Console.WriteLine($"[ERROR] SaveChangesAsync falló: {ex.Message}");
                ModelState.AddModelError("", "Ocurrió un error al guardar el ahorro.");

                // 🔹 Recargamos SelectList antes de devolver la vista
                viewModel.Intensidades = new SelectList(_context.Intensidades, "IdIntensidad", "Nombre", viewModel.IdIntensidad);
                viewModel.Estados = new SelectList(_context.Estados, "IdEstado", "Nombre", viewModel.IdEstado);

                return View(viewModel);
            }

            // 🔹 Si todo sale bien, redirigimos al Index
            return RedirectToAction(nameof(Index));
        }

        // ======================= EDIT (GET) =======================
        // Muestra el formulario de edición de un ahorro
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            // 🔹 Buscamos el ahorro en la BD
            var ahorro = await _context.Ahorros.FindAsync(id);
            if (ahorro == null) return NotFound();

            // 🔹 Verificamos que el ahorro pertenezca al usuario logueado
            if (ahorro.IdUsuario != ObtenerIdUsuarioLogueado()) return Forbid();

            // 🔹 Creamos el ViewModel y cargamos los SelectList
            var viewModel = new AhorroViewModel
            {
                IdAhorro = ahorro.IdAhorro,
                Nombre = ahorro.Nombre,
                Descripcion = ahorro.Descripcion,
                Porcentaje = ahorro.Porcentaje,
                IdIntensidad = ahorro.IdIntensidad,
                IdEstado = ahorro.IdEstado,
                Estados = new SelectList(_context.Estados, "IdEstado", "Nombre", ahorro.IdEstado),
                Intensidades = new SelectList(_context.Intensidades, "IdIntensidad", "Nombre", ahorro.IdIntensidad)
            };

            return View(viewModel);
        }

        // ======================= EDIT (POST) =======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AhorroViewModel model)
        {
            // 1️⃣ Validación del modelo
            if (!ModelState.IsValid)
            {
                // Volvemos a cargar las SelectList para que el dropdown funcione
                model.Estados = new SelectList(_context.Estados, "IdEstado", "Nombre", model.IdEstado);
                model.Intensidades = new SelectList(_context.Intensidades, "IdIntensidad", "Nombre", model.IdIntensidad);
                return View(model);
            }

            // 2️⃣ Buscar el ahorro en la base de datos
            var ahorro = await _context.Ahorros.FindAsync(model.IdAhorro);
            if (ahorro == null) return NotFound();

            // 3️⃣ Verificar que el ahorro pertenezca al usuario logueado
            if (ahorro.IdUsuario != ObtenerIdUsuarioLogueado()) return Forbid();

            // 4️⃣ No permitir editar si el ahorro está finalizado
            if (ahorro.IdEstado == 3) // 3 = Finalizado
            {
                TempData["Error"] = "No se puede editar un ahorro finalizado.";
                return RedirectToAction("Index");
            }

            // 5️⃣ Actualizar los campos permitidos
            ahorro.Nombre = model.Nombre;
            ahorro.Descripcion = model.Descripcion;
            ahorro.Porcentaje = model.Porcentaje;
            ahorro.IdIntensidad = model.IdIntensidad;
            ahorro.IdEstado = model.IdEstado;

            try
            {
                // 6️⃣ Guardar cambios en la base de datos
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

            // 7️⃣ Redirigir al índice de ahorros
            return RedirectToAction("Index");
        }



        // ======================= DELETE (GET) =======================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var ahorro = await _context.Ahorros
                .Include(a => a.IdEstadoNavigation)
                .Include(a => a.IdIntensidadNavigation)
                .FirstOrDefaultAsync(a => a.IdAhorro == id);

            if (ahorro == null) return NotFound();
            if (ahorro.IdUsuario != ObtenerIdUsuarioLogueado()) return Forbid();

            var viewModel = new AhorroViewModel
            {
                IdAhorro = ahorro.IdAhorro,
                Nombre = ahorro.Nombre,
                Descripcion = ahorro.Descripcion,
                Porcentaje = ahorro.Porcentaje,
                IdIntensidad = ahorro.IdIntensidad,
                IdEstado = ahorro.IdEstado,
                NombreEstado = ahorro.IdEstadoNavigation?.Nombre,
                NombreIntensidad = ahorro.IdIntensidadNavigation?.Nombre
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
