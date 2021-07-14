using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class addfieldcodetouser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailVerifyCode",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordForgotCode",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailVerifyCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordForgotCode",
                table: "Users");
        }
    }
}
