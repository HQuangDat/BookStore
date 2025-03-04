using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookWarehouseRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Warehouse",
                table: "Book");

            migrationBuilder.DropIndex(
                name: "IX_Book_WarehouseId",
                table: "Book");

            migrationBuilder.CreateTable(
                name: "BookWarehouse",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookWarehouse", x => new { x.BookId, x.WarehouseId });
                    table.ForeignKey(
                        name: "FK_BookWarehouse_Book",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookWarehouse_Warehouse",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouse",
                        principalColumn: "WarehouseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookWarehouse_WarehouseId",
                table: "BookWarehouse",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookWarehouse");

            migrationBuilder.CreateIndex(
                name: "IX_Book_WarehouseId",
                table: "Book",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Warehouse",
                table: "Book",
                column: "WarehouseId",
                principalTable: "Warehouse",
                principalColumn: "WarehouseId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
