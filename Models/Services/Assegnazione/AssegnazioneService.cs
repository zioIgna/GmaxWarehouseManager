using Gmax.Data;
using Gmax.Models.Entities;
using Gmax.Models.ViewModels.AssegnazioneModal;
using Gmax.Models.ViewModels.OrdineProdCompCK;
using Microsoft.EntityFrameworkCore;

namespace Gmax.Models.Services.Assegnazione
{
    public class AssegnazioneService : IAssegnazioneService
    {
        private readonly GmaxDbContext context;

        public AssegnazioneService(GmaxDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Entities.AssegnazioneMagazzino>> GetAssegnazioneListByNrolancioNrosottolancioCodartTipoartAsync(int nroLancio, int nroSottolancio, string tipoArticolo, string codiceArticolo)
        {
            var query = context.AssegnazioniMagazzino.Where(a => a.NroLancio == nroLancio
                && a.NroSottolancio == nroSottolancio
                && a.TipoArticolo.Equals(tipoArticolo)
                && a.CodiceArticolo.Equals(codiceArticolo));

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<AssegnazioneMagazzino>> GetAssegnazioneListByCodartTipoartAsync(string tipoArticolo, string codiceArticolo)
        {
            var query = context.AssegnazioniMagazzino.Where(a => 
                a.TipoArticolo.Equals(tipoArticolo)
                && a.CodiceArticolo.Equals(codiceArticolo));

            return await query.ToListAsync();
        }

        public async Task<AssegnazioneMagazzino> CreateAssegnazioneMagazzinoFromViewModelAsync(AssegnazioneModalViewModel model, int magazzinoSceltoId, int magazzinoDestinazioneId)
        {
            AssegnazioneMagazzino assegnazione = new();
            assegnazione.NroLancio = model.NroLancio;
            assegnazione.NroSottolancio = model.NroSottolancio;
            assegnazione.TipoArticolo = model.TipoArticolo;
            assegnazione.CodiceArticolo = model.CodiceArticolo;
            assegnazione.DataAssegnazione = DateTime.Now;
            assegnazione.Quantita = Decimal.ToInt32(model.QtaVersamento);
            assegnazione.MagazzinoOrigineId = magazzinoSceltoId;
            assegnazione.MagazzinoDestinazioneId = magazzinoDestinazioneId;

            context.Add(assegnazione);
            await context.SaveChangesAsync();

            return assegnazione;
        }

        public IEnumerable<AssegnazioneMagazzino>? GetRelevantAssegnazioneList(AssegnazioneModalViewModel viewModel, IEnumerable<AssegnazioneMagazzino>? assegnazioneList, Entities.Magazzino magazzino)
        {
            return assegnazioneList?
                                    .Where(a =>
                                        a.TipoArticolo.Equals(viewModel.TipoArticolo) &&
                                        a.CodiceArticolo.Equals(viewModel.CodiceArticolo) &&
                                        a.MagazzinoOrigineId.Equals(magazzino.Id) &&
                                        a.DataAssegnazione > magazzino.GiacenzaList.FirstOrDefault(
                                            g => g.TipoArticolo.Equals(viewModel.TipoArticolo) &&
                                            g.CodiceArticolo.Equals(viewModel.CodiceArticolo))?.DataInserimento);
        }

        public int CalculateAlreadyAssignedQuantity(IEnumerable<AssegnazioneMagazzino>? relevantAssegnazioneList)
        {
            return relevantAssegnazioneList != null ? relevantAssegnazioneList.Sum(a => a.Quantita) : 0;
        }
    }
}
