using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class ChnagesInPlanSubscribeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompanyUserMapping_UserId",
                table: "CompanyUserMapping");

            migrationBuilder.DropColumn(
                name: "IsPaymentVerified",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "IsPaymentVerified",
                table: "PlanSubscribe",
                newName: "IsDeleted");

            migrationBuilder.AddColumn<int>(
                name: "Plan",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Plan",
                table: "PlanSubscribe",
                type: "int",
                maxLength: 320,
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(320)",
                oldMaxLength: 320,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "PlanSubscribe",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "PlanSubscribe",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUserMapping_UserId",
                table: "CompanyUserMapping",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompanyUserMapping_UserId",
                table: "CompanyUserMapping");

            migrationBuilder.DropColumn(
                name: "Plan",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "PlanSubscribe");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "PlanSubscribe");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "PlanSubscribe",
                newName: "IsPaymentVerified");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaymentVerified",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Plan",
                table: "PlanSubscribe",
                type: "nvarchar(320)",
                maxLength: 320,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 320);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUserMapping_UserId",
                table: "CompanyUserMapping",
                column: "UserId",
                unique: true);
        }
    }
}
