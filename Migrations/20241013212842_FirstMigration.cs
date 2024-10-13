using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gmax.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articolo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    TipoArticolo = table.Column<string>(type: "NVARCHAR2(3)", maxLength: 3, nullable: false),
                    CodiceArticolo = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false),
                    Descrizione = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: true),
                    UnitaMisuraGestione = table.Column<string>(type: "NVARCHAR2(3)", maxLength: 3, nullable: true),
                    QtaScortaMin = table.Column<decimal>(type: "DECIMAL(14,4)", precision: 14, scale: 4, nullable: true),
                    CodUbicazione = table.Column<string>(type: "NVARCHAR2(6)", maxLength: 6, nullable: true),
                    QtaImpCliente = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    DataInserimento = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articolo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articolo_TipoArticolo_CodiceArticolo",
                table: "Articolo",
                columns: new[] { "TipoArticolo", "CodiceArticolo" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articolo");
        }
    }
}
