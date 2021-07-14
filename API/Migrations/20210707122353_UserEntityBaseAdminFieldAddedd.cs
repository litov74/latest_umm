using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class UserEntityBaseAdminFieldAddedd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBaseUserOfCompany",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBaseUserOfCompany",
                table: "Users");
        }
    }
}
