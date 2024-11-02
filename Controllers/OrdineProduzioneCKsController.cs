using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gmax.Data;
using Gmax.Models.Entities;
using Gmax.Models.Services.OrdineCK;
using Gmax.Models.Extensions;
using Gmax.Models.ExtensionMethods;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Gmax.Controllers
{
    public class OrdineProduzioneCKsController : Controller
    {
        private readonly GmaxDbContext _context;
        private readonly IOrdineProduzioneCKService ordineProduzioneCKService;

        public OrdineProduzioneCKsController(GmaxDbContext context, IOrdineProduzioneCKService ordineProduzioneCKService)
        {
            _context = context;
            this.ordineProduzioneCKService = ordineProduzioneCKService;
        }

        // GET: OrdineProduzioneCKs
        public async Task<IActionResult> Index()
        {
            var ordiniCKListViewModel = await ordineProduzioneCKService.GetOrdineProdCKListViewModelAsync();

            return View(ordiniCKListViewModel);
            //var gmaxDbContext = _context.OrdiniProduzioneCK
            //    .Include(o => o.ArtLancio)
            //    .Include(o => o.ArtComponenteList);
            //return View(await gmaxDbContext.ToListAsync());
        }

        // GET: OrdineProduzioneCKs/Details/5
        public async Task<IActionResult> Details(int nroLancio, int nroSottolancio)
        {
            if (nroLancio == 0 || nroSottolancio == 0)
            {
                return NotFound();
            }

            var ordineProduzioneCKDetailViewModel = await ordineProduzioneCKService.GetOrdineProduzioneCKDetailViewModelAsync(nroLancio, nroSottolancio);

            return View(ordineProduzioneCKDetailViewModel);

            //var ordineProduzioneCK = await _context.OrdiniProduzioneCK
            //    .Include(o => o.ArtLancio)
            //    .Include(o => o.ArtComponenteList)
            //    .FirstOrDefaultAsync(m => m.NroLancio == nroLancio && m.NroSottolancio == nroSottolancio);
            //if (ordineProduzioneCK == null)
            //{
            //    return NotFound();
            //}

            //return View(ordineProduzioneCK);
        }

        public async Task<IActionResult> InlineInput(string tipoarticolo, string codarticolo, int nrolancio, int nrosottolancio)
        {
            OrdineProduzioneCK? ordineProduzioneCK = await ordineProduzioneCKService.GetOrdineProduzioneCKByKeyAsync(nrolancio, nrosottolancio);
            if (ordineProduzioneCK == null)
            {
                throw new Exception("Ordine di produzione non trovato");
            }
            OrdineProdCompCK? ordineProdCompCK = ordineProduzioneCK.OrdineProdCompCKList?.FirstOrDefault(opc => opc.TipoArticolo == tipoarticolo && opc.CodiceArticolo == codarticolo);
            if (ordineProdCompCK == null)
            {
                throw new Exception($"Ordine di produzione componente non trovato, TipoArticolo: {tipoarticolo}, CodiceArticolo: {codarticolo}, NumLancio: {nrolancio}, NumSottolancio: {nrosottolancio}");
            }

            return PartialView("/Views/Shared/Input/_OrdineProdCompCKInlineInput.cshtml", ordineProdCompCK.AsInlineInputViewModel());
        }

        /// <summary>
        /// Metodo non utilizzato
        /// </summary>
        /// <param name="tipoarticolo"></param>
        /// <param name="codicearticolo"></param>
        /// <param name="nrolancio"></param>
        /// <param name="nrosottolancio"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IActionResult> InlineOutput(string tipoarticolo, string codicearticolo, int nrolancio, int nrosottolancio)
        {
            OrdineProduzioneCK? ordineProduzioneCK = await ordineProduzioneCKService.GetOrdineProduzioneCKByKeyAsync(nrolancio, nrosottolancio);
            if (ordineProduzioneCK == null)
            {
                throw new Exception("Ordine di produzione non trovato");
            }
            OrdineProdCompCK? ordineProdCompCK = ordineProduzioneCK.OrdineProdCompCKList?.FirstOrDefault(opc => opc.TipoArticolo == tipoarticolo && opc.CodiceArticolo == codicearticolo);
            if (ordineProdCompCK == null)
            {
                throw new Exception($"Ordine di produzione componente non trovato, TipoArticolo: {tipoarticolo}, CodiceArticolo: {codicearticolo}, NumLancio: {nrolancio}, NumSottolancio: {nrosottolancio}");
            }

            return PartialView("/Views/Shared/Output/_OrdineProdCompCKInlineOutput.cshtml", ordineProdCompCK);
        }

        public async Task<IActionResult> EditInline(Models.ViewModels.OrdineProdCompCK.OrdineProdCompCKInlineInputViewModel opcInputModel)
        {
            if (opcInputModel == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                OrdineProdCompCK? updatedOrdineProdCompCK = null;
                try
                {
                    updatedOrdineProdCompCK = await ordineProduzioneCKService.AddAssegnazioneMagazzinoToOrdineProdCompAsync(opcInputModel);
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Non è stato possibile aggiornare la quantità dell'articolo con riferimenti TipoArticolo: {opcInputModel.TipoArticolo}, CodiceArticolo: {opcInputModel.CodiceArticolo}";
                    return PartialView("/Views/Shared/Output/_OrdineProdCompCKInlineInput.cshtml", opcInputModel);
                    //return BadRequest(ex.Message);
                }
                TempData["ConfirmationMessageInline"] = "Quantità aggiornata con successo";
                return PartialView("/Views/Shared/Output/_OrdineProdCompCKInlineOutput.cshtml", updatedOrdineProdCompCK);
            }
            //IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            TempData["ErrorMessageInline"] = "Valori inseriti non corretti, non è stato possibile aggiornare la quantità articolo";
            return PartialView("/Views/Shared/Input/_OrdineProdCompCKInlineInput.cshtml", opcInputModel);
        }

        #region Metodi da Scaffolding
        // GET: OrdineProduzioneCKs/Create
        public IActionResult Create()
        {
            ViewData["TipoArtLancio"] = new SelectList(_context.ArticoliCK, "TipoArticolo", "TipoArticolo");
            return View();
        }

        // POST: OrdineProduzioneCKs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NroLancio,NroSottolancio,Stato,TipoArtLancio,CodArtLancio,DataCreazione,DataPrevCons")] OrdineProduzioneCK ordineProduzioneCK)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ordineProduzioneCK);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoArtLancio"] = new SelectList(_context.ArticoliCK, "TipoArticolo", "TipoArticolo", ordineProduzioneCK.TipoArtLancio);
            return View(ordineProduzioneCK);
        }

        // GET: OrdineProduzioneCKs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordineProduzioneCK = await _context.OrdiniProduzioneCK.FindAsync(id);
            if (ordineProduzioneCK == null)
            {
                return NotFound();
            }
            ViewData["TipoArtLancio"] = new SelectList(_context.ArticoliCK, "TipoArticolo", "TipoArticolo", ordineProduzioneCK.TipoArtLancio);
            return View(ordineProduzioneCK);
        }

        // POST: OrdineProduzioneCKs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NroLancio,NroSottolancio,Stato,TipoArtLancio,CodArtLancio,DataCreazione,DataPrevCons")] OrdineProduzioneCK ordineProduzioneCK)
        {
            if (id != ordineProduzioneCK.NroLancio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ordineProduzioneCK);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdineProduzioneCKExists(ordineProduzioneCK.NroLancio))
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
            ViewData["TipoArtLancio"] = new SelectList(_context.ArticoliCK, "TipoArticolo", "TipoArticolo", ordineProduzioneCK.TipoArtLancio);
            return View(ordineProduzioneCK);
        }

        // GET: OrdineProduzioneCKs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordineProduzioneCK = await _context.OrdiniProduzioneCK
                .Include(o => o.ArtLancio)
                .FirstOrDefaultAsync(m => m.NroLancio == id);
            if (ordineProduzioneCK == null)
            {
                return NotFound();
            }

            return View(ordineProduzioneCK);
        }

        // POST: OrdineProduzioneCKs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ordineProduzioneCK = await _context.OrdiniProduzioneCK.FindAsync(id);
            if (ordineProduzioneCK != null)
            {
                _context.OrdiniProduzioneCK.Remove(ordineProduzioneCK);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdineProduzioneCKExists(int id)
        {
            return _context.OrdiniProduzioneCK.Any(e => e.NroLancio == id);
        }
        #endregion
    }
}
