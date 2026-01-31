using Gmax.Data;
using Gmax.Models.Entities;
using Gmax.Models.Extensions;
using Gmax.Models.Interfaces;
using Gmax.Models.Services.ArticoloCK;
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
        private readonly IArticoloCKService articoloCKService;

        public OrdineProduzioneCKService(
            GmaxDbContext _context,
            IOrdineProdCompCKService ordineProdCompCKService,
            IGiacenzaService giacenzaService,
            IMagazzinoService magazzinoService,
            IAssegnazioneService assegnazioneService,
            IArticoloCKService articoloCKService)
        {
            this.context = _context;
            this.ordineProdCompCKService = ordineProdCompCKService;
            this.giacenzaService = giacenzaService;
            this.magazzinoService = magazzinoService;
            this.assegnazioneService = assegnazioneService;
            this.articoloCKService = articoloCKService;
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
                    int alreadyAssignedQuantity = 0;
                    int incomingQuantity = 0;
                    if (assegnazioneList != null && assegnazioneList.Any())
                    {
                        IEnumerable<AssegnazioneMagazzino>? relevantAssegnazioneNegativaList = assegnazioneService.FilterAssegnazioneListPerMagorigineDatainserimento(viewModel, assegnazioneList, magazzino);
                        alreadyAssignedQuantity = assegnazioneService.CalculateAssignedQuantity(relevantAssegnazioneNegativaList);

                        IEnumerable<AssegnazioneMagazzino>? relevantAssegnazionePositivaList = assegnazioneService.FilterAssegnazioneListPerMagdestinazioneDatainserimento(viewModel, assegnazioneList, magazzino);
                        incomingQuantity = assegnazioneService.CalculateAssignedQuantity(relevantAssegnazionePositivaList);
                    }

                    disponibilitaDict.Add(magazzino.CodMagazzino, magazzino.GiacenzaList.First(
                                g => g.TipoArticolo.Equals(viewModel.TipoArticolo) &&
                                g.CodiceArticolo.Equals(viewModel.CodiceArticolo)).QtaGiacenza - alreadyAssignedQuantity + incomingQuantity);
                }
            }
            if (disponibilitaDict.Count == 0)
            {
                disponibilitaDict.Add("---", 0);
            }

            SetDisponibilitaMagByDictionary(disponibilitaDict, viewModel);
        }

        public void SetDisponibilitaMagByDictionary(Dictionary<string, int> dict, AssegnazioneModalViewModel viewModel)
        {
            SelectList disponibilitaMagazzini = new SelectList(dict.OrderByDescending(x => x.Key), "Value", "Key");
            viewModel.DisponibilitaMagazzini = disponibilitaMagazzini;
        }

        public async Task<int> CalculateDisponibilitaMagFromAssegnazioniAsync(int magId, string tipoArt, string codArt)
        {
            var assegnazioneList = await assegnazioneService.GetAssegnazioneListByMagdestidTipoartCodart(magId, tipoArt, codArt);
            return (int)assegnazioneList?.Sum(a => a.Quantita);
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
    
        public async Task<AssegnazioneModalViewModel> CreateAssegnazioneModalViewModelFromModelBaseAsync(IAssegnazioneViewModelBase viewModelBase)
        {
            AssegnazioneModalViewModel assegnazioneModalViewModel = new AssegnazioneModalViewModel();
            assegnazioneModalViewModel.NroLancio = int.Parse(viewModelBase.NroLancio);
            assegnazioneModalViewModel.NroSottolancio = int.Parse(viewModelBase.NroSottolancio);
            assegnazioneModalViewModel.TipoArticolo = viewModelBase.TipoArticolo;
            assegnazioneModalViewModel.CodiceArticolo = viewModelBase.CodArticolo;
            assegnazioneModalViewModel.Articolo = await articoloCKService.GetArticoloCKByKeyAsync(viewModelBase.TipoArticolo, viewModelBase.CodArticolo);

            AssegnazioneMagazzino? prevAssegnazione = null;
            if (viewModelBase.PrevAssegnazioneId != 0)
            {
                prevAssegnazione = await assegnazioneService.GetAssegnazioneMagazzinoByIdAsync(viewModelBase.PrevAssegnazioneId);
                if (prevAssegnazione == null)
                {
                    throw new Exception("Non è stato possibile recuperare l'assegnazione magazzino con id: " + viewModelBase.PrevAssegnazioneId);
                }
            }

            if (!viewModelBase.IsRevertOperation)
            {
                assegnazioneModalViewModel.SetDefaultMagDestCode();
            }
            else
            {
                string magDestCode = await assegnazioneService.GetMagOriginCodeFromAssegnazioneIdAsync(viewModelBase.PrevAssegnazioneId);
                assegnazioneModalViewModel.MagazzinoDestinazione = magDestCode;
            }

            if (!viewModelBase.IsRevertOperation)
            {
                await CalculateDisponibilitaMagazzini(assegnazioneModalViewModel);
            }
            else
            {
                Dictionary<string, int> disponibilitaMagazzinoOrigineDict = new Dictionary<string, int>();
                Entities.Magazzino magOrigine = await magazzinoService.GetMagazzinoByIdAsync(prevAssegnazione.MagazzinoDestinazioneId);
                ExpGiacenza giacenzaMagOrigine = await giacenzaService.GetGiacenzaByCodMagAndTipoArtAndCodArtAsync(assegnazioneModalViewModel.MagazzinoOrigineSelezionato, viewModelBase.TipoArticolo, viewModelBase.CodArticolo);
                int qtaDispMagOrigineDaGiacenza = giacenzaMagOrigine?.QtaGiacenza ?? 0;
                IEnumerable<AssegnazioneMagazzino> assegnazionePositivaListByArtMagorigin = await assegnazioneService.GetAssegnazioneListByMagdestidTipoartCodart(magOrigine.Id, viewModelBase.TipoArticolo, viewModelBase.CodArticolo);
                int qtaDispPositivaMagOrigine = assegnazionePositivaListByArtMagorigin?.Sum(a => a.Quantita) ?? 0;
                IEnumerable<AssegnazioneMagazzino> assegnazioneNegativaListByArtMagorigin = await assegnazioneService.GetAssegnazioneListByMagoriginidTipoartCodart(magOrigine.Id, viewModelBase.TipoArticolo, viewModelBase.CodArticolo);
                int qtaDispNegativaMagOrigine = assegnazioneNegativaListByArtMagorigin?.Sum(a => a.Quantita) ?? 0;
                int qtaDisponibileMagOrigine = qtaDispMagOrigineDaGiacenza + qtaDispPositivaMagOrigine - qtaDispNegativaMagOrigine;
                disponibilitaMagazzinoOrigineDict.Add(magOrigine.CodMagazzino, qtaDisponibileMagOrigine);
                SetDisponibilitaMagByDictionary(disponibilitaMagazzinoOrigineDict, assegnazioneModalViewModel);
            }

            Entities.Magazzino magDestinazione = await magazzinoService.GetMagazzinoByCodeAsync(assegnazioneModalViewModel.MagazzinoDestinazione);
            if (magDestinazione == null)
            {
                magDestinazione = await magazzinoService.CreateMagazzinoFromNrolancioNrosottolancioAsync(assegnazioneModalViewModel.NroLancio, assegnazioneModalViewModel.NroSottolancio);
                if (magDestinazione == null)
                {
                    throw new Exception($"Non è stato possibile creare un magazzino per il Nro Lancio {viewModelBase.NroLancio} e Nro Sottolancio {viewModelBase.NroSottolancio}");
                }
            }

            int qtaDisponibileDaGiacenza = 0;
            if (viewModelBase.PrevAssegnazioneId != 0)
            {
                qtaDisponibileDaGiacenza = (await giacenzaService.GetGiacenzaByCodMagAndTipoArtAndCodArtAsync(assegnazioneModalViewModel.MagazzinoDestinazione, viewModelBase.TipoArticolo, viewModelBase.CodArticolo)).QtaGiacenza;
            }
            IEnumerable<AssegnazioneMagazzino> assegnazionePositivaListByArtMagdest = await assegnazioneService.GetAssegnazioneListByMagdestcodTipoartCodart(assegnazioneModalViewModel.MagazzinoDestinazione, viewModelBase.TipoArticolo, viewModelBase.CodArticolo);
            int qtaDispPositivaMagDestinazione = assegnazionePositivaListByArtMagdest.Sum(a => a.Quantita);
            IEnumerable<AssegnazioneMagazzino> assegnazioneNegativaListByArtMagdest = await assegnazioneService.GetAssegnazioneListByMagoriginidTipoartCodart(magDestinazione.Id, viewModelBase.TipoArticolo, viewModelBase.CodArticolo);
            int qtaDispNegativaMagDestinazione = assegnazioneNegativaListByArtMagdest.Sum(a => a.Quantita);
            assegnazioneModalViewModel.QtaDisponibileMagDestinazione = qtaDisponibileDaGiacenza + qtaDispPositivaMagDestinazione - qtaDispNegativaMagDestinazione;

            assegnazioneModalViewModel.IsRevertOperation = viewModelBase.IsRevertOperation;

            assegnazioneModalViewModel.PreviousAssegnazione = viewModelBase.PrevAssegnazioneId;

            return assegnazioneModalViewModel;
        }

        public async Task<AssegnazioneModalViewModel> CreateAssegnazioneModalViewModelFromAssegnazioneidAsync(int assegnazioneId)
        {
            AssegnazioneMagazzino? assegnazione = await assegnazioneService.GetAssegnazioneMagazzinoByIdAsync(assegnazioneId);
            if (assegnazione == null)
            {
                throw new Exception("Non è stato possibile recuperare i valori dell'assegnazione con Id: " + assegnazioneId);
            }


            AssegnazioneModalViewModel assegnazioneModalViewModel = new AssegnazioneModalViewModel();
            assegnazioneModalViewModel.NroLancio = assegnazione.NroLancio;
            assegnazioneModalViewModel.NroSottolancio = assegnazione.NroSottolancio;
            assegnazioneModalViewModel.TipoArticolo = assegnazione.TipoArticolo;
            assegnazioneModalViewModel.CodiceArticolo = assegnazione.CodiceArticolo;
            assegnazioneModalViewModel.PrevAssegnazioneId = assegnazioneId;
            //assegnazioneModalViewModel.Articolo = await articoloCKService.GetArticoloCKByKeyAsync(assegnazione.TipoArticolo, assegnazione.CodiceArticolo);
            assegnazioneModalViewModel.IsRevertOperation = true;

            return await CreateAssegnazioneModalViewModelAsync(assegnazioneModalViewModel);
        }

        private async Task<IEnumerable<AssegnazioneMagazzino>> FilterOutOldAssegnazioniAsync(IEnumerable<AssegnazioneMagazzino> assegnazioneList, int magazzinoId)
        {
            Entities.Magazzino magazzino = await magazzinoService.GetMagazzinoByIdAsync(magazzinoId);
            if (magazzino == null)
            {
                throw new Exception("Non è stato possibile recuperare il magazzino con id: " + magazzinoId);
            }
            ExpGiacenza giacenza = await giacenzaService.GetGiacenzaByCodMagAndTipoArtAndCodArtAsync(magazzino.CodMagazzino, assegnazioneList.First().TipoArticolo, assegnazioneList.First().CodiceArticolo);
            return assegnazioneList.Where(a => a.DataAssegnazione > giacenza.DataInserimento);
        }
    
        public async Task<AssegnazioneModalViewModel> CreateAssegnazioneModalViewModelAsync(IAssegnazioneViewModelBase viewModelBase)
        {
            AssegnazioneModalViewModel assegnazioneModalViewModel = new AssegnazioneModalViewModel();
            assegnazioneModalViewModel.NroLancio = int.Parse(viewModelBase.NroLancio);
            assegnazioneModalViewModel.NroSottolancio = int.Parse(viewModelBase.NroSottolancio);
            assegnazioneModalViewModel.TipoArticolo = viewModelBase.TipoArticolo;
            assegnazioneModalViewModel.CodiceArticolo = viewModelBase.CodArticolo;
            assegnazioneModalViewModel.Articolo = await articoloCKService.GetArticoloCKByKeyAsync(viewModelBase.TipoArticolo, viewModelBase.CodArticolo);
            assegnazioneModalViewModel.IsRevertOperation = viewModelBase.IsRevertOperation;
            assegnazioneModalViewModel.PreviousAssegnazione = viewModelBase.PrevAssegnazioneId;

            AssegnazioneMagazzino? prevAssegnazione = null;
            if (!viewModelBase.IsRevertOperation)
            {
                #region Magazzini Origine
                await CalculateDisponibilitaMagazzini(assegnazioneModalViewModel);
                #endregion

                #region Magazzino Destinazione
                assegnazioneModalViewModel.SetDefaultMagDestCode();
                Entities.Magazzino magDestinazione = await magazzinoService.GetMagazzinoByCodeAsync(assegnazioneModalViewModel.MagazzinoDestinazione);
                if (magDestinazione == null)
                {
                    magDestinazione = await magazzinoService.CreateMagazzinoFromNrolancioNrosottolancioAsync(assegnazioneModalViewModel.NroLancio, assegnazioneModalViewModel.NroSottolancio);
                    if (magDestinazione == null)
                    {
                        throw new Exception($"Non è stato possibile creare un magazzino per il Nro Lancio {viewModelBase.NroLancio} e Nro Sottolancio {viewModelBase.NroSottolancio}");
                    }
                }

                ExpGiacenza giacenzaMagDest = await giacenzaService.GetGiacenzaByCodMagAndTipoArtAndCodArtAsync(assegnazioneModalViewModel.MagazzinoDestinazione, viewModelBase.TipoArticolo, viewModelBase.CodArticolo);
                int qtaDispMagDestDaGiacenza = giacenzaMagDest?.QtaGiacenza ?? 0;
                IEnumerable<AssegnazioneMagazzino> assegnazionePositivaList = await assegnazioneService.GetAssegnazioneListByMagdestidTipoartCodart(magDestinazione.Id, viewModelBase.TipoArticolo, viewModelBase.CodArticolo);
                int qtaDispMagDestPositiva = assegnazionePositivaList?.Sum(a => a.Quantita) ?? 0;
                IEnumerable<AssegnazioneMagazzino> assegnazioneNegativaList = await assegnazioneService.GetAssegnazioneListByMagoriginidTipoartCodart(magDestinazione.Id, viewModelBase.TipoArticolo, viewModelBase.CodArticolo);
                int qtaDispMagDestNegativa = assegnazioneNegativaList?.Sum(a => a.Quantita) ?? 0;
                int qtaDispMagDestOverall = qtaDispMagDestDaGiacenza + qtaDispMagDestPositiva - qtaDispMagDestNegativa;
                assegnazioneModalViewModel.QtaDisponibileMagDestinazione = qtaDispMagDestOverall;
                #endregion
            }
            else
            {
                prevAssegnazione = await assegnazioneService.GetAssegnazioneMagazzinoByIdAsync(viewModelBase.PrevAssegnazioneId);
                if (prevAssegnazione == null)
                {
                    throw new Exception("Non è stato possibile recuperare l'assegnazione magazzino con id: " + viewModelBase.PrevAssegnazioneId);
                }
                #region Magazzino Origine
                Entities.Magazzino magOrigine = await magazzinoService.GetMagazzinoByIdAsync(prevAssegnazione.MagazzinoDestinazioneId);
                Dictionary<string, int> disponibilitaMagazzinoOrigineDict = new Dictionary<string, int>();
                ExpGiacenza giacenzaMagOrigine = await giacenzaService.GetGiacenzaByCodMagAndTipoArtAndCodArtAsync(magOrigine.CodMagazzino, viewModelBase.TipoArticolo, viewModelBase.CodArticolo);
                int qtaDispMagOrigineDaGiacenza = giacenzaMagOrigine?.QtaGiacenza ?? 0;
                IEnumerable<AssegnazioneMagazzino> assegnazionePositivaListByArtMagorigin = await assegnazioneService.GetAssegnazioneListByMagdestidTipoartCodart(magOrigine.Id, viewModelBase.TipoArticolo, viewModelBase.CodArticolo);
                int qtaDispPositivaMagOrigine = assegnazionePositivaListByArtMagorigin?.Sum(a => a.Quantita) ?? 0;
                IEnumerable<AssegnazioneMagazzino> assegnazioneNegativaListByArtMagorigin = await assegnazioneService.GetAssegnazioneListByMagoriginidTipoartCodart(magOrigine.Id, viewModelBase.TipoArticolo, viewModelBase.CodArticolo);
                int qtaDispNegativaMagOrigine = assegnazioneNegativaListByArtMagorigin?.Sum(a => a.Quantita) ?? 0;
                int qtaDispMagOrigineOverall = qtaDispMagOrigineDaGiacenza + qtaDispPositivaMagOrigine - qtaDispNegativaMagOrigine;
                disponibilitaMagazzinoOrigineDict.Add(magOrigine.CodMagazzino, qtaDispMagOrigineOverall);
                SetDisponibilitaMagByDictionary(disponibilitaMagazzinoOrigineDict, assegnazioneModalViewModel);
                #endregion

                #region Magazzino Destinazione
                Entities.Magazzino magDest = await magazzinoService.GetMagazzinoByIdAsync(prevAssegnazione.MagazzinoOrigineId);
                if (magDest == null)
                {
                    throw new Exception("Non è stato possibile recuperare il magazzino con id: " + prevAssegnazione.MagazzinoOrigineId); //await assegnazioneService.GetMagOriginCodeFromAssegnazioneIdAsync(viewModelBase.prevAssegnazioneId);
                }
                assegnazioneModalViewModel.MagazzinoDestinazione = magDest.CodMagazzino;
                ExpGiacenza giacenzaMagDest = await giacenzaService.GetGiacenzaByCodMagAndTipoArtAndCodArtAsync(magDest.CodMagazzino, viewModelBase.TipoArticolo, viewModelBase.CodArticolo);
                int qtaDispMagDestDaGiacenza = giacenzaMagDest?.QtaGiacenza ?? 0;
                IEnumerable<AssegnazioneMagazzino> assegnazionePositivaListByArtMagdest = await assegnazioneService.GetAssegnazioneListByMagdestidTipoartCodart(magDest.Id, viewModelBase.TipoArticolo, viewModelBase.CodArticolo);
                int qtaDispPositivaMagDest = assegnazionePositivaListByArtMagdest?.Sum(a => a.Quantita) ?? 0;
                IEnumerable<AssegnazioneMagazzino> assegnazioneNegativaListByArtMagdest = await assegnazioneService.GetAssegnazioneListByMagoriginidTipoartCodart(magDest.Id, viewModelBase.TipoArticolo, viewModelBase.CodArticolo);
                int qtaDispNegativaMagDest = assegnazioneNegativaListByArtMagdest?.Sum(a => a.Quantita) ?? 0;
                int qtaDispMagDestOverall = qtaDispMagDestDaGiacenza + qtaDispPositivaMagDest - qtaDispNegativaMagDest;
                assegnazioneModalViewModel.QtaDisponibileMagDestinazione = qtaDispMagDestOverall;
                #endregion
            }

            return assegnazioneModalViewModel;
        }
    }
}
