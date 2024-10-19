using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gmax.Data;
using Gmax.Models.Entities;

namespace Gmax.Controllers
{
    public class ArticoloController : Controller
    {
        private readonly GmaxDbContext _context;

        public ArticoloController(GmaxDbContext context)
        {
            _context = context;
        }

        // GET: Articolo
        public async Task<IActionResult> Index()
        {
            return View(await _context.ArticoliIntKey.ToListAsync());
        }

        // GET: Articolo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articolo = await _context.ArticoliIntKey
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articolo == null)
            {
                return NotFound();
            }

            return View(articolo);
        }

        // GET: Articolo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Articolo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TipoArticolo,CodiceArticolo,Descrizione,UnitaMisuraGestione,QtaScortaMin,CodUbicazione,CodCostruttore,QtaImpCliente,DataInserimento")] Articolo articolo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(articolo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(articolo);
        }

        // GET: Articolo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articolo = await _context.ArticoliIntKey.FindAsync(id);
            if (articolo == null)
            {
                return NotFound();
            }
            return View(articolo);
        }

        // POST: Articolo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TipoArticolo,CodiceArticolo,Descrizione,UnitaMisuraGestione,QtaScortaMin,CodUbicazione,CodCostruttore,QtaImpCliente,DataInserimento")] Articolo articolo)
        {
            if (id != articolo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(articolo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticoloExists(articolo.Id))
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
            return View(articolo);
        }

        // GET: Articolo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articolo = await _context.ArticoliIntKey
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articolo == null)
            {
                return NotFound();
            }

            return View(articolo);
        }

        // POST: Articolo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articolo = await _context.ArticoliIntKey.FindAsync(id);
            if (articolo != null)
            {
                _context.ArticoliIntKey.Remove(articolo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticoloExists(int id)
        {
            return _context.ArticoliIntKey.Any(e => e.Id == id);
        }
    }
}
