using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItemzApp.API.Migrations
{
    public partial class IntroducingProjectAndItemzTypeRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "ItemzTypes",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "ItemzTypes",
                keyColumn: "Id",
                keyValue: new Guid("611639db-577a-48f6-9b08-f6aef477368f"),
                column: "ProjectId",
                value: new Guid("42f62a6c-fcda-4dac-a06c-406ac1c17770"));

            migrationBuilder.UpdateData(
                table: "ItemzTypes",
                keyColumn: "Id",
                keyValue: new Guid("8414bf58-6331-4b3e-bbf0-f780950a337b"),
                column: "ProjectId",
                value: new Guid("b69cf0d7-70ad-4f73-aa4a-8daad5181e1e"));

            migrationBuilder.CreateIndex(
                name: "IX_ItemzTypes_ProjectId",
                table: "ItemzTypes",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemzTypes_Projects_ProjectId",
                table: "ItemzTypes",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemzTypes_Projects_ProjectId",
                table: "ItemzTypes");

            migrationBuilder.DropIndex(
                name: "IX_ItemzTypes_ProjectId",
                table: "ItemzTypes");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ItemzTypes");
        }
    }
}
