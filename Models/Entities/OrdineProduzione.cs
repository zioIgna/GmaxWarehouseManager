using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Gmax.Models.Entities
{
    [Index(nameof(NroLancio),nameof(NroSottolancio),IsUnique = true)]
    public class OrdineProduzione
    {
        public int Id { get; set; }
        [Range(0, 99999999)]
        public int NroLancio { get; set; }
        [Range(0, 999)]
        public int NroSottolancio { get; set; }

        [StringLength(6, ErrorMessage = "Il valore del campo {0} non può superare i {1} caratteri. ")]
        public required string Stato { get; set; }

        //[StringLength(3)]
        //public required string TipoArtLancio { get; set; }
        //public int CodArtLancio { get; set; }
        public int ArtLancioId { get; set; }
        public Articolo ArtLancio { get; set; } = null!;
        public DateTime DataCreazione { get; set; }
        public DateTime DataPrevCons { get; set; }

        //public List<Articolo> ArtLancioList { get; set; } = new List<Articolo>();
        public List<Articolo> ArtComponenteList { get; set; } = [];
    }
}
