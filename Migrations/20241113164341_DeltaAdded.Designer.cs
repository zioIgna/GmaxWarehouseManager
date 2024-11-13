﻿// <auto-generated />
using System;
using Gmax.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oracle.EntityFrameworkCore.Metadata;

#nullable disable

namespace Gmax.Migrations
{
    [DbContext(typeof(GmaxDbContext))]
    [Migration("20241113164341_DeltaAdded")]
    partial class DeltaAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            OracleModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Gmax.Models.Entities.Articolo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CodCostruttore")
                        .HasMaxLength(256)
                        .HasColumnType("NVARCHAR2(256)");

                    b.Property<string>("CodUbicazione")
                        .HasMaxLength(6)
                        .HasColumnType("NVARCHAR2(6)");

                    b.Property<string>("CodiceArticolo")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("NVARCHAR2(30)");

                    b.Property<DateTime?>("DataInserimento")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<string>("Descrizione")
                        .HasMaxLength(120)
                        .HasColumnType("NVARCHAR2(120)");

                    b.Property<int?>("QtaImpCliente")
                        .HasColumnType("NUMBER(10)");

                    b.Property<decimal?>("QtaScortaMin")
                        .HasPrecision(14, 4)
                        .HasColumnType("DECIMAL(14,4)");

                    b.Property<string>("TipoArticolo")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("NVARCHAR2(3)");

                    b.Property<string>("UnitaMisuraGestione")
                        .HasMaxLength(3)
                        .HasColumnType("NVARCHAR2(3)");

                    b.HasKey("Id");

                    b.HasIndex("TipoArticolo", "CodiceArticolo")
                        .IsUnique();

