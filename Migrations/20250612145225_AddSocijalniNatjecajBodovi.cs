using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class AddSocijalniNatjecajBodovi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SocijalniNatjecajBodovi",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZahtjevId = table.Column<long>(type: "bigint", nullable: false),
                    BodoviStambeniStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviSastavKucanstva = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviPoClanu = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviMaloljetni = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviPunoljetniUzdrzavani = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviZajamcenaNaknada = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviNjegovatelj = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviDoplatakZaNjegu = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviOdraslihInvalidnina = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviMaloljetnihInvalidnina = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviZrtvaNasilja = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviAlternativnaSkrb = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviIznad55 = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviObrana = table.Column<float>(type: "real", nullable: false),
                    BodoviSeksualnoNasilje = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviCivilniStradalnici = table.Column<byte>(type: "tinyint", nullable: false),
                    UkupnoBodova = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocijalniNatjecajBodovi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajBodovi_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajBodovi_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajBodovi_SocijalniNatjecajZahtjevi_ZahtjevId",
                        column: x => x.ZahtjevId,
                        principalTable: "SocijalniNatjecajZahtjevi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajBodovi_CreatedBy",
                table: "SocijalniNatjecajBodovi",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajBodovi_UpdatedBy",
                table: "SocijalniNatjecajBodovi",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajBodovi_ZahtjevId",
                table: "SocijalniNatjecajBodovi",
                column: "ZahtjevId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocijalniNatjecajBodovi");
        }
    }
}
