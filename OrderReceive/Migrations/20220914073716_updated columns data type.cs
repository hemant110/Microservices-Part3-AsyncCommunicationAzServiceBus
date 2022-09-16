using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OrderReceive.Migrations
{
    public partial class updatedcolumnsdatatype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "QtyPlanned",
                table: "OrderLines",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AlterColumn<int>(
                name: "QtyPicked",
                table: "OrderLines",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AlterColumn<int>(
                name: "QtyPerBox",
                table: "OrderLines",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AlterColumn<int>(
                name: "QtyOrdered",
                table: "OrderLines",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AlterColumn<int>(
                name: "QtyAllocated",
                table: "OrderLines",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.UpdateData(
                table: "OrderHeader",
                keyColumn: "OrderHeaderId",
                keyValue: new Guid("6313179f-7837-473a-a4d5-a5571b43e6a6"),
                column: "Order_Status",
                value: "Created");

            migrationBuilder.UpdateData(
                table: "OrderHeader",
                keyColumn: "OrderHeaderId",
                keyValue: new Guid("b0788d2f-8003-43c1-92a4-edc76a7c5dde"),
                column: "Order_Status",
                value: "Created");

            migrationBuilder.UpdateData(
                table: "OrderLines",
                keyColumn: "Id",
                keyValue: new Guid("6313179f-7837-473a-a4d5-a5571b43e6a6"),
                columns: new[] { "QtyAllocated", "QtyOrdered", "QtyPerBox", "QtyPicked", "QtyPlanned", "Status" },
                values: new object[] { 0, 10, 5, 0, 10, "Created" });

            migrationBuilder.UpdateData(
                table: "OrderLines",
                keyColumn: "Id",
                keyValue: new Guid("b0788d2f-8003-43c1-92a4-edc76a7c5dde"),
                columns: new[] { "QtyAllocated", "QtyOrdered", "QtyPerBox", "QtyPicked", "QtyPlanned", "Status" },
                values: new object[] { 0, 10, 5, 0, 10, "Created" });

            migrationBuilder.UpdateData(
                table: "OrderLines",
                keyColumn: "Id",
                keyValue: new Guid("bf3f3002-7e53-441e-8b76-f6280be284aa"),
                columns: new[] { "QtyAllocated", "QtyOrdered", "QtyPerBox", "QtyPicked", "QtyPlanned", "Status" },
                values: new object[] { 0, 10, 5, 0, 10, "Created" });

            migrationBuilder.UpdateData(
                table: "OrderLines",
                keyColumn: "Id",
                keyValue: new Guid("fe98f549-e790-4e9f-aa16-18c2292a2ee9"),
                columns: new[] { "QtyAllocated", "QtyOrdered", "QtyPerBox", "QtyPicked", "QtyPlanned", "Status" },
                values: new object[] { 0, 10, 10, 0, 10, "Created" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "QtyPlanned",
                table: "OrderLines",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyPicked",
                table: "OrderLines",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyPerBox",
                table: "OrderLines",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyOrdered",
                table: "OrderLines",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "QtyAllocated",
                table: "OrderLines",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "OrderHeader",
                keyColumn: "OrderHeaderId",
                keyValue: new Guid("6313179f-7837-473a-a4d5-a5571b43e6a6"),
                column: "Order_Status",
                value: "Pending");

            migrationBuilder.UpdateData(
                table: "OrderHeader",
                keyColumn: "OrderHeaderId",
                keyValue: new Guid("b0788d2f-8003-43c1-92a4-edc76a7c5dde"),
                column: "Order_Status",
                value: "Pending");

            migrationBuilder.UpdateData(
                table: "OrderLines",
                keyColumn: "Id",
                keyValue: new Guid("6313179f-7837-473a-a4d5-a5571b43e6a6"),
                columns: new[] { "QtyAllocated", "QtyOrdered", "QtyPerBox", "QtyPicked", "QtyPlanned", "Status" },
                values: new object[] { 0m, 10m, 5m, 0m, 10m, "Pending" });

            migrationBuilder.UpdateData(
                table: "OrderLines",
                keyColumn: "Id",
                keyValue: new Guid("b0788d2f-8003-43c1-92a4-edc76a7c5dde"),
                columns: new[] { "QtyAllocated", "QtyOrdered", "QtyPerBox", "QtyPicked", "QtyPlanned", "Status" },
                values: new object[] { 0m, 10m, 5m, 0m, 10m, "Pending" });

            migrationBuilder.UpdateData(
                table: "OrderLines",
                keyColumn: "Id",
                keyValue: new Guid("bf3f3002-7e53-441e-8b76-f6280be284aa"),
                columns: new[] { "QtyAllocated", "QtyOrdered", "QtyPerBox", "QtyPicked", "QtyPlanned", "Status" },
                values: new object[] { 0m, 10m, 5m, 0m, 10m, "Pending" });

            migrationBuilder.UpdateData(
                table: "OrderLines",
                keyColumn: "Id",
                keyValue: new Guid("fe98f549-e790-4e9f-aa16-18c2292a2ee9"),
                columns: new[] { "QtyAllocated", "QtyOrdered", "QtyPerBox", "QtyPicked", "QtyPlanned", "Status" },
                values: new object[] { 0m, 10m, 10m, 0m, 10m, "Pending" });
        }
    }
}
