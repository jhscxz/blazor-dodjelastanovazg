using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSocijalniZahtjevModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajClanovi_SocijalniNatjecajZahtjevi_NatjecajId",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajZahtjevi_AspNetUsers_EditedBy",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropTable(
                name: "DostavljenaDokumentacijaClanova");

            migrationBuilder.DropTable(
                name: "SocijalniKucanstvoDokumenti");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajZahtjevi_EditedBy",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SocijalniNatjecajClanovi",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropColumn(
                name: "EditedBy",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.RenameTable(
                name: "SocijalniNatjecajClanovi",
                newName: "SocijalniNatjecajClan");

            migrationBuilder.RenameColumn(
                name: "Aktivan",
                table: "SocijalniNatjecajZahtjevi",
                newName: "RezultatObrade");

            migrationBuilder.RenameIndex(
                name: "IX_SocijalniNatjecajClanovi_NatjecajId",
                table: "SocijalniNatjecajClan",
                newName: "IX_SocijalniNatjecajClan_NatjecajId");

            migrationBuilder.AlterTable(
                name: "SocijalniNatjecajZahtjevi")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SocijalniNatjecajZahtjeviHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AlterColumn<decimal>(
                name: "UkupniPrihodKucanstva",
                table: "SocijalniNatjecajZahtjevi",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AlterColumn<byte>(
                name: "StambeniStatusKucanstva",
                table: "SocijalniNatjecajZahtjevi",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<byte>(
                name: "SastavKucanstva",
                table: "SocijalniNatjecajZahtjevi",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<bool>(
                name: "ImaUseljivuNekretninu",
                table: "SocijalniNatjecajZahtjevi",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<byte>(
                name: "BrojGodinaPrebivanja",
                table: "SocijalniNatjecajZahtjevi",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<string>(
                name: "Adresa",
                table: "SocijalniNatjecajZahtjevi",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NapomenaObrade",
                table: "SocijalniNatjecajZahtjevi",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PeriodEnd",
                table: "SocijalniNatjecajZahtjevi",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .Annotation("SqlServer:TemporalIsPeriodEndColumn", true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PeriodStart",
                table: "SocijalniNatjecajZahtjevi",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .Annotation("SqlServer:TemporalIsPeriodStartColumn", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocijalniNatjecajClan",
                table: "SocijalniNatjecajClan",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajClan_SocijalniNatjecajZahtjevi_NatjecajId",
                table: "SocijalniNatjecajClan",
                column: "NatjecajId",
                principalTable: "SocijalniNatjecajZahtjevi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajClan_SocijalniNatjecajZahtjevi_NatjecajId",
                table: "SocijalniNatjecajClan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SocijalniNatjecajClan",
                table: "SocijalniNatjecajClan");

            migrationBuilder.DropColumn(
                name: "NapomenaObrade",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropColumn(
                name: "PeriodEnd",
                table: "SocijalniNatjecajZahtjevi")
                .Annotation("SqlServer:TemporalIsPeriodEndColumn", true);

            migrationBuilder.DropColumn(
                name: "PeriodStart",
                table: "SocijalniNatjecajZahtjevi")
                .Annotation("SqlServer:TemporalIsPeriodStartColumn", true);

            migrationBuilder.RenameTable(
                name: "SocijalniNatjecajClan",
                newName: "SocijalniNatjecajClanovi");

            migrationBuilder.RenameColumn(
                name: "RezultatObrade",
                table: "SocijalniNatjecajZahtjevi",
                newName: "Aktivan");

            migrationBuilder.RenameIndex(
                name: "IX_SocijalniNatjecajClan_NatjecajId",
                table: "SocijalniNatjecajClanovi",
                newName: "IX_SocijalniNatjecajClanovi_NatjecajId");

            migrationBuilder.AlterTable(
                name: "SocijalniNatjecajZahtjevi")
                .OldAnnotation("SqlServer:IsTemporal", true)
                .OldAnnotation("SqlServer:TemporalHistoryTableName", "SocijalniNatjecajZahtjeviHistory")
                .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
                .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AlterColumn<decimal>(
                name: "UkupniPrihodKucanstva",
                table: "SocijalniNatjecajZahtjevi",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "StambeniStatusKucanstva",
                table: "SocijalniNatjecajZahtjevi",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "SastavKucanstva",
                table: "SocijalniNatjecajZahtjevi",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "ImaUseljivuNekretninu",
                table: "SocijalniNatjecajZahtjevi",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "BrojGodinaPrebivanja",
                table: "SocijalniNatjecajZahtjevi",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Adresa",
                table: "SocijalniNatjecajZahtjevi",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EditedBy",
                table: "SocijalniNatjecajZahtjevi",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocijalniNatjecajClanovi",
                table: "SocijalniNatjecajClanovi",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DostavljenaDokumentacijaClanova",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClanId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Dokument = table.Column<byte>(type: "tinyint", nullable: false),
                    Dostavljeno = table.Column<bool>(type: "bit", nullable: false),
                    Napomena = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DostavljenaDokumentacijaClanova", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DostavljenaDokumentacijaClanova_SocijalniNatjecajClanovi_ClanId",
                        column: x => x.ClanId,
                        principalTable: "SocijalniNatjecajClanovi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SocijalniKucanstvoDokumenti",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NatjecajZahtjevId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Dokument = table.Column<byte>(type: "tinyint", nullable: false),
                    Dostavljeno = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocijalniKucanstvoDokumenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocijalniKucanstvoDokumenti_SocijalniNatjecajZahtjevi_NatjecajZahtjevId",
                        column: x => x.NatjecajZahtjevId,
                        principalTable: "SocijalniNatjecajZahtjevi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajZahtjevi_EditedBy",
                table: "SocijalniNatjecajZahtjevi",
                column: "EditedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DostavljenaDokumentacijaClanova_ClanId",
                table: "DostavljenaDokumentacijaClanova",
                column: "ClanId");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniKucanstvoDokumenti_NatjecajZahtjevId",
                table: "SocijalniKucanstvoDokumenti",
                column: "NatjecajZahtjevId");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajClanovi_SocijalniNatjecajZahtjevi_NatjecajId",
                table: "SocijalniNatjecajClanovi",
                column: "NatjecajId",
                principalTable: "SocijalniNatjecajZahtjevi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajZahtjevi_AspNetUsers_EditedBy",
                table: "SocijalniNatjecajZahtjevi",
                column: "EditedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
