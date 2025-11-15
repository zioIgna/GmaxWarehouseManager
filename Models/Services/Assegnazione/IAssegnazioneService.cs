using Gmax.Models.Entities;

namespace Gmax.Models.Services.Assegnazione
{
    public interface IAssegnazioneService
    {
        Task<IEnumerable<AssegnazioneMagazzino>> GetAssegnazioneListByNrolancioNrosottolancioCodartTipoartAsync(int nroLancio, int nroSottolancio, string tipoArticolo, string codiceArticolo);
    }
}