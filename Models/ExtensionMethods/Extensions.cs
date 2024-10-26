using Gmax.Models.Entities;
using Gmax.Models.ViewModels.OrdineCK;

namespace Gmax.Models.Extensions
{
    public static class Extensions
    {
        public static OrdineProduzioneCKRowListViewModel AsRowListViewModel(this OrdineProduzioneCK ordineProduzioneCK)
        {
            OrdineProduzioneCKRowListViewModel rowListViewModel = new OrdineProduzioneCKRowListViewModel();
            rowListViewModel.TipoArtLancio = ordineProduzioneCK.TipoArtLancio;
            rowListViewModel.CodArtLancio = ordineProduzioneCK.CodArtLancio;
            rowListViewModel.ArtLancio = ordineProduzioneCK.ArtLancio;
            rowListViewModel.NroLancio = ordineProduzioneCK.NroLancio;
            rowListViewModel.NroSottolancio = ordineProduzioneCK.NroSottolancio;
            rowListViewModel.Stato = ordineProduzioneCK.Stato;
            rowListViewModel.DataCreazione = ordineProduzioneCK.DataCreazione;
            rowListViewModel.DataPrevCons = ordineProduzioneCK.DataPrevCons;

            return rowListViewModel;
        }
    }
}
