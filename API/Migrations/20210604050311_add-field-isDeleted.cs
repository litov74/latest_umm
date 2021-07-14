using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class addfieldisDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompanyUserMapping_UserId",
                table: "CompanyUserMapping");

            migrationBuilder.RenameColumn(
                name: "Active",
                table: "CompanyUserMapping",
                newName: "IsDeleted");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CompanyUserMapping",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Companies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUserMapping_UserId",
                table: "CompanyUserMapping",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompanyUserMapping_UserId",
                table: "CompanyUserMapping");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CompanyUserMapping");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "CompanyUserMapping",
                newName: "Active");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUserMapping_UserId",
                table: "CompanyUserMapping",
                column: "UserId");
        }
    }
}
