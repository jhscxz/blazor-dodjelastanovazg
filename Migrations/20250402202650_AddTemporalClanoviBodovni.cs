using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class AddTemporalClanoviBodovni : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajClan_SocijalniNatjecajZahtjevi_NatjecajId",
                table: "SocijalniNatjecajClan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SocijalniNatjecajClan",
                table: "SocijalniNatjecajClan");

            migrationBuilder.DropColumn(
                name: "BrojGodinaPrebivanja",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropColumn(
                name: "ImaUseljivuNekretninu",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropColumn(
                name: "SastavKucanstva",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropColumn(
                name: "StambeniStatusKucanstva",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropColumn(
                name: "UkupniPrihodKucanstva",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropColumn(
                name: "Adresa",
                table: "SocijalniNatjecajClan");

            migrationBuilder.DropColumn(
                name: "DatumRodenja",
                table: "SocijalniNatjecajClan");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "SocijalniNatjecajClan");

            migrationBuilder.DropColumn(
                name: "ImeIPrezime",
                table: "SocijalniNatjecajClan");

            migrationBuilder.RenameTable(
                name: "SocijalniNatjecajClan",
                newName: "SocijalniNatjecajClanovi");

            migrationBuilder.RenameColumn(
                name: "NatjecajId",
                table: "SocijalniNatjecajClanovi",
                newName: "ZahtjevId");

            migrationBuilder.RenameIndex(
                name: "IX_SocijalniNatjecajClan_NatjecajId",
                table: "SocijalniNatjecajClanovi",
                newName: "IX_SocijalniNatjecajClanovi_ZahtjevId");

            migrationBuilder.AlterTable(
                name: "SocijalniNatjecajClanovi")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SocijalniNatjecajClanoviHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AddColumn<string>(
                name: "ImePrezime",
                table: "SocijalniNatjecajClanovi",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PeriodEnd",
                table: "SocijalniNatjecajClanovi",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .Annotation("SqlServer:TemporalIsPeriodEndColumn", true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PeriodStart",
                table: "SocijalniNatjecajClanovi",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .Annotation("SqlServer:TemporalIsPeriodStartColumn", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocijalniNatjecajClanovi",
                table: "SocijalniNatjecajClanovi",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SocijalniNatjecajBodovniPodaci",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZahtjevId = table.Column<long>(type: "bigint", nullable: false),
                    UkupniPrihodKucanstva = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    BrojGodinaPrebivanja = table.Column<byte>(type: "tinyint", nullable: true),
                    StambeniStatusKucanstva = table.Column<byte>(type: "tinyint", nullable: true),
                    SastavKucanstva = table.Column<byte>(type: "tinyint", nullable: true),
                    BrojMaloljetneDjece = table.Column<byte>(type: "tinyint", nullable: true),
                    BrojUzdrzavanePunoljetneDjece = table.Column<byte>(type: "tinyint", nullable: true),
                    BrojMaloljetnihKorisnikaInvalidnine = table.Column<byte>(type: "tinyint", nullable: true),
                    BrojOdraslihKorisnikaInvalidnine = table.Column<byte>(type: "tinyint", nullable: true),
                    ZrtvaObiteljskogNasilja = table.Column<byte>(type: "tinyint", nullable: true),
                    BrojOsobaUAlternativnojSkrbi = table.Column<byte>(type: "tinyint", nullable: true),
                    BrojMjeseciObranaSuvereniteta = table.Column<byte>(type: "tinyint", nullable: true),
                    BrojClanovaZrtavaSeksualnogNasiljaDomovinskiRat = table.Column<byte>(type: "tinyint", nullable: true),
                    BrojCivilnihStradalnika = table.Column<byte>(type: "tinyint", nullable: true),
                    ManjeOd35Godina = table.Column<byte>(type: "tinyint", nullable: true),
                    ObrazovanjeBaccMaster = table.Column<byte>(type: "tinyint", nullable: true),
                    EditedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    EditedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocijalniNatjecajBodovniPodaci", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajBodovniPodaci_AspNetUsers_EditedByUserId",
                        column: x => x.EditedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajBodovniPodaci_SocijalniNatjecajZahtjevi_ZahtjevId",
                        column: x => x.ZahtjevId,
                        principalTable: "SocijalniNatjecajZahtjevi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SocijalniNatjecajBodovniPodaciHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajBodovniPodaci_EditedByUserId",
                table: "SocijalniNatjecajBodovniPodaci",
                column: "EditedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajBodovniPodaci_ZahtjevId",
                table: "SocijalniNatjecajBodovniPodaci",
                column: "ZahtjevId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajClanovi_SocijalniNatjecajZahtjevi_ZahtjevId",
                table: "SocijalniNatjecajClanovi",
                column: "ZahtjevId",
                principalTable: "SocijalniNatjecajZahtjevi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajClanovi_SocijalniNatjecajZahtjevi_ZahtjevId",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropTable(
                name: "SocijalniNatjecajBodovniPodaci")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SocijalniNatjecajBodovniPodaciHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SocijalniNatjecajClanovi",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropColumn(
                name: "ImePrezime",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropColumn(
                name: "PeriodEnd",
                table: "SocijalniNatjecajClanovi")
                .Annotation("SqlServer:TemporalIsPeriodEndColumn", true);

            migrationBuilder.DropColumn(
                name: "PeriodStart",
                table: "SocijalniNatjecajClanovi")
                .Annotation("SqlServer:TemporalIsPeriodStartColumn", true);

            migrationBuilder.RenameTable(
                name: "SocijalniNatjecajClanovi",
                newName: "SocijalniNatjecajClan")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SocijalniNatjecajClanoviHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.RenameColumn(
                name: "ZahtjevId",
                table: "SocijalniNatjecajClan",
                newName: "NatjecajId");

            migrationBuilder.RenameIndex(
                name: "IX_SocijalniNatjecajClanovi_ZahtjevId",
                table: "SocijalniNatjecajClan",
                newName: "IX_SocijalniNatjecajClan_NatjecajId");

            migrationBuilder.AlterTable(
                name: "SocijalniNatjecajClan")
                .OldAnnotation("SqlServer:IsTemporal", true)
                .OldAnnotation("SqlServer:TemporalHistoryTableName", "SocijalniNatjecajClanoviHistory")
                .OldAnnotation("SqlServer:TemporalHistoryTableSchema", null)
                .OldAnnotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .OldAnnotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AddColumn<byte>(
                name: "BrojGodinaPrebivanja",
                table: "SocijalniNatjecajZahtjevi",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ImaUseljivuNekretninu",
                table: "SocijalniNatjecajZahtjevi",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "SastavKucanstva",
                table: "SocijalniNatjecajZahtjevi",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "StambeniStatusKucanstva",
                table: "SocijalniNatjecajZahtjevi",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UkupniPrihodKucanstva",
                table: "SocijalniNatjecajZahtjevi",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Adresa",
                table: "SocijalniNatjecajClan",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DatumRodenja",
                table: "SocijalniNatjecajClan",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "SocijalniNatjecajClan",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImeIPrezime",
                table: "SocijalniNatjecajClan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
    }
}
