using Gmax.Data;
using Gmax.Models.Entities;
using Gmax.Models.Interfaces;
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

        public IEnumerable<AssegnazioneMagazzino>? FilterAssegnazioneListPerMagorigineDatainserimento(AssegnazioneModalViewModel viewModel, IEnumerable<AssegnazioneMagazzino> assegnazioneList, Entities.Magazzino magazzino)
        {
            IEnumerable<AssegnazioneMagazzino>? filteredAssegnazioneList = assegnazioneList
                                    .Where(a =>
                                    //TODO: eliminare la condizione di equivalenza su TipoArticolo e CodiceArticolo:
                                        a.TipoArticolo.Equals(viewModel.TipoArticolo) &&
                                        a.CodiceArticolo.Equals(viewModel.CodiceArticolo) &&
                                        a.MagazzinoOrigineId.Equals(magazzino.Id));

            if (filteredAssegnazioneList != null && filteredAssegnazioneList.Any() && magazzino.GiacenzaList != null && magazzino.GiacenzaList.Any())
            {
                filteredAssegnazioneList = filteredAssegnazioneList?.Where(a =>
                    a.DataAssegnazione > magazzino.GiacenzaList
                        .OrderByDescending(a => a.DataInserimento)
                        .First(
                            TipoartCodartMatch(viewModel))?
                        .DataInserimento);
            }

            return filteredAssegnazioneList;
        }

        private static Func<ExpGiacenza, bool> TipoartCodartMatch(ITipoartCodart viewModel)
        {
            return g => g.TipoArticolo.Equals(viewModel.TipoArticolo) &&
                                        g.CodiceArticolo.Equals(viewModel.CodiceArticolo);
        }

        public IEnumerable<AssegnazioneMagazzino>? FilterAssegnazioneListPerMagdestinazioneDatainserimento(AssegnazioneModalViewModel viewModel, IEnumerable<AssegnazioneMagazzino> assegnazioneList, Entities.Magazzino magazzino)
        {
            IEnumerable<AssegnazioneMagazzino>? filteredAssegnazioneList = assegnazioneList
                        .Where(a =>
                            a.MagazzinoDestinazioneId.Equals(magazzino.Id));

            if (filteredAssegnazioneList != null && filteredAssegnazioneList.Any() && magazzino.GiacenzaList != null && magazzino.GiacenzaList.Any())
            {
                filteredAssegnazioneList = filteredAssegnazioneList?.Where(a =>
                    a.DataAssegnazione > magazzino.GiacenzaList
                        .OrderByDescending(a => a.DataInserimento)
                        .First(
                            TipoartCodartMatch(viewModel))?
                        .DataInserimento);
            }

            return filteredAssegnazioneList;
        }
        public int CalculateAssignedQuantity(IEnumerable<AssegnazioneMagazzino>? relevantAssegnazioneList)
        {
            return relevantAssegnazioneList != null ? relevantAssegnazioneList.Sum(a => a.Quantita) : 0;
        }

        public async Task<AssegnazioneMagazzino?> GetAssegnazioneMagazzinoByIdAsync(int assegnazioneId)
        {
            var query = context.AssegnazioniMagazzino.FirstOrDefaultAsync(a => a.Id == assegnazioneId);

            return await query;
        }

        public async Task<IEnumerable<AssegnazioneMagazzino>> GetAssegnazioneListByMagoriginidTipoartCodart(int magId, string tipoArticolo, string codiceArticolo)
        {
            var query = context.AssegnazioniMagazzino.
                Where(a => a.MagazzinoOrigineId == magId
                && a.TipoArticolo.Equals(tipoArticolo)
                && a.CodiceArticolo.Equals(codiceArticolo));

            return await query.ToListAsync();
        }
    }
}
