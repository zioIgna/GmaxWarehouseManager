using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gmax.Migrations
{
    /// <inheritdoc />
    public partial class ArticoloCompositeKeyInverted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdiniProdCompCK_ArticoliCK_CodiceArticolo_TipoArticolo",
                table: "OrdiniProdCompCK");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdiniProduzioneCK_ArticoliCK_TipoArtLancio_CodArtLancio",
                table: "OrdiniProduzioneCK");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdiniProdCompCK",
                table: "OrdiniProdCompCK");

            migrationBuilder.DropIndex(
                name: "IX_OrdiniProdCompCK_NroLancio_NroSottolancio",
                table: "OrdiniProdCompCK");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArticoliCK",
                table: "ArticoliCK");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdiniProdCompCK",
                table: "OrdiniProdCompCK",
                columns: new[] { "NroLancio", "NroSottolancio", "TipoArticolo", "CodiceArticolo" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArticoliCK",
                table: "ArticoliCK",
                columns: new[] { "TipoArticolo", "CodiceArticolo" });

            migrationBuilder.CreateIndex(
                name: "IX_OrdiniProdCompCK_TipoArticolo_CodiceArticolo",
                table: "OrdiniProdCompCK",
                columns: new[] { "TipoArticolo", "CodiceArticolo" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrdiniProdCompCK_ArticoliCK_TipoArticolo_CodiceArticolo",
                table: "OrdiniProdCompCK",
                columns: new[] { "TipoArticolo", "CodiceArticolo" },
                principalTable: "ArticoliCK",
                principalColumns: new[] { "TipoArticolo", "CodiceArticolo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdiniProduzioneCK_ArticoliCK_TipoArtLancio_CodArtLancio",
                table: "OrdiniProduzioneCK",
                columns: new[] { "TipoArtLancio", "CodArtLancio" },
                principalTable: "ArticoliCK",
                principalColumns: new[] { "TipoArticolo", "CodiceArticolo" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdiniProdCompCK_ArticoliCK_TipoArticolo_CodiceArticolo",
                table: "OrdiniProdCompCK");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdiniProduzioneCK_ArticoliCK_TipoArtLancio_CodArtLancio",
                table: "OrdiniProduzioneCK");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdiniProdCompCK",
                table: "OrdiniProdCompCK");

            migrationBuilder.DropIndex(
                name: "IX_OrdiniProdCompCK_TipoArticolo_CodiceArticolo",
                table: "OrdiniProdCompCK");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArticoliCK",
                table: "ArticoliCK");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdiniProdCompCK",
                table: "OrdiniProdCompCK",
                columns: new[] { "CodiceArticolo", "TipoArticolo", "NroLancio", "NroSottolancio" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArticoliCK",
                table: "ArticoliCK",
                columns: new[] { "CodiceArticolo", "TipoArticolo" });

            migrationBuilder.CreateIndex(
                name: "IX_OrdiniProdCompCK_NroLancio_NroSottolancio",
                table: "OrdiniProdCompCK",
                columns: new[] { "NroLancio", "NroSottolancio" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrdiniProdCompCK_ArticoliCK_CodiceArticolo_TipoArticolo",
                table: "OrdiniProdCompCK",
                columns: new[] { "CodiceArticolo", "TipoArticolo" },
                principalTable: "ArticoliCK",
                principalColumns: new[] { "CodiceArticolo", "TipoArticolo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdiniProduzioneCK_ArticoliCK_TipoArtLancio_CodArtLancio",
                table: "OrdiniProduzioneCK",
                columns: new[] { "TipoArtLancio", "CodArtLancio" },
                principalTable: "ArticoliCK",
                principalColumns: new[] { "CodiceArticolo", "TipoArticolo" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
