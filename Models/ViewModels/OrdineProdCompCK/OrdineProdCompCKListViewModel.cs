using Gmax.Models.Entities;
using Gmax.Models.ViewModels.AssegnazioneModal;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gmax.Models.ViewModels.OrdineProdCompCK
{
    public class OrdineProdCompCKListViewModel : IEntitaAssegnabile
    {
        public int NroLancio { get; set; }
        public int NroSottolancio { get; set; }
        public OrdineProduzioneCK? OrdineProduzioneCK { get; set; }
        public int SeqOp { get; set; }
        public int SeqArt { get; set; }
        public string TipoArticolo { get; set; }
        public string CodiceArticolo { get; set; }
        public Entities.ArticoloCK? Articolo { get; set; }
        public decimal QtaPrevista { get; set; }
        public decimal QtaGiaScaricata { get; set; }
        public decimal FabbisognoGlobale { get; set; }
        public decimal DisponibilitaGlobale { get; set; }
        public decimal InOrdineAcquisto { get; set; }
        public decimal QtaDisponibileMagDestinazione { get; set; }
        public decimal QtaVersamento {  get; set; }
        public string MagazzinoDestinazione { get => NroLancio + "-" + NroSottolancio; }
        public List<AssegnazioneMagazzino>? Assegnazioni { get; set; }
        public SelectList? DisponibilitaMagazzini { get; set; }
        public string MagazzinoOrigineSelezionato { get; set; }
    }
}
