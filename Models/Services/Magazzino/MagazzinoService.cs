using Gmax.Data;
using Microsoft.EntityFrameworkCore;

namespace Gmax.Models.Services.Magazzino
{
    public class MagazzinoService : IMagazzinoService
    {
        private readonly GmaxDbContext _context;

        public MagazzinoService(GmaxDbContext context)
        {
            this._context = context;
        }

        public async Task<Entities.Magazzino> GetMagazzinoByCodeAsync(string code)
        {
            return await _context.Magazzino.FirstOrDefaultAsync(m => m.CodMagazzino.Equals(code));
        }

        public async Task<Entities.Magazzino> GetMagazzinoByNLancioAndNSottolancioAsync(int nroLancio, int nroSottolancio)
        {
            return await _context.Magazzino
                .FirstOrDefaultAsync(m => m.NroLancio == nroLancio && m.NroSottolancio == nroSottolancio);
        }
    }
}
