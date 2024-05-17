using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebShop.Migrations
{
    /// <inheritdoc />
    public partial class bidFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBid");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Bid",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bid_UserId",
                table: "Bid",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bid_User_UserId",
                table: "Bid",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bid_User_UserId",
                table: "Bid");

            migrationBuilder.DropIndex(
                name: "IX_Bid_UserId",
                table: "Bid");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Bid");

            migrationBuilder.CreateTable(
                name: "UserBid",
                columns: table => new
                {
                    UserBidId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BidId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBid", x => x.UserBidId);
                    table.ForeignKey(
                        name: "FK_UserBid_Bid_BidId",
                        column: x => x.BidId,
                        principalTable: "Bid",
                        principalColumn: "BidId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBid_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBid_BidId",
                table: "UserBid",
                column: "BidId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBid_UserId",
                table: "UserBid",
                column: "UserId");
        }
    }
}
