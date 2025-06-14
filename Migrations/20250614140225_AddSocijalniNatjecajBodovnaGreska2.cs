using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class AddSocijalniNatjecajBodovnaGreska2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajBodovnaGreska_AspNetUsers_CreatedBy",
                table: "SocijalniNatjecajBodovnaGreska");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajBodovnaGreska_AspNetUsers_UpdatedBy",
                table: "SocijalniNatjecajBodovnaGreska");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajBodovnaGreska_SocijalniNatjecajZahtjevi_ZahtjevId",
                table: "SocijalniNatjecajBodovnaGreska");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SocijalniNatjecajBodovnaGreska",
                table: "SocijalniNatjecajBodovnaGreska");

            migrationBuilder.RenameTable(
                name: "SocijalniNatjecajBodovnaGreska",
                newName: "SocijalniNatjecajBodovnaGreske");

            migrationBuilder.RenameIndex(
                name: "IX_SocijalniNatjecajBodovnaGreska_ZahtjevId",
                table: "SocijalniNatjecajBodovnaGreske",
                newName: "IX_SocijalniNatjecajBodovnaGreske_ZahtjevId");

            migrationBuilder.RenameIndex(
                name: "IX_SocijalniNatjecajBodovnaGreska_UpdatedBy",
                table: "SocijalniNatjecajBodovnaGreske",
                newName: "IX_SocijalniNatjecajBodovnaGreske_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_SocijalniNatjecajBodovnaGreska_CreatedBy",
                table: "SocijalniNatjecajBodovnaGreske",
                newName: "IX_SocijalniNatjecajBodovnaGreske_CreatedBy");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocijalniNatjecajBodovnaGreske",
                table: "SocijalniNatjecajBodovnaGreske",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajBodovnaGreske_AspNetUsers_CreatedBy",
                table: "SocijalniNatjecajBodovnaGreske",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajBodovnaGreske_AspNetUsers_UpdatedBy",
                table: "SocijalniNatjecajBodovnaGreske",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajBodovnaGreske_SocijalniNatjecajZahtjevi_ZahtjevId",
                table: "SocijalniNatjecajBodovnaGreske",
                column: "ZahtjevId",
                principalTable: "SocijalniNatjecajZahtjevi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajBodovnaGreske_AspNetUsers_CreatedBy",
                table: "SocijalniNatjecajBodovnaGreske");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajBodovnaGreske_AspNetUsers_UpdatedBy",
                table: "SocijalniNatjecajBodovnaGreske");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajBodovnaGreske_SocijalniNatjecajZahtjevi_ZahtjevId",
                table: "SocijalniNatjecajBodovnaGreske");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SocijalniNatjecajBodovnaGreske",
                table: "SocijalniNatjecajBodovnaGreske");

            migrationBuilder.RenameTable(
                name: "SocijalniNatjecajBodovnaGreske",
                newName: "SocijalniNatjecajBodovnaGreska");

            migrationBuilder.RenameIndex(
                name: "IX_SocijalniNatjecajBodovnaGreske_ZahtjevId",
                table: "SocijalniNatjecajBodovnaGreska",
                newName: "IX_SocijalniNatjecajBodovnaGreska_ZahtjevId");

            migrationBuilder.RenameIndex(
                name: "IX_SocijalniNatjecajBodovnaGreske_UpdatedBy",
                table: "SocijalniNatjecajBodovnaGreska",
                newName: "IX_SocijalniNatjecajBodovnaGreska_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_SocijalniNatjecajBodovnaGreske_CreatedBy",
                table: "SocijalniNatjecajBodovnaGreska",
                newName: "IX_SocijalniNatjecajBodovnaGreska_CreatedBy");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocijalniNatjecajBodovnaGreska",
                table: "SocijalniNatjecajBodovnaGreska",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajBodovnaGreska_AspNetUsers_CreatedBy",
                table: "SocijalniNatjecajBodovnaGreska",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajBodovnaGreska_AspNetUsers_UpdatedBy",
                table: "SocijalniNatjecajBodovnaGreska",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajBodovnaGreska_SocijalniNatjecajZahtjevi_ZahtjevId",
                table: "SocijalniNatjecajBodovnaGreska",
                column: "ZahtjevId",
                principalTable: "SocijalniNatjecajZahtjevi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
