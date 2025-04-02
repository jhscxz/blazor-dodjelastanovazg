using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class AddSocijalniKucanstvoDokumentacija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajClanovi_SocijalniNatjecaji_NatjecajId",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropTable(
                name: "SocijalniNatjecaji");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "SocijalniNatjecajClanovi",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SocijalniNatjecajZahtjevi",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KlasaPredmeta = table.Column<int>(type: "int", nullable: false),
                    DatumPodnosenjaZahtjeva = table.Column<DateOnly>(type: "date", nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UkupniPrihodKucanstva = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    StambeniStatusKucanstva = table.Column<byte>(type: "tinyint", nullable: false),
                    SastavKucanstva = table.Column<byte>(type: "tinyint", nullable: false),
                    ImaUseljivuNekretninu = table.Column<bool>(type: "bit", nullable: false),
                    BrojGodinaPrebivanja = table.Column<byte>(type: "tinyint", nullable: false),
                    Aktivan = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EditedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NatjecajId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocijalniNatjecajZahtjevi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajZahtjevi_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajZahtjevi_AspNetUsers_EditedBy",
                        column: x => x.EditedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajZahtjevi_Natjecaji_NatjecajId",
                        column: x => x.NatjecajId,
                        principalTable: "Natjecaji",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SocijalniKucanstvoDokumenti",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NatjecajZahtjevId = table.Column<long>(type: "bigint", nullable: false),
                    DokazniDokumentId = table.Column<int>(type: "int", nullable: false),
                    Dostavljeno = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocijalniKucanstvoDokumenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocijalniKucanstvoDokumenti_DokazniDokumenti_DokazniDokumentId",
                        column: x => x.DokazniDokumentId,
                        principalTable: "DokazniDokumenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SocijalniKucanstvoDokumenti_SocijalniNatjecajZahtjevi_NatjecajZahtjevId",
                        column: x => x.NatjecajZahtjevId,
                        principalTable: "SocijalniNatjecajZahtjevi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniKucanstvoDokumenti_DokazniDokumentId",
                table: "SocijalniKucanstvoDokumenti",
                column: "DokazniDokumentId");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniKucanstvoDokumenti_NatjecajZahtjevId",
                table: "SocijalniKucanstvoDokumenti",
                column: "NatjecajZahtjevId");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajZahtjevi_CreatedBy",
                table: "SocijalniNatjecajZahtjevi",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajZahtjevi_EditedBy",
                table: "SocijalniNatjecajZahtjevi",
                column: "EditedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajZahtjevi_NatjecajId",
                table: "SocijalniNatjecajZahtjevi",
                column: "NatjecajId");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajClanovi_SocijalniNatjecajZahtjevi_NatjecajId",
                table: "SocijalniNatjecajClanovi",
                column: "NatjecajId",
                principalTable: "SocijalniNatjecajZahtjevi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajClanovi_SocijalniNatjecajZahtjevi_NatjecajId",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropTable(
                name: "SocijalniKucanstvoDokumenti");

            migrationBuilder.DropTable(
                name: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.CreateTable(
                name: "SocijalniNatjecaji",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EditedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NatjecajId = table.Column<long>(type: "bigint", nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aktivan = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumPodnosenjaZahtjeva = table.Column<DateOnly>(type: "date", nullable: false),
                    KlasaPredmeta = table.Column<int>(type: "int", nullable: false),
                    SastavKucanstva = table.Column<byte>(type: "tinyint", nullable: false),
                    StambeniStatusKucanstva = table.Column<byte>(type: "tinyint", nullable: false),
                    UkupniPrihodKucanstva = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocijalniNatjecaji", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecaji_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecaji_AspNetUsers_EditedBy",
                        column: x => x.EditedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecaji_Natjecaji_NatjecajId",
                        column: x => x.NatjecajId,
                        principalTable: "Natjecaji",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecaji_CreatedBy",
                table: "SocijalniNatjecaji",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecaji_EditedBy",
                table: "SocijalniNatjecaji",
                column: "EditedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecaji_NatjecajId",
                table: "SocijalniNatjecaji",
                column: "NatjecajId");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajClanovi_SocijalniNatjecaji_NatjecajId",
                table: "SocijalniNatjecajClanovi",
                column: "NatjecajId",
                principalTable: "SocijalniNatjecaji",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
