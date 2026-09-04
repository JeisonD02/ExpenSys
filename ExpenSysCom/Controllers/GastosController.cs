using ExpenSysCom.Data;
using ExpenSysCom.Enums;
using ExpenSysCom.Models;
using ExpenSysCom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

[Authorize]
public class GastosController : Controller
{
    private readonly ExpenSysDBContext _context;

    public GastosController(ExpenSysDBContext context)
    {
        _context = context;
    }


    private string ObtenerIdUsuarioLogueado()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim?.Value ?? string.Empty;
    }


    // ======================= INDEX =======================
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return await CargarGastosFiltrados(null, null);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(DateTime? fechaInicio, DateTime? fechaFin)
    {
        return await CargarGastosFiltrados(fechaInicio, fechaFin);
    }

    private async Task<IActionResult> CargarGastosFiltrados(DateTime? fechaInicio, DateTime? fechaFin)
    {
        string idUsuario = ObtenerIdUsuarioLogueado();

        var query = _context.Gastos
            .Where(g => g.IdUsuario == idUsuario)
            .AsQueryable();

        if (fechaInicio.HasValue)
        {
            query = query.Where(g => g.Fecha >= fechaInicio.Value);
            ViewBag.FechaInicio = fechaInicio.Value.ToString("yyyy-MM-dd");
        }

        if (fechaFin.HasValue)
        {
            query = query.Where(g => g.Fecha <= fechaFin.Value);
            ViewBag.FechaFin = fechaFin.Value.ToString("yyyy-MM-dd");
        }

        var gastos = await query.ToListAsync();
        return View(gastos);
    }



    // ======================= CREATE (GET) =======================
    public IActionResult Create()
    {
        var model = new GastoViewModel
        {
    
        };

        return View(model);
    }

    // ======================= CREATE (POST) =======================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GastoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var usuarioId = ObtenerIdUsuarioLogueado();
        
        if (string.IsNullOrEmpty(usuarioId))
        {
            ModelState.AddModelError("", "Usuario no autenticado.");
        
            return View(model);
        }

        var gasto = new Gasto
        {
            Nombre = model.Nombre,
            Descripcion = model.Descripcion,
            Valor = model.Valor,
            Fecha = model.Fecha,
            IdUsuario = usuarioId,
            CategoriaGasto = model.CategoriaGasto
        };

        _context.Gastos.Add(gasto);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "¡Gasto registrado exitosamente!";
        return RedirectToAction(nameof(Index));
    }

    // ======================= EDIT (GET) =======================
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        string usuarioId = ObtenerIdUsuarioLogueado();

        var gasto = await _context.Gastos
            .FirstOrDefaultAsync(g => g.IdGasto == id && g.IdUsuario == usuarioId);

        if (gasto == null) return NotFound();

        var model = new GastoViewModel
        {
            IdGasto = gasto.IdGasto,
            Nombre = gasto.Nombre,
            Descripcion = gasto.Descripcion,
            Valor = gasto.Valor,
            Fecha = gasto.Fecha,
            CategoriaGasto = gasto.CategoriaGasto,
        };

        return View(model);
    }



    // ======================= EDIT (POST) =======================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, GastoViewModel model)
    {
        if (id != model.IdGasto) return NotFound();

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            string usuarioId = ObtenerIdUsuarioLogueado();

            var gasto = await _context.Gastos
                .FirstOrDefaultAsync(g => g.IdGasto == id && g.IdUsuario == usuarioId);

            if (gasto == null) return NotFound();

            gasto.Nombre = model.Nombre;
            gasto.Descripcion = model.Descripcion;
            gasto.Valor = model.Valor;
            gasto.Fecha = model.Fecha;
            gasto.CategoriaGasto = model.CategoriaGasto;

            _context.Update(gasto);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "¡Gasto actualizado exitosamente!";

            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Gastos.Any(e => e.IdGasto == id))
                return NotFound();
            else
                throw;
        }
    }

    // ======================= DELETE (GET) =======================
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        string usuarioId = ObtenerIdUsuarioLogueado();

        var gasto = await _context.Gastos
            .FirstOrDefaultAsync(g => g.IdGasto == id && g.IdUsuario == usuarioId);

        if (gasto == null) return NotFound();

        return View(gasto);
    }

    // ======================= DELETE (POST) =======================
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        string usuarioId = ObtenerIdUsuarioLogueado();

        var gasto = await _context.Gastos
            .FirstOrDefaultAsync(g => g.IdGasto == id && g.IdUsuario == usuarioId);

        if (gasto == null) return NotFound();

        _context.Gastos.Remove(gasto);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "¡Gasto eliminado exitosamente!";
        return RedirectToAction(nameof(Index));
    }




    // ======================= DETAILS =======================
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        string usuarioId = ObtenerIdUsuarioLogueado();

        var gasto = await _context.Gastos
            .FirstOrDefaultAsync(g => g.IdGasto == id && g.IdUsuario == usuarioId);

        if (gasto == null) return NotFound();

        return View(gasto);
    }


    //============= PDF =================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExportarPdfGastos(DateTime? fechaInicio, DateTime? fechaFin)
    {
        var userId = ObtenerIdUsuarioLogueado();

        var query = _context.Gastos
            .Where(g => g.IdUsuario == userId)
            .AsQueryable();

        if (fechaInicio.HasValue)
            query = query.Where(g => g.Fecha >= fechaInicio.Value);

        if (fechaFin.HasValue)
            query = query.Where(g => g.Fecha <= fechaFin.Value);

        var gastos = await query.ToListAsync();

        var datosReporte = gastos.Select(g => new DatoReporte
        {
            Nombre = g.Nombre,
            Categoria = g.CategoriaGasto.ToString(), 
            Descripcion = g.Descripcion,
            Valor = g.Valor,
            Fecha = g.Fecha
        })
        .ToList();

        var reportePdf = new ReportePdf();

        byte[] archivoPdf = reportePdf.CrearPdf(
            tipoReporte: "Gastos",
            nombreEmpresa: "Tu Empresa",
            usuario: User.Identity.Name ?? "Usuario",
            items: datosReporte,
            rutaLogo: "wwwroot/img/Logo_ExpenSys.jpg",
            desde: fechaInicio?.ToString("yyyy-MM-dd") ?? "Inicio",
            hasta: fechaFin?.ToString("yyyy-MM-dd") ?? "Fin"
        );

        string nombreArchivo = $"ReporteGastos_{fechaInicio?.ToString("yyyyMMdd") ?? "Inicio"}_a_{fechaFin?.ToString("yyyyMMdd") ?? "Fin"}.pdf";

        return File(archivoPdf, "application/pdf", "ReporteGastos.pdf");
    }
}
