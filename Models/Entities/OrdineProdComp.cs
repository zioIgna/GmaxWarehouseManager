using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gmax.Models.Entities
{
    public class OrdineProdComp
    {
        //public int Id { get; set; }

        //[Range(0, 99999999)]
        //public int NroLancio { get; set; }
        //[Range(0, 999)]
        //public int NroSottolancio { get; set; }
        public int OrdineProduzioneId { get; set; }
        public OrdineProduzione OrdineProduzione { get; set; } = null!;

        [Range(0, 999)]
        public int SeqOp { get; set; }
        [Range(0, 999)]
        public int SeqArt { get; set; }
        //[MaxLength(3)]
        //[Required]
        //public string TipoArticolo { get; set; }

        //[MaxLength(30)]
        //[Required]
        //public string CodiceArticolo { get; set; }
        public int ArticoloId { get; set; }
        public Articolo Articolo { get; set; } = null!;
        [Precision(14, 4)]
        public decimal QtaPrevista { get; set; }
        [Precision(14, 4)]
        public decimal QtaGiaScaricata { get; set; }
    }
}
