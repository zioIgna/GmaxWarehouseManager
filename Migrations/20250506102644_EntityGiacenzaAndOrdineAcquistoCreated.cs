using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Gmax.Migrations
{
    /// <inheritdoc />
    public partial class EntityGiacenzaAndOrdineAcquistoCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Magazzino",
                newName: "CodMagazzino");

            migrationBuilder.CreateTable(
                name: "ExpGiacenze",
                columns: table => new
                {
                    TipoArticolo = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CodiceArticolo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CodMagazzino = table.Column<string>(type: "text", nullable: false),
                    QtaGiacenza = table.Column<int>(type: "integer", nullable: false),
                    DataInserimento = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpGiacenze", x => new { x.TipoArticolo, x.CodiceArticolo, x.CodMagazzino });
                });

            migrationBuilder.CreateTable(
                name: "ExpOrdiniAcquisto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipoArticolo = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CodiceArticolo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    NroOrdineAcquisto = table.Column<int>(type: "integer", nullable: false),
                    QtaOrdineFornitore = table.Column<int>(type: "integer", nullable: false),
                    DataOrdineAcquisto = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataConsegnaAcquisto = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataInserimento = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpOrdiniAcquisto", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpOrdiniAcquisto_TipoArticolo_CodiceArticolo_NroOrdineAcqu~",
                table: "ExpOrdiniAcquisto",
                columns: new[] { "TipoArticolo", "CodiceArticolo", "NroOrdineAcquisto", "DataConsegnaAcquisto" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpGiacenze");

            migrationBuilder.DropTable(
                name: "ExpOrdiniAcquisto");

            migrationBuilder.RenameColumn(
                name: "CodMagazzino",
                table: "Magazzino",
                newName: "Name");
        }
    }
}
