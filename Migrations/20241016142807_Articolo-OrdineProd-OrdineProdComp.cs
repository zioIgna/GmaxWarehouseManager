using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gmax.Migrations
{
    /// <inheritdoc />
    public partial class ArticoloOrdineProdOrdineProdComp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrdineProduzione",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NroLancio = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NroSottolancio = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Stato = table.Column<string>(type: "NVARCHAR2(6)", maxLength: 6, nullable: false),
                    ArtLancioId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DataCreazione = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DataPrevCons = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdiniProduzione", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdiniProduzione_Articoli_ArtLancioId",
                        column: x => x.ArtLancioId,
                        principalTable: "Articolo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrdineProdComp",
                columns: table => new
                {
                    OrdineProduzioneId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ArticoloId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    SeqOp = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    SeqArt = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    QtaPrevista = table.Column<decimal>(type: "DECIMAL(14,4)", precision: 14, scale: 4, nullable: false),
                    QtaGiaScaricata = table.Column<decimal>(type: "DECIMAL(14,4)", precision: 14, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdiniProdComp", x => new { x.ArticoloId, x.OrdineProduzioneId });
                    table.ForeignKey(
                        name: "FK_OrdiniProdComp_Articoli_ArticoloId",
                        column: x => x.ArticoloId,
                        principalTable: "Articolo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdiniProdComp_OrdiniProduzione_OrdineProduzioneId",
                        column: x => x.OrdineProduzioneId,
                        principalTable: "OrdineProduzione",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrdiniProdComp_OrdineProduzioneId",
                table: "OrdineProdComp",
                column: "OrdineProduzioneId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdiniProduzione_ArtLancioId",
                table: "OrdineProduzione",
                column: "ArtLancioId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdiniProduzione_NroLancio_NroSottolancio",
                table: "OrdineProduzione",
                columns: new[] { "NroLancio", "NroSottolancio" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrdineProdComp");

            migrationBuilder.DropTable(
                name: "OrdineProduzione");
        }
    }
}
