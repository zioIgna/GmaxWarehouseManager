using Gmax.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Gmax.Models.Entities
{
    [Index(nameof(NroLancio), nameof(NroSottolancio), IsUnique = true)]
    public class Magazzino
    {
        public int Id { get; set; }
        private string _name;
        public string Name 
        {
            get => _name;
            set => _name = NroLancio.ToString() + "-" + NroSottolancio.ToString();
        }
        [Range(0, 99999999)]
        public int? NroLancio { get; set; }
        [Range(0, 999)]
        public int? NroSottolancio { get; set; }
        public OrdineProduzioneCK? OrdineProduzioneCK { get; set; }
        public DateTime DataAssegnazione { get; set; } = DateTime.Now;
        public StatoAttivo statoAttivo { get; set; } = StatoAttivo.Attivo;
        public TipoMagazzino TipoMagazzino { get; set; } = TipoMagazzino.Virtuale;
        public List<AssegnazioneMagazzino> AssegnazioneOrigineList { get; set; } = [];
        public List<AssegnazioneMagazzino> AssegnazioneDestinazioneList { get; set; } = [];
    }
}
