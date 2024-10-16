using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Gmax.Models.Entities
{
    //[PrimaryKey(nameof(CodiceArticolo), nameof(TipoArticolo))]
    [Index(nameof(TipoArticolo), nameof(CodiceArticolo), IsUnique = true)]
    public class Articolo
    {
        public int Id { get; set; }
        
        [MaxLength(3)]
        [Required]
        public string TipoArticolo { get; set; }
        
        [MaxLength(30)]
        [Required]
        public string CodiceArticolo { get; set; }

        [MaxLength(120)]
        public string? Descrizione { get; set; }

        [MaxLength(3)]
        public string? UnitaMisuraGestione { get; set; }
        
        [Precision(14, 4)]
        public decimal? QtaScortaMin { get; set; }

        [MaxLength(6)]
        public string? CodUbicazione { get; set; }

        [MaxLength(256)]
        public string? CodCostruttore { get; set; }

        public int? QtaImpCliente { get; set; }
        public DateTime? DataInserimento { get; set; }
        public List<OrdineProduzione> OrdineProduzioneLancioList { get; set; } = new List<OrdineProduzione>();
        public List<OrdineProduzione> OrdineProduzioneComponenteList { get; set; } = new List<OrdineProduzione>();

    }
}
