using Microsoft.EntityFrameworkCore.Migrations;

namespace QCService.Migrations
{
    public partial class qualitycheckaddedcolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Qty_Passed",
                table: "QualityCheck",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Qty_Received",
                table: "QualityCheck",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Qty_Passed",
                table: "QualityCheck");

            migrationBuilder.DropColumn(
                name: "Qty_Received",
                table: "QualityCheck");
        }
    }
}
