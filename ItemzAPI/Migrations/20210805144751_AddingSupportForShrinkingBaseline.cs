using Microsoft.EntityFrameworkCore.Migrations;

namespace ItemzApp.API.Migrations
{
    public partial class AddingSupportForShrinkingBaseline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isIncluded",
                table: "BaselineItemz",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isIncluded",
                table: "BaselineItemz");
        }
    }
}
