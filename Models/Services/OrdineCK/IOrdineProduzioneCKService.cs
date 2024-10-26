using Gmax.Models.Entities;
using Gmax.Models.ViewModels.OrdineCK;

namespace Gmax.Models.Services.OrdineCK
{
    public interface IOrdineProduzioneCKService
    {
        Task<OrdineProduzioneCK?> GetOrdineProduzioneCKByPKasync(int nroLancio, int nroSottolancio);
        Task<ICollection<OrdineProduzioneCK>> GetOrdineProduzioneCKListAsync();
        Task<OrdineProduzioneCKListViewModel> GetOrdineProdCKListViewModelAsync();
    }
}