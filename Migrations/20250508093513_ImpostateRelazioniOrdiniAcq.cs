using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Gmax.Migrations
{
    /// <inheritdoc />
    public partial class ImpostateRelazioniOrdiniAcq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpOrdiniAcquisto",
                table: "ExpOrdiniAcquisto");

            migrationBuilder.DropIndex(
                name: "IX_ExpOrdiniAcquisto_TipoArticolo_CodiceArticolo_NroOrdineAcqu~",
                table: "ExpOrdiniAcquisto");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ExpOrdiniAcquisto");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpOrdiniAcquisto",
                table: "ExpOrdiniAcquisto",
                columns: new[] { "TipoArticolo", "CodiceArticolo", "NroOrdineAcquisto", "DataConsegnaAcquisto" });

            migrationBuilder.AddForeignKey(
                name: "FK_ExpOrdiniAcquisto_ArticoliCK_TipoArticolo_CodiceArticolo",
                table: "ExpOrdiniAcquisto",
                columns: new[] { "TipoArticolo", "CodiceArticolo" },
                principalTable: "ArticoliCK",
                principalColumns: new[] { "TipoArticolo", "CodiceArticolo" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpOrdiniAcquisto_ArticoliCK_TipoArticolo_CodiceArticolo",
                table: "ExpOrdiniAcquisto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpOrdiniAcquisto",
                table: "ExpOrdiniAcquisto");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ExpOrdiniAcquisto",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpOrdiniAcquisto",
                table: "ExpOrdiniAcquisto",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ExpOrdiniAcquisto_TipoArticolo_CodiceArticolo_NroOrdineAcqu~",
                table: "ExpOrdiniAcquisto",
                columns: new[] { "TipoArticolo", "CodiceArticolo", "NroOrdineAcquisto", "DataConsegnaAcquisto" },
                unique: true);
        }
    }
}
