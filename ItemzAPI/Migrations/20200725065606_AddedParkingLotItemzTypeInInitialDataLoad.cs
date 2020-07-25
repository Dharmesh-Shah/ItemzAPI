using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItemzApp.API.Migrations
{
    public partial class AddedParkingLotItemzTypeInInitialDataLoad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ItemzTypes",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "Name", "ProjectId", "Status" },
                values: new object[] { new Guid("473fe535-1420-42cf-8a40-224388b8df24"), "User 1", new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "This is Parking Lot system ItemzType", "Parking Lot", new Guid("42f62a6c-fcda-4dac-a06c-406ac1c17770"), "Active" });

            migrationBuilder.InsertData(
                table: "ItemzTypes",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "Name", "ProjectId", "Status" },
                values: new object[] { new Guid("1a069648-6ad6-4c8a-be05-be747bdeb8da"), "User 1", new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "This is Parking Lot system ItemzType", "Parking Lot", new Guid("b69cf0d7-70ad-4f73-aa4a-8daad5181e1e"), "Active" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ItemzTypes",
                keyColumn: "Id",
                keyValue: new Guid("1a069648-6ad6-4c8a-be05-be747bdeb8da"));

            migrationBuilder.DeleteData(
                table: "ItemzTypes",
                keyColumn: "Id",
                keyValue: new Guid("473fe535-1420-42cf-8a40-224388b8df24"));
        }
    }
}
