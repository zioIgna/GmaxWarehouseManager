using Gmax.Models.Entities;
using Gmax.Models.ViewModels.OrdineCK;

namespace Gmax.Models.Services.OrdineCK
{
    public interface IOrdineProduzioneCKService
    {
        Task<OrdineProduzioneCK?> GetOrdineProduzioneCKByKeyAsync(int nroLancio, int nroSottolancio);
        Task<ICollection<OrdineProduzioneCK>> GetOrdineProduzioneCKListAsync();
        Task<OrdineProduzioneCKListViewModel> GetOrdineProdCKListViewModelAsync();
        Task<OrdineProduzioneCKDetailViewModel> GetOrdineProduzioneCKDetailViewModelAsync(int nroLancio, int nroSottolancio);
    }
}