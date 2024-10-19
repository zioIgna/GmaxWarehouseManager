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

        public DbSet<Articolo> ArticoliIntKey { get; set; }
        public DbSet<OrdineProduzione> OrdiniProduzioneIntKey { get; set; }
        public DbSet<OrdineProdComp> OrdiniProdCompIntKey { get; set; }
        public DbSet<ArticoloCK> ArticoliCK { get; set; }
        public DbSet<OrdineProduzioneCK> OrdiniProduzioneCK { get; set; }
        public DbSet<OrdineProdCompCK> OrdiniProdCompCK { get; set; }

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

            modelBuilder.Entity<ArticoloCK>()
                .HasMany(art => art.OrdineProduzioneLancioList)
                .WithOne(ord => ord.ArtLancio)
                .HasForeignKey(ord => new { ord.TipoArtLancio, ord.CodArtLancio });
            modelBuilder.Entity<ArticoloCK>()
                .HasMany(art => art.OrdineProduzioneComponenteList)
                .WithMany(ord => ord.ArtComponenteList)
                .UsingEntity<OrdineProdCompCK>(
                    r => r.HasOne<OrdineProduzioneCK>().WithMany().HasForeignKey(e => new { e.NroLancio, e.NroSottolancio }),//.HasPrincipalKey(e => new {e.NroLancio, e.NroSottolancio})
                    l => l.HasOne<ArticoloCK>().WithMany().HasForeignKey(e => new { e.TipoArticolo, e.CodiceArticolo })//.HasPrincipalKey(e => new {e.TipoArticolo, e.CodiceArticolo}),
                );
        }
    }
}
