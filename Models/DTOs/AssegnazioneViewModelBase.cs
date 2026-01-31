using Gmax.Models.Interfaces;

namespace Gmax.Models.DTOs
{
    public class AssegnazioneViewModelBase : IAssegnazioneViewModelBase
    {
        public AssegnazioneViewModelBase(string nroLancio, string nroSottolancio, string tipoArticolo, string codArticolo, int prevAssegnazioneId, bool isRevertOperation)
        {
            this.NroLancio = nroLancio;
            this.NroSottolancio = nroSottolancio;
            this.TipoArticolo = tipoArticolo;
            this.CodArticolo = codArticolo;
            this.PrevAssegnazioneId = prevAssegnazioneId;
            this.IsRevertOperation = isRevertOperation;
        }

        public string NroLancio { get; set; }
        public string NroSottolancio { get; set; }
        public string TipoArticolo { get; set; }
        public string CodArticolo { get; set; }
        public int PrevAssegnazioneId { get; set; }
        public bool IsRevertOperation { get; set; }
    }
}
