using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class AddPostotakProsjekaToPrihodi2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PostotakProsjeka",
                table: "SocijalniPrihodi",
                type: "decimal(7,2)",
                precision: 7,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PostotakProsjeka",
                table: "SocijalniPrihodi",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(7,2)",
                oldPrecision: 7,
                oldScale: 2,
                oldNullable: true);
        }
    }
}
