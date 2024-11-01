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
            var ordineProduzioneCKDetailViewModel = ordineProduzioneCK.AsDetailViewModel();

            return ordineProduzioneCKDetailViewModel;
        }

        public async Task<OrdineProdCompCK> AddAssegnazioneMagazzinoToOrdineProdCompAsync(OrdineProdCompCKInlineInputViewModel opcInputModel)
        {
            OrdineProduzioneCK? ordineProduzioneCK = await GetOrdineProduzioneCKByKeyAsync(opcInputModel.NroLancio, opcInputModel.NroSottolancio);
            if (ordineProduzioneCK == null)
            {
                throw new Exception($"Non è stato possibile individuare l'ordine di produzione con Nro Lancio: {opcInputModel.NroLancio} e Nro Sottolancio: {opcInputModel.NroSottolancio}");
            }
            OrdineProdCompCK? ordineProdCompCK = ordineProduzioneCK.OrdineProdCompCKList?.FirstOrDefault(opc => opc.TipoArticolo == opcInputModel.TipoArticolo && opc.CodiceArticolo == opcInputModel.CodiceArticolo);
            if (ordineProdCompCK == null)
            {
                throw new Exception($"Ordine di produzione componente non trovato, TipoArticolo: {opcInputModel.TipoArticolo}, CodiceArticolo: {opcInputModel.CodiceArticolo}, NumLancio: {opcInputModel.NroLancio}, NumSottolancio: {opcInputModel.NroSottolancio}");
            }

            AssegnazioneMagazzino assegnazioneMagazzino = new AssegnazioneMagazzino
            {
                TipoArticolo = opcInputModel.TipoArticolo,
                CodiceArticolo = opcInputModel.CodiceArticolo,
                NroLancio = opcInputModel.NroLancio,
                NroSottolancio = opcInputModel.NroSottolancio,
                DataAssegnazione = DateTime.Now,
                Quantita = opcInputModel.NuovaQuantitaAssegnazione
            };
            ordineProdCompCK.Assegnazioni.Add(assegnazioneMagazzino);
            await context.SaveChangesAsync();

            OrdineProduzioneCK? updatedOrdineProduzioneCK = await GetOrdineProduzioneCKByKeyAsync(opcInputModel.NroLancio, opcInputModel.NroSottolancio);
            if (updatedOrdineProduzioneCK == null)
            {
                throw new Exception($"Non è stato possibile aggiornare la quantità per l'articolo con riferimenti TipoArticolo: {opcInputModel.TipoArticolo} e CodiceArticolo: {opcInputModel.CodiceArticolo}");
            }
            OrdineProdCompCK? updatedOrdineProdCompCK = updatedOrdineProduzioneCK.OrdineProdCompCKList.FirstOrDefault(opc => opc.TipoArticolo == opcInputModel.TipoArticolo && opc.CodiceArticolo == opcInputModel.CodiceArticolo);
            if (updatedOrdineProdCompCK == null)
            {
                throw new Exception($"Non è stato possibile recuperare la nuova quantità per l'articolo con riferimenti TipoArticolo: {opcInputModel.TipoArticolo} e CodiceArticolo: {opcInputModel.CodiceArticolo}");
            }

            return updatedOrdineProdCompCK;
        }
    }
}
