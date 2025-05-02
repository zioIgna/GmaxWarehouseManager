using Gmax.Models.ViewModels.OrdineProdCompCK;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Gmax.Models.Entities
{
    public class OrdineProdCompCK
    {
        [Range(0, 99999999)]
        public int NroLancio { get; set; }
        [Range(0, 999)]
        public int NroSottolancio { get; set; }
        //public int OrdineProduzioneId { get; set; }
        public OrdineProduzioneCK OrdineProduzioneCK { get; set; } = null!;

        [Range(0, 999)]
        public int SeqOp { get; set; }
        
        [Range(0, 999)]
        public int SeqArt { get; set; }

        [MaxLength(3)]
        [Required]
        public string TipoArticolo { get; set; }

        [MaxLength(30)]
        [Required]
        public string CodiceArticolo { get; set; }
        //public int ArticoloId { get; set; }
        public ArticoloCK Articolo { get; set; } = null!;
        [Precision(14, 4)]
        public decimal QtaPrevista { get; set; }
        [Precision(14, 4)]
        public int QtaGiaScaricata { get; set; }

        public List<AssegnazioneMagazzino> Assegnazioni { get; set; }

        public OrdineProdCompCKListViewModel AsListViewModel()
        {
            OrdineProdCompCKListViewModel listViewModel = new();
            listViewModel.NroLancio = NroLancio;
            listViewModel.NroSottolancio = NroSottolancio;
            listViewModel.OrdineProduzioneCK = OrdineProduzioneCK;
            listViewModel.SeqOp = SeqOp;
            listViewModel.SeqArt = SeqArt;
            listViewModel.TipoArticolo = TipoArticolo;
            listViewModel.CodiceArticolo = CodiceArticolo;
            listViewModel.Articolo = Articolo;
            listViewModel.QtaPrevista = QtaPrevista;
            listViewModel.QtaGiaScaricata = QtaGiaScaricata;
            listViewModel.Assegnazioni = Assegnazioni;

            return listViewModel;
        }
    }
}
