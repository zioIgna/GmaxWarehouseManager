using Gmax.Models.Entities;

namespace Gmax.Models.Services.Giacenza
{
    public interface IGiacenzaService
    {
        Task<List<ExpGiacenza>> GetGiacenzaListByTipoArtCodArtAsync(string tipoArt, string codArt);
        Task<ExpGiacenza> GetGiacenzaByCodMagAndTipoArtAndCodArtAsync(string codMag, string tipoArt, string codArt);
    }
}