using ExpenSysCom.Data;
using ExpenSysCom.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenSysCom.Controllers
{
    [Authorize(Roles = "Admin")]

    public class CategoriasIngresosController : Controller
    {
        private readonly ExpenSysDBContext _context;

        public CategoriasIngresosController(ExpenSysDBContext context)
        {
            _context = context;
        }

        // GET: CategoriasIngresos
        public async Task<IActionResult> Index()
        {
            return View(await _context.CategoriasIngresos.ToListAsync());
        }

        // GET: CategoriasIngresos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriasIngreso = await _context.CategoriasIngresos
                .FirstOrDefaultAsync(m => m.IdCategoriaIngreso == id);
            if (categoriasIngreso == null)
            {
                return NotFound();
            }

            return View(categoriasIngreso);
        }

        // GET: CategoriasIngresos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoriasIngresos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCategoriaIngreso,Nombre")] CategoriasIngreso categoriasIngreso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoriasIngreso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoriasIngreso);
        }

        // GET: CategoriasIngresos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriasIngreso = await _context.CategoriasIngresos.FindAsync(id);
            if (categoriasIngreso == null)
            {
                return NotFound();
            }
            return View(categoriasIngreso);
        }

        // POST: CategoriasIngresos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCategoriaIngreso,Nombre")] CategoriasIngreso categoriasIngreso)
        {
            if (id != categoriasIngreso.IdCategoriaIngreso)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoriasIngreso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriasIngresoExists(categoriasIngreso.IdCategoriaIngreso))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categoriasIngreso);
        }

        // GET: CategoriasIngresos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoriasIngreso = await _context.CategoriasIngresos
                .FirstOrDefaultAsync(m => m.IdCategoriaIngreso == id);
            if (categoriasIngreso == null)
            {
                return NotFound();
            }

            return View(categoriasIngreso);
        }

        // POST: CategoriasIngresos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoriasIngreso = await _context.CategoriasIngresos.FindAsync(id);
            if (categoriasIngreso != null)
            {
                _context.CategoriasIngresos.Remove(categoriasIngreso);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriasIngresoExists(int id)
        {
            return _context.CategoriasIngresos.Any(e => e.IdCategoriaIngreso == id);
        }
    }
}
