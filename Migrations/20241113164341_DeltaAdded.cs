using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gmax.Migrations
{
    /// <inheritdoc />
    public partial class DeltaAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Delta",
                table: "AssegnazioniMagazzino",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SorgenteAssegnazione",
                table: "AssegnazioniMagazzino",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Delta",
                table: "AssegnazioniMagazzino");

            migrationBuilder.DropColumn(
                name: "SorgenteAssegnazione",
                table: "AssegnazioniMagazzino");
        }
    }
}
