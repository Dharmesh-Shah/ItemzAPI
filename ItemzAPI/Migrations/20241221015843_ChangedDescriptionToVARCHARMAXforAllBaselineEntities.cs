using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItemzApp.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangedDescriptionToVARCHARMAXforAllBaselineEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "BaselineItemzType",
                type: "VARCHAR(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1028)",
                oldMaxLength: 1028,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "BaselineItemz",
                type: "VARCHAR(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1028)",
                oldMaxLength: 1028,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Baseline",
                type: "VARCHAR(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1028)",
                oldMaxLength: 1028,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "BaselineItemzType",
                type: "nvarchar(1028)",
                maxLength: 1028,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(MAX)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "BaselineItemz",
                type: "nvarchar(1028)",
                maxLength: 1028,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(MAX)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Baseline",
                type: "nvarchar(1028)",
                maxLength: 1028,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(MAX)",
                oldNullable: true);
        }
    }
}
