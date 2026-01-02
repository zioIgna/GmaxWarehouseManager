using Gmax.Data;
using Gmax.Models.Entities;
using Gmax.Models.Interfaces;
using Gmax.Models.Services.Assegnazione;
using Gmax.Models.Services.Giacenza;
using Gmax.Models.Services.Magazzino;
using Gmax.Models.ViewModels.AssegnazioneModal;
using Gmax.Models.ViewModels.OrdineProdCompCK;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Expressions;

namespace Gmax.Models.Services.OrdineProdCompCK
{
    public class OrdineProdCompCKService : IOrdineProdCompCKService
    {
        private readonly GmaxDbContext context;
        private readonly IMagazzinoService magazzinoService;
        private readonly IGiacenzaService giacenzaService;

        public OrdineProdCompCKService(GmaxDbContext _context, IMagazzinoService magazzinoService, IGiacenzaService giacenzaService)
        {
            context = _context;
            this.magazzinoService = magazzinoService;
            this.giacenzaService = giacenzaService;
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

        //public async Task InitMagDestQtaDisp(Entities.OrdineProdCompCK ordineProdComp)
        //{
        //    Entities.Magazzino magazzino = await magazzinoService.GetMagazzinoByNLancioAndNSottolancioAsync(ordineProdComp.NroLancio, ordineProdComp.NroSottolancio);
        //    ExpGiacenza expGiacenza = await giacenzaService.GetGiacenzaByCodMagAndTipoArtAndCodArtAsync(magazzino.CodMagazzino, ordineProdComp.TipoArticolo, ordineProdComp.CodiceArticolo);
        //    //ordineProdComp.
        //}
        
        public async Task InitMagDestQtaDisp(IEntitaAssegnabile ordineProdComp)
        {
            Entities.Magazzino magazzino = await magazzinoService.GetMagazzinoByNLancioAndNSottolancioAsync(ordineProdComp.NroLancio, ordineProdComp.NroSottolancio);
            ExpGiacenza? expGiacenza = null;
            if (magazzino != null)
            {
                expGiacenza = await giacenzaService.GetGiacenzaByCodMagAndTipoArtAndCodArtAsync(magazzino.CodMagazzino, ordineProdComp.TipoArticolo, ordineProdComp.CodiceArticolo);
            }
            ordineProdComp.QtaDisponibileMagDestinazione = expGiacenza != null ? expGiacenza.QtaGiacenza : 0;
        }

        public async Task<int> GetQtaDispDaGiacenza(ITipoartCodartNrolancioNrosottolancio entita)
        {
            Entities.Magazzino magazzino = await magazzinoService.GetMagazzinoByNLancioAndNSottolancioAsync(entita.NroLancio, entita.NroSottolancio);
            ExpGiacenza? expGiacenza = null;
            if (magazzino != null)
            {
                expGiacenza = await giacenzaService.GetGiacenzaByCodMagAndTipoArtAndCodArtAsync(magazzino.CodMagazzino, entita.TipoArticolo, entita.CodiceArticolo);
            }
            return expGiacenza != null ? expGiacenza.QtaGiacenza : 0;
        }

        public async Task<int> GetQtaDispDaGiacenza(int magId, string tipoArt, string codArt)
        {
            Entities.Magazzino magazzino = await magazzinoService.GetMagazzinoByIdAsync(magId);
            ExpGiacenza? expGiacenza = null;
            if (magazzino != null)
            {
                expGiacenza = await giacenzaService.GetGiacenzaByCodMagAndTipoArtAndCodArtAsync(magazzino.CodMagazzino, tipoArt, codArt);
            }
            return expGiacenza != null ? expGiacenza.QtaGiacenza : 0;
        }
    }
}
