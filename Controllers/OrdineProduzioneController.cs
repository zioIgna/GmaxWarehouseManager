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
    public class OrdineProduzioneController : Controller
    {
        private readonly GmaxDbContext _context;

        public OrdineProduzioneController(GmaxDbContext context)
        {
            _context = context;
        }

        // GET: OrdineProduzione
        public async Task<IActionResult> Index()
        {
            var gmaxDbContext = _context.OrdiniProduzioneIntKey.Include(o => o.ArtLancio);
            return View(await gmaxDbContext.ToListAsync());
        }

        // GET: OrdineProduzione/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordineProduzione = await _context.OrdiniProduzioneIntKey
                .Include(o => o.ArtLancio)
                .Include(o => o.ArtComponenteList)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ordineProduzione == null)
            {
                return NotFound();
            }

            return View(ordineProduzione);
        }

        // GET: OrdineProduzione/Create
        public IActionResult Create()
        {
            ViewData["ArtLancioId"] = new SelectList(_context.ArticoliIntKey, "Id", "CodiceArticolo");
            return View();
        }

        // POST: OrdineProduzione/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NroLancio,NroSottolancio,Stato,ArtLancioId,DataCreazione,DataPrevCons")] OrdineProduzione ordineProduzione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ordineProduzione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            ViewData["ArtLancioId"] = new SelectList(_context.ArticoliIntKey, "Id", "CodiceArticolo", ordineProduzione.ArtLancioId);
            return View(ordineProduzione);
        }

        // GET: OrdineProduzione/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordineProduzione = await _context.OrdiniProduzioneIntKey.FindAsync(id);
            if (ordineProduzione == null)
            {
                return NotFound();
            }
            ViewData["ArtLancioId"] = new SelectList(_context.ArticoliIntKey, "Id", "CodiceArticolo", ordineProduzione.ArtLancioId);
            return View(ordineProduzione);
        }

        // POST: OrdineProduzione/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NroLancio,NroSottolancio,Stato,ArtLancioId,DataCreazione,DataPrevCons")] OrdineProduzione ordineProduzione)
        {
            if (id != ordineProduzione.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ordineProduzione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdineProduzioneExists(ordineProduzione.Id))
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
            ViewData["ArtLancioId"] = new SelectList(_context.ArticoliIntKey, "Id", "CodiceArticolo", ordineProduzione.ArtLancioId);
            return View(ordineProduzione);
        }

        // GET: OrdineProduzione/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordineProduzione = await _context.OrdiniProduzioneIntKey
                .Include(o => o.ArtLancio)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ordineProduzione == null)
            {
                return NotFound();
            }

            return View(ordineProduzione);
        }

        // POST: OrdineProduzione/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ordineProduzione = await _context.OrdiniProduzioneIntKey.FindAsync(id);
            if (ordineProduzione != null)
            {
                _context.OrdiniProduzioneIntKey.Remove(ordineProduzione);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdineProduzioneExists(int id)
        {
            return _context.OrdiniProduzioneIntKey.Any(e => e.Id == id);
        }
    }
}
