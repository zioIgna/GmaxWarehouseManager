namespace Gmax.Models.Services.Magazzino
{
    public interface IMagazzinoService
    {
        Task<Entities.Magazzino> GetMagazzinoByCodeAsync(string code);
        Task<Entities.Magazzino> GetMagazzinoByNLancioAndNSottolancioAsync(int nroLancio, int nroSottolancio);
        Task<Entities.Magazzino> CreateMagazzinoFromNrolancioNrosottolancioAsync(int nroLancio, int nroSottolancio);
        Task<Entities.Magazzino> GetMagazzinoByIdAsync(int id);
    }
}