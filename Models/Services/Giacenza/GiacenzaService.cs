using Gmax.Data;
using Gmax.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gmax.Models.Services.Giacenza
{
    public class GiacenzaService : IGiacenzaService
    {
        private readonly GmaxDbContext _context;

        public GiacenzaService(GmaxDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExpGiacenza>> GetGiacenzaListByTipoArtCodArtAsync(string tipoArt, string codArt)
        {
            var query = _context.ExpGiacenze
                .Include(g => g.Magazzino)
                .Where(g => g.TipoArticolo.Equals(tipoArt) && g.CodiceArticolo.Equals(codArt));

            return await query.ToListAsync();
        }
    }
}
