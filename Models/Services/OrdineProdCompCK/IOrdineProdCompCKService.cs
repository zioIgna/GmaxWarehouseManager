
using Gmax.Models.ViewModels.AssegnazioneModal;
using Gmax.Models.ViewModels.OrdineProdCompCK;

namespace Gmax.Models.Services.OrdineProdCompCK
{
    public interface IOrdineProdCompCKService
    {
        Task<Entities.OrdineProdCompCK?> GetOrdineProdCompCKByKeyAsync(int nroLancio, int nroSottolancio, string tipoArticolo, string codiceArticolo);
        Task<Entities.AssegnazioneMagazzino?> GetLastAssegnazioneMagazzinoForOrdineProdCompCKAsync(int nroLancio, int nroSottolancio, string tipoArticolo, string codiceArticolo);
        Task<IEnumerable<Entities.OrdineProdCompCK>> GetPianificatoOpcListAsync(string tipoArticolo, string codArticolo);
        //Task InitMagDestQtaDisp(Entities.OrdineProdCompCK ordineProdComp);
        Task InitMagDestQtaDisp(IEntitaAssegnabile ordineProdComp);
    }
}