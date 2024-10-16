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
        public DbSet<OrdineProduzione> OrdiniProduzione { get; set; }
        public DbSet<OrdineProdComp> OrdiniProdComp { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Articolo>()
                .HasMany(e => e.OrdineProduzioneLancioList)
                .WithOne(e => e.ArtLancio)
                .HasForeignKey(e => e.ArtLancioId)
                .HasPrincipalKey(e => e.Id);
            modelBuilder.Entity<Articolo>()
                .HasMany(e => e.OrdineProduzioneComponenteList)
                .WithMany(e => e.ArtComponenteList)
                .UsingEntity<OrdineProdComp>();
        }
    }
}
