using Gmax.Data;
using Gmax.Models.Entities;
using Gmax.Models.ExtensionMethods;
using Gmax.Models.Extensions;
using Gmax.Models.Services.ArticoloCK;
using Gmax.Models.Services.Assegnazione;
using Gmax.Models.Services.Magazzino;
using Gmax.Models.Services.OrdineCK;
using Gmax.Models.Services.OrdineProdCompCK;
using Gmax.Models.ViewModels.AssegnazioneModal;
using Gmax.Models.ViewModels.OrdineProdCompCK;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Gmax.Controllers
{
    public class OrdineProduzioneCKsController : Controller
    {
        private readonly GmaxDbContext _context;
        private readonly IOrdineProduzioneCKService ordineProduzioneCKService;
        private readonly IMagazzinoService magazzinoService;
        private readonly IArticoloCKService articoloCKService;
        private readonly IOrdineProdCompCKService ordineProdCompCKService;
        private readonly IAssegnazioneService assegnazioneService;

        public OrdineProduzioneCKsController(GmaxDbContext context, IOrdineProduzioneCKService ordineProduzioneCKService, IMagazzinoService magazzinoService, IArticoloCKService articoloCKService, IOrdineProdCompCKService ordineProdCompCKService, IAssegnazioneService assegnazioneService)
        {
            _context = context;
            this.ordineProduzioneCKService = ordineProduzioneCKService;
            this.magazzinoService = magazzinoService;
            this.articoloCKService = articoloCKService;
            this.ordineProdCompCKService = ordineProdCompCKService;
            this.assegnazioneService = assegnazioneService;
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

            var ordineProdCompCKListViewModel = ordineProduzioneCK.AsDetailViewModel();

            return PartialView("/Views/Shared/Output/_OrdineProdCompCKInlineOutput.cshtml", ordineProdCompCKListViewModel);
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
                    return RedirectToAction(nameof(InlineInput), new { tipoArticolo = opcInputModel.TipoArticolo, codArticolo = opcInputModel.CodiceArticolo, nroLancio = opcInputModel.NroLancio, nroSottolancio = opcInputModel.NroSottolancio });
                }
                TempData["ConfirmationMessageInline"] = "Quantità aggiornata con successo";
                return PartialView("/Views/Shared/Output/_OrdineProdCompCKInlineOutput.cshtml", updatedOrdineProdCompCK);
            }
            //IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            TempData["ErrorMessageInline"] = "Valori inseriti non corretti, non è stato possibile aggiornare la quantità articolo";
            return RedirectToAction(nameof(InlineInput), new { tipoArticolo = opcInputModel.TipoArticolo, codiceArticolo = opcInputModel.CodiceArticolo, nroLancio = opcInputModel.NroLancio, nroSottolancio = opcInputModel.NroSottolancio });
            //return PartialView("/Views/Shared/Input/_OrdineProdCompCKInlineInput.cshtml", opcInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> AssegnaValoreAMagazzino(OrdineProdCompCKListViewModel model)
        {
            if (model.MagazzinoOrigineSelezionato == null)
            {
                throw new ArgumentNullException("Non è stato ricevuto il riferimento del magazzino di origine");
            }
            Magazzino magazzinoScelto = await magazzinoService.GetMagazzinoByCodeAsync(model.MagazzinoOrigineSelezionato);
            if (magazzinoScelto == null)
            {
                throw new KeyNotFoundException("Non è stato possibile identificare il magazzino con codice: " + model.MagazzinoOrigineSelezionato);
            }
            Magazzino magazzinoDestinazione = await magazzinoService.GetMagazzinoByCodeAsync(model.MagazzinoDestinazione);
            if (magazzinoDestinazione == null)
            {
                magazzinoDestinazione = await magazzinoService.CreateMagazzinoFromNrolancioNrosottolancioAsync(model.NroLancio, model.NroSottolancio);
            }
            await assegnazioneService.CreateAssegnazioneMagazzinoFromViewModelAsync(model, magazzinoScelto.Id, magazzinoDestinazione.Id);

            return Ok();
        }

        public async Task<IActionResult> OpenAssegnazioneModal()
        {
            return View();
        }

        public async Task<IActionResult> EditModalAsync(string nrolancio, string nrosottolancio, string tipoarticolo, string codarticolo)
        {
            AssegnazioneModalViewModel assegnazioneModalViewModel = new AssegnazioneModalViewModel();
            assegnazioneModalViewModel.NroLancio = int.Parse(nrolancio);
            assegnazioneModalViewModel.NroSottolancio = int.Parse(nrosottolancio);
            assegnazioneModalViewModel.TipoArticolo = tipoarticolo;
            assegnazioneModalViewModel.CodiceArticolo = codarticolo;
            assegnazioneModalViewModel.Articolo = await articoloCKService.GetArticoloCKByKeyAsync(tipoarticolo, codarticolo);
            await ordineProduzioneCKService.CalculateDisponibilitaMagazzini(assegnazioneModalViewModel);
            await ordineProdCompCKService.InitMagDestQtaDisp(assegnazioneModalViewModel);

            //var vm = _repo.GetMagazzinoViewModel(id);
            //return PartialView("/Views/Shared/Modal/_TestModal.cshtml");  //, vm
            return PartialView("/Views/Shared/Modal/_AssegnazioneModal.cshtml", assegnazioneModalViewModel);  //, vm
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
