using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditFieldsToAllEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajBodovniPodaci_AspNetUsers_EditedByUserId",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.RenameColumn(
                name: "EditedByUserId",
                table: "SocijalniNatjecajBodovniPodaci",
                newName: "UpdatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_SocijalniNatjecajBodovniPodaci_EditedByUserId",
                table: "SocijalniNatjecajBodovniPodaci",
                newName: "IX_SocijalniNatjecajBodovniPodaci_UpdatedByUserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "SocijalniNatjecajKucanstvoPodaci",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SocijalniNatjecajKucanstvoPodaci",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "SocijalniNatjecajKucanstvoPodaci",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "SocijalniNatjecajKucanstvoPodaci",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "SocijalniNatjecajKucanstvoPodaci",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "SocijalniNatjecajClanovi",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SocijalniNatjecajClanovi",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "SocijalniNatjecajClanovi",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "SocijalniNatjecajClanovi",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "SocijalniNatjecajClanovi",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Natjecaji",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Natjecaji",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Natjecaji",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Natjecaji",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "Natjecaji",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajKucanstvoPodaci_CreatedByUserId",
                table: "SocijalniNatjecajKucanstvoPodaci",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajKucanstvoPodaci_UpdatedByUserId",
                table: "SocijalniNatjecajKucanstvoPodaci",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajClanovi_CreatedByUserId",
                table: "SocijalniNatjecajClanovi",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajClanovi_UpdatedByUserId",
                table: "SocijalniNatjecajClanovi",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajBodovniPodaci_CreatedByUserId",
                table: "SocijalniNatjecajBodovniPodaci",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Natjecaji_CreatedByUserId",
                table: "Natjecaji",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Natjecaji_UpdatedByUserId",
                table: "Natjecaji",
                column: "UpdatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Natjecaji_AspNetUsers_CreatedByUserId",
                table: "Natjecaji",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Natjecaji_AspNetUsers_UpdatedByUserId",
                table: "Natjecaji",
                column: "UpdatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajBodovniPodaci_AspNetUsers_CreatedByUserId",
                table: "SocijalniNatjecajBodovniPodaci",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajBodovniPodaci_AspNetUsers_UpdatedByUserId",
                table: "SocijalniNatjecajBodovniPodaci",
                column: "UpdatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajClanovi_AspNetUsers_CreatedByUserId",
                table: "SocijalniNatjecajClanovi",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajClanovi_AspNetUsers_UpdatedByUserId",
                table: "SocijalniNatjecajClanovi",
                column: "UpdatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajKucanstvoPodaci_AspNetUsers_CreatedByUserId",
                table: "SocijalniNatjecajKucanstvoPodaci",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajKucanstvoPodaci_AspNetUsers_UpdatedByUserId",
                table: "SocijalniNatjecajKucanstvoPodaci",
                column: "UpdatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Natjecaji_AspNetUsers_CreatedByUserId",
                table: "Natjecaji");

            migrationBuilder.DropForeignKey(
                name: "FK_Natjecaji_AspNetUsers_UpdatedByUserId",
                table: "Natjecaji");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajBodovniPodaci_AspNetUsers_CreatedByUserId",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajBodovniPodaci_AspNetUsers_UpdatedByUserId",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajClanovi_AspNetUsers_CreatedByUserId",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajClanovi_AspNetUsers_UpdatedByUserId",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajKucanstvoPodaci_AspNetUsers_CreatedByUserId",
                table: "SocijalniNatjecajKucanstvoPodaci");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajKucanstvoPodaci_AspNetUsers_UpdatedByUserId",
                table: "SocijalniNatjecajKucanstvoPodaci");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajKucanstvoPodaci_CreatedByUserId",
                table: "SocijalniNatjecajKucanstvoPodaci");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajKucanstvoPodaci_UpdatedByUserId",
                table: "SocijalniNatjecajKucanstvoPodaci");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajClanovi_CreatedByUserId",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajClanovi_UpdatedByUserId",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajBodovniPodaci_CreatedByUserId",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropIndex(
                name: "IX_Natjecaji_CreatedByUserId",
                table: "Natjecaji");

            migrationBuilder.DropIndex(
                name: "IX_Natjecaji_UpdatedByUserId",
                table: "Natjecaji");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SocijalniNatjecajKucanstvoPodaci");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "SocijalniNatjecajKucanstvoPodaci");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "SocijalniNatjecajKucanstvoPodaci");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "SocijalniNatjecajKucanstvoPodaci");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Natjecaji");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Natjecaji");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Natjecaji");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "Natjecaji");

            migrationBuilder.RenameColumn(
                name: "UpdatedByUserId",
                table: "SocijalniNatjecajBodovniPodaci",
                newName: "EditedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_SocijalniNatjecajBodovniPodaci_UpdatedByUserId",
                table: "SocijalniNatjecajBodovniPodaci",
                newName: "IX_SocijalniNatjecajBodovniPodaci_EditedByUserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "SocijalniNatjecajKucanstvoPodaci",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "SocijalniNatjecajClanovi",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Natjecaji",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajBodovniPodaci_AspNetUsers_EditedByUserId",
                table: "SocijalniNatjecajBodovniPodaci",
                column: "EditedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
