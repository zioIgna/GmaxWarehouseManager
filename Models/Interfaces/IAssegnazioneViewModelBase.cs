namespace Gmax.Models.Interfaces
{
    public interface IAssegnazioneViewModelBase
    {
        public string NroLancio { get; set; }
        public string NroSottolancio { get; set; }
        public string TipoArticolo { get; set; }
        public string CodArticolo { get; set; }
        public int PrevAssegnazioneId { get; set; }
        public bool IsRevertOperation { get; set; }
    }
}
