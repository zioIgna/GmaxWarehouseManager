using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gmax.Migrations
{
    /// <inheritdoc />
    public partial class QtaGiaScaricataFromDecToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "QtaGiaScaricata",
                table: "OrdiniProdCompCK",
                type: "NUMBER(14,4)",
                precision: 14,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(14,4)",
                oldPrecision: 14,
                oldScale: 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "QtaGiaScaricata",
                table: "OrdiniProdCompCK",
                type: "DECIMAL(14,4)",
                precision: 14,
                scale: 4,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "NUMBER(14,4)",
                oldPrecision: 14,
                oldScale: 4);
        }
    }
}
