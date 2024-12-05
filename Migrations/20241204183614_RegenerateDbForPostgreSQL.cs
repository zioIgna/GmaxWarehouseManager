using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Gmax.Migrations
{
    /// <inheritdoc />
    public partial class RegenerateDbForPostgreSQL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticoliCK",
                columns: table => new
                {
                    TipoArticolo = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CodiceArticolo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Descrizione = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    UnitaMisuraGestione = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    QtaScortaMin = table.Column<decimal>(type: "numeric(14,4)", precision: 14, scale: 4, nullable: true),
                    CodUbicazione = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: true),
                    CodCostruttore = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    QtaImpCliente = table.Column<int>(type: "integer", nullable: true),
                    DataInserimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticoliCK", x => new { x.TipoArticolo, x.CodiceArticolo });
                });

            migrationBuilder.CreateTable(
                name: "OrdiniProduzioneCK",
                columns: table => new
                {
                    NroLancio = table.Column<int>(type: "integer", nullable: false),
                    NroSottolancio = table.Column<int>(type: "integer", nullable: false),
                    Stato = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    TipoArtLancio = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CodArtLancio = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    DataCreazione = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataPrevCons = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdiniProduzioneCK", x => new { x.NroLancio, x.NroSottolancio });
                    table.ForeignKey(
                        name: "FK_OrdiniProduzioneCK_ArticoliCK_TipoArtLancio_CodArtLancio",
                        columns: x => new { x.TipoArtLancio, x.CodArtLancio },
                        principalTable: "ArticoliCK",
                        principalColumns: new[] { "TipoArticolo", "CodiceArticolo" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrdiniProdCompCK",
                columns: table => new
                {
                    NroLancio = table.Column<int>(type: "integer", nullable: false),
                    NroSottolancio = table.Column<int>(type: "integer", nullable: false),
                    TipoArticolo = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CodiceArticolo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    SeqOp = table.Column<int>(type: "integer", nullable: false),
                    SeqArt = table.Column<int>(type: "integer", nullable: false),
                    QtaPrevista = table.Column<decimal>(type: "numeric(14,4)", precision: 14, scale: 4, nullable: false),
                    QtaGiaScaricata = table.Column<int>(type: "integer", precision: 14, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdiniProdCompCK", x => new { x.NroLancio, x.NroSottolancio, x.TipoArticolo, x.CodiceArticolo });
                    table.ForeignKey(
                        name: "FK_OrdiniProdCompCK_ArticoliCK_TipoArticolo_CodiceArticolo",
                        columns: x => new { x.TipoArticolo, x.CodiceArticolo },
                        principalTable: "ArticoliCK",
                        principalColumns: new[] { "TipoArticolo", "CodiceArticolo" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdiniProdCompCK_OrdiniProduzioneCK_NroLancio_NroSottolancio",
                        columns: x => new { x.NroLancio, x.NroSottolancio },
                        principalTable: "OrdiniProduzioneCK",
                        principalColumns: new[] { "NroLancio", "NroSottolancio" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssegnazioniMagazzino",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NroLancio = table.Column<int>(type: "integer", nullable: false),
                    NroSottolancio = table.Column<int>(type: "integer", nullable: false),
                    TipoArticolo = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CodiceArticolo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    DataAssegnazione = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Quantita = table.Column<int>(type: "integer", nullable: false),
                    Delta = table.Column<int>(type: "integer", nullable: false),
                    SorgenteAssegnazione = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssegnazioniMagazzino", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssegnazioniMagazzino_OrdiniProdCompCK_NroLancio_NroSottola~",
                        columns: x => new { x.NroLancio, x.NroSottolancio, x.TipoArticolo, x.CodiceArticolo },
                        principalTable: "OrdiniProdCompCK",
                        principalColumns: new[] { "NroLancio", "NroSottolancio", "TipoArticolo", "CodiceArticolo" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssegnazioniMagazzino_NroLancio_NroSottolancio_TipoArticolo~",
                table: "AssegnazioniMagazzino",
                columns: new[] { "NroLancio", "NroSottolancio", "TipoArticolo", "CodiceArticolo" });

            migrationBuilder.CreateIndex(
                name: "IX_OrdiniProdCompCK_TipoArticolo_CodiceArticolo",
                table: "OrdiniProdCompCK",
                columns: new[] { "TipoArticolo", "CodiceArticolo" });

            migrationBuilder.CreateIndex(
                name: "IX_OrdiniProduzioneCK_TipoArtLancio_CodArtLancio",
                table: "OrdiniProduzioneCK",
                columns: new[] { "TipoArtLancio", "CodArtLancio" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssegnazioniMagazzino");

            migrationBuilder.DropTable(
                name: "OrdiniProdCompCK");

            migrationBuilder.DropTable(
                name: "OrdiniProduzioneCK");

            migrationBuilder.DropTable(
                name: "ArticoliCK");
        }
    }
}
