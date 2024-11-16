using Gmax.Data;
using Microsoft.EntityFrameworkCore;

namespace Gmax.Models.Services.ArticoloCK
{
    public class ArticoloCKService : IArticoloCKService
    {
        private readonly GmaxDbContext context;

        public ArticoloCKService(GmaxDbContext _context)
        {
            context = _context;
        }

        public async Task<Entities.ArticoloCK?> GetArticoloCKByKeyAsync(string tipoArticolo, string codiceArticolo)
        {
            Task<Entities.ArticoloCK?> query = context.ArticoliCK.FirstOrDefaultAsync(a => a.TipoArticolo.Equals(tipoArticolo) && a.CodiceArticolo == codiceArticolo);

            return await query;
        }
    }
}