                    b.ToTable("ArticoliIntKey");
                });

            modelBuilder.Entity("Gmax.Models.Entities.ArticoloCK", b =>
                {
                    b.Property<string>("TipoArticolo")
                        .HasMaxLength(3)
                        .HasColumnType("NVARCHAR2(3)");

                    b.Property<string>("CodiceArticolo")
                        .HasMaxLength(30)
                        .HasColumnType("NVARCHAR2(30)");

                    b.Property<string>("CodCostruttore")
                        .HasMaxLength(256)
                        .HasColumnType("NVARCHAR2(256)");

                    b.Property<string>("CodUbicazione")
                        .HasMaxLength(6)
                        .HasColumnType("NVARCHAR2(6)");

                    b.Property<DateTime?>("DataInserimento")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<string>("Descrizione")
                        .HasMaxLength(120)
                        .HasColumnType("NVARCHAR2(120)");

                    b.Property<int?>("QtaImpCliente")
                        .HasColumnType("NUMBER(10)");

                    b.Property<decimal?>("QtaScortaMin")
                        .HasPrecision(14, 4)
                        .HasColumnType("DECIMAL(14,4)");

                    b.Property<string>("UnitaMisuraGestione")
                        .HasMaxLength(3)
                        .HasColumnType("NVARCHAR2(3)");

                    b.HasKey("TipoArticolo", "CodiceArticolo");

                    b.ToTable("ArticoliCK");
                });

            modelBuilder.Entity("Gmax.Models.Entities.AssegnazioneMagazzino", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CodiceArticolo")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("NVARCHAR2(30)");

                    b.Property<DateTime>("DataAssegnazione")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<int>("Delta")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("NroLancio")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("NroSottolancio")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("Quantita")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("SorgenteAssegnazione")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("TipoArticolo")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("NVARCHAR2(3)");

                    b.HasKey("Id");

                    b.HasIndex("NroLancio", "NroSottolancio", "TipoArticolo", "CodiceArticolo");

                    b.ToTable("AssegnazioniMagazzino");
                });

            modelBuilder.Entity("Gmax.Models.Entities.OrdineProdComp", b =>
                {
                    b.Property<int>("ArticoloId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("OrdineProduzioneId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<decimal>("QtaGiaScaricata")
                        .HasPrecision(14, 4)
                        .HasColumnType("DECIMAL(14,4)");

                    b.Property<decimal>("QtaPrevista")
                        .HasPrecision(14, 4)
                        .HasColumnType("DECIMAL(14,4)");

                    b.Property<int>("SeqArt")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("SeqOp")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("ArticoloId", "OrdineProduzioneId");

                    b.HasIndex("OrdineProduzioneId");

                    b.ToTable("OrdiniProdCompIntKey");
                });

            modelBuilder.Entity("Gmax.Models.Entities.OrdineProdCompCK", b =>
                {
                    b.Property<int>("NroLancio")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("NroSottolancio")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("TipoArticolo")
                        .HasMaxLength(3)
                        .HasColumnType("NVARCHAR2(3)");

                    b.Property<string>("CodiceArticolo")
                        .HasMaxLength(30)
                        .HasColumnType("NVARCHAR2(30)");

                    b.Property<int>("QtaGiaScaricata")
                        .HasPrecision(14, 4)
                        .HasColumnType("NUMBER(14,4)");

                    b.Property<decimal>("QtaPrevista")
                        .HasPrecision(14, 4)
                        .HasColumnType("DECIMAL(14,4)");

                    b.Property<int>("SeqArt")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("SeqOp")
                        .HasColumnType("NUMBER(10)");

                    b.HasKey("NroLancio", "NroSottolancio", "TipoArticolo", "CodiceArticolo");

                    b.HasIndex("TipoArticolo", "CodiceArticolo");

                    b.ToTable("OrdiniProdCompCK");
                });

            modelBuilder.Entity("Gmax.Models.Entities.OrdineProduzione", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("NUMBER(10)");

                    OraclePropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ArtLancioId")
                        .HasColumnType("NUMBER(10)");

                    b.Property<DateTime>("DataCreazione")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<DateTime>("DataPrevCons")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<int>("NroLancio")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("NroSottolancio")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("Stato")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("NVARCHAR2(6)");

                    b.HasKey("Id");

                    b.HasIndex("ArtLancioId");

                    b.HasIndex("NroLancio", "NroSottolancio")
                        .IsUnique();

                    b.ToTable("OrdiniProduzioneIntKey");
                });

            modelBuilder.Entity("Gmax.Models.Entities.OrdineProduzioneCK", b =>
                {
                    b.Property<int>("NroLancio")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("NroSottolancio")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("CodArtLancio")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("NVARCHAR2(30)");

                    b.Property<DateTime>("DataCreazione")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<DateTime>("DataPrevCons")
                        .HasColumnType("TIMESTAMP(7)");

                    b.Property<string>("Stato")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("NVARCHAR2(6)");

                    b.Property<string>("TipoArtLancio")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("NVARCHAR2(3)");

                    b.HasKey("NroLancio", "NroSottolancio");

                    b.HasIndex("TipoArtLancio", "CodArtLancio");

                    b.ToTable("OrdiniProduzioneCK");
                });

            modelBuilder.Entity("Gmax.Models.Entities.AssegnazioneMagazzino", b =>
                {
                    b.HasOne("Gmax.Models.Entities.OrdineProdCompCK", "OrdineProdCompCK")
                        .WithMany("Assegnazioni")
                        .HasForeignKey("NroLancio", "NroSottolancio", "TipoArticolo", "CodiceArticolo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OrdineProdCompCK");
                });

            modelBuilder.Entity("Gmax.Models.Entities.OrdineProdComp", b =>
                {
                    b.HasOne("Gmax.Models.Entities.Articolo", "Articolo")
                        .WithMany()
                        .HasForeignKey("ArticoloId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gmax.Models.Entities.OrdineProduzione", "OrdineProduzione")
                        .WithMany()
                        .HasForeignKey("OrdineProduzioneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Articolo");

                    b.Navigation("OrdineProduzione");
                });

            modelBuilder.Entity("Gmax.Models.Entities.OrdineProdCompCK", b =>
                {
                    b.HasOne("Gmax.Models.Entities.OrdineProduzioneCK", "OrdineProduzioneCK")
                        .WithMany("OrdineProdCompCKList")
                        .HasForeignKey("NroLancio", "NroSottolancio")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gmax.Models.Entities.ArticoloCK", "Articolo")
                        .WithMany("OrdineProdCompCKList")
                        .HasForeignKey("TipoArticolo", "CodiceArticolo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Articolo");

                    b.Navigation("OrdineProduzioneCK");
                });

            modelBuilder.Entity("Gmax.Models.Entities.OrdineProduzione", b =>
                {
                    b.HasOne("Gmax.Models.Entities.Articolo", "ArtLancio")
                        .WithMany("OrdineProduzioneLancioList")
                        .HasForeignKey("ArtLancioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ArtLancio");
                });

            modelBuilder.Entity("Gmax.Models.Entities.OrdineProduzioneCK", b =>
                {
                    b.HasOne("Gmax.Models.Entities.ArticoloCK", "ArtLancio")
                        .WithMany("OrdineProduzioneLancioList")
                        .HasForeignKey("TipoArtLancio", "CodArtLancio")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ArtLancio");
                });

            modelBuilder.Entity("Gmax.Models.Entities.Articolo", b =>
                {
                    b.Navigation("OrdineProduzioneLancioList");
                });

            modelBuilder.Entity("Gmax.Models.Entities.ArticoloCK", b =>
                {
                    b.Navigation("OrdineProdCompCKList");

                    b.Navigation("OrdineProduzioneLancioList");
                });

            modelBuilder.Entity("Gmax.Models.Entities.OrdineProdCompCK", b =>
                {
                    b.Navigation("Assegnazioni");
                });

            modelBuilder.Entity("Gmax.Models.Entities.OrdineProduzioneCK", b =>
                {
                    b.Navigation("OrdineProdCompCKList");
                });
#pragma warning restore 612, 618
        }
    }
}
