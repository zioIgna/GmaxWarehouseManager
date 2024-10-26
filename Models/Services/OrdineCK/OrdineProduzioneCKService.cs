﻿using Gmax.Data;
using Gmax.Models.Entities;
using Gmax.Models.Extensions;
using Gmax.Models.ViewModels.OrdineCK;
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
    }
}
