using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItemzApp.API.Migrations
{
    public partial class IntroductionOfItemzType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "ItemzTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Status = table.Column<string>(maxLength: 64, nullable: false),
                    Description = table.Column<string>(maxLength: 1028, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 128, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemzTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemzTypeJoinItemz",
                columns: table => new
                {
                    ItemzTypeId = table.Column<Guid>(nullable: false),
                    ItemzId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemzTypeJoinItemz", x => new { x.ItemzTypeId, x.ItemzId });
                    table.ForeignKey(
                        name: "FK_ItemzTypeJoinItemz_Itemzs_ItemzId",
                        column: x => x.ItemzId,
                        principalTable: "Itemzs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemzTypeJoinItemz_ItemzTypes_ItemzTypeId",
                        column: x => x.ItemzTypeId,
                        principalTable: "ItemzTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ItemzTypes",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "Name", "Status" },
                values: new object[,]
                {
                    { new Guid("611639db-577a-48f6-9b08-f6aef477368f"), "User 1", new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "This is ItemzType 1", "ItemzType 1", "Active" },
                    { new Guid("8414bf58-6331-4b3e-bbf0-f780950a337b"), "User 1", new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "This is ItemzType 2", "ItemzType 2", "Active" }
                });

            migrationBuilder.InsertData(
                table: "Itemzs",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "Name", "Priority", "Status" },
                values: new object[,]
                {
                    { new Guid("885b8e56-ffe6-4bc9-82e2-ce23230991db"), "User 3", new DateTimeOffset(new DateTime(2019, 7, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Requirements to be described here.", "Item 3", "Low", "New" },
                    { new Guid("0850bc8a-84c4-4c52-8ab6-b06e7bc2195b"), "User 4", new DateTimeOffset(new DateTime(2019, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Requirements to be described here.", "Item 4", "Low", "New" },
                    { new Guid("4adde56d-8081-45ea-bd37-5c830b63873b"), "User 5", new DateTimeOffset(new DateTime(2019, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Requirements to be described here.", "Item 5", "Low", "New" }
                });

            migrationBuilder.InsertData(
                table: "ItemzTypeJoinItemz",
                columns: new[] { "ItemzTypeId", "ItemzId" },
                values: new object[] { new Guid("611639db-577a-48f6-9b08-f6aef477368f"), new Guid("9153a516-d69e-4364-b17e-03b87442e21c") });

            migrationBuilder.InsertData(
                table: "ItemzTypeJoinItemz",
                columns: new[] { "ItemzTypeId", "ItemzId" },
                values: new object[] { new Guid("8414bf58-6331-4b3e-bbf0-f780950a337b"), new Guid("5e76f8e8-d3e7-41db-b084-f64c107c6783") });

            migrationBuilder.CreateIndex(
                name: "IX_ItemzTypeJoinItemz_ItemzId",
                table: "ItemzTypeJoinItemz",
                column: "ItemzId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemzTypeJoinItemz");

            migrationBuilder.DropTable(
                name: "ItemzTypes");

            migrationBuilder.DeleteData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("0850bc8a-84c4-4c52-8ab6-b06e7bc2195b"));

            migrationBuilder.DeleteData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("4adde56d-8081-45ea-bd37-5c830b63873b"));

            migrationBuilder.DeleteData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("885b8e56-ffe6-4bc9-82e2-ce23230991db"));

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
    }
}
