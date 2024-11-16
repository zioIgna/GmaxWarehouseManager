using Gmax.Models.Entities;
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
    }
}