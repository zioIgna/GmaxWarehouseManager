using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gmax.Migrations
{
    /// <inheritdoc />
    public partial class MagazzinoGiacenzaOneToManyRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(
            //    name: "IX_ExpGiacenze_CodMagazzino",
            //    table: "ExpGiacenze");

            migrationBuilder.CreateIndex(
                name: "IX_ExpGiacenze_CodMagazzino",
                table: "ExpGiacenze",
                column: "CodMagazzino");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExpGiacenze_CodMagazzino",
                table: "ExpGiacenze");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ExpGiacenze_CodMagazzino",
            //    table: "ExpGiacenze",
            //    column: "CodMagazzino",
            //    unique: true);
        }
    }
}
