using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class bidModelFixAndItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerUserId",
                table: "Item",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserOwnerId",
                table: "Item",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Item_OwnerUserId",
                table: "Item",
                column: "OwnerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_User_OwnerUserId",
                table: "Item",
                column: "OwnerUserId",
                principalTable: "User",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_User_OwnerUserId",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Item_OwnerUserId",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "OwnerUserId",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "UserOwnerId",
                table: "Item");
        }
    }
}
