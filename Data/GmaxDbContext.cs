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

        public DbSet<ArticoloCK> ArticoliCK { get; set; }
        public DbSet<OrdineProduzioneCK> OrdiniProduzioneCK { get; set; }
        public DbSet<OrdineProdCompCK> OrdiniProdCompCK { get; set; }
        public DbSet<AssegnazioneMagazzino> AssegnazioniMagazzino { get; set; }
        public DbSet<Magazzino> Magazzino { get; set; }
        public DbSet<ExpGiacenza> ExpGiacenze { get; set; }
        public DbSet<ExpOrdiniAcq> ExpOrdiniAcquisto { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ArticoloCK>()
                .HasMany(art => art.OrdineProduzioneLancioList)
                .WithOne(ord => ord.ArtLancio)
                .HasForeignKey(ord => new { ord.TipoArtLancio, ord.CodArtLancio });
            modelBuilder.Entity<ArticoloCK>()
                .HasMany(art => art.OrdineProduzioneComponenteList)
                .WithMany(ord => ord.ArtComponenteList)
                .UsingEntity<OrdineProdCompCK>(
                    r => r.HasOne<OrdineProduzioneCK>(opc => opc.OrdineProduzioneCK).WithMany(op => op.OrdineProdCompCKList).HasForeignKey(e => new { e.NroLancio, e.NroSottolancio }),//.HasPrincipalKey(e => new {e.NroLancio, e.NroSottolancio})
                    l => l.HasOne<ArticoloCK>(opc => opc.Articolo).WithMany(a => a.OrdineProdCompCKList).HasForeignKey(e => new { e.TipoArticolo, e.CodiceArticolo })//.HasPrincipalKey(e => new {e.TipoArticolo, e.CodiceArticolo}),
                );
            //modelBuilder.Entity<OrdineProduzioneCK>()
            //    .HasMany(op => op.OrdineProdCompCKList)
            //    .WithOne(opc => opc.OrdineProduzione).HasForeignKey(opc => new { opc.NroLancio, opc.NroSottolancio });
            modelBuilder.Entity<AssegnazioneMagazzino>()
                .HasOne(e => e.OrdineProdCompCK)
                .WithMany(e => e.Assegnazioni)
                .HasForeignKey(e => new { e.NroLancio, e.NroSottolancio, e.TipoArticolo, e.CodiceArticolo });

            modelBuilder.Entity<Magazzino>()
                .HasOne(m => m.OrdineProduzioneCK)
                .WithOne(o => o.Magazzino)
                .HasForeignKey<Magazzino>(m => new { m.NroLancio, m.NroSottolancio });

            modelBuilder.Entity<Magazzino>()
                .Property(m => m.CodMagazzino)
                .HasField("_codMagazzino");

            modelBuilder.Entity<ExpGiacenza>()
                .HasOne(g => g.Articolo)
                .WithMany(a => a.GiacenzaList)
                .HasForeignKey(g => new { g.TipoArticolo, g.CodiceArticolo });
            modelBuilder.Entity<ExpGiacenza>()
                .HasOne(g => g.Magazzino)
                .WithMany(m => m.GiacenzaList)
                .HasPrincipalKey(m => m.CodMagazzino)
                .HasForeignKey(g => g.CodMagazzino);

            modelBuilder.Entity<AssegnazioneMagazzino>()
                .HasOne(am => am.MagazzinoOrigine)
                .WithMany(m => m.AssegnazioneOrigineList)
                .HasForeignKey(am => am.MagazzinoOrigineId);
            modelBuilder.Entity<AssegnazioneMagazzino>()
                .HasOne(am => am.MagazzinoDestinazione)
                .WithMany(m => m.AssegnazioneDestinazioneList)
                .HasForeignKey(am => am.MagazzinoDestinazioneId);

            modelBuilder.Entity<ExpOrdiniAcq>()
                .HasOne(o => o.Articolo)
                .WithMany(a => a.OrdiniAcqList)
                .HasForeignKey(o => new { o.TipoArticolo, o.CodiceArticolo });
        }
    }
}
