using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Gmax.Models.Entities
{
    [PrimaryKey(nameof(NroLancio), nameof(NroSottolancio))]
    public class OrdineProduzioneCK
    {
        [Range(0, 99999999)]
        public int NroLancio { get; set; }
        [Range(0, 999)]
        public int NroSottolancio { get; set; }

        [StringLength(6, ErrorMessage = "Il valore del campo {0} non può superare i {1} caratteri. ")]
        public string Stato { get; set; }

        [StringLength(3)]
        [Required]
        public string TipoArtLancio { get; set; }

        [MaxLength(30)]
        [Required]
        public string CodArtLancio { get; set; }
        //public int ArtLancioId { get; set; }
        public ArticoloCK ArtLancio { get; set; } = null!;
        public DateTime DataCreazione { get; set; }
        public DateTime DataPrevCons { get; set; }

        //public List<Articolo> ArtLancioList { get; set; } = new List<Articolo>();
        public List<ArticoloCK> ArtComponenteList { get; set; } = [];
        public List<OrdineProdCompCK> OrdineProdCompCKList { get; } = [];
    }
}
