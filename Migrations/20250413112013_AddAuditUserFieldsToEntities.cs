using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditUserFieldsToEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Natjecaji_AspNetUsers_CreatedByUserId",
                table: "Natjecaji");

            migrationBuilder.DropForeignKey(
                name: "FK_Natjecaji_AspNetUsers_UpdatedByUserId",
                table: "Natjecaji");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajBodovniPodaci_AspNetUsers_CreatedByUserId",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajBodovniPodaci_AspNetUsers_UpdatedByUserId",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajClanovi_AspNetUsers_CreatedByUserId",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajClanovi_AspNetUsers_UpdatedByUserId",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajKucanstvoPodaci_AspNetUsers_CreatedByUserId",
                table: "SocijalniNatjecajKucanstvoPodaci");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajKucanstvoPodaci_AspNetUsers_UpdatedByUserId",
                table: "SocijalniNatjecajKucanstvoPodaci");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajZahtjevi_AspNetUsers_CreatedByUserId",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajZahtjevi_AspNetUsers_UpdatedByUserId",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajZahtjevi_CreatedByUserId",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajZahtjevi_UpdatedByUserId",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajKucanstvoPodaci_CreatedByUserId",
                table: "SocijalniNatjecajKucanstvoPodaci");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajKucanstvoPodaci_UpdatedByUserId",
                table: "SocijalniNatjecajKucanstvoPodaci");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajClanovi_CreatedByUserId",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajClanovi_UpdatedByUserId",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajBodovniPodaci_CreatedByUserId",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajBodovniPodaci_UpdatedByUserId",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropIndex(
                name: "IX_Natjecaji_CreatedByUserId",
                table: "Natjecaji");

            migrationBuilder.DropIndex(
                name: "IX_Natjecaji_UpdatedByUserId",
                table: "Natjecaji");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "SocijalniNatjecajKucanstvoPodaci");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "SocijalniNatjecajKucanstvoPodaci");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "Natjecaji");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "Natjecaji");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "SocijalniNatjecajZahtjevi",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "SocijalniNatjecajZahtjevi",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "SocijalniNatjecajKucanstvoPodaci",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "SocijalniNatjecajKucanstvoPodaci",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "SocijalniNatjecajClanovi",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "SocijalniNatjecajClanovi",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Natjecaji",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Natjecaji",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajZahtjevi_CreatedBy",
                table: "SocijalniNatjecajZahtjevi",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajZahtjevi_UpdatedBy",
                table: "SocijalniNatjecajZahtjevi",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajKucanstvoPodaci_CreatedBy",
                table: "SocijalniNatjecajKucanstvoPodaci",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajKucanstvoPodaci_UpdatedBy",
                table: "SocijalniNatjecajKucanstvoPodaci",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajClanovi_CreatedBy",
                table: "SocijalniNatjecajClanovi",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajClanovi_UpdatedBy",
                table: "SocijalniNatjecajClanovi",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajBodovniPodaci_CreatedBy",
                table: "SocijalniNatjecajBodovniPodaci",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajBodovniPodaci_UpdatedBy",
                table: "SocijalniNatjecajBodovniPodaci",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Natjecaji_CreatedBy",
                table: "Natjecaji",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Natjecaji_UpdatedBy",
                table: "Natjecaji",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Natjecaji_AspNetUsers_CreatedBy",
                table: "Natjecaji",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Natjecaji_AspNetUsers_UpdatedBy",
                table: "Natjecaji",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajBodovniPodaci_AspNetUsers_CreatedBy",
                table: "SocijalniNatjecajBodovniPodaci",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajBodovniPodaci_AspNetUsers_UpdatedBy",
                table: "SocijalniNatjecajBodovniPodaci",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajClanovi_AspNetUsers_CreatedBy",
                table: "SocijalniNatjecajClanovi",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajClanovi_AspNetUsers_UpdatedBy",
                table: "SocijalniNatjecajClanovi",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajKucanstvoPodaci_AspNetUsers_CreatedBy",
                table: "SocijalniNatjecajKucanstvoPodaci",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajKucanstvoPodaci_AspNetUsers_UpdatedBy",
                table: "SocijalniNatjecajKucanstvoPodaci",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajZahtjevi_AspNetUsers_CreatedBy",
                table: "SocijalniNatjecajZahtjevi",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajZahtjevi_AspNetUsers_UpdatedBy",
                table: "SocijalniNatjecajZahtjevi",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Natjecaji_AspNetUsers_CreatedBy",
                table: "Natjecaji");

            migrationBuilder.DropForeignKey(
                name: "FK_Natjecaji_AspNetUsers_UpdatedBy",
                table: "Natjecaji");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajBodovniPodaci_AspNetUsers_CreatedBy",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajBodovniPodaci_AspNetUsers_UpdatedBy",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajClanovi_AspNetUsers_CreatedBy",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajClanovi_AspNetUsers_UpdatedBy",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajKucanstvoPodaci_AspNetUsers_CreatedBy",
                table: "SocijalniNatjecajKucanstvoPodaci");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajKucanstvoPodaci_AspNetUsers_UpdatedBy",
                table: "SocijalniNatjecajKucanstvoPodaci");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajZahtjevi_AspNetUsers_CreatedBy",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropForeignKey(
                name: "FK_SocijalniNatjecajZahtjevi_AspNetUsers_UpdatedBy",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajZahtjevi_CreatedBy",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajZahtjevi_UpdatedBy",
                table: "SocijalniNatjecajZahtjevi");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajKucanstvoPodaci_CreatedBy",
                table: "SocijalniNatjecajKucanstvoPodaci");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajKucanstvoPodaci_UpdatedBy",
                table: "SocijalniNatjecajKucanstvoPodaci");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajClanovi_CreatedBy",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajClanovi_UpdatedBy",
                table: "SocijalniNatjecajClanovi");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajBodovniPodaci_CreatedBy",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropIndex(
                name: "IX_SocijalniNatjecajBodovniPodaci_UpdatedBy",
                table: "SocijalniNatjecajBodovniPodaci");

            migrationBuilder.DropIndex(
                name: "IX_Natjecaji_CreatedBy",
                table: "Natjecaji");

            migrationBuilder.DropIndex(
                name: "IX_Natjecaji_UpdatedBy",
                table: "Natjecaji");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "SocijalniNatjecajZahtjevi",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "SocijalniNatjecajZahtjevi",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "SocijalniNatjecajZahtjevi",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "SocijalniNatjecajZahtjevi",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "SocijalniNatjecajKucanstvoPodaci",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "SocijalniNatjecajKucanstvoPodaci",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "SocijalniNatjecajKucanstvoPodaci",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "SocijalniNatjecajKucanstvoPodaci",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "SocijalniNatjecajClanovi",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "SocijalniNatjecajClanovi",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "SocijalniNatjecajClanovi",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "SocijalniNatjecajClanovi",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "SocijalniNatjecajBodovniPodaci",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Natjecaji",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Natjecaji",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "Natjecaji",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByUserId",
                table: "Natjecaji",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajZahtjevi_CreatedByUserId",
                table: "SocijalniNatjecajZahtjevi",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajZahtjevi_UpdatedByUserId",
                table: "SocijalniNatjecajZahtjevi",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajKucanstvoPodaci_CreatedByUserId",
                table: "SocijalniNatjecajKucanstvoPodaci",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajKucanstvoPodaci_UpdatedByUserId",
                table: "SocijalniNatjecajKucanstvoPodaci",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajClanovi_CreatedByUserId",
                table: "SocijalniNatjecajClanovi",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajClanovi_UpdatedByUserId",
                table: "SocijalniNatjecajClanovi",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajBodovniPodaci_CreatedByUserId",
                table: "SocijalniNatjecajBodovniPodaci",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajBodovniPodaci_UpdatedByUserId",
                table: "SocijalniNatjecajBodovniPodaci",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Natjecaji_CreatedByUserId",
                table: "Natjecaji",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Natjecaji_UpdatedByUserId",
                table: "Natjecaji",
                column: "UpdatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Natjecaji_AspNetUsers_CreatedByUserId",
                table: "Natjecaji",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Natjecaji_AspNetUsers_UpdatedByUserId",
                table: "Natjecaji",
                column: "UpdatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajBodovniPodaci_AspNetUsers_CreatedByUserId",
                table: "SocijalniNatjecajBodovniPodaci",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajBodovniPodaci_AspNetUsers_UpdatedByUserId",
                table: "SocijalniNatjecajBodovniPodaci",
                column: "UpdatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajClanovi_AspNetUsers_CreatedByUserId",
                table: "SocijalniNatjecajClanovi",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajClanovi_AspNetUsers_UpdatedByUserId",
                table: "SocijalniNatjecajClanovi",
                column: "UpdatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajKucanstvoPodaci_AspNetUsers_CreatedByUserId",
                table: "SocijalniNatjecajKucanstvoPodaci",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajKucanstvoPodaci_AspNetUsers_UpdatedByUserId",
                table: "SocijalniNatjecajKucanstvoPodaci",
                column: "UpdatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajZahtjevi_AspNetUsers_CreatedByUserId",
                table: "SocijalniNatjecajZahtjevi",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SocijalniNatjecajZahtjevi_AspNetUsers_UpdatedByUserId",
                table: "SocijalniNatjecajZahtjevi",
                column: "UpdatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
