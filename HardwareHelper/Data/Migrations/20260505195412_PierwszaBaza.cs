using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareHelper.Data.Migrations
{
    /// <inheritdoc />
    public partial class PierwszaBaza : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CzescZamienne",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cena = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CzescZamienne", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zlecenia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypUrzadzenia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Producent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumerSeryjny = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpisUsterki = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CzyJestZasilacz = table.Column<bool>(type: "bit", nullable: false),
                    NaprawaGwarancyjna = table.Column<bool>(type: "bit", nullable: false),
                    DataZakupu = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KodPocztowy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Miasto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ulica = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Numer = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ServiceNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zlecenia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zlecenia_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CzescZamiennaZlecenie",
                columns: table => new
                {
                    CzescZamiennaId = table.Column<int>(type: "int", nullable: false),
                    ZleceniaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CzescZamiennaZlecenie", x => new { x.CzescZamiennaId, x.ZleceniaId });
                    table.ForeignKey(
                        name: "FK_CzescZamiennaZlecenie_CzescZamienne_CzescZamiennaId",
                        column: x => x.CzescZamiennaId,
                        principalTable: "CzescZamienne",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CzescZamiennaZlecenie_Zlecenia_ZleceniaId",
                        column: x => x.ZleceniaId,
                        principalTable: "Zlecenia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wiadomosci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tresc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataWyslania = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ZlecenieId = table.Column<int>(type: "int", nullable: false),
                    NadawcaId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wiadomosci", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wiadomosci_AspNetUsers_NadawcaId",
                        column: x => x.NadawcaId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Wiadomosci_Zlecenia_ZlecenieId",
                        column: x => x.ZlecenieId,
                        principalTable: "Zlecenia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CzescZamiennaZlecenie_ZleceniaId",
                table: "CzescZamiennaZlecenie",
                column: "ZleceniaId");

            migrationBuilder.CreateIndex(
                name: "IX_Wiadomosci_NadawcaId",
                table: "Wiadomosci",
                column: "NadawcaId");

            migrationBuilder.CreateIndex(
                name: "IX_Wiadomosci_ZlecenieId",
                table: "Wiadomosci",
                column: "ZlecenieId");

            migrationBuilder.CreateIndex(
                name: "IX_Zlecenia_UserId",
                table: "Zlecenia",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CzescZamiennaZlecenie");

            migrationBuilder.DropTable(
                name: "Wiadomosci");

            migrationBuilder.DropTable(
                name: "CzescZamienne");

            migrationBuilder.DropTable(
                name: "Zlecenia");
        }
    }
}
