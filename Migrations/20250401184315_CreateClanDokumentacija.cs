using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class CreateClanDokumentacija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DokazniDokumenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tip = table.Column<int>(type: "int", nullable: false),
                    Obavezno = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DokazniDokumenti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SocijalniNatjecajClanovi",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImeIPrezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Srodstvo = table.Column<int>(type: "int", nullable: false),
                    Oib = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    DatumRodenja = table.Column<DateOnly>(type: "date", nullable: true),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NatjecajId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocijalniNatjecajClanovi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajClanovi_SocijalniNatjecaji_NatjecajId",
                        column: x => x.NatjecajId,
                        principalTable: "SocijalniNatjecaji",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DostavljenaDokumentacijaClanova",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClanId = table.Column<long>(type: "bigint", nullable: false),
                    DokazniDokumentId = table.Column<int>(type: "int", nullable: false),
                    Dostavljeno = table.Column<bool>(type: "bit", nullable: false),
                    Napomena = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DostavljenaDokumentacijaClanova", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DostavljenaDokumentacijaClanova_DokazniDokumenti_DokazniDokumentId",
                        column: x => x.DokazniDokumentId,
                        principalTable: "DokazniDokumenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DostavljenaDokumentacijaClanova_SocijalniNatjecajClanovi_ClanId",
                        column: x => x.ClanId,
                        principalTable: "SocijalniNatjecajClanovi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DostavljenaDokumentacijaClanova_ClanId",
                table: "DostavljenaDokumentacijaClanova",
                column: "ClanId");

            migrationBuilder.CreateIndex(
                name: "IX_DostavljenaDokumentacijaClanova_DokazniDokumentId",
                table: "DostavljenaDokumentacijaClanova",
                column: "DokazniDokumentId");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajClanovi_NatjecajId",
                table: "SocijalniNatjecajClanovi",
                column: "NatjecajId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DostavljenaDokumentacijaClanova");

            migrationBuilder.DropTable(
                name: "DokazniDokumenti");

            migrationBuilder.DropTable(
                name: "SocijalniNatjecajClanovi");
        }
    }
}
