using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gmax.Migrations
{
    /// <inheritdoc />
    public partial class EntityMagazzinoEdited : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Magazzino_OrdiniProduzioneCK_NroLancio_NroSottolancio",
                table: "Magazzino");

            migrationBuilder.AlterColumn<int>(
                name: "NroSottolancio",
                table: "Magazzino",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "NroLancio",
                table: "Magazzino",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Magazzino_OrdiniProduzioneCK_NroLancio_NroSottolancio",
                table: "Magazzino",
                columns: new[] { "NroLancio", "NroSottolancio" },
                principalTable: "OrdiniProduzioneCK",
                principalColumns: new[] { "NroLancio", "NroSottolancio" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Magazzino_OrdiniProduzioneCK_NroLancio_NroSottolancio",
                table: "Magazzino");

            migrationBuilder.AlterColumn<int>(
                name: "NroSottolancio",
                table: "Magazzino",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NroLancio",
                table: "Magazzino",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Magazzino_OrdiniProduzioneCK_NroLancio_NroSottolancio",
                table: "Magazzino",
                columns: new[] { "NroLancio", "NroSottolancio" },
                principalTable: "OrdiniProduzioneCK",
                principalColumns: new[] { "NroLancio", "NroSottolancio" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
