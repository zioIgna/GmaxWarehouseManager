using Gmax.Models.Entities;
using Gmax.Models.ViewModels.OrdineProdCompCK;

namespace Gmax.Models.ExtensionMethods
{
    public static class OrdineProdCompCKExtensions
    {
        public static OrdineProdCompCKInlineInputViewModel AsInlineInputViewModel(this OrdineProdCompCK ordineProdCompCK)
        {
            OrdineProdCompCKInlineInputViewModel inlinenputViewModel = new OrdineProdCompCKInlineInputViewModel();
            inlinenputViewModel.NroLancio = ordineProdCompCK.NroLancio;
            inlinenputViewModel.NroSottolancio = ordineProdCompCK.NroSottolancio;
            inlinenputViewModel.SeqOp = ordineProdCompCK.SeqOp;
            inlinenputViewModel.SeqArt = ordineProdCompCK.SeqArt;
            inlinenputViewModel.TipoArticolo = ordineProdCompCK.TipoArticolo;
            inlinenputViewModel.CodiceArticolo = ordineProdCompCK.CodiceArticolo;
            inlinenputViewModel.Articolo = ordineProdCompCK.Articolo;
            inlinenputViewModel.QtaPrevista = ordineProdCompCK.QtaPrevista;
            inlinenputViewModel.QtaGiaScaricata = ordineProdCompCK.QtaGiaScaricata;
            inlinenputViewModel.Assegnazioni = ordineProdCompCK.Assegnazioni;

            return inlinenputViewModel;
        }
    }
}
