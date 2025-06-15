using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class AddPrihod2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniPrihod_AspNetUsers_CreatedBy",
                table: "SocijalniPrihod");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniPrihod_AspNetUsers_UpdatedBy",
                table: "SocijalniPrihod");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniPrihod_SocijalniNatjecajKucanstvoPodaci_Id",
                table: "SocijalniPrihod");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SocijalniPrihod",
                table: "SocijalniPrihod");

            migrationBuilder.RenameTable(
                name: "SocijalniPrihod",
                newName: "SocijalniPrihodi");

            migrationBuilder.RenameIndex(
                name: "IX_SocijalniPrihod_UpdatedBy",
                table: "SocijalniPrihodi",
                newName: "IX_SocijalniPrihodi_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_SocijalniPrihod_CreatedBy",
                table: "SocijalniPrihodi",
                newName: "IX_SocijalniPrihodi_CreatedBy");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocijalniPrihodi",
                table: "SocijalniPrihodi",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniPrihodi_AspNetUsers_CreatedBy",
                table: "SocijalniPrihodi",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniPrihodi_AspNetUsers_UpdatedBy",
                table: "SocijalniPrihodi",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniPrihodi_SocijalniNatjecajKucanstvoPodaci_Id",
                table: "SocijalniPrihodi",
                column: "Id",
                principalTable: "SocijalniNatjecajKucanstvoPodaci",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniPrihodi_AspNetUsers_CreatedBy",
                table: "SocijalniPrihodi");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniPrihodi_AspNetUsers_UpdatedBy",
                table: "SocijalniPrihodi");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniPrihodi_SocijalniNatjecajKucanstvoPodaci_Id",
                table: "SocijalniPrihodi");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SocijalniPrihodi",
                table: "SocijalniPrihodi");

            migrationBuilder.RenameTable(
                name: "SocijalniPrihodi",
                newName: "SocijalniPrihod");

            migrationBuilder.RenameIndex(
                name: "IX_SocijalniPrihodi_UpdatedBy",
                table: "SocijalniPrihod",
                newName: "IX_SocijalniPrihod_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_SocijalniPrihodi_CreatedBy",
                table: "SocijalniPrihod",
                newName: "IX_SocijalniPrihod_CreatedBy");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocijalniPrihod",
                table: "SocijalniPrihod",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniPrihod_AspNetUsers_CreatedBy",
                table: "SocijalniPrihod",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniPrihod_AspNetUsers_UpdatedBy",
                table: "SocijalniPrihod",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniPrihod_SocijalniNatjecajKucanstvoPodaci_Id",
                table: "SocijalniPrihod",
                column: "Id",
                principalTable: "SocijalniNatjecajKucanstvoPodaci",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
