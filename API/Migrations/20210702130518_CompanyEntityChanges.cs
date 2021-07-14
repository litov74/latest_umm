using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class CompanyEntityChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PlanSubscribeEntity_CompanyId",
                table: "PlanSubscribeEntity");

            migrationBuilder.DropIndex(
                name: "IX_CompanyHierarchyItemsEntities_CompanyId",
                table: "CompanyHierarchyItemsEntities");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Companies",
                newName: "IsEnabled");

            migrationBuilder.RenameColumn(
                name: "CompanyAccess",
                table: "Companies",
                newName: "AccessType");

            migrationBuilder.CreateTable(
                name: "CompanyLoadingLevelEntities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    Week = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    HierarchyItemId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    From = table.Column<DateTime>(type: "datetime2", nullable: true),
                    To = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HierarchyItemParentCompanyHierarchyItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyLoadingLevelEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyLoadingLevelEntities_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyLoadingLevelEntities_CompanyHierarchyItemsEntities_HierarchyItemParentCompanyHierarchyItemId",
                        column: x => x.HierarchyItemParentCompanyHierarchyItemId,
                        principalTable: "CompanyHierarchyItemsEntities",
                        principalColumn: "ParentCompanyHierarchyItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: "9881C482-63CB-40B9-9AE9-B60351CBC3DE",
                column: "IsEnabled",
                value: false);

            migrationBuilder.CreateIndex(
                name: "IX_PlanSubscribeEntity_CompanyId",
                table: "PlanSubscribeEntity",
                column: "CompanyId",
                unique: true,
                filter: "[CompanyId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyHierarchyItemsEntities_CompanyId",
                table: "CompanyHierarchyItemsEntities",
                column: "CompanyId",
                unique: true,
                filter: "[CompanyId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyLoadingLevelEntities_CompanyId",
                table: "CompanyLoadingLevelEntities",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyLoadingLevelEntities_HierarchyItemParentCompanyHierarchyItemId",
                table: "CompanyLoadingLevelEntities",
                column: "HierarchyItemParentCompanyHierarchyItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyLoadingLevelEntities");

            migrationBuilder.DropIndex(
                name: "IX_PlanSubscribeEntity_CompanyId",
                table: "PlanSubscribeEntity");

            migrationBuilder.DropIndex(
                name: "IX_CompanyHierarchyItemsEntities_CompanyId",
                table: "CompanyHierarchyItemsEntities");

            migrationBuilder.RenameColumn(
                name: "IsEnabled",
                table: "Companies",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "AccessType",
                table: "Companies",
                newName: "CompanyAccess");

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: "9881C482-63CB-40B9-9AE9-B60351CBC3DE",
                column: "IsActive",
                value: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanSubscribeEntity_CompanyId",
                table: "PlanSubscribeEntity",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyHierarchyItemsEntities_CompanyId",
                table: "CompanyHierarchyItemsEntities",
                column: "CompanyId");
        }
    }
}
