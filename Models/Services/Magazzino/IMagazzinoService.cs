





namespace Gmax.Models.Services.Magazzino
{
    public interface IMagazzinoService
    {
        Task<Entities.Magazzino> GetMagazzinoByCodeAsync(string code);
    }
}