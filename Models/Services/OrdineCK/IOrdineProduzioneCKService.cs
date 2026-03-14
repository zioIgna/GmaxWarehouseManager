using Gmax.Models.Entities;
using Gmax.Models.Interfaces;
using Gmax.Models.ViewModels.AssegnazioneModal;
using Gmax.Models.ViewModels.OrdineCK;
using Gmax.Models.ViewModels.OrdineProdCompCK;

namespace Gmax.Models.Services.OrdineCK
{
    public interface IOrdineProduzioneCKService
    {
        Task<OrdineProduzioneCK?> GetOrdineProduzioneCKByKeyAsync(int nroLancio, int nroSottolancio);
        Task<ICollection<OrdineProduzioneCK>> GetOrdineProduzioneCKListAsync();
        Task<OrdineProduzioneCKListViewModel> GetOrdineProdCKListViewModelAsync();
        Task<OrdineProduzioneCKDetailViewModel> GetOrdineProduzioneCKDetailViewModelAsync(int nroLancio, int nroSottolancio);
        Task<Entities.OrdineProdCompCK> AddAssegnazioneMagazzinoToOrdineProdCompAsync(OrdineProdCompCKInlineInputViewModel opcInputModel);
        Task CalculateDisponibilitaMagazzini(AssegnazioneModalViewModel viewModel);
        void SetDisponibilitaMagByDictionary(Dictionary<string, int> dict, AssegnazioneModalViewModel viewModel);
        Task<int> CalculateDisponibilitaMagFromAssegnazioniAsync(int magId, string tipoArt, string codArt);
        Task<AssegnazioneModalViewModel> CreateAssegnazioneModalViewModelFromModelBaseAsync(IAssegnazioneViewModelBase viewModelBase);
        Task<AssegnazioneModalViewModel> CreateAssegnazioneModalViewModelFromAssegnazioneidAsync(int assegnazioneId);
        Task<AssegnazioneModalViewModel> CreateAssegnazioneModalViewModelAsync(IAssegnazioneViewModelBase viewModelBase);
        Task<AssegnazioneModalViewModel> CreateAssegnazioneModalViewModelFromRevertAsync(AssegnazioneModalViewModel viewModelBase);
    }
}