using Gmax.Data;
using Microsoft.EntityFrameworkCore;

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
    }
}
