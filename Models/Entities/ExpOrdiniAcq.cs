using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Gmax.Models.Entities
{
    [PrimaryKey(nameof(TipoArticolo), nameof(CodiceArticolo), nameof(NroOrdineAcquisto), nameof(DataConsegnaAcquisto))]
    public class ExpOrdiniAcq
    {
        [MaxLength(3)]
        [Required]
        public string TipoArticolo { get; set; }

        [MaxLength(30)]
        [Required]
        public string CodiceArticolo { get; set; }
        public ArticoloCK Articolo { get; set; }

        [Required]
        public int NroOrdineAcquisto { get; set; }
        public int QtaOrdineFornitore { get; set; }
        public DateTime DataOrdineAcquisto { get; set; }
        [Required]
        public DateTime DataConsegnaAcquisto { get; set; }
        public DateTime DataInserimento {  get; set; }
    }
}
