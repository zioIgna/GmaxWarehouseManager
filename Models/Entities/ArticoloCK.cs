using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Gmax.Models.Entities
{
    [PrimaryKey(nameof(TipoArticolo),nameof(CodiceArticolo))]
    public class ArticoloCK
    {
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
        public List<OrdineProduzioneCK> OrdineProduzioneLancioList { get; set; } = new List<OrdineProduzioneCK>();
        public List<OrdineProduzioneCK> OrdineProduzioneComponenteList { get; set; } = new List<OrdineProduzioneCK>();
        public List<OrdineProdCompCK> OrdineProdCompCKList { get; set; }
    }
}
