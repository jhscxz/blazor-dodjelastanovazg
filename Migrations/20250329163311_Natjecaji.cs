using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class Natjecaji : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.CreateTable(
                name: "Natjecaji",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriustiviIliSocijalni = table.Column<byte>(type: "tinyint", nullable: false),
                    Godina = table.Column<int>(type: "int", nullable: false),
                    Klasa = table.Column<int>(type: "int", nullable: false),
                    ProsjekPlace = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Zakljucen = table.Column<byte>(type: "tinyint", nullable: false),
                    DatumObjave = table.Column<DateOnly>(type: "date", nullable: false),
                    RokZaPrijavu = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Natjecaji", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SocijalniNatjecaji",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KlasaPredmeta = table.Column<int>(type: "int", nullable: false),
                    DatumPodnosenjaZahtjeva = table.Column<DateOnly>(type: "date", nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UkupniPrihodKucanstva = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    StambeniStatusKucanstva = table.Column<byte>(type: "tinyint", nullable: false),
                    SastavKucanstva = table.Column<byte>(type: "tinyint", nullable: false),
                    Aktivan = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    EditedBy = table.Column<long>(type: "bigint", nullable: true),
                    NatjecajId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EditedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocijalniNatjecaji", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecaji_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecaji_AspNetUsers_EditedByUserId",
                        column: x => x.EditedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecaji_Natjecaji_NatjecajId",
                        column: x => x.NatjecajId,
                        principalTable: "Natjecaji",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecaji_CreatedByUserId",
                table: "SocijalniNatjecaji",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecaji_EditedByUserId",
                table: "SocijalniNatjecaji",
                column: "EditedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecaji_NatjecajId",
                table: "SocijalniNatjecaji",
                column: "NatjecajId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocijalniNatjecaji");

            migrationBuilder.DropTable(
                name: "Natjecaji");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }
    }
}
