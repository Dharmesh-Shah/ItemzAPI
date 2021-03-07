using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItemzApp.API.Migrations
{
    public partial class AfterUpgradingToDOTNET5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ItemzTypes",
                keyColumn: "Id",
                keyValue: new Guid("1a069648-6ad6-4c8a-be05-be747bdeb8da"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 5, 30, 0, 0)));

            migrationBuilder.UpdateData(
                table: "ItemzTypes",
                keyColumn: "Id",
                keyValue: new Guid("473fe535-1420-42cf-8a40-224388b8df24"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 5, 30, 0, 0)));

            migrationBuilder.UpdateData(
                table: "ItemzTypes",
                keyColumn: "Id",
                keyValue: new Guid("52ca1dfc-b187-47fc-a379-57af33404b34"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 5, 30, 0, 0)));

            migrationBuilder.UpdateData(
                table: "ItemzTypes",
                keyColumn: "Id",
                keyValue: new Guid("611639db-577a-48f6-9b08-f6aef477368f"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 5, 30, 0, 0)));

            migrationBuilder.UpdateData(
                table: "ItemzTypes",
                keyColumn: "Id",
                keyValue: new Guid("8414bf58-6331-4b3e-bbf0-f780950a337b"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 5, 30, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("0850bc8a-84c4-4c52-8ab6-b06e7bc2195b"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2019, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 5, 30, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("4adde56d-8081-45ea-bd37-5c830b63873b"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2019, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 5, 30, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("5e76f8e8-d3e7-41db-b084-f64c107c6783"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2019, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 5, 30, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("885b8e56-ffe6-4bc9-82e2-ce23230991db"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2019, 7, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 5, 30, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("9153a516-d69e-4364-b17e-03b87442e21c"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 5, 30, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("42f62a6c-fcda-4dac-a06c-406ac1c17770"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 5, 30, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("b69cf0d7-70ad-4f73-aa4a-8daad5181e1e"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 5, 30, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ItemzTypes",
                keyColumn: "Id",
                keyValue: new Guid("1a069648-6ad6-4c8a-be05-be747bdeb8da"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "ItemzTypes",
                keyColumn: "Id",
                keyValue: new Guid("473fe535-1420-42cf-8a40-224388b8df24"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "ItemzTypes",
                keyColumn: "Id",
                keyValue: new Guid("52ca1dfc-b187-47fc-a379-57af33404b34"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "ItemzTypes",
                keyColumn: "Id",
                keyValue: new Guid("611639db-577a-48f6-9b08-f6aef477368f"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "ItemzTypes",
                keyColumn: "Id",
                keyValue: new Guid("8414bf58-6331-4b3e-bbf0-f780950a337b"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("0850bc8a-84c4-4c52-8ab6-b06e7bc2195b"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2019, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("4adde56d-8081-45ea-bd37-5c830b63873b"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2019, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("5e76f8e8-d3e7-41db-b084-f64c107c6783"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2019, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("885b8e56-ffe6-4bc9-82e2-ce23230991db"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2019, 7, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("9153a516-d69e-4364-b17e-03b87442e21c"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("42f62a6c-fcda-4dac-a06c-406ac1c17770"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("b69cf0d7-70ad-4f73-aa4a-8daad5181e1e"),
                column: "CreatedDate",
                value: new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)));
        }
    }
}
