using Gmax.Data;
using Gmax.Models.Entities;
using Gmax.Models.Extensions;
using Gmax.Models.Interfaces;
using Gmax.Models.Services.Assegnazione;
using Gmax.Models.Services.Giacenza;
using Gmax.Models.Services.Magazzino;
using Gmax.Models.Services.OrdineProdCompCK;
using Gmax.Models.ViewModels.AssegnazioneModal;
using Gmax.Models.ViewModels.OrdineCK;
using Gmax.Models.ViewModels.OrdineProdCompCK;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Gmax.Models.Services.OrdineCK
{
    public class OrdineProduzioneCKService : IOrdineProduzioneCKService
    {
        private readonly GmaxDbContext context;
        private readonly IOrdineProdCompCKService ordineProdCompCKService;
        private readonly IGiacenzaService giacenzaService;
        private readonly IMagazzinoService magazzinoService;
        private readonly IAssegnazioneService assegnazioneService;

        public OrdineProduzioneCKService(
            GmaxDbContext _context,
            IOrdineProdCompCKService ordineProdCompCKService,
            IGiacenzaService giacenzaService,
            IMagazzinoService magazzinoService,
            IAssegnazioneService assegnazioneService)
        {
            this.context = _context;
            this.ordineProdCompCKService = ordineProdCompCKService;
            this.giacenzaService = giacenzaService;
            this.magazzinoService = magazzinoService;
            this.assegnazioneService = assegnazioneService;
        }

        public async Task<ICollection<OrdineProduzioneCK>> GetOrdineProduzioneCKListAsync()
        {
            IQueryable<OrdineProduzioneCK> query = context.OrdiniProduzioneCK
                .Include(o => o.ArtLancio);

            return await query.ToListAsync();
        }

        public async Task<OrdineProduzioneCKListViewModel> GetOrdineProdCKListViewModelAsync()
        {
            var ordini = await GetOrdineProduzioneCKListAsync();
            var ordiniProdCKListViewModel = new OrdineProduzioneCKListViewModel();
            ordiniProdCKListViewModel.Rows.AddRange(ordini.Select(o => o.AsRowListViewModel()));

            return ordiniProdCKListViewModel;
        }

        public async Task<OrdineProduzioneCK?> GetOrdineProduzioneCKByKeyAsync(int nroLancio, int nroSottolancio)
        {
            IQueryable<OrdineProduzioneCK> query = context.OrdiniProduzioneCK
                .Include(op => op.OrdineProdCompCKList)
                    .ThenInclude(opc => opc.Assegnazioni.OrderByDescending(a => a.DataAssegnazione))
                        //.ThenInclude(a => a.MagazzinoOrigine)
                        //    .ThenInclude(m => m.Giacenza)
                .Include(op => op.OrdineProdCompCKList)
                    .ThenInclude(opc => opc.Articolo)
                        .ThenInclude(a => a.GiacenzaList)  //.Where(g => g.CodMagazzino.Equals("MAG01")))
                            //.ThenInclude(g => g.Magazzino)
                .Include(op => op.OrdineProdCompCKList)
                    .ThenInclude(opc => opc.Articolo)
                        .ThenInclude(a => a.OrdiniAcqList)
                .Include(op => op.ArtLancio)
                //.Include(op => op.ArtComponenteList)
                //    .ThenInclude(a => a.OrdineProdCompCKList)
                //        .ThenInclude(opc => opc.Assegnazioni)
                .Where(op => op.NroLancio == nroLancio && op.NroSottolancio == nroSottolancio);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<OrdineProduzioneCKDetailViewModel> GetOrdineProduzioneCKDetailViewModelAsync(int nroLancio, int nroSottolancio)
        {
            var ordineProduzioneCK = await GetOrdineProduzioneCKByKeyAsync(nroLancio, nroSottolancio);
            if (ordineProduzioneCK == null)
            {
                throw new Exception($"Non è stato possibile recuperare l'ordine di produzione con riferimenti nroLancio: {nroLancio}, nroSottolancio: {nroSottolancio}");
            }
            ordineProduzioneCK = await ConditionallyInitializeOPAsync(nroLancio, nroSottolancio, ordineProduzioneCK);
            var ordineProduzioneCKDetailViewModel = ordineProduzioneCK.AsDetailViewModel();
            foreach (var opcDetView in ordineProduzioneCKDetailViewModel.OrdineProdCompCKList)
            {
                await ordineProdCompCKService.InitMagDestQtaDisp(opcDetView);
            }
            ordineProduzioneCKDetailViewModel = await CalculateGlobalValues(ordineProduzioneCKDetailViewModel);

            return ordineProduzioneCKDetailViewModel;
        }

        private async Task<OrdineProduzioneCKDetailViewModel> CalculateGlobalValues(OrdineProduzioneCKDetailViewModel ordineProduzioneCK)
        {
            foreach (var opc in ordineProduzioneCK.OrdineProdCompCKList)
            {
                await CalculateFabbisognoGlobale(opc);
                CalculateDisponibilitaGlobale(opc);
                CalculateOrdineAcquisto(opc);
                //await  CalculateDisponibilitaMagazzini(opc);
            }

            return ordineProduzioneCK;
        }

        private async Task CalculateFabbisognoGlobale(OrdineProdCompCKListViewModel opc)
        {
            var opcPianificatoList = await ordineProdCompCKService.GetPianificatoOpcListAsync(opc.TipoArticolo, opc.CodiceArticolo);
            opc.FabbisognoGlobale = opcPianificatoList.Sum(opc => opc.QtaPrevista);
        }

        private static void CalculateDisponibilitaGlobale(OrdineProdCompCKListViewModel opc)
        {
            var giacenza = opc.Articolo?.GiacenzaList?.SingleOrDefault(g => g.CodMagazzino.Equals("MAG01"));
            opc.DisponibilitaGlobale = giacenza == null ? 0 : giacenza.QtaGiacenza; // (decimal)giacenza?.QtaGiacenza;
        }

        private void CalculateOrdineAcquisto(OrdineProdCompCKListViewModel opc)
        {
            var ordineAcq = opc.Articolo?.OrdiniAcqList?.Where(o => o.DataConsegnaAcquisto >= DateTime.Today);
            if (ordineAcq != null && ordineAcq.Any())
            {
                opc.InOrdineAcquisto = ordineAcq.Sum(oa => oa.QtaOrdineFornitore);
            }
        }

        private async Task CalculateDisponibilitaMagazzini(OrdineProdCompCKListViewModel opc)
        {
            var giacenzaList = await giacenzaService.GetGiacenzaListByTipoArtCodArtAsync(opc.Articolo.TipoArticolo, opc.Articolo.CodiceArticolo);
            giacenzaList = FilterOutMagazzinoDestinazione(opc, giacenzaList);
            await CheckForMissingMagazziniAsync(giacenzaList);

            IEnumerable<Entities.Magazzino>? magazzinoList;
            var disponibilitaDict = new Dictionary<string, int>();
            if (giacenzaList.Any())
            {
                magazzinoList = giacenzaList.Select(g => g.Magazzino).Where(m => m?.TipoMagazzino == Enums.TipoMagazzino.Fisico);
                foreach (var magazzino in magazzinoList)
                {
                    var relevantAssegnazioneList = opc.Assegnazioni?
                        .Where(a =>
                            a.TipoArticolo.Equals(opc.TipoArticolo) &&
                            a.CodiceArticolo.Equals(opc.CodiceArticolo) &&
                            a.DataAssegnazione > magazzino.GiacenzaList.FirstOrDefault(
                                g => g.TipoArticolo.Equals(opc.TipoArticolo) &&
                                g.CodiceArticolo.Equals(opc.CodiceArticolo))?.DataInserimento);
                    int alreadyAssignedQuantity = relevantAssegnazioneList != null ? relevantAssegnazioneList.Sum(a => a.Quantita) : 0;
                    disponibilitaDict.Add(
                        magazzino.CodMagazzino,
                        magazzino.GiacenzaList.First(
                                g => g.TipoArticolo.Equals(opc.TipoArticolo) &&
                                g.CodiceArticolo.Equals(opc.CodiceArticolo)).QtaGiacenza - alreadyAssignedQuantity);
                }
            }
            if (disponibilitaDict.Count == 0)
            {
                disponibilitaDict.Add("---", 0);
            }

            SelectList disponibilitaMagazzini = new SelectList(disponibilitaDict.OrderByDescending(x => x.Key), "Value", "Key");

            opc.DisponibilitaMagazzini = disponibilitaMagazzini;
        }

        public async Task CalculateDisponibilitaMagazzini(AssegnazioneModalViewModel viewModel)
        {
            var giacenzaList = await giacenzaService.GetGiacenzaListByTipoArtCodArtAsync(viewModel.TipoArticolo, viewModel.CodiceArticolo);
            giacenzaList = FilterOutMagazzinoDestinazione(viewModel.MagazzinoDestinazione, giacenzaList);
            await CheckForMissingMagazziniAsync(giacenzaList);

            var assegnazioneList = await assegnazioneService.GetAssegnazioneListByCodartTipoartAsync(viewModel.TipoArticolo, viewModel.CodiceArticolo);

            IEnumerable<Entities.Magazzino>? magazzinoList;
            var disponibilitaDict = new Dictionary<string, int>();
            if (giacenzaList.Any())
            {
                magazzinoList = giacenzaList.Select(g => g.Magazzino).Where(m => m?.TipoMagazzino == Enums.TipoMagazzino.Fisico);
                foreach (var magazzino in magazzinoList)
                {
                    IEnumerable<AssegnazioneMagazzino>? relevantAssegnazioneList = assegnazioneService.GetRelevantAssegnazioneList(viewModel, assegnazioneList, magazzino);
                    int alreadyAssignedQuantity = assegnazioneService.CalculateAlreadyAssignedQuantity(relevantAssegnazioneList);
                    disponibilitaDict.Add(magazzino.CodMagazzino, magazzino.GiacenzaList.First(
                                g => g.TipoArticolo.Equals(viewModel.TipoArticolo) &&
                                g.CodiceArticolo.Equals(viewModel.CodiceArticolo)).QtaGiacenza - alreadyAssignedQuantity);
                }
            }
            if (disponibilitaDict.Count == 0)
            {
                disponibilitaDict.Add("---", 0);
            }

            SelectList disponibilitaMagazzini = new SelectList(disponibilitaDict.OrderByDescending(x => x.Key), "Value", "Key");

            viewModel.DisponibilitaMagazzini = disponibilitaMagazzini;
        }

        private List<ExpGiacenza> FilterOutMagazzinoDestinazione(OrdineProdCompCKListViewModel opc, List<ExpGiacenza> giacenzaList)
        {
            var filteredList = giacenzaList.Where(g => g.CodMagazzino != opc.MagazzinoDestinazione);
            return filteredList.ToList();
        }

        private List<ExpGiacenza> FilterOutMagazzinoDestinazione(string magazzinoDestinazione, List<ExpGiacenza> giacenzaList)
        {
            var filteredList = giacenzaList.Where(g => g.CodMagazzino != magazzinoDestinazione);
            return filteredList.ToList();
        }

        private async Task CheckForMissingMagazziniAsync(List<ExpGiacenza> giacenzaList)
        {
            foreach (var giacenza in giacenzaList.Where(giacenza => giacenza.Magazzino == null))
            {
                giacenza.Magazzino = await magazzinoService.GetMagazzinoByCodeAsync(giacenza.CodMagazzino);
            }
        }

        private async Task<OrdineProduzioneCK> ConditionallyInitializeOPAsync(int nroLancio, int nroSottolancio, OrdineProduzioneCK ordineProduzioneCK)
        {
            var isOrdineProduzioneInitialized = true;
            foreach (var opc in ordineProduzioneCK.OrdineProdCompCKList)
            {
                if (opc.Assegnazioni == null || !opc.Assegnazioni.Any())
                {
                    isOrdineProduzioneInitialized = false;
                    await InitializeAssegnazioni(nroLancio, nroSottolancio, opc);
                }
            }
            if (!isOrdineProduzioneInitialized)
            {
                ordineProduzioneCK = await GetOrdineProduzioneCKByKeyAsync(nroLancio, nroSottolancio);
                if (ordineProduzioneCK == null)
                {
                    throw new Exception($"Non è stato possibile recuperare l'ordine di produzione con riferimenti nroLancio: {nroLancio}, nroSottolancio: {nroSottolancio}");
                }
            }

            return ordineProduzioneCK;
        }

        private async Task InitializeAssegnazioni(int nroLancio, int nroSottolancio, Entities.OrdineProdCompCK opc)
        {
            if (opc.Assegnazioni == null)
            {
                opc.Assegnazioni = new List<Entities.AssegnazioneMagazzino> { };
            }
            Entities.AssegnazioneMagazzino primaAssegnazioneMagazzino = new Entities.AssegnazioneMagazzino();
            primaAssegnazioneMagazzino.NroLancio = nroLancio;
            primaAssegnazioneMagazzino.NroSottolancio = nroSottolancio;
            primaAssegnazioneMagazzino.CodiceArticolo = opc.CodiceArticolo;
            primaAssegnazioneMagazzino.TipoArticolo = opc.TipoArticolo;
            primaAssegnazioneMagazzino.Quantita = opc.QtaGiaScaricata;
            primaAssegnazioneMagazzino.SorgenteAssegnazione = Enums.SorgenteAssegnazione.FromSystem;
            primaAssegnazioneMagazzino.DataAssegnazione = DateTime.Now;
            primaAssegnazioneMagazzino.Delta = 0;

            await context.AssegnazioniMagazzino.AddAsync(primaAssegnazioneMagazzino);
            await context.SaveChangesAsync();
        }

        public async Task<Entities.OrdineProdCompCK> AddAssegnazioneMagazzinoToOrdineProdCompAsync(OrdineProdCompCKInlineInputViewModel opcInputModel)
        {
            Entities.OrdineProdCompCK currentOrdineProdCompCK = await GetCurrentOrdineProdCompCKAsync(opcInputModel);

            RemoveAssegnazioniBeforeLastAssegnazioneFromSystem(currentOrdineProdCompCK);

            CreateNewAssegnazioneFromUser(opcInputModel, currentOrdineProdCompCK);

            await context.SaveChangesAsync();

            Entities.OrdineProdCompCK updatedOrdineProdCompCK = await GetUpdatedOrdineProdCompCK(opcInputModel);

            return updatedOrdineProdCompCK;
        }

        private async Task<Entities.OrdineProdCompCK> GetCurrentOrdineProdCompCKAsync(OrdineProdCompCKInlineInputViewModel opcInputModel)
        {
            OrdineProduzioneCK? ordineProduzioneCK = await GetOrdineProduzioneCKByKeyAsync(opcInputModel.NroLancio, opcInputModel.NroSottolancio);
            if (ordineProduzioneCK == null)
            {
                throw new Exception($"Non è stato possibile individuare l'ordine di produzione con Nro Lancio: {opcInputModel.NroLancio} e Nro Sottolancio: {opcInputModel.NroSottolancio}");
            }
            Entities.OrdineProdCompCK? ordineProdCompCK = ordineProduzioneCK.OrdineProdCompCKList?.FirstOrDefault(opc => opc.TipoArticolo == opcInputModel.TipoArticolo && opc.CodiceArticolo == opcInputModel.CodiceArticolo);
            if (ordineProdCompCK == null)
            {
                throw new Exception($"Ordine di produzione componente non trovato, TipoArticolo: {opcInputModel.TipoArticolo}, CodiceArticolo: {opcInputModel.CodiceArticolo}, NumLancio: {opcInputModel.NroLancio}, NumSottolancio: {opcInputModel.NroSottolancio}");
            }

            return ordineProdCompCK;
        }

        private void RemoveAssegnazioniBeforeLastAssegnazioneFromSystem(Entities.OrdineProdCompCK ordineProdCompCK)
        {
            Entities.AssegnazioneMagazzino? lastAssegnazioneMagDaSistema = ordineProdCompCK.Assegnazioni?.OrderByDescending(a => a.DataAssegnazione).FirstOrDefault(a => a.SorgenteAssegnazione == Enums.SorgenteAssegnazione.FromSystem);
            if (lastAssegnazioneMagDaSistema != null)
            {
                var oldAssegnazioniMag = ordineProdCompCK.Assegnazioni?.Where(a => a.DataAssegnazione < lastAssegnazioneMagDaSistema.DataAssegnazione);
                if (oldAssegnazioniMag != null && oldAssegnazioniMag.Any())
                {
                    context.RemoveRange(oldAssegnazioniMag);
                }
            }
        }

        private static void CreateNewAssegnazioneFromUser(OrdineProdCompCKInlineInputViewModel opcInputModel, Entities.OrdineProdCompCK ordineProdCompCK)
        {
            Entities.AssegnazioneMagazzino? lastAssegnazioneMagazzino = ordineProdCompCK.Assegnazioni?.OrderByDescending(a => a.DataAssegnazione).FirstOrDefault();
            int quantitaPrecedente = 0;
            if (lastAssegnazioneMagazzino != null)
            {
                quantitaPrecedente = lastAssegnazioneMagazzino.Quantita;
            }

            Entities.AssegnazioneMagazzino assegnazioneMagazzino = new Entities.AssegnazioneMagazzino
            {
                TipoArticolo = opcInputModel.TipoArticolo,
                CodiceArticolo = opcInputModel.CodiceArticolo,
                NroLancio = opcInputModel.NroLancio,
                NroSottolancio = opcInputModel.NroSottolancio,
                DataAssegnazione = DateTime.Now,
                Quantita = opcInputModel.NuovaQuantitaAssegnazione,
                SorgenteAssegnazione = Enums.SorgenteAssegnazione.FromUser,
                Delta = opcInputModel.NuovaQuantitaAssegnazione - quantitaPrecedente
            };
            ordineProdCompCK.Assegnazioni.Add(assegnazioneMagazzino);
        }

        private async Task<Entities.OrdineProdCompCK> GetUpdatedOrdineProdCompCK(OrdineProdCompCKInlineInputViewModel opcInputModel)
        {
            OrdineProduzioneCK? updatedOrdineProduzioneCK = await GetOrdineProduzioneCKByKeyAsync(opcInputModel.NroLancio, opcInputModel.NroSottolancio);
            if (updatedOrdineProduzioneCK == null)
            {
                throw new Exception($"Non è stato possibile aggiornare la quantità per l'articolo con riferimenti TipoArticolo: {opcInputModel.TipoArticolo} e CodiceArticolo: {opcInputModel.CodiceArticolo}");
            }
            Entities.OrdineProdCompCK? updatedOrdineProdCompCK = updatedOrdineProduzioneCK.OrdineProdCompCKList.FirstOrDefault(opc => opc.TipoArticolo == opcInputModel.TipoArticolo && opc.CodiceArticolo == opcInputModel.CodiceArticolo);
            if (updatedOrdineProdCompCK == null)
            {
                throw new Exception($"Non è stato possibile recuperare la nuova quantità per l'articolo con riferimenti TipoArticolo: {opcInputModel.TipoArticolo} e CodiceArticolo: {opcInputModel.CodiceArticolo}");
            }

            return updatedOrdineProdCompCK;
        }

        public async Task<int> CreateAssegnazioneAsync()
        {
            throw new NotImplementedException();
        }
    }
}
