using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class FinalCompanyHierarchyItemsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyAccess",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CompanyHierarchyItemsEntities",
                columns: table => new
                {
                    ParentCompanyHierarchyItemId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 36, nullable: false),
                    HierarchyLevel = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    EmployeeCount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyHierarchyItemsEntities", x => x.ParentCompanyHierarchyItemId);
                    table.ForeignKey(
                        name: "FK_CompanyHierarchyItemsEntities_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyHierarchyItemsEntities_CompanyId",
                table: "CompanyHierarchyItemsEntities",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyHierarchyItemsEntities");

            migrationBuilder.DropColumn(
                name: "CompanyAccess",
                table: "Companies");
        }
    }
}
