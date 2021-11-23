using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace HappyBusProject.Migrations
{
    public partial class Orders_NewColumn_DesiredDepartureTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DesiredDepartureTime",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DesiredDepartureTime",
                table: "Orders");
        }
    }
}
