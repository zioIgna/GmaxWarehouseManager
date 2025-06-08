using Gmax.Data;
using Gmax.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Expressions;

namespace Gmax.Models.Services.OrdineProdCompCK
{
    public class OrdineProdCompCKService : IOrdineProdCompCKService
    {
        private readonly GmaxDbContext context;

        public OrdineProdCompCKService(GmaxDbContext _context)
        {
            context = _context;
        }

        public async Task<Entities.OrdineProdCompCK?> GetOrdineProdCompCKByKeyAsync(int nroLancio, int nroSottolancio, string tipoArticolo, string codiceArticolo)
        {
            var query = context.OrdiniProdCompCK
                .Include(opc => opc.Assegnazioni.OrderByDescending(a => a.DataAssegnazione))
                .FirstOrDefaultAsync(opc => opc.NroLancio == nroLancio && opc.NroSottolancio == nroSottolancio && opc.TipoArticolo == tipoArticolo && opc.CodiceArticolo == codiceArticolo);

            return await query;
        }

        public async Task<Entities.AssegnazioneMagazzino?> GetLastAssegnazioneMagazzinoForOrdineProdCompCKAsync(int nroLancio, int nroSottolancio, string tipoArticolo, string codiceArticolo)
        {
            Entities.OrdineProdCompCK? ordineProdCompCK = await GetOrdineProdCompCKByKeyAsync(nroLancio, nroSottolancio, tipoArticolo, codiceArticolo);
            if (ordineProdCompCK == null)
            {
                throw new Exception("Non è stato possibile recuperare l'ordine di produzione comp richiesto");
            }
            Entities.AssegnazioneMagazzino? ultimaAssegnazioneMagazzino = ordineProdCompCK.Assegnazioni.FirstOrDefault();

            return ultimaAssegnazioneMagazzino;
        }

        private IQueryable<Entities.OrdineProdCompCK> GetOpcListByTipoAndCodiceArt(string tipoArticolo, string codArticolo)
        {
            var query = context.OrdiniProdCompCK
                .Include(opc => opc.OrdineProduzioneCK)
                .Where(opc => opc.TipoArticolo.Equals(tipoArticolo)
                    && opc.CodiceArticolo.Equals(codArticolo));

            return query;
        }

        public async Task<IEnumerable<Entities.OrdineProdCompCK>> GetPianificatoOpcListAsync(string tipoArticolo, string codArticolo)
        {
            IQueryable<Entities.OrdineProdCompCK> opcList = GetOpcListByTipoAndCodiceArt(tipoArticolo, codArticolo);

            return await FilterListByPredicateAsync(opcList, OrdineProdIsPianificato());
        }

        private async Task<IEnumerable<T>> FilterListByPredicateAsync<T>(IQueryable<T> collection, Expression<Func<T, bool>> predicate)
        {
            var filteredList = collection.Where(predicate);

            return await filteredList.ToListAsync();
        }

        private static Expression<Func<Entities.OrdineProdCompCK, bool>> OrdineProdIsPianificato()
        {
            return opc => opc.OrdineProduzioneCK.Stato.Equals("P");
        }
    }
}
