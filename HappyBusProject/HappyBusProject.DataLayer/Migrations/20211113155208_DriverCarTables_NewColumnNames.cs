using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyBusProject.Migrations
{
    public partial class DriverCarTables_NewColumnNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Age",
                table: "Drivers",
                newName: "DriverAge");

            migrationBuilder.RenameColumn(
                name: "Brand",
                table: "Cars",
                newName: "CarBrand");

            migrationBuilder.RenameColumn(
                name: "Age",
                table: "Cars",
                newName: "CarAge");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CarID",
                table: "Orders",
                column: "CarID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CarID",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "DriverAge",
                table: "Drivers",
                newName: "Age");

            migrationBuilder.RenameColumn(
                name: "CarBrand",
                table: "Cars",
                newName: "Brand");

            migrationBuilder.RenameColumn(
                name: "CarAge",
                table: "Cars",
                newName: "Age");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                columns: new[] { "CarID", "CustomerID" });
        }
    }
}
