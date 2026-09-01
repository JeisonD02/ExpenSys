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

    public class IntensidadesController : Controller
    {
        private readonly ExpenSysDBContext _context;

        public IntensidadesController(ExpenSysDBContext context)
        {
            _context = context;
        }

        // GET: Intensidades
        public async Task<IActionResult> Index()
        {
            return View(await _context.Intensidades.ToListAsync());
        }

        // GET: Intensidades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var intensidade = await _context.Intensidades
                .FirstOrDefaultAsync(m => m.IdIntensidad == id);
            if (intensidade == null)
            {
                return NotFound();
            }

            return View(intensidade);
        }

        // GET: Intensidades/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Intensidades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdIntensidad,Nombre")] Intensidad intensidade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(intensidade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(intensidade);
        }

        // GET: Intensidades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var intensidade = await _context.Intensidades.FindAsync(id);
            if (intensidade == null)
            {
                return NotFound();
            }
            return View(intensidade);
        }

        // POST: Intensidades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdIntensidad,Nombre")] Intensidad intensidade)
        {
            if (id != intensidade.IdIntensidad)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(intensidade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IntensidadeExists(intensidade.IdIntensidad))
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
            return View(intensidade);
        }

        // GET: Intensidades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var intensidade = await _context.Intensidades
                .FirstOrDefaultAsync(m => m.IdIntensidad == id);
            if (intensidade == null)
            {
                return NotFound();
            }

            return View(intensidade);
        }

        // POST: Intensidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var intensidade = await _context.Intensidades.FindAsync(id);
            if (intensidade != null)
            {
                _context.Intensidades.Remove(intensidade);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IntensidadeExists(int id)
        {
            return _context.Intensidades.Any(e => e.IdIntensidad == id);
        }
    }
}
