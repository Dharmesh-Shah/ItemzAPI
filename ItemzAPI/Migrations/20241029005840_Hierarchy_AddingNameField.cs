using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItemzApp.API.Migrations
{
    /// <inheritdoc />
    public partial class Hierarchy_AddingNameField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ItemzHierarchy",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BaselineItemzHierarchy",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "ItemzHierarchy");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BaselineItemzHierarchy");
        }
    }
}
