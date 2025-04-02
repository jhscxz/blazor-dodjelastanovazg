using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDokumentacijaEnumRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DostavljenaDokumentacijaClanova_DokazniDokumenti_DokazniDokumentId",
                table: "DostavljenaDokumentacijaClanova");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniKucanstvoDokumenti_DokazniDokumenti_DokazniDokumentId",
                table: "SocijalniKucanstvoDokumenti");

            migrationBuilder.DropTable(
                name: "DokazniDokumenti");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniKucanstvoDokumenti_DokazniDokumentId",
                table: "SocijalniKucanstvoDokumenti");

            migrationBuilder.DropIndex(
                name: "IX_DostavljenaDokumentacijaClanova_DokazniDokumentId",
                table: "DostavljenaDokumentacijaClanova");

            migrationBuilder.DropColumn(
                name: "DokazniDokumentId",
                table: "SocijalniKucanstvoDokumenti");

            migrationBuilder.DropColumn(
                name: "DokazniDokumentId",
                table: "DostavljenaDokumentacijaClanova");

            migrationBuilder.AddColumn<byte>(
                name: "Dokument",
                table: "SocijalniKucanstvoDokumenti",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "Dokument",
                table: "DostavljenaDokumentacijaClanova",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dokument",
                table: "SocijalniKucanstvoDokumenti");

            migrationBuilder.DropColumn(
                name: "Dokument",
                table: "DostavljenaDokumentacijaClanova");

            migrationBuilder.AddColumn<int>(
                name: "DokazniDokumentId",
                table: "SocijalniKucanstvoDokumenti",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DokazniDokumentId",
                table: "DostavljenaDokumentacijaClanova",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DokazniDokumenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Obavezno = table.Column<bool>(type: "bit", nullable: false),
                    Tip = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DokazniDokumenti", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniKucanstvoDokumenti_DokazniDokumentId",
                table: "SocijalniKucanstvoDokumenti",
                column: "DokazniDokumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DostavljenaDokumentacijaClanova_DokazniDokumentId",
                table: "DostavljenaDokumentacijaClanova",
                column: "DokazniDokumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_DostavljenaDokumentacijaClanova_DokazniDokumenti_DokazniDokumentId",
                table: "DostavljenaDokumentacijaClanova",
                column: "DokazniDokumentId",
                principalTable: "DokazniDokumenti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniKucanstvoDokumenti_DokazniDokumenti_DokazniDokumentId",
                table: "SocijalniKucanstvoDokumenti",
                column: "DokazniDokumentId",
                principalTable: "DokazniDokumenti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
