using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Gmax.Migrations
{
    /// <inheritdoc />
    public partial class EntityMagazzinoCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DataPrevCons",
                table: "OrdiniProduzioneCK",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCreazione",
                table: "OrdiniProduzioneCK",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataAssegnazione",
                table: "AssegnazioniMagazzino",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<int>(
                name: "MagazzinoDestinazioneId",
                table: "AssegnazioniMagazzino",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MagazzinoOrigineId",
                table: "AssegnazioniMagazzino",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataInserimento",
                table: "ArticoliCK",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Magazzino",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NroLancio = table.Column<int>(type: "integer", nullable: false),
                    NroSottolancio = table.Column<int>(type: "integer", nullable: false),
                    DataAssegnazione = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    statoAttivo = table.Column<int>(type: "integer", nullable: false),
                    TipoMagazzino = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Magazzino", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Magazzino_OrdiniProduzioneCK_NroLancio_NroSottolancio",
                        columns: x => new { x.NroLancio, x.NroSottolancio },
                        principalTable: "OrdiniProduzioneCK",
                        principalColumns: new[] { "NroLancio", "NroSottolancio" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssegnazioniMagazzino_MagazzinoDestinazioneId",
                table: "AssegnazioniMagazzino",
                column: "MagazzinoDestinazioneId");

            migrationBuilder.CreateIndex(
                name: "IX_AssegnazioniMagazzino_MagazzinoOrigineId",
                table: "AssegnazioniMagazzino",
                column: "MagazzinoOrigineId");

            migrationBuilder.CreateIndex(
                name: "IX_Magazzino_NroLancio_NroSottolancio",
                table: "Magazzino",
                columns: new[] { "NroLancio", "NroSottolancio" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AssegnazioniMagazzino_Magazzino_MagazzinoDestinazioneId",
                table: "AssegnazioniMagazzino",
                column: "MagazzinoDestinazioneId",
                principalTable: "Magazzino",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssegnazioniMagazzino_Magazzino_MagazzinoOrigineId",
                table: "AssegnazioniMagazzino",
                column: "MagazzinoOrigineId",
                principalTable: "Magazzino",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssegnazioniMagazzino_Magazzino_MagazzinoDestinazioneId",
                table: "AssegnazioniMagazzino");

            migrationBuilder.DropForeignKey(
                name: "FK_AssegnazioniMagazzino_Magazzino_MagazzinoOrigineId",
                table: "AssegnazioniMagazzino");

            migrationBuilder.DropTable(
                name: "Magazzino");

            migrationBuilder.DropIndex(
                name: "IX_AssegnazioniMagazzino_MagazzinoDestinazioneId",
                table: "AssegnazioniMagazzino");

            migrationBuilder.DropIndex(
                name: "IX_AssegnazioniMagazzino_MagazzinoOrigineId",
                table: "AssegnazioniMagazzino");

            migrationBuilder.DropColumn(
                name: "MagazzinoDestinazioneId",
                table: "AssegnazioniMagazzino");

            migrationBuilder.DropColumn(
                name: "MagazzinoOrigineId",
                table: "AssegnazioniMagazzino");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataPrevCons",
                table: "OrdiniProduzioneCK",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCreazione",
                table: "OrdiniProduzioneCK",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataAssegnazione",
                table: "AssegnazioniMagazzino",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataInserimento",
                table: "ArticoliCK",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);
        }
    }
}
