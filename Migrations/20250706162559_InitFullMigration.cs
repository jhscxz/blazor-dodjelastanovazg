using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class InitFullMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PosjedujeNekretninuZG",
                table: "SocijalniNatjecajZahtjevi",
                newName: "PosjedujeNekretninuZg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PosjedujeNekretninuZg",
                table: "SocijalniNatjecajZahtjevi",
                newName: "PosjedujeNekretninuZG");
        }
    }
}
