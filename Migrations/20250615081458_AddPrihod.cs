using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class AddPrihod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UkupniPrihodKucanstva",
                table: "SocijalniNatjecajKucanstvoPodaci");

            migrationBuilder.CreateTable(
                name: "SocijalniPrihod",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UkupniPrihodKucanstva = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    PrihodPoClanu = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    IspunjavaUvjetPrihoda = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocijalniPrihod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocijalniPrihod_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniPrihod_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniPrihod_SocijalniNatjecajKucanstvoPodaci_Id",
                        column: x => x.Id,
                        principalTable: "SocijalniNatjecajKucanstvoPodaci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniPrihod_CreatedBy",
                table: "SocijalniPrihod",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniPrihod_UpdatedBy",
                table: "SocijalniPrihod",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocijalniPrihod");

            migrationBuilder.AddColumn<decimal>(
                name: "UkupniPrihodKucanstva",
                table: "SocijalniNatjecajKucanstvoPodaci",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
