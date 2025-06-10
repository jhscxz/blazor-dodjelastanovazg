using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class AddKucanstvoAuditToZahtjev : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "KucanstvoUpdatedAt",
                table: "SocijalniNatjecajZahtjevi",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KucanstvoUpdatedBy",
                table: "SocijalniNatjecajZahtjevi",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KucanstvoUpdatedAt",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropColumn(
                name: "KucanstvoUpdatedBy",
                table: "SocijalniNatjecajZahtjevi");
        }
    }
}
