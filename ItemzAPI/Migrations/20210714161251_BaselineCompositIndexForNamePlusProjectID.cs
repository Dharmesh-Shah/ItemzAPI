using Microsoft.EntityFrameworkCore.Migrations;

namespace ItemzApp.API.Migrations
{
    public partial class BaselineCompositIndexForNamePlusProjectID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Baseline_Name",
                table: "Baseline");

            migrationBuilder.DropIndex(
                name: "IX_Baseline_ProjectId",
                table: "Baseline");

            migrationBuilder.CreateIndex(
                name: "IX_Baseline_ProjectId_Name",
                table: "Baseline",
                columns: new[] { "ProjectId", "Name" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Baseline_ProjectId_Name",
                table: "Baseline");

            migrationBuilder.CreateIndex(
                name: "IX_Baseline_Name",
                table: "Baseline",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Baseline_ProjectId",
                table: "Baseline",
                column: "ProjectId");
        }
    }
}
