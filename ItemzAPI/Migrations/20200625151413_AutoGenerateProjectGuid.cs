using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItemzApp.API.Migrations
{
    public partial class AutoGenerateProjectGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("522eba11-b8b2-40e7-873c-983f1c355c37"));

            migrationBuilder.DeleteData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("7eae6d78-36e6-495b-b64f-845b583381a9"));

            migrationBuilder.DeleteData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("e816de73-5c50-4841-944d-c63b651c00e6"));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Projects",
                nullable: false,
                defaultValueSql: "NEWID()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.InsertData(
                table: "Itemzs",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "Name", "Priority", "Status" },
                values: new object[] { new Guid("0fa2f0eb-ed40-4265-8c0c-c24bbc84799f"), "User 3", new DateTimeOffset(new DateTime(2019, 7, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Requirements to be described here.", "Item 3", "Low", "New" });

            migrationBuilder.InsertData(
                table: "Itemzs",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "Name", "Priority", "Status" },
                values: new object[] { new Guid("853c9a62-1f5f-4368-8bc9-f94c68d29fce"), "User 4", new DateTimeOffset(new DateTime(2019, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Requirements to be described here.", "Item 4", "Low", "New" });

            migrationBuilder.InsertData(
                table: "Itemzs",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "Name", "Priority", "Status" },
                values: new object[] { new Guid("04f4d829-d230-4b76-bf95-4d2bed426f6b"), "User 5", new DateTimeOffset(new DateTime(2019, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Requirements to be described here.", "Item 5", "Low", "New" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("04f4d829-d230-4b76-bf95-4d2bed426f6b"));

            migrationBuilder.DeleteData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("0fa2f0eb-ed40-4265-8c0c-c24bbc84799f"));

            migrationBuilder.DeleteData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("853c9a62-1f5f-4368-8bc9-f94c68d29fce"));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldDefaultValueSql: "NEWID()");

            migrationBuilder.InsertData(
                table: "Itemzs",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "Name", "Priority", "Status" },
                values: new object[] { new Guid("e816de73-5c50-4841-944d-c63b651c00e6"), "User 3", new DateTimeOffset(new DateTime(2019, 7, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Requirements to be described here.", "Item 3", "Low", "New" });

            migrationBuilder.InsertData(
                table: "Itemzs",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "Name", "Priority", "Status" },
                values: new object[] { new Guid("7eae6d78-36e6-495b-b64f-845b583381a9"), "User 4", new DateTimeOffset(new DateTime(2019, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Requirements to be described here.", "Item 4", "Low", "New" });

            migrationBuilder.InsertData(
                table: "Itemzs",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "Name", "Priority", "Status" },
                values: new object[] { new Guid("522eba11-b8b2-40e7-873c-983f1c355c37"), "User 5", new DateTimeOffset(new DateTime(2019, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Requirements to be described here.", "Item 5", "Low", "New" });
        }
    }
}
