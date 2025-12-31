using Gmax.Models.Entities;
using Gmax.Models.Services.Assegnazione;
using Gmax.Models.Services.Giacenza;
using Gmax.Models.Services.Magazzino;
using Gmax.Models.ViewModels.AssegnazioneModal;

namespace Gmax.Models.Services.Validazione
{
    public class AssegnazioneValidator : IAssegnazioneValidator
    {
        private readonly IMagazzinoService magazzinoService;
        private readonly IAssegnazioneService assegnazioneService;

        public AssegnazioneValidator(IMagazzinoService magazzinoService, IAssegnazioneService assegnazioneService)
        {
            this.magazzinoService = magazzinoService;
            this.assegnazioneService = assegnazioneService;
        }

        public async Task<IEnumerable<(string MemberName, string ErrorMessage)>> ValidateAsync(AssegnazioneModalViewModel model, CancellationToken cancellationToken = default)
        {
            var errors = new List<(string, string)>();

            if (model.MagazzinoOrigineSelezionato == null)
            {
                errors.Add((nameof(model.MagazzinoOrigineSelezionato), "Selezionare un magazzino sorgente."));
                return errors;
            }

            if (model.MagazzinoOrigineSelezionato.Equals("---"))
            {
                errors.Add((nameof(model.MagazzinoOrigineSelezionato), "Nessun magazzino dispone di questo articolo."));
                return errors;
            }

            if (model.QtaVersamento <= 0)
            {
                errors.Add((nameof(model.QtaVersamento), "La quantità deve essere maggiore di 0."));
                return errors;
            }

            var magazzino = await magazzinoService.GetMagazzinoByCodeAsync(model.MagazzinoOrigineSelezionato);
            if (magazzino == null)
            {
                errors.Add((nameof(model.MagazzinoOrigineSelezionato), "Non è stato possibile recuperare il magazzino selezionato."));
                return errors;
            }
            int qtaGiacenza = magazzino.GiacenzaList.First(
                    g => g.TipoArticolo.Equals(model.TipoArticolo) &&
                    g.CodiceArticolo.Equals(model.CodiceArticolo)).QtaGiacenza;

            var assegnazioneList = await assegnazioneService.GetAssegnazioneListByCodartTipoartAsync(model.TipoArticolo, model.CodiceArticolo);
            IEnumerable<AssegnazioneMagazzino>? relevantAssegnazioneList = assegnazioneService.GetRelevantAssegnazioneList(model, assegnazioneList, magazzino);
            int alreadyAssignedQuantity = assegnazioneService.CalculateAlreadyAssignedQuantity(relevantAssegnazioneList);

            int qtaDisp = qtaGiacenza - alreadyAssignedQuantity;

            if (model.QtaVersamento > qtaDisp)
            {
                errors.Add((nameof(model.QtaVersamento), "La quantità versata non può essere maggiore della quantità disponibile."));
            }

            return errors;
        }
    }
}
