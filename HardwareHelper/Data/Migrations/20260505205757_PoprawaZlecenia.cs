using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareHelper.Data.Migrations
{
    /// <inheritdoc />
    public partial class PoprawaZlecenia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TypUrzadzenia",
                table: "Zlecenia",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "NumerZlecenia",
                table: "Zlecenia",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PrzewidywanaDataZakonczenia",
                table: "Zlecenia",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumerZlecenia",
                table: "Zlecenia");

            migrationBuilder.DropColumn(
                name: "PrzewidywanaDataZakonczenia",
                table: "Zlecenia");

            migrationBuilder.AlterColumn<string>(
                name: "TypUrzadzenia",
                table: "Zlecenia",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
