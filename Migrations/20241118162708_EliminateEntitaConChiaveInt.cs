using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gmax.Migrations
{
    /// <inheritdoc />
    public partial class EliminateEntitaConChiaveInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrdiniProdCompIntKey");

            migrationBuilder.DropTable(
                name: "OrdiniProduzioneIntKey");

            migrationBuilder.DropTable(
                name: "ArticoliIntKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticoliIntKey",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CodCostruttore = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    CodUbicazione = table.Column<string>(type: "NVARCHAR2(6)", maxLength: 6, nullable: true),
                    CodiceArticolo = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    DataInserimento = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    Descrizione = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: true),
                    QtaImpCliente = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    QtaScortaMin = table.Column<decimal>(type: "DECIMAL(14,4)", precision: 14, scale: 4, nullable: true),
                    TipoArticolo = table.Column<string>(type: "NVARCHAR2(3)", maxLength: 3, nullable: false),
                    UnitaMisuraGestione = table.Column<string>(type: "NVARCHAR2(3)", maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticoliIntKey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrdiniProduzioneIntKey",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ArtLancioId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DataCreazione = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DataPrevCons = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    NroLancio = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NroSottolancio = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Stato = table.Column<string>(type: "NVARCHAR2(6)", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdiniProduzioneIntKey", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdiniProduzioneIntKey_ArticoliIntKey_ArtLancioId",
                        column: x => x.ArtLancioId,
                        principalTable: "ArticoliIntKey",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrdiniProdCompIntKey",
                columns: table => new
                {
                    ArticoloId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    OrdineProduzioneId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    QtaGiaScaricata = table.Column<decimal>(type: "DECIMAL(14,4)", precision: 14, scale: 4, nullable: false),
                    QtaPrevista = table.Column<decimal>(type: "DECIMAL(14,4)", precision: 14, scale: 4, nullable: false),
                    SeqArt = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    SeqOp = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdiniProdCompIntKey", x => new { x.ArticoloId, x.OrdineProduzioneId });
                    table.ForeignKey(
                        name: "FK_OrdiniProdCompIntKey_ArticoliIntKey_ArticoloId",
                        column: x => x.ArticoloId,
                        principalTable: "ArticoliIntKey",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdiniProdCompIntKey_OrdiniProduzioneIntKey_OrdineProduzioneId",
                        column: x => x.OrdineProduzioneId,
                        principalTable: "OrdiniProduzioneIntKey",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticoliIntKey_TipoArticolo_CodiceArticolo",
                table: "ArticoliIntKey",
                columns: new[] { "TipoArticolo", "CodiceArticolo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrdiniProdCompIntKey_OrdineProduzioneId",
                table: "OrdiniProdCompIntKey",
                column: "OrdineProduzioneId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdiniProduzioneIntKey_ArtLancioId",
                table: "OrdiniProduzioneIntKey",
                column: "ArtLancioId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdiniProduzioneIntKey_NroLancio_NroSottolancio",
                table: "OrdiniProduzioneIntKey",
                columns: new[] { "NroLancio", "NroSottolancio" },
                unique: true);
        }
    }
}
