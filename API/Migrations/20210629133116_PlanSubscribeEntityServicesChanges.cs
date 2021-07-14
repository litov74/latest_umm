using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class PlanSubscribeEntityServicesChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "PlanSubscribeEntity",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountUser",
                table: "PlanSubscribeEntity",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "PlanSubscribeEntity",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CompanyUserMapping",
                keyColumns: new[] { "CompanyId", "UserId" },
                keyValues: new object[] { "9881C482-63CB-40B9-9AE9-B60351CBC3DE", "2911461D-B77E-E911-B2F3-0A645F4F4675" },
                column: "AccessLevel",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_PlanSubscribeEntity_CompanyId",
                table: "PlanSubscribeEntity",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanSubscribeEntity_Companies_CompanyId",
                table: "PlanSubscribeEntity",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanSubscribeEntity_Companies_CompanyId",
                table: "PlanSubscribeEntity");

            migrationBuilder.DropIndex(
                name: "IX_PlanSubscribeEntity_CompanyId",
                table: "PlanSubscribeEntity");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "PlanSubscribeEntity");

            migrationBuilder.DropColumn(
                name: "CountUser",
                table: "PlanSubscribeEntity");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "PlanSubscribeEntity");

            migrationBuilder.UpdateData(
                table: "CompanyUserMapping",
                keyColumns: new[] { "CompanyId", "UserId" },
                keyValues: new object[] { "9881C482-63CB-40B9-9AE9-B60351CBC3DE", "2911461D-B77E-E911-B2F3-0A645F4F4675" },
                column: "AccessLevel",
                value: 0);
        }
    }
}
