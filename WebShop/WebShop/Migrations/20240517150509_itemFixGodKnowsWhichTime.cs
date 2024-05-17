using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class itemFixGodKnowsWhichTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_User_OwnerUserId",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "UserOwnerId",
                table: "Item");

            migrationBuilder.RenameColumn(
                name: "OwnerUserId",
                table: "Item",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Item_OwnerUserId",
                table: "Item",
                newName: "IX_Item_UserId");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Item",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Item_User_UserId",
                table: "Item",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_User_UserId",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Item");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Item",
                newName: "OwnerUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Item_UserId",
                table: "Item",
                newName: "IX_Item_OwnerUserId");

            migrationBuilder.AddColumn<int>(
                name: "UserOwnerId",
                table: "Item",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Item_User_OwnerUserId",
                table: "Item",
                column: "OwnerUserId",
                principalTable: "User",
                principalColumn: "UserId");
        }
    }
}
