using Gmax.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gmax.Data
{
    public class GmaxDbContext : DbContext
    {
        public GmaxDbContext(DbContextOptions<GmaxDbContext> options) : base(options) { }

        public GmaxDbContext()
        {
            
        }

        public DbSet<Articolo> Articoli { get; set; }
    }
}
