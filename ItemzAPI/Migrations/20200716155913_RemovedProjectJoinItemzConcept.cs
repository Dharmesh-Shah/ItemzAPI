using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItemzApp.API.Migrations
{
    public partial class RemovedProjectJoinItemzConcept : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectJoinItemz");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectJoinItemz",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemzId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectJoinItemz", x => new { x.ProjectId, x.ItemzId });
                    table.ForeignKey(
                        name: "FK_ProjectJoinItemz_Itemzs_ItemzId",
                        column: x => x.ItemzId,
                        principalTable: "Itemzs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectJoinItemz_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ProjectJoinItemz",
                columns: new[] { "ProjectId", "ItemzId" },
                values: new object[] { new Guid("42f62a6c-fcda-4dac-a06c-406ac1c17770"), new Guid("9153a516-d69e-4364-b17e-03b87442e21c") });

            migrationBuilder.InsertData(
                table: "ProjectJoinItemz",
                columns: new[] { "ProjectId", "ItemzId" },
                values: new object[] { new Guid("b69cf0d7-70ad-4f73-aa4a-8daad5181e1e"), new Guid("5e76f8e8-d3e7-41db-b084-f64c107c6783") });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectJoinItemz_ItemzId",
                table: "ProjectJoinItemz",
                column: "ItemzId");
        }
    }
}
