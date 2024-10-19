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
    public class ArticoloCKsController : Controller
    {
        private readonly GmaxDbContext _context;

        public ArticoloCKsController(GmaxDbContext context)
        {
            _context = context;
        }

        // GET: ArticoloCKs
        public async Task<IActionResult> Index()
        {
            return View(await _context.ArticoliCK.ToListAsync());
        }

        // GET: ArticoloCKs/Details/5
        public async Task<IActionResult> Details(string tipoArticolo, string codiceArticolo)
        {
            if (string.IsNullOrWhiteSpace(tipoArticolo) || string.IsNullOrWhiteSpace(codiceArticolo))
            {
                return NotFound();
            }

            var articoloCK = await _context.ArticoliCK
                .FirstOrDefaultAsync(m => m.TipoArticolo.Equals(tipoArticolo.Trim()) && m.CodiceArticolo.Equals(codiceArticolo.Trim()));
            if (articoloCK == null)
            {
                return NotFound();
            }

            return View(articoloCK);
        }

        // GET: ArticoloCKs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ArticoloCKs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TipoArticolo,CodiceArticolo,Descrizione,UnitaMisuraGestione,QtaScortaMin,CodUbicazione,CodCostruttore,QtaImpCliente,DataInserimento")] ArticoloCK articoloCK)
        {
            if (ModelState.IsValid)
            {
                _context.Add(articoloCK);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(articoloCK);
        }

        // GET: ArticoloCKs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articoloCK = await _context.ArticoliCK.FindAsync(id);
            if (articoloCK == null)
            {
                return NotFound();
            }
            return View(articoloCK);
        }

        // POST: ArticoloCKs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TipoArticolo,CodiceArticolo,Descrizione,UnitaMisuraGestione,QtaScortaMin,CodUbicazione,CodCostruttore,QtaImpCliente,DataInserimento")] ArticoloCK articoloCK)
        {
            if (id != articoloCK.TipoArticolo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(articoloCK);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticoloCKExists(articoloCK.TipoArticolo))
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
            return View(articoloCK);
        }

        // GET: ArticoloCKs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articoloCK = await _context.ArticoliCK
                .FirstOrDefaultAsync(m => m.TipoArticolo == id);
            if (articoloCK == null)
            {
                return NotFound();
            }

            return View(articoloCK);
        }

        // POST: ArticoloCKs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var articoloCK = await _context.ArticoliCK.FindAsync(id);
            if (articoloCK != null)
            {
                _context.ArticoliCK.Remove(articoloCK);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticoloCKExists(string id)
        {
            return _context.ArticoliCK.Any(e => e.TipoArticolo == id);
        }
    }
}
