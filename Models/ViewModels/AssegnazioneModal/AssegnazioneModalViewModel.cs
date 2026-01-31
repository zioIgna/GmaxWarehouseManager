using Gmax.Models.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Gmax.Models.ViewModels.AssegnazioneModal
{
    public class AssegnazioneModalViewModel : IAssegnazioneViewModelBase, IEntitaAssegnabile
    {
        public int NroLancio { get; set; }
        public int NroSottolancio { get; set; }
        public string TipoArticolo { get; set; }
        public string CodiceArticolo { get; set; }
        public Entities.ArticoloCK? Articolo { get; set; }
        public SelectList? DisponibilitaMagazzini { get; set; }
        public string MagazzinoOrigineSelezionato { get; set; }
        public string MagazzinoDestinazione { get; set; }
        public decimal QtaDisponibileMagDestinazione { get; set; }
        public decimal QtaVersamento { get; set; }
        public int PreviousAssegnazione { get; set; }
        public bool IsRevertOperation { get; set; } = false;
        public string CodArticolo { get => CodiceArticolo; set => CodiceArticolo = value; }
        public int PrevAssegnazioneId { get => PreviousAssegnazione; set => PreviousAssegnazione = value; }
        string IAssegnazioneViewModelBase.NroLancio { get => NroLancio.ToString(); set => NroLancio = int.Parse(value); }
        string IAssegnazioneViewModelBase.NroSottolancio { get => NroSottolancio.ToString(); set => NroSottolancio = int.Parse(value); }

        public void SetDefaultMagDestCode()
        {
            MagazzinoDestinazione = this.NroLancio + "-" + this.NroSottolancio;
        }
    }
}
