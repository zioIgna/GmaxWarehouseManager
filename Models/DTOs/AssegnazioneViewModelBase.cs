using Gmax.Models.Interfaces;

namespace Gmax.Models.DTOs
{
    public class AssegnazioneViewModelBase : IAssegnazioneViewModelBase
    {
        public AssegnazioneViewModelBase(string nroLancio, string nroSottolancio, string tipoArticolo, string codArticolo, int prevAssegnazioneId, decimal qtaVersamento, bool isRevertOperation)
        {
            this.NroLancio = nroLancio;
            this.NroSottolancio = nroSottolancio;
            this.TipoArticolo = tipoArticolo;
            this.CodArticolo = codArticolo;
            this.PrevAssegnazioneId = prevAssegnazioneId;
            this.QtaVersamento = qtaVersamento;
            this.IsRevertOperation = isRevertOperation;
        }

        public string NroLancio { get; set; }
        public string NroSottolancio { get; set; }
        public string TipoArticolo { get; set; }
        public string CodArticolo { get; set; }
        public int PrevAssegnazioneId { get; set; }
        public bool IsRevertOperation { get; set; }
        public decimal QtaVersamento { get; set; }
    }
}
