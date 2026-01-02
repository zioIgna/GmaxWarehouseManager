using Gmax.Data;
using Gmax.Models.Entities;
using Gmax.Models.ExtensionMethods;
using Gmax.Models.Extensions;
using Gmax.Models.Services.ArticoloCK;
using Gmax.Models.Services.Assegnazione;
using Gmax.Models.Services.Giacenza;
using Gmax.Models.Services.Magazzino;
using Gmax.Models.Services.OrdineCK;
using Gmax.Models.Services.OrdineProdCompCK;
using Gmax.Models.Services.Validazione;
using Gmax.Models.ViewModels.AssegnazioneModal;
using Gmax.Models.ViewModels.OrdineProdCompCK;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        private readonly IAssegnazioneValidator validator;
        private readonly IGiacenzaService giacenzaSevice;

        public OrdineProduzioneCKsController(GmaxDbContext context, IOrdineProduzioneCKService ordineProduzioneCKService, IMagazzinoService magazzinoService, IArticoloCKService articoloCKService, IOrdineProdCompCKService ordineProdCompCKService, IAssegnazioneService assegnazioneService, IAssegnazioneValidator validator, IGiacenzaService giacenzaSevice)
        {
            _context = context;
            this.ordineProduzioneCKService = ordineProduzioneCKService;
            this.magazzinoService = magazzinoService;
            this.articoloCKService = articoloCKService;
            this.ordineProdCompCKService = ordineProdCompCKService;
            this.assegnazioneService = assegnazioneService;
            this.validator = validator;
            this.giacenzaSevice = giacenzaSevice;
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
        public async Task<IActionResult> AssegnaValoreAMagazzino(AssegnazioneModalViewModel model, CancellationToken cancellationToken)
        {
            var errors = await validator.ValidateAsync(model, cancellationToken);
            foreach (var (member, message) in errors)
            {
                ModelState.AddModelError(member, message);
            }

            if (!ModelState.IsValid)
            {
                return await EditModalAsync(model.NroLancio.ToString(), model.NroSottolancio.ToString(), model.TipoArticolo, model.CodiceArticolo, 0);
            }

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
            AssegnazioneMagazzino assegnazioneMag = await assegnazioneService.CreateAssegnazioneMagazzinoFromViewModelAsync(model, magazzinoScelto.Id, magazzinoDestinazione.Id);
            if (assegnazioneMag == null)
            {
                throw new DbUpdateException("Il versamento non è andato a buon fine.");
            }

            return await EditModalAsync(model.NroLancio.ToString(), model.NroSottolancio.ToString(), model.TipoArticolo, model.CodiceArticolo, assegnazioneMag.Id);
        }

        public async Task<IActionResult> OpenAssegnazioneModal()
        {
            return View();
        }

        public async Task<IActionResult> EditModalAsync(string nroLancio, string nroSottolancio, string tipoArticolo, string codArticolo, int prevAssegnazioneId)
        {
            AssegnazioneModalViewModel assegnazioneModalViewModel = new AssegnazioneModalViewModel();
            assegnazioneModalViewModel.NroLancio = int.Parse(nroLancio);
            assegnazioneModalViewModel.NroSottolancio = int.Parse(nroSottolancio);
            assegnazioneModalViewModel.TipoArticolo = tipoArticolo;
            assegnazioneModalViewModel.CodiceArticolo = codArticolo;
            assegnazioneModalViewModel.Articolo = await articoloCKService.GetArticoloCKByKeyAsync(tipoArticolo, codArticolo);
            await ordineProduzioneCKService.CalculateDisponibilitaMagazzini(assegnazioneModalViewModel);

            assegnazioneModalViewModel.SetDefaultMagDestCode();

            assegnazioneModalViewModel.QtaDisponibileMagDestinazione = (await assegnazioneService.GetAssegnazioneListByNrolancioNrosottolancioCodartTipoartAsync(
                int.Parse(nroLancio),
                int.Parse(nroSottolancio),
                tipoArticolo,
                codArticolo)).Sum(a => a.Quantita);

            assegnazioneModalViewModel.PreviousAssegnazione = prevAssegnazioneId;
            ModelState.Remove(nameof(assegnazioneModalViewModel.PreviousAssegnazione));

            return PartialView("/Views/Shared/Modal/_AssegnazioneModal.cshtml", assegnazioneModalViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> InvertiVersamento(int assegnazioneId)
        {
            AssegnazioneMagazzino assegnazione = await assegnazioneService.GetAssegnazioneMagazzinoByIdAsync(assegnazioneId);
            if (assegnazione == null)
            {
                throw new Exception("Non è stato possibile recuperare i valori dell'assegnazione con Id: " + assegnazioneId);
            }

            AssegnazioneModalViewModel assegnazioneModalViewModel = new AssegnazioneModalViewModel();
            assegnazioneModalViewModel.NroLancio = assegnazione.NroLancio;
            assegnazioneModalViewModel.NroSottolancio = assegnazione.NroSottolancio;
            assegnazioneModalViewModel.TipoArticolo = assegnazione.TipoArticolo;
            assegnazioneModalViewModel.CodiceArticolo = assegnazione.CodiceArticolo;
            assegnazioneModalViewModel.Articolo = await articoloCKService.GetArticoloCKByKeyAsync(assegnazione.TipoArticolo, assegnazione.CodiceArticolo);
            
            int disponibilitaNewMagOrigine = await ordineProduzioneCKService.CalculateDisponibilitaMagFromAssegnazioniAsync(assegnazione.NroLancio, assegnazione.NroSottolancio, assegnazione.TipoArticolo, assegnazione.CodiceArticolo);
            Magazzino newMagOrigine = await magazzinoService.GetMagazzinoByNLancioAndNSottolancioAsync(assegnazione.NroLancio, assegnazione.NroSottolancio);
            if (newMagOrigine == null)
            {
                throw new Exception($"Non è stato possibile recuperare il magazzino con nroLancio {assegnazione.NroLancio} e nroSottolancio {assegnazione.NroSottolancio}");
            }
            Dictionary<string, int> dict = new Dictionary<string, int>();
            dict.Add(newMagOrigine.CodMagazzino, disponibilitaNewMagOrigine);
            ordineProduzioneCKService.SetDisponibilitaMagByDictionary(dict, assegnazioneModalViewModel);

            Magazzino newMagDestinazione = await magazzinoService.GetMagazzinoByIdAsync(assegnazione.MagazzinoOrigineId);
            if (newMagDestinazione == null)
            {
                throw new Exception("Non è stato trovato il magazzino di origine del versamento con Id: " + assegnazione.MagazzinoOrigineId);
            }
            assegnazioneModalViewModel.MagazzinoDestinazione = newMagDestinazione.CodMagazzino;
            int disponibilitaNewMagDestDaGiacenza = await ordineProdCompCKService.GetQtaDispDaGiacenza(assegnazione.MagazzinoOrigineId, assegnazione.TipoArticolo, assegnazione.CodiceArticolo);
            IEnumerable<AssegnazioneMagazzino> assegnazioneList = await assegnazioneService.GetAssegnazioneListByMagoriginidTipoartCodart(assegnazione.MagazzinoOrigineId, assegnazione.TipoArticolo, assegnazione.CodiceArticolo);
            if (assegnazioneList.Any())
            {
                assegnazioneList = await FilterOutOldAssegnazioniAsync(assegnazioneList, assegnazione.MagazzinoOrigineId);
            }
            int qtaGiaAssegnata = assegnazioneList.Sum(a => a.Quantita);
            assegnazioneModalViewModel.QtaDisponibileMagDestinazione = disponibilitaNewMagDestDaGiacenza - qtaGiaAssegnata;

            assegnazioneModalViewModel.QtaVersamento = (decimal)assegnazione.Quantita;

            assegnazioneModalViewModel.PreviousAssegnazione = 0;
            ModelState.Remove(nameof(assegnazioneModalViewModel.PreviousAssegnazione));
            return PartialView("/Views/Shared/Modal/_ValorizzazioneRevert.cshtml", assegnazioneModalViewModel);
        }

        private async Task<IEnumerable<AssegnazioneMagazzino>> FilterOutOldAssegnazioniAsync(IEnumerable<AssegnazioneMagazzino> assegnazioneList, int magazzinoId)
        {
            Magazzino magazzino = await magazzinoService.GetMagazzinoByIdAsync(magazzinoId);
            if (magazzino == null)
            {
                throw new Exception("Non è stato possibile recuperare il magazzino con id: " + magazzinoId);
            }
            ExpGiacenza giacenza = await giacenzaSevice.GetGiacenzaByCodMagAndTipoArtAndCodArtAsync(magazzino.CodMagazzino, assegnazioneList.First().TipoArticolo, assegnazioneList.First().CodiceArticolo);
            return assegnazioneList.Where(a => a.DataAssegnazione > giacenza.DataInserimento);
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
