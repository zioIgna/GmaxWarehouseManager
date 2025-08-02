





namespace Gmax.Models.Services.Magazzino
{
    public interface IMagazzinoService
    {
        Task<Entities.Magazzino> GetMagazzinoByCodeAsync(string code);
        Task<Entities.Magazzino> GetMagazzinoByNLancioAndNSottolancioAsync(int nroLancio, int nroSottolancio);
    }
}