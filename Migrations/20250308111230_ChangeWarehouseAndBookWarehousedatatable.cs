using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.Migrations
{
    /// <inheritdoc />
    public partial class ChangeWarehouseAndBookWarehousedatatable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remaining",
                table: "Warehouse");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "BookWarehouse",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "BookWarehouse");

            migrationBuilder.AddColumn<int>(
                name: "Remaining",
                table: "Warehouse",
                type: "int",
                nullable: true);
        }
    }
}
