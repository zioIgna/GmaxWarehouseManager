using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gmax.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedDescrizioneLengthForArticoloCK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Descrizione",
                table: "ArticoliCK",
                type: "NVARCHAR2(130)",
                maxLength: 130,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(120)",
                oldMaxLength: 120,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Descrizione",
                table: "ArticoliCK",
                type: "NVARCHAR2(120)",
                maxLength: 120,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(130)",
                oldMaxLength: 130,
                oldNullable: true);
        }
    }
}
