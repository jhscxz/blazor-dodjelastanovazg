using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class AddPostotakProsjekaToPrihodi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PostotakProsjeka",
                table: "SocijalniPrihodi",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostotakProsjeka",
                table: "SocijalniPrihodi");
        }
    }
}
