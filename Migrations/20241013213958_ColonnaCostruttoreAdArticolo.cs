using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gmax.Migrations
{
    /// <inheritdoc />
    public partial class ColonnaCostruttoreAdArticolo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodCostruttore",
                table: "Articolo",
                type: "NVARCHAR2(256)",
                maxLength: 256,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodCostruttore",
                table: "Articolo");
        }
    }
}
