using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItemzApp.API.Migrations
{
    public partial class AddedIsSystemPropertyToItemzType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSystem",
                table: "ItemzTypes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "ItemzTypes",
                keyColumn: "Id",
                keyValue: new Guid("1a069648-6ad6-4c8a-be05-be747bdeb8da"),
                column: "IsSystem",
                value: true);

            migrationBuilder.UpdateData(
                table: "ItemzTypes",
                keyColumn: "Id",
                keyValue: new Guid("473fe535-1420-42cf-8a40-224388b8df24"),
                column: "IsSystem",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSystem",
                table: "ItemzTypes");
        }
    }
}
