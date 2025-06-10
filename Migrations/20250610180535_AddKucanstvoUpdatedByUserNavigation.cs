using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class AddKucanstvoUpdatedByUserNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "KucanstvoUpdatedBy",
                table: "SocijalniNatjecajZahtjevi",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajZahtjevi_KucanstvoUpdatedBy",
                table: "SocijalniNatjecajZahtjevi",
                column: "KucanstvoUpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajZahtjevi_AspNetUsers_KucanstvoUpdatedBy",
                table: "SocijalniNatjecajZahtjevi",
                column: "KucanstvoUpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajZahtjevi_AspNetUsers_KucanstvoUpdatedBy",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajZahtjevi_KucanstvoUpdatedBy",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.AlterColumn<string>(
                name: "KucanstvoUpdatedBy",
                table: "SocijalniNatjecajZahtjevi",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
