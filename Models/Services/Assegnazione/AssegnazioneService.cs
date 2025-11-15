using Gmax.Data;
using Microsoft.EntityFrameworkCore;

namespace Gmax.Models.Services.Assegnazione
{
    public class AssegnazioneService : IAssegnazioneService
    {
        private readonly GmaxDbContext context;

        public AssegnazioneService(GmaxDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Entities.AssegnazioneMagazzino>> GetAssegnazioneListByNrolancioNrosottolancioCodartTipoartAsync(int nroLancio, int nroSottolancio, string tipoArticolo, string codiceArticolo)
        {
            var query = context.AssegnazioniMagazzino.Where(a => a.NroLancio == nroLancio
                && a.NroSottolancio == nroSottolancio
                && a.TipoArticolo.Equals(tipoArticolo)
                && a.CodiceArticolo.Equals(codiceArticolo));

            return await query.ToListAsync();
        }
    }
}
