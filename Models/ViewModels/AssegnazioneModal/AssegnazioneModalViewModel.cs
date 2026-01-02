using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gmax.Models.ViewModels.AssegnazioneModal
{
    public class AssegnazioneModalViewModel : IEntitaAssegnabile
    {
        public int NroLancio { get; set; }
        public int NroSottolancio { get; set; }
        public string TipoArticolo { get; set; }
        public string CodiceArticolo { get; set; }
        public Entities.ArticoloCK? Articolo { get; set; }
        public SelectList? DisponibilitaMagazzini { get; set; }
        public string MagazzinoOrigineSelezionato { get; set; }
        public string MagazzinoDestinazione { get => NroLancio + "-" + NroSottolancio; }
        public decimal QtaDisponibileMagDestinazione { get; set; }
        public decimal QtaVersamento { get; set; }
        public int PreviousAssegnazione { get; set; }
        public bool IsRevertOperation { get; set; } = false;
    }
}
