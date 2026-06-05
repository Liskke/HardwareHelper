using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareHelper.Data.Migrations
{
    /// <inheritdoc />
    public partial class DodaniePodsumowanKosztow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "KosztRobocizny",
                table: "Zlecenia",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PodsumowanieNaprawy",
                table: "Zlecenia",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KosztRobocizny",
                table: "Zlecenia");

            migrationBuilder.DropColumn(
                name: "PodsumowanieNaprawy",
                table: "Zlecenia");
        }
    }
}
