using Microsoft.EntityFrameworkCore.Migrations;

namespace BethanyPieShop.Migrations
{
    public partial class addOrder4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdressLine1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AdressLine2",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine1",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressLine1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "AdressLine1",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdressLine2",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
