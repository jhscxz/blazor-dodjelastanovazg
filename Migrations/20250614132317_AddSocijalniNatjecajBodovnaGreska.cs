using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class AddSocijalniNatjecajBodovnaGreska : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SocijalniNatjecajBodovnaGreska",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZahtjevId = table.Column<long>(type: "bigint", nullable: false),
                    Kod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Poruka = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocijalniNatjecajBodovnaGreska", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajBodovnaGreska_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajBodovnaGreska_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajBodovnaGreska_SocijalniNatjecajZahtjevi_ZahtjevId",
                        column: x => x.ZahtjevId,
                        principalTable: "SocijalniNatjecajZahtjevi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajBodovnaGreska_CreatedBy",
                table: "SocijalniNatjecajBodovnaGreska",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajBodovnaGreska_UpdatedBy",
                table: "SocijalniNatjecajBodovnaGreska",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajBodovnaGreska_ZahtjevId",
                table: "SocijalniNatjecajBodovnaGreska",
                column: "ZahtjevId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocijalniNatjecajBodovnaGreska");
        }
    }
}
