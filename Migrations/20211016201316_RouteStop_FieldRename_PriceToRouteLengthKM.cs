using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyBusProject.Migrations
{
    public partial class RouteStop_FieldRename_PriceToRouteLengthKM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "RouteStops");

            migrationBuilder.AddColumn<int>(
                name: "RouteLengthKM",
                table: "RouteStops",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RouteLengthKM",
                table: "RouteStops");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "RouteStops",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
