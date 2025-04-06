using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class AddSocijalniKucanstvoPodaci : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SocijalniNatjecajKucanstvoPodaci",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZahtjevId = table.Column<long>(type: "bigint", nullable: false),
                    UkupniPrihodKucanstva = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    PrebivanjeOd = table.Column<DateOnly>(type: "date", nullable: false),
                    StambeniStatusKucanstva = table.Column<byte>(type: "tinyint", nullable: false),
                    SastavKucanstva = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocijalniNatjecajKucanstvoPodaci", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajKucanstvoPodaci_SocijalniNatjecajZahtjevi_ZahtjevId",
                        column: x => x.ZahtjevId,
                        principalTable: "SocijalniNatjecajZahtjevi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajKucanstvoPodaci_ZahtjevId",
                table: "SocijalniNatjecajKucanstvoPodaci",
                column: "ZahtjevId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocijalniNatjecajKucanstvoPodaci");
        }
    }
}
