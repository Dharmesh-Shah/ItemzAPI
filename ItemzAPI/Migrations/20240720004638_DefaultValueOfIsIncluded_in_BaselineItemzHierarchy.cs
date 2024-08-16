using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItemzApp.API.Migrations
{
    /// <inheritdoc />
    public partial class DefaultValueOfIsIncluded_in_BaselineItemzHierarchy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "isIncluded",
                table: "BaselineItemzHierarchy",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "isIncluded",
                table: "BaselineItemzHierarchy",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);
        }
    }
}
