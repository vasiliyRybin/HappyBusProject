using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyBusProject.Migrations
{
    public partial class Try4RestoreRouteStop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RouteStops",
                columns: table => new
                {
                    PointID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RouteLengthKM = table.Column<int>(type: "int", nullable: false),
                    RouteDirection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Test = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteStops", x => x.PointID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RouteStops");
        }
    }
}
