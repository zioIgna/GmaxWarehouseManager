using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gmax.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articoli",
                columns: table => new
                {
                    CodiceArticolo = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    TipoArticolo = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Descrizione = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UnitaMisura = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    QtaScortaMin = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    QtaImpCliente = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articoli", x => new { x.CodiceArticolo, x.TipoArticolo });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articoli");
        }
    }
}
