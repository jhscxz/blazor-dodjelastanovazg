using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DodjelaStanovaZG.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Natjecaji",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriustiviIliSocijalni = table.Column<byte>(type: "tinyint", nullable: false),
                    Klasa = table.Column<int>(type: "int", nullable: false),
                    ProsjekPlace = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Zakljucen = table.Column<byte>(type: "tinyint", nullable: false),
                    DatumObjave = table.Column<DateOnly>(type: "date", nullable: false),
                    RokZaPrijavu = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Natjecaji", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Natjecaji_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Natjecaji_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SocijalniNatjecajZahtjevi",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KlasaPredmeta = table.Column<int>(type: "int", nullable: false),
                    DatumPodnosenjaZahtjeva = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RezultatObrade = table.Column<byte>(type: "tinyint", nullable: false),
                    ManualniRezultatObrade = table.Column<byte>(type: "tinyint", nullable: false),
                    NapomenaObrade = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    NatjecajId = table.Column<long>(type: "bigint", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocijalniNatjecajZahtjevi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajZahtjevi_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajZahtjevi_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajZahtjevi_Natjecaji_NatjecajId",
                        column: x => x.NatjecajId,
                        principalTable: "Natjecaji",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SocijalniNatjecajZahtjeviHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "SocijalniNatjecajBodovi",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZahtjevId = table.Column<long>(type: "bigint", nullable: false),
                    BodoviStambeniStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviSastavKucanstva = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviPoClanu = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviMaloljetni = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviPunoljetniUzdrzavani = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviZajamcenaNaknada = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviNjegovatelj = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviDoplatakZaNjegu = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviOdraslihInvalidnina = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviMaloljetnihInvalidnina = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviZrtvaNasilja = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviAlternativnaSkrb = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviIznad55 = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviObrana = table.Column<float>(type: "real", nullable: false),
                    BodoviSeksualnoNasilje = table.Column<byte>(type: "tinyint", nullable: false),
                    BodoviCivilniStradalnici = table.Column<byte>(type: "tinyint", nullable: false),
                    UkupnoBodova = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocijalniNatjecajBodovi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajBodovi_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajBodovi_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajBodovi_SocijalniNatjecajZahtjevi_ZahtjevId",
                        column: x => x.ZahtjevId,
                        principalTable: "SocijalniNatjecajZahtjevi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SocijalniNatjecajBodovnaGreske",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZahtjevId = table.Column<long>(type: "bigint", nullable: false),
                    Kod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Poruka = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocijalniNatjecajBodovnaGreske", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajBodovnaGreske_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajBodovnaGreske_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajBodovnaGreske_SocijalniNatjecajZahtjevi_ZahtjevId",
                        column: x => x.ZahtjevId,
                        principalTable: "SocijalniNatjecajZahtjevi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SocijalniNatjecajBodovniPodaci",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZahtjevId = table.Column<long>(type: "bigint", nullable: false),
                    BrojUzdrzavanePunoljetneDjece = table.Column<byte>(type: "tinyint", nullable: false),
                    PrimateljZajamceneMinimalneNaknade = table.Column<bool>(type: "bit", nullable: false),
                    StatusRoditeljaNjegovatelja = table.Column<bool>(type: "bit", nullable: false),
                    KorisnikDoplatkaZaPomoc = table.Column<bool>(type: "bit", nullable: false),
                    BrojOdraslihKorisnikaInvalidnine = table.Column<byte>(type: "tinyint", nullable: false),
                    BrojMaloljetnihKorisnikaInvalidnine = table.Column<byte>(type: "tinyint", nullable: false),
                    ZrtvaObiteljskogNasilja = table.Column<bool>(type: "bit", nullable: false),
                    BrojOsobaUAlternativnojSkrbi = table.Column<byte>(type: "tinyint", nullable: false),
                    BrojMjeseciObranaSuvereniteta = table.Column<byte>(type: "tinyint", nullable: false),
                    BrojClanovaZrtavaSeksualnogNasilja = table.Column<byte>(type: "tinyint", nullable: false),
                    BrojCivilnihStradalnika = table.Column<byte>(type: "tinyint", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocijalniNatjecajBodovniPodaci", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajBodovniPodaci_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajBodovniPodaci_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
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

            migrationBuilder.CreateTable(
                name: "SocijalniNatjecajClanovi",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZahtjevId = table.Column<long>(type: "bigint", nullable: false),
                    ImePrezime = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Oib = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    Srodstvo = table.Column<int>(type: "int", nullable: true),
                    DatumRodjenja = table.Column<DateOnly>(type: "date", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocijalniNatjecajClanovi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajClanovi_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajClanovi_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajClanovi_SocijalniNatjecajZahtjevi_ZahtjevId",
                        column: x => x.ZahtjevId,
                        principalTable: "SocijalniNatjecajZahtjevi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SocijalniNatjecajClanoviHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "SocijalniNatjecajKucanstvoPodaci",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZahtjevId = table.Column<long>(type: "bigint", nullable: false),
                    PrebivanjeOd = table.Column<DateOnly>(type: "date", nullable: true),
                    StambeniStatusKucanstva = table.Column<byte>(type: "tinyint", nullable: false),
                    SastavKucanstva = table.Column<byte>(type: "tinyint", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodEndColumn", true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:TemporalIsPeriodStartColumn", true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocijalniNatjecajKucanstvoPodaci", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajKucanstvoPodaci_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajKucanstvoPodaci_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniNatjecajKucanstvoPodaci_SocijalniNatjecajZahtjevi_ZahtjevId",
                        column: x => x.ZahtjevId,
                        principalTable: "SocijalniNatjecajZahtjevi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SocijalniNatjecajKucanstvoPodaciHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateTable(
                name: "SocijalniPrihodi",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UkupniPrihodKucanstva = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    PrihodPoClanu = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    PostotakProsjeka = table.Column<decimal>(type: "decimal(7,2)", precision: 7, scale: 2, nullable: true),
                    IspunjavaUvjetPrihoda = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocijalniPrihodi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocijalniPrihodi_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniPrihodi_AspNetUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocijalniPrihodi_SocijalniNatjecajKucanstvoPodaci_Id",
                        column: x => x.Id,
                        principalTable: "SocijalniNatjecajKucanstvoPodaci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Natjecaji_CreatedBy",
                table: "Natjecaji",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Natjecaji_UpdatedBy",
                table: "Natjecaji",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajBodovi_CreatedBy",
                table: "SocijalniNatjecajBodovi",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajBodovi_UpdatedBy",
                table: "SocijalniNatjecajBodovi",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajBodovi_ZahtjevId",
                table: "SocijalniNatjecajBodovi",
                column: "ZahtjevId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajBodovnaGreske_CreatedBy",
                table: "SocijalniNatjecajBodovnaGreske",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajBodovnaGreske_UpdatedBy",
                table: "SocijalniNatjecajBodovnaGreske",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajBodovnaGreske_ZahtjevId",
                table: "SocijalniNatjecajBodovnaGreske",
                column: "ZahtjevId");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajBodovniPodaci_CreatedBy",
                table: "SocijalniNatjecajBodovniPodaci",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajBodovniPodaci_UpdatedBy",
                table: "SocijalniNatjecajBodovniPodaci",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajBodovniPodaci_ZahtjevId",
                table: "SocijalniNatjecajBodovniPodaci",
                column: "ZahtjevId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajClanovi_CreatedBy",
                table: "SocijalniNatjecajClanovi",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajClanovi_UpdatedBy",
                table: "SocijalniNatjecajClanovi",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajClanovi_ZahtjevId",
                table: "SocijalniNatjecajClanovi",
                column: "ZahtjevId");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajKucanstvoPodaci_CreatedBy",
                table: "SocijalniNatjecajKucanstvoPodaci",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajKucanstvoPodaci_UpdatedBy",
                table: "SocijalniNatjecajKucanstvoPodaci",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajKucanstvoPodaci_ZahtjevId",
                table: "SocijalniNatjecajKucanstvoPodaci",
                column: "ZahtjevId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajZahtjevi_CreatedBy",
                table: "SocijalniNatjecajZahtjevi",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajZahtjevi_NatjecajId",
                table: "SocijalniNatjecajZahtjevi",
                column: "NatjecajId");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniNatjecajZahtjevi_UpdatedBy",
                table: "SocijalniNatjecajZahtjevi",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniPrihodi_CreatedBy",
                table: "SocijalniPrihodi",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SocijalniPrihodi_UpdatedBy",
                table: "SocijalniPrihodi",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "SocijalniNatjecajBodovi");

            migrationBuilder.DropTable(
                name: "SocijalniNatjecajBodovnaGreske");

            migrationBuilder.DropTable(
                name: "SocijalniNatjecajBodovniPodaci")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SocijalniNatjecajBodovniPodaciHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "SocijalniNatjecajClanovi")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SocijalniNatjecajClanoviHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "SocijalniPrihodi");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "SocijalniNatjecajKucanstvoPodaci")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SocijalniNatjecajKucanstvoPodaciHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "SocijalniNatjecajZahtjevi")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SocijalniNatjecajZahtjeviHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", null)
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropTable(
                name: "Natjecaji");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
