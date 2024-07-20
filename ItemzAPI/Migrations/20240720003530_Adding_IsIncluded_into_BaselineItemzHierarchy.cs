using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItemzApp.API.Migrations
{
    /// <inheritdoc />
    public partial class Adding_IsIncluded_into_BaselineItemzHierarchy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isIncluded",
                table: "BaselineItemzHierarchy",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isIncluded",
                table: "BaselineItemzHierarchy");
        }
    }
}
