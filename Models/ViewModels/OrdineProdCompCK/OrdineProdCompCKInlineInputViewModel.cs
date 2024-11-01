using Gmax.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Gmax.Models.ViewModels.OrdineProdCompCK
{
    public class OrdineProdCompCKInlineInputViewModel
    {
        public int NroLancio { get; set; }
        public int NroSottolancio { get; set; }
        public int SeqOp { get; set; }
        public int SeqArt { get; set; }
        public string TipoArticolo { get; set; }
        public string CodiceArticolo { get; set; }
        public Entities.ArticoloCK? Articolo { get; set; }
        public decimal QtaPrevista { get; set; }
        public decimal QtaGiaScaricata { get; set; }

        public List<AssegnazioneMagazzino>? Assegnazioni { get; set; }
        public int NuovaQuantitaAssegnazione { get; set; }
    }
}
