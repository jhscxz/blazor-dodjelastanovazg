using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSocijalniNatjecajBodovniPodaci2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrojClanovaZrtavaSeksualnogNasiljaDomovinskiRat",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropColumn(
                name: "ManjeOd35Godina",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropColumn(
                name: "ObrazovanjeBaccMaster",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.AlterColumn<bool>(
                name: "ZrtvaObiteljskogNasilja",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "BrojUzdrzavanePunoljetneDjece",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "BrojOsobaUAlternativnojSkrbi",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "BrojOdraslihKorisnikaInvalidnine",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "BrojMjeseciObranaSuvereniteta",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "BrojMaloljetnihKorisnikaInvalidnine",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "BrojCivilnihStradalnika",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "BrojClanovaZrtavaSeksualnogNasilja",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<bool>(
                name: "KorisnikDoplatkaZaPomoc",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PrimateljZajamceneMinimalneNaknade",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "StatusRoditeljaNjegovatelja",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrojClanovaZrtavaSeksualnogNasilja",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropColumn(
                name: "KorisnikDoplatkaZaPomoc",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropColumn(
                name: "PrimateljZajamceneMinimalneNaknade",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropColumn(
                name: "StatusRoditeljaNjegovatelja",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.AlterColumn<byte>(
                name: "ZrtvaObiteljskogNasilja",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<byte>(
                name: "BrojUzdrzavanePunoljetneDjece",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<byte>(
                name: "BrojOsobaUAlternativnojSkrbi",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<byte>(
                name: "BrojOdraslihKorisnikaInvalidnine",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<byte>(
                name: "BrojMjeseciObranaSuvereniteta",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<byte>(
                name: "BrojMaloljetnihKorisnikaInvalidnine",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<byte>(
                name: "BrojCivilnihStradalnika",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AddColumn<byte>(
                name: "BrojClanovaZrtavaSeksualnogNasiljaDomovinskiRat",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "ManjeOd35Godina",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "ObrazovanjeBaccMaster",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "tinyint",
                nullable: true);
        }
    }
}
