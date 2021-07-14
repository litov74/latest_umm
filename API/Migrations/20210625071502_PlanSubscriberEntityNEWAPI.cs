using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class PlanSubscriberEntityNEWAPI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubscriptionTierEntity_Users_UserId",
                table: "SubscriptionTierEntity");

            migrationBuilder.DropIndex(
                name: "IX_SubscriptionTierEntity_UserId",
                table: "SubscriptionTierEntity");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SubscriptionTierEntity");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaymentVerified",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PlanSubscribeEntity",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    SubscriptionTierId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Plan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPaymentVerified = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanSubscribeEntity", x => new { x.UserId, x.SubscriptionTierId });
                    table.ForeignKey(
                        name: "FK_PlanSubscribeEntity_SubscriptionTierEntity_SubscriptionTierId",
                        column: x => x.SubscriptionTierId,
                        principalTable: "SubscriptionTierEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanSubscribeEntity_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanSubscribeEntity_SubscriptionTierId",
                table: "PlanSubscribeEntity",
                column: "SubscriptionTierId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanSubscribeEntity");

            migrationBuilder.DropColumn(
                name: "IsPaymentVerified",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "SubscriptionTierEntity",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionTierEntity_UserId",
                table: "SubscriptionTierEntity",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubscriptionTierEntity_Users_UserId",
                table: "SubscriptionTierEntity",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
