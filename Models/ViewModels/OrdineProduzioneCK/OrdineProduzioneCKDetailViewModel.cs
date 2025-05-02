using Gmax.Models.Entities;
using Gmax.Models.ViewModels.OrdineProdCompCK;
using System.ComponentModel.DataAnnotations;

namespace Gmax.Models.ViewModels.OrdineCK
{
    public class OrdineProduzioneCKDetailViewModel
    {
        public int NroLancio { get; set; }
        public int NroSottolancio { get; set; }
        public string Stato { get; set; }
        public string TipoArtLancio { get; set; }
        public string CodArtLancio { get; set; }
        public Entities.ArticoloCK ArtLancio { get; set; } = null!;
        public DateTime DataCreazione { get; set; }
        public DateTime DataPrevCons { get; set; }
        public List<Entities.ArticoloCK> ArtComponenteList { get; set; } = [];
        public List<OrdineProdCompCKListViewModel> OrdineProdCompCKList { get; set; } = [];
    }
}
