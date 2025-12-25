using Gmax.Models.Interfaces;

namespace Gmax.Models.ViewModels.AssegnazioneModal
{
    public interface IEntitaAssegnabile : ITipoartCodartNrolancioNrosottolancio
    {
        public decimal QtaDisponibileMagDestinazione { get; set; }
    }
}
