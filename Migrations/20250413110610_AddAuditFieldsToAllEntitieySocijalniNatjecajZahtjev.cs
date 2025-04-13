using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditFieldsToAllEntitieySocijalniNatjecajZahtjev : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajZahtjevi_AspNetUsers_CreatedBy",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajZahtjevi_CreatedBy",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "SocijalniNatjecajZahtjevi",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "SocijalniNatjecajZahtjevi",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "SocijalniNatjecajZahtjevi",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "SocijalniNatjecajZahtjevi",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "SocijalniNatjecajZahtjevi",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajZahtjevi_CreatedByUserId",
                table: "SocijalniNatjecajZahtjevi",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajZahtjevi_UpdatedByUserId",
                table: "SocijalniNatjecajZahtjevi",
                column: "UpdatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajZahtjevi_AspNetUsers_CreatedByUserId",
                table: "SocijalniNatjecajZahtjevi",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajZahtjevi_AspNetUsers_UpdatedByUserId",
                table: "SocijalniNatjecajZahtjevi",
                column: "UpdatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajZahtjevi_AspNetUsers_CreatedByUserId",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajZahtjevi_AspNetUsers_UpdatedByUserId",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajZahtjevi_CreatedByUserId",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajZahtjevi_UpdatedByUserId",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "SocijalniNatjecajZahtjevi",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "SocijalniNatjecajZahtjevi",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajZahtjevi_CreatedBy",
                table: "SocijalniNatjecajZahtjevi",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajZahtjevi_AspNetUsers_CreatedBy",
                table: "SocijalniNatjecajZahtjevi",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
