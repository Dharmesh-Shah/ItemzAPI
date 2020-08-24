using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItemzApp.API.Migrations
{
    public partial class IntroducingItemzSeverityENUM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Severity",
                table: "Itemzs",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("0850bc8a-84c4-4c52-8ab6-b06e7bc2195b"),
                column: "Severity",
                value: "Medium");

            migrationBuilder.UpdateData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("4adde56d-8081-45ea-bd37-5c830b63873b"),
                column: "Severity",
                value: "Medium");

            migrationBuilder.UpdateData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("5e76f8e8-d3e7-41db-b084-f64c107c6783"),
                column: "Severity",
                value: "Medium");

            migrationBuilder.UpdateData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("885b8e56-ffe6-4bc9-82e2-ce23230991db"),
                column: "Severity",
                value: "Medium");

            migrationBuilder.UpdateData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("9153a516-d69e-4364-b17e-03b87442e21c"),
                column: "Severity",
                value: "Medium");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Severity",
                table: "Itemzs");
        }
    }
}
