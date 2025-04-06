using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSocijalniNatjecajBodovniPodaci : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrojGodinaPrebivanja",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropColumn(
                name: "BrojMaloljetneDjece",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropColumn(
                name: "SastavKucanstva",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropColumn(
                name: "StambeniStatusKucanstva",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropColumn(
                name: "UkupniPrihodKucanstva",
                table: "SocijalniNatjecajBodovniPodaci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "BrojGodinaPrebivanja",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "BrojMaloljetneDjece",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "SastavKucanstva",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "StambeniStatusKucanstva",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UkupniPrihodKucanstva",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: true);
        }
    }
}
