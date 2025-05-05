using Gmax.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Gmax.Models.Entities
{
    public class Magazzino
    {
        public int Id { get; set; }
        private string _name;
        public string Name 
        {
            get => _name;
            set => _name = NroLancio.ToString() + NroSottolancio.ToString();
        }
        [Range(0, 99999999)]
        public int NroLancio { get; set; }
        [Range(0, 999)]
        public int NroSottolancio { get; set; }
        public OrdineProduzioneCK? OrdineProduzioneCK { get; set; }
        public DateTime DataAssegnazione { get; set; } = DateTime.Now;
        public StatoAttivo statoAttivo { get; set; }
        public TipoMagazzino TipoMagazzino { get; set; }
        public IEnumerable<AssegnazioneMagazzino> AssegnazioneOrigineList { get; set; }
        public IEnumerable<AssegnazioneMagazzino> AssegnazioneDestinazioneList { get; set; }
    }
}
