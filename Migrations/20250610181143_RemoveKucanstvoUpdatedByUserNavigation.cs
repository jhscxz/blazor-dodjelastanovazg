using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class RemoveKucanstvoUpdatedByUserNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajZahtjevi_AspNetUsers_KucanstvoUpdatedBy",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajZahtjevi_KucanstvoUpdatedBy",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropColumn(
                name: "KucanstvoUpdatedAt",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropColumn(
                name: "KucanstvoUpdatedBy",
                table: "SocijalniNatjecajZahtjevi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "KucanstvoUpdatedAt",
                table: "SocijalniNatjecajZahtjevi",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KucanstvoUpdatedBy",
                table: "SocijalniNatjecajZahtjevi",
                type: "nvarchar(450)",
                nullable: true);

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
    }
}
