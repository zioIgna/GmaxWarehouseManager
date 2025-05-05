using Gmax.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Gmax.Models.Entities
{
    public class AssegnazioneMagazzino
    {
        public int Id { get; set; }

        [Range(0, 99999999)]
        public int NroLancio { get; set; }

        [Range(0, 999)]
        public int NroSottolancio { get; set; }

        [MaxLength(3)]
        [Required]
        public string TipoArticolo { get; set; }

        [MaxLength(30)]
        [Required]
        public string CodiceArticolo { get; set; }
        public OrdineProdCompCK OrdineProdCompCK { get; set; }
        public int MagazzinoOrigineId { get; set; }
        public Magazzino MagazzinoOrigine { get; set; }
        public int MagazzinoDestinazioneId { get; set; }
        public Magazzino MagazzinoDestinazione { get; set; }
        public DateTime DataAssegnazione { get; set; } = DateTime.Now;
        public int Quantita { get; set; }
        public int Delta { get; set; }
        public SorgenteAssegnazione SorgenteAssegnazione { get; set; }
    }
}
