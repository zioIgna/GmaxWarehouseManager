using Gmax.Data;
using Gmax.Models.Entities;
using Gmax.Models.Extensions;
using Gmax.Models.ViewModels.OrdineCK;
using Gmax.Models.ViewModels.OrdineProdCompCK;
using Microsoft.EntityFrameworkCore;

namespace Gmax.Models.Services.OrdineCK
{
    public class OrdineProduzioneCKService : IOrdineProduzioneCKService
    {
        private readonly GmaxDbContext context;

        public OrdineProduzioneCKService(GmaxDbContext _context)
        {
            this.context = _context;
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
                .Include(op => op.OrdineProdCompCKList)
                    .ThenInclude(opc => opc.Articolo)
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

            return ordineProduzioneCKDetailViewModel;
        }

        private async Task<OrdineProduzioneCK> ConditionallyInitializeOPAsync(int nroLancio, int nroSottolancio, OrdineProduzioneCK? ordineProduzioneCK)
        {
            var isOrdineProduzioneInitialized = true;
            foreach (var opc in ordineProduzioneCK!.OrdineProdCompCKList)
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

            Entities.AssegnazioneMagazzino lastAssegnazioneMagDaSistema = ordineProdCompCK.Assegnazioni.OrderByDescending(a => a.DataAssegnazione).FirstOrDefault(a => a.SorgenteAssegnazione == Enums.SorgenteAssegnazione.FromSystem);
            if (lastAssegnazioneMagDaSistema != null)
            {
                var oldAssegnazioniMag = ordineProdCompCK.Assegnazioni.Where(a => a.DataAssegnazione < lastAssegnazioneMagDaSistema.DataAssegnazione);
                if (oldAssegnazioniMag != null && oldAssegnazioniMag.Any())
                {
                    context.RemoveRange(oldAssegnazioniMag);
                }
            }

            Entities.AssegnazioneMagazzino lastAssegnazioneMagazzino = ordineProdCompCK.Assegnazioni.OrderByDescending(a => a.DataAssegnazione).FirstOrDefault();
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
            await context.SaveChangesAsync();

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
    }
}
