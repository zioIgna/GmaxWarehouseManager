using Gmax.Models.Entities;
using Gmax.Models.ViewModels.OrdineCK;

namespace Gmax.Models.Extensions
{
    public static class OrdineProduzioneCKExtensions
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

        public static OrdineProduzioneCKDetailViewModel AsDetailViewModel(this OrdineProduzioneCK ordineProduzioneCK)
        {
            OrdineProduzioneCKDetailViewModel detailViewModel = new OrdineProduzioneCKDetailViewModel();
            detailViewModel.NroLancio = ordineProduzioneCK.NroLancio;
            detailViewModel.NroSottolancio = ordineProduzioneCK.NroSottolancio;
            detailViewModel.Stato = ordineProduzioneCK.Stato;
            detailViewModel.TipoArtLancio = ordineProduzioneCK.TipoArtLancio;
            detailViewModel.CodArtLancio = ordineProduzioneCK.CodArtLancio;
            detailViewModel.ArtLancio = ordineProduzioneCK.ArtLancio;
            detailViewModel.DataCreazione = ordineProduzioneCK.DataCreazione;
            detailViewModel.DataPrevCons = ordineProduzioneCK.DataPrevCons;
            detailViewModel.ArtComponenteList = ordineProduzioneCK.ArtComponenteList;
            detailViewModel.OrdineProdCompCKList = ordineProduzioneCK.OrdineProdCompCKList;

            return detailViewModel;
        }
    }
}
