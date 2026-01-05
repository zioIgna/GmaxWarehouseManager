using Gmax.Models.Entities;
using Gmax.Models.ViewModels.AssegnazioneModal;
using Gmax.Models.ViewModels.OrdineProdCompCK;

namespace Gmax.Models.Services.Assegnazione
{
    public interface IAssegnazioneService
    {
        Task<IEnumerable<AssegnazioneMagazzino>> GetAssegnazioneListByNrolancioNrosottolancioCodartTipoartAsync(int nroLancio, int nroSottolancio, string tipoArticolo, string codiceArticolo);

        Task<IEnumerable<AssegnazioneMagazzino>> GetAssegnazioneListByCodartTipoartAsync(string tipoArticolo, string codiceArticolo);
        Task<AssegnazioneMagazzino> CreateAssegnazioneMagazzinoFromViewModelAsync(AssegnazioneModalViewModel model, int magazzinoSceltoId, int magazzinoDestinazioneId);
        IEnumerable<AssegnazioneMagazzino>? FilterAssegnazioneListPerMagorigineDatainserimento(AssegnazioneModalViewModel viewModel, IEnumerable<AssegnazioneMagazzino>? assegnazioneList, Entities.Magazzino magazzino);
        IEnumerable<AssegnazioneMagazzino>? FilterAssegnazioneListPerMagdestinazioneDatainserimento(AssegnazioneModalViewModel viewModel, IEnumerable<AssegnazioneMagazzino> assegnazioneList, Entities.Magazzino magazzino);
        int CalculateAssignedQuantity(IEnumerable<AssegnazioneMagazzino>? relevantAssegnazioneList);
        Task<AssegnazioneMagazzino?> GetAssegnazioneMagazzinoByIdAsync(int assegnazioneId);
        Task<IEnumerable<AssegnazioneMagazzino>> GetAssegnazioneListByMagoriginidTipoartCodart(int magId, string tipoArticolo, string codiceArticolo);
        Task<IEnumerable<AssegnazioneMagazzino>> GetAssegnazioneListByMagdestidTipoartCodart(int magId, string tipoArticolo, string codiceArticolo);
        Task<IEnumerable<AssegnazioneMagazzino>> GetAssegnazioneListByMagdestcodTipoartCodart(string magCod, string tipoArticolo, string codiecArticolo);
    }
}