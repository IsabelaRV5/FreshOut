using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFoodItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItem_GroceryLists_GroceryListId",
                table: "FoodItem");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodItem_LocationLists_LocationListId",
                table: "FoodItem");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodItem_Users_UserId",
                table: "FoodItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FoodItem",
                table: "FoodItem");

            migrationBuilder.RenameTable(
                name: "FoodItem",
                newName: "FoodItems");

            migrationBuilder.RenameIndex(
                name: "IX_FoodItem_UserId",
                table: "FoodItems",
                newName: "IX_FoodItems_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FoodItem_LocationListId",
                table: "FoodItems",
                newName: "IX_FoodItems_LocationListId");

            migrationBuilder.RenameIndex(
                name: "IX_FoodItem_GroceryListId",
                table: "FoodItems",
                newName: "IX_FoodItems_GroceryListId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FoodItems",
                table: "FoodItems",
                column: "FoodItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_GroceryLists_GroceryListId",
                table: "FoodItems",
                column: "GroceryListId",
                principalTable: "GroceryLists",
                principalColumn: "GroceryListId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_LocationLists_LocationListId",
                table: "FoodItems",
                column: "LocationListId",
                principalTable: "LocationLists",
                principalColumn: "LocationListId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItems_Users_UserId",
                table: "FoodItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_GroceryLists_GroceryListId",
                table: "FoodItems");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_LocationLists_LocationListId",
                table: "FoodItems");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodItems_Users_UserId",
                table: "FoodItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FoodItems",
                table: "FoodItems");

            migrationBuilder.RenameTable(
                name: "FoodItems",
                newName: "FoodItem");

            migrationBuilder.RenameIndex(
                name: "IX_FoodItems_UserId",
                table: "FoodItem",
                newName: "IX_FoodItem_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FoodItems_LocationListId",
                table: "FoodItem",
                newName: "IX_FoodItem_LocationListId");

            migrationBuilder.RenameIndex(
                name: "IX_FoodItems_GroceryListId",
                table: "FoodItem",
                newName: "IX_FoodItem_GroceryListId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FoodItem",
                table: "FoodItem",
                column: "FoodItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItem_GroceryLists_GroceryListId",
                table: "FoodItem",
                column: "GroceryListId",
                principalTable: "GroceryLists",
                principalColumn: "GroceryListId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItem_LocationLists_LocationListId",
                table: "FoodItem",
                column: "LocationListId",
                principalTable: "LocationLists",
                principalColumn: "LocationListId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodItem_Users_UserId",
                table: "FoodItem",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
