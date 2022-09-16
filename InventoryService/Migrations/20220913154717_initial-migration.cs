using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryService.Migrations
{
    public partial class initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    InventoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Inventory_Tag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Inventory_CreationDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Inventory_CreationTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Inventory_oLineId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Inventory_Product = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Inventory_ProductDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Inventory_Qty = table.Column<int>(type: "int", nullable: false),
                    Inventory_Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Inventory_LockStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Inventory_LockCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Inventory_Order = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Customer_Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Customer_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Warehouse_Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Warehouse_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company_Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedTime = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.InventoryId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inventory");
        }
    }
}
