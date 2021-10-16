using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyBusProject.Migrations
{
    public partial class RouteStop_NewField_RouteDirection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RouteDirection",
                table: "RouteStops",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RouteDirection",
                table: "RouteStops");
        }
    }
}
