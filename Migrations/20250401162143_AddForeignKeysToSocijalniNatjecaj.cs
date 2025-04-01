using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeysToSocijalniNatjecaj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecaji_AspNetUsers_CreatedByUserId",
                table: "SocijalniNatjecaji");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecaji_AspNetUsers_EditedByUserId",
                table: "SocijalniNatjecaji");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecaji_CreatedByUserId",
                table: "SocijalniNatjecaji");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecaji_EditedByUserId",
                table: "SocijalniNatjecaji");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "SocijalniNatjecaji");

            migrationBuilder.DropColumn(
                name: "EditedByUserId",
                table: "SocijalniNatjecaji");

            migrationBuilder.AlterColumn<string>(
                name: "EditedBy",
                table: "SocijalniNatjecaji",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "SocijalniNatjecaji",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecaji_CreatedBy",
                table: "SocijalniNatjecaji",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecaji_EditedBy",
                table: "SocijalniNatjecaji",
                column: "EditedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecaji_AspNetUsers_CreatedBy",
                table: "SocijalniNatjecaji",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecaji_AspNetUsers_EditedBy",
                table: "SocijalniNatjecaji",
                column: "EditedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecaji_AspNetUsers_CreatedBy",
                table: "SocijalniNatjecaji");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecaji_AspNetUsers_EditedBy",
                table: "SocijalniNatjecaji");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecaji_CreatedBy",
                table: "SocijalniNatjecaji");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecaji_EditedBy",
                table: "SocijalniNatjecaji");

            migrationBuilder.AlterColumn<long>(
                name: "EditedBy",
                table: "SocijalniNatjecaji",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreatedBy",
                table: "SocijalniNatjecaji",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "SocijalniNatjecaji",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedByUserId",
                table: "SocijalniNatjecaji",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecaji_CreatedByUserId",
                table: "SocijalniNatjecaji",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecaji_EditedByUserId",
                table: "SocijalniNatjecaji",
                column: "EditedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecaji_AspNetUsers_CreatedByUserId",
                table: "SocijalniNatjecaji",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecaji_AspNetUsers_EditedByUserId",
                table: "SocijalniNatjecaji",
                column: "EditedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
