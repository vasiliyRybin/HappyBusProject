using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyBusProject.Migrations
{
    public partial class RouteStop_RemovedTestColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test",
                table: "RouteStops");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Test",
                table: "RouteStops",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
