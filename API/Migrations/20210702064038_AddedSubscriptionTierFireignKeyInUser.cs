using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class AddedSubscriptionTierFireignKeyInUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Plan",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "SubscriptionTierId",
                table: "Users",
                type: "nvarchar(36)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_SubscriptionTierId",
                table: "Users",
                column: "SubscriptionTierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_SubscriptionTierEntity_SubscriptionTierId",
                table: "Users",
                column: "SubscriptionTierId",
                principalTable: "SubscriptionTierEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_SubscriptionTierEntity_SubscriptionTierId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SubscriptionTierId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SubscriptionTierId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "Plan",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
