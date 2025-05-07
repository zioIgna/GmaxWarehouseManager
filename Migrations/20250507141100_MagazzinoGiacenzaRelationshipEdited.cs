using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gmax.Migrations
{
    /// <inheritdoc />
    public partial class MagazzinoGiacenzaRelationshipEdited : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Magazzino_CodMagazzino",
                table: "Magazzino",
                column: "CodMagazzino");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ExpGiacenze_CodMagazzino",
            //    table: "ExpGiacenze",
            //    column: "CodMagazzino",
            //    unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpGiacenze_ArticoliCK_TipoArticolo_CodiceArticolo",
                table: "ExpGiacenze",
                columns: new[] { "TipoArticolo", "CodiceArticolo" },
                principalTable: "ArticoliCK",
                principalColumns: new[] { "TipoArticolo", "CodiceArticolo" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpGiacenze_Magazzino_CodMagazzino",
                table: "ExpGiacenze",
                column: "CodMagazzino",
                principalTable: "Magazzino",
                principalColumn: "CodMagazzino",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpGiacenze_ArticoliCK_TipoArticolo_CodiceArticolo",
                table: "ExpGiacenze");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpGiacenze_Magazzino_CodMagazzino",
                table: "ExpGiacenze");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Magazzino_CodMagazzino",
                table: "Magazzino");

            //migrationBuilder.DropIndex(
            //    name: "IX_ExpGiacenze_CodMagazzino",
            //    table: "ExpGiacenze");
        }
    }
}
