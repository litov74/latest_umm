using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class UserIdinPlanEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PlanSubscribe",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanSubscribe_UserId",
                table: "PlanSubscribe",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanSubscribe_Users_UserId",
                table: "PlanSubscribe",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanSubscribe_Users_UserId",
                table: "PlanSubscribe");

            migrationBuilder.DropIndex(
                name: "IX_PlanSubscribe_UserId",
                table: "PlanSubscribe");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PlanSubscribe");
        }
    }
}
