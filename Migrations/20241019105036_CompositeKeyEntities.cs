using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gmax.Migrations
{
    /// <inheritdoc />
    public partial class CompositeKeyEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdiniProdComps_Articoli_ArticoloId",
                table: "OrdiniProdComps");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdiniProdComps_OrdiniProduzione_OrdineProduzioneId",
                table: "OrdiniProdComps");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdiniProduzione_Articoli_ArtLancioId",
                table: "OrdiniProduzione");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdiniProduzione",
                table: "OrdiniProduzione");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdiniProdComps",
                table: "OrdiniProdComps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Articoli",
                table: "Articoli");

            migrationBuilder.RenameTable(
                name: "OrdiniProduzione",
                newName: "OrdineProduzione");

            migrationBuilder.RenameTable(
                name: "OrdiniProdComps",
                newName: "OrdineProdComp");

            migrationBuilder.RenameTable(
                name: "Articoli",
                newName: "Articolo");

            migrationBuilder.RenameIndex(
                name: "IX_OrdiniProduzione_NroLancio_NroSottolancio",
                table: "OrdineProduzione",
                newName: "IX_OrdineProduzione_NroLancio_NroSottolancio");

            migrationBuilder.RenameIndex(
                name: "IX_OrdiniProduzione_ArtLancioId",
                table: "OrdineProduzione",
                newName: "IX_OrdineProduzione_ArtLancioId");

            migrationBuilder.RenameIndex(
                name: "IX_OrdiniProdComps_OrdineProduzioneId",
                table: "OrdineProdComp",
                newName: "IX_OrdineProdComp_OrdineProduzioneId");

            migrationBuilder.RenameIndex(
                name: "IX_Articoli_TipoArticolo_CodiceArticolo",
                table: "Articolo",
                newName: "IX_Articolo_TipoArticolo_CodiceArticolo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdineProduzione",
                table: "OrdineProduzione",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdineProdComp",
                table: "OrdineProdComp",
                columns: new[] { "ArticoloId", "OrdineProduzioneId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Articolo",
                table: "Articolo",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ArticoliCK",
                columns: table => new
                {
                    TipoArticolo = table.Column<string>(type: "NVARCHAR2(3)", maxLength: 3, nullable: false),
                    CodiceArticolo = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    Descrizione = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: true),
                    UnitaMisuraGestione = table.Column<string>(type: "NVARCHAR2(3)", maxLength: 3, nullable: true),
                    QtaScortaMin = table.Column<decimal>(type: "DECIMAL(14,4)", precision: 14, scale: 4, nullable: true),
                    CodUbicazione = table.Column<string>(type: "NVARCHAR2(6)", maxLength: 6, nullable: true),
                    CodCostruttore = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    QtaImpCliente = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    DataInserimento = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticoliCK", x => new { x.CodiceArticolo, x.TipoArticolo });
                });

            migrationBuilder.CreateTable(
                name: "OrdiniProduzioneCK",
                columns: table => new
                {
                    NroLancio = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NroSottolancio = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Stato = table.Column<string>(type: "NVARCHAR2(6)", maxLength: 6, nullable: false),
                    TipoArtLancio = table.Column<string>(type: "NVARCHAR2(3)", maxLength: 3, nullable: false),
                    CodArtLancio = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    DataCreazione = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DataPrevCons = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdiniProduzioneCK", x => new { x.NroLancio, x.NroSottolancio });
                    table.ForeignKey(
                        name: "FK_OrdiniProduzioneCK_ArticoliCK_TipoArtLancio_CodArtLancio",
                        columns: x => new { x.TipoArtLancio, x.CodArtLancio },
                        principalTable: "ArticoliCK",
                        principalColumns: new[] { "CodiceArticolo", "TipoArticolo" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrdiniProdCompCK",
                columns: table => new
                {
                    NroLancio = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NroSottolancio = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TipoArticolo = table.Column<string>(type: "NVARCHAR2(3)", maxLength: 3, nullable: false),
                    CodiceArticolo = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    SeqOp = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    SeqArt = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    QtaPrevista = table.Column<decimal>(type: "DECIMAL(14,4)", precision: 14, scale: 4, nullable: false),
                    QtaGiaScaricata = table.Column<decimal>(type: "DECIMAL(14,4)", precision: 14, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdiniProdCompCK", x => new { x.CodiceArticolo, x.TipoArticolo, x.NroLancio, x.NroSottolancio });
                    table.ForeignKey(
                        name: "FK_OrdiniProdCompCK_ArticoliCK_CodiceArticolo_TipoArticolo",
                        columns: x => new { x.CodiceArticolo, x.TipoArticolo },
                        principalTable: "ArticoliCK",
                        principalColumns: new[] { "CodiceArticolo", "TipoArticolo" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdiniProdCompCK_OrdiniProduzioneCK_NroLancio_NroSottolancio",
                        columns: x => new { x.NroLancio, x.NroSottolancio },
                        principalTable: "OrdiniProduzioneCK",
                        principalColumns: new[] { "NroLancio", "NroSottolancio" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrdiniProdCompCK_NroLancio_NroSottolancio",
                table: "OrdiniProdCompCK",
                columns: new[] { "NroLancio", "NroSottolancio" });

            migrationBuilder.CreateIndex(
                name: "IX_OrdiniProduzioneCK_TipoArtLancio_CodArtLancio",
                table: "OrdiniProduzioneCK",
                columns: new[] { "TipoArtLancio", "CodArtLancio" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrdineProdComp_Articolo_ArticoloId",
                table: "OrdineProdComp",
                column: "ArticoloId",
                principalTable: "Articolo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdineProdComp_OrdineProduzione_OrdineProduzioneId",
                table: "OrdineProdComp",
                column: "OrdineProduzioneId",
                principalTable: "OrdineProduzione",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdineProduzione_Articolo_ArtLancioId",
                table: "OrdineProduzione",
                column: "ArtLancioId",
                principalTable: "Articolo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdineProdComp_Articolo_ArticoloId",
                table: "OrdineProdComp");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdineProdComp_OrdineProduzione_OrdineProduzioneId",
                table: "OrdineProdComp");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdineProduzione_Articolo_ArtLancioId",
                table: "OrdineProduzione");

            migrationBuilder.DropTable(
                name: "OrdiniProdCompCK");

            migrationBuilder.DropTable(
                name: "OrdiniProduzioneCK");

            migrationBuilder.DropTable(
                name: "ArticoliCK");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdineProduzione",
                table: "OrdineProduzione");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdineProdComp",
                table: "OrdineProdComp");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Articolo",
                table: "Articolo");

            migrationBuilder.RenameTable(
                name: "OrdineProduzione",
                newName: "OrdiniProduzione");

            migrationBuilder.RenameTable(
                name: "OrdineProdComp",
                newName: "OrdiniProdComps");

            migrationBuilder.RenameTable(
                name: "Articolo",
                newName: "Articoli");

            migrationBuilder.RenameIndex(
                name: "IX_OrdineProduzione_NroLancio_NroSottolancio",
                table: "OrdiniProduzione",
                newName: "IX_OrdiniProduzione_NroLancio_NroSottolancio");

            migrationBuilder.RenameIndex(
                name: "IX_OrdineProduzione_ArtLancioId",
                table: "OrdiniProduzione",
                newName: "IX_OrdiniProduzione_ArtLancioId");

            migrationBuilder.RenameIndex(
                name: "IX_OrdineProdComp_OrdineProduzioneId",
                table: "OrdiniProdComps",
                newName: "IX_OrdiniProdComps_OrdineProduzioneId");

            migrationBuilder.RenameIndex(
                name: "IX_Articolo_TipoArticolo_CodiceArticolo",
                table: "Articoli",
                newName: "IX_Articoli_TipoArticolo_CodiceArticolo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdiniProduzione",
                table: "OrdiniProduzione",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdiniProdComps",
                table: "OrdiniProdComps",
                columns: new[] { "ArticoloId", "OrdineProduzioneId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Articoli",
                table: "Articoli",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdiniProdComps_Articoli_ArticoloId",
                table: "OrdiniProdComps",
                column: "ArticoloId",
                principalTable: "Articoli",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdiniProdComps_OrdiniProduzione_OrdineProduzioneId",
                table: "OrdiniProdComps",
                column: "OrdineProduzioneId",
                principalTable: "OrdiniProduzione",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdiniProduzione_Articoli_ArtLancioId",
                table: "OrdiniProduzione",
                column: "ArtLancioId",
                principalTable: "Articoli",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
