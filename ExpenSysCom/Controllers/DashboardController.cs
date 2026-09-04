using ExpenSysCom.Data;
using ExpenSysCom.ViewModels;
using ExpenSysCom.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ExpenSysCom.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ExpenSysDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(ExpenSysDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // 1️⃣ Obtener usuario autenticado
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction("Index", "Login");

            var usuario = await _userManager.FindByIdAsync(userId);
            if (usuario == null) return RedirectToAction("Index", "Login");

            // 2️⃣ Totales y listas principales
            var gastos = await _context.Gastos.Where(g => g.IdUsuario == userId).ToListAsync();
            var ingresos = await _context.Ingresos.Where(i => i.IdUsuario == userId).ToListAsync();
            var metas = await _context.Metas.Where(m => m.IdUsuario == userId).ToListAsync();
            var ahorros = await _context.Ahorros.Where(a => a.IdUsuario == userId).ToListAsync();

            // 3️⃣ Filtros para gráfico
            var hoy = DateTime.Today;

            // — Días (últimos 7 días)
            var dias = Enumerable.Range(0, 7)
                                 .Select(i => hoy.AddDays(-i))
                                 .OrderBy(d => d)
                                 .ToList();

            var ingresosDia = new List<decimal>();
            var gastosDia = new List<decimal>();

            foreach (var dia in dias)
            {
                ingresosDia.Add(await _context.Ingresos
                    .Where(i => i.IdUsuario == userId && i.Fecha.Date == dia)
                    .SumAsync(i => (decimal?)i.Valor) ?? 0);

                gastosDia.Add(await _context.Gastos
                    .Where(g => g.IdUsuario == userId && g.Fecha.Date == dia)
                    .SumAsync(g => (decimal?)g.Valor) ?? 0);
            }

            // — Semanas (desde inicio del año)
            var fechaInicioAno = new DateTime(hoy.Year, 1, 1);
            var semanas = new List<string>();
            var ingresosSemana = new List<decimal>();
            var gastosSemana = new List<decimal>();

            int numeroSemana = 1;
            for (var fecha = fechaInicioAno; fecha <= hoy; fecha = fecha.AddDays(7))
            {
                var fechaFinSemana = fecha.AddDays(6);
                semanas.Add($"Semana {numeroSemana}");

                ingresosSemana.Add(await _context.Ingresos
                    .Where(i => i.IdUsuario == userId && i.Fecha.Date >= fecha && i.Fecha.Date <= fechaFinSemana)
                    .SumAsync(i => (decimal?)i.Valor) ?? 0);

                gastosSemana.Add(await _context.Gastos
                    .Where(g => g.IdUsuario == userId && g.Fecha.Date >= fecha && g.Fecha.Date <= fechaFinSemana)
                    .SumAsync(g => (decimal?)g.Valor) ?? 0);

                numeroSemana++;
            }

            // — Meses
            var meses = Enumerable.Range(1, 12)
                                  .Select(m => new DateTime(hoy.Year, m, 1))
                                  .ToList();

            var ingresosMes = new List<decimal>();
            var gastosMes = new List<decimal>();

            foreach (var mes in meses)
            {
                ingresosMes.Add(await _context.Ingresos
                    .Where(i => i.IdUsuario == userId && i.Fecha.Month == mes.Month && i.Fecha.Year == hoy.Year)
                    .SumAsync(i => (decimal?)i.Valor) ?? 0);

                gastosMes.Add(await _context.Gastos
                    .Where(g => g.IdUsuario == userId && g.Fecha.Month == mes.Month && g.Fecha.Year == hoy.Year)
                    .SumAsync(g => (decimal?)g.Valor) ?? 0);
            }

            // — Años
            var anosRegistrados = await _context.Ingresos
                .Where(i => i.IdUsuario == userId)
                .Select(i => i.Fecha.Year)
                .Distinct()
                .OrderBy(y => y)
                .ToListAsync();

            var ingresosAno = new List<decimal>();
            var gastosAno = new List<decimal>();

            foreach (var ano in anosRegistrados)
            {
                ingresosAno.Add(await _context.Ingresos
                    .Where(i => i.IdUsuario == userId && i.Fecha.Year == ano)
                    .SumAsync(i => (decimal?)i.Valor) ?? 0);

                gastosAno.Add(await _context.Gastos
                    .Where(g => g.IdUsuario == userId && g.Fecha.Year == ano)
                    .SumAsync(g => (decimal?)g.Valor) ?? 0);
            }

            // 4️⃣ Crear ViewModel final
            var viewModel = new DashboardViewModel
            {
                NombreUsuario = usuario.UserName,
                Gastos = gastos,
                Ingresos = ingresos,
                Metas = metas,
                Ahorros = ahorros,
                Meses = meses.Select(m => m.ToString("MMMM")).ToList(),

                TotalGastos = gastos.Sum(g => g.Valor),
                TotalIngresos = ingresos.Sum(i => i.Valor),
                TotalAhorros = ahorros.Count,
                TotalMetas = metas.Count,

                Dias = dias.Select(d => d.ToString("dd/MM")).ToList(),
                IngresosDia = ingresosDia,
                GastosDia = gastosDia,

                Semanas = semanas,
                IngresosSemana = ingresosSemana,
                GastosSemana = gastosSemana,

                IngresosMes = ingresosMes,
                GastosMes = gastosMes,

                Anos = anosRegistrados.Select(y => y.ToString()).ToList(),
                IngresosAno = ingresosAno,
                GastosAno = gastosAno
            };

            return View(viewModel);
        }
    }
}
