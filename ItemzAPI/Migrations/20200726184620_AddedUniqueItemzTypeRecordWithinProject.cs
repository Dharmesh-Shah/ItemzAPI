using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItemzApp.API.Migrations
{
    public partial class AddedUniqueItemzTypeRecordWithinProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ItemzTypes",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "Name", "ProjectId", "Status" },
                values: new object[] { new Guid("52ca1dfc-b187-47fc-a379-57af33404b34"), "User 1", new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Used for testing that ItemzType Name remains unique within a given Project", "TESTING - Unique ItemzType Name", new Guid("b69cf0d7-70ad-4f73-aa4a-8daad5181e1e"), "Active" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ItemzTypes",
                keyColumn: "Id",
                keyValue: new Guid("52ca1dfc-b187-47fc-a379-57af33404b34"));
        }
    }
}
