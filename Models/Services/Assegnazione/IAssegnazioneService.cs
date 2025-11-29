using Gmax.Models.Entities;
using Gmax.Models.ViewModels.OrdineProdCompCK;

namespace Gmax.Models.Services.Assegnazione
{
    public interface IAssegnazioneService
    {
        Task<IEnumerable<AssegnazioneMagazzino>> GetAssegnazioneListByNrolancioNrosottolancioCodartTipoartAsync(int nroLancio, int nroSottolancio, string tipoArticolo, string codiceArticolo);
        Task<AssegnazioneMagazzino> CreateAssegnazioneMagazzinoFromViewModelAsync(OrdineProdCompCKListViewModel model, int magazzinoSceltoId, int magazzinoDestinazioneId);
    }
}