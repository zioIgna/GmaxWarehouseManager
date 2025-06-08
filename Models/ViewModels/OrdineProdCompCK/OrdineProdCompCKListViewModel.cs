using Gmax.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gmax.Models.ViewModels.OrdineProdCompCK
{
    public class OrdineProdCompCKListViewModel
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
        public List<AssegnazioneMagazzino>? Assegnazioni { get; set; }
        public SelectList? DisponibilitaMagazzini { get; set; }
    }
}
