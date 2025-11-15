namespace Gmax.Models.ViewModels.AssegnazioneModal
{
    public interface IEntitaAssegnabile
    {
        public int NroLancio { get; set; }
        public int NroSottolancio { get; set; }
        public string TipoArticolo { get; set; }
        public string CodiceArticolo { get; set; }
        public decimal QtaDisponibileMagDestinazione { get; set; }
    }
}
