using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gmax.Migrations
{
    /// <inheritdoc />
    public partial class EntitaAssegnazioneMagazzino : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssegnazioniMagazzino",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NroLancio = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NroSottolancio = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TipoArticolo = table.Column<string>(type: "NVARCHAR2(3)", maxLength: 3, nullable: false),
                    CodiceArticolo = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    DataAssegnazione = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    Quantita = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssegnazioniMagazzino", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssegnazioniMagazzino_OrdiniProdCompCK_NroLancio_NroSottolancio_TipoArticolo_CodiceArticolo",
                        columns: x => new { x.NroLancio, x.NroSottolancio, x.TipoArticolo, x.CodiceArticolo },
                        principalTable: "OrdiniProdCompCK",
                        principalColumns: new[] { "NroLancio", "NroSottolancio", "TipoArticolo", "CodiceArticolo" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssegnazioniMagazzino_NroLancio_NroSottolancio_TipoArticolo_CodiceArticolo",
                table: "AssegnazioniMagazzino",
                columns: new[] { "NroLancio", "NroSottolancio", "TipoArticolo", "CodiceArticolo" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssegnazioniMagazzino");
        }
    }
}
