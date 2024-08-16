using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.SqlServer.Types;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ItemzApp.API.Migrations
{
    /// <inheritdoc />
    public partial class RemovingSampleData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ItemzHierarchy",
                keyColumn: "Id",
                keyValue: new Guid("18b014c8-6c31-408f-b8f3-3a4f342abbb1"));

            migrationBuilder.DeleteData(
                table: "ItemzHierarchy",
                keyColumn: "Id",
                keyValue: new Guid("1a069648-6ad6-4c8a-be05-be747bdeb8da"));

            migrationBuilder.DeleteData(
                table: "ItemzHierarchy",
                keyColumn: "Id",
                keyValue: new Guid("42f62a6c-fcda-4dac-a06c-406ac1c17770"));

            migrationBuilder.DeleteData(
                table: "ItemzHierarchy",
                keyColumn: "Id",
                keyValue: new Guid("473fe535-1420-42cf-8a40-224388b8df24"));

            migrationBuilder.DeleteData(
                table: "ItemzHierarchy",
                keyColumn: "Id",
                keyValue: new Guid("52ca1dfc-b187-47fc-a379-57af33404b34"));

            migrationBuilder.DeleteData(
                table: "ItemzHierarchy",
                keyColumn: "Id",
                keyValue: new Guid("5e76f8e8-d3e7-41db-b084-f64c107c6783"));

            migrationBuilder.DeleteData(
                table: "ItemzHierarchy",
                keyColumn: "Id",
                keyValue: new Guid("611639db-577a-48f6-9b08-f6aef477368f"));

            migrationBuilder.DeleteData(
                table: "ItemzHierarchy",
                keyColumn: "Id",
                keyValue: new Guid("8414bf58-6331-4b3e-bbf0-f780950a337b"));

            migrationBuilder.DeleteData(
                table: "ItemzHierarchy",
                keyColumn: "Id",
                keyValue: new Guid("9153a516-d69e-4364-b17e-03b87442e21c"));

            migrationBuilder.DeleteData(
                table: "ItemzHierarchy",
                keyColumn: "Id",
                keyValue: new Guid("b69cf0d7-70ad-4f73-aa4a-8daad5181e1e"));

            migrationBuilder.DeleteData(
                table: "ItemzTypeJoinItemz",
                keyColumns: new[] { "ItemzId", "ItemzTypeId" },
                keyValues: new object[] { new Guid("9153a516-d69e-4364-b17e-03b87442e21c"), new Guid("611639db-577a-48f6-9b08-f6aef477368f") });

            migrationBuilder.DeleteData(
                table: "ItemzTypeJoinItemz",
                keyColumns: new[] { "ItemzId", "ItemzTypeId" },
                keyValues: new object[] { new Guid("5e76f8e8-d3e7-41db-b084-f64c107c6783"), new Guid("8414bf58-6331-4b3e-bbf0-f780950a337b") });

            migrationBuilder.DeleteData(
                table: "ItemzTypes",
                keyColumn: "Id",
                keyValue: new Guid("1a069648-6ad6-4c8a-be05-be747bdeb8da"));

            migrationBuilder.DeleteData(
                table: "ItemzTypes",
                keyColumn: "Id",
                keyValue: new Guid("473fe535-1420-42cf-8a40-224388b8df24"));

            migrationBuilder.DeleteData(
                table: "ItemzTypes",
                keyColumn: "Id",
                keyValue: new Guid("52ca1dfc-b187-47fc-a379-57af33404b34"));

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

            migrationBuilder.DeleteData(
                table: "ItemzTypes",
                keyColumn: "Id",
                keyValue: new Guid("611639db-577a-48f6-9b08-f6aef477368f"));

            migrationBuilder.DeleteData(
                table: "ItemzTypes",
                keyColumn: "Id",
                keyValue: new Guid("8414bf58-6331-4b3e-bbf0-f780950a337b"));

            migrationBuilder.DeleteData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("5e76f8e8-d3e7-41db-b084-f64c107c6783"));

            migrationBuilder.DeleteData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("9153a516-d69e-4364-b17e-03b87442e21c"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("42f62a6c-fcda-4dac-a06c-406ac1c17770"));

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: new Guid("b69cf0d7-70ad-4f73-aa4a-8daad5181e1e"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ItemzHierarchy",
                columns: new[] { "Id", "ItemzHierarchyId", "RecordType" },
                values: new object[,]
                {
                    { new Guid("18b014c8-6c31-408f-b8f3-3a4f342abbb1"), Microsoft.SqlServer.Types.SqlHierarchyId.Parse("/"), "Repository" },
                    { new Guid("1a069648-6ad6-4c8a-be05-be747bdeb8da"), Microsoft.SqlServer.Types.SqlHierarchyId.Parse("/2/1/"), "ItemzType" },
                    { new Guid("42f62a6c-fcda-4dac-a06c-406ac1c17770"), Microsoft.SqlServer.Types.SqlHierarchyId.Parse("/1/"), "Project" },
                    { new Guid("473fe535-1420-42cf-8a40-224388b8df24"), Microsoft.SqlServer.Types.SqlHierarchyId.Parse("/1/1/"), "ItemzType" },
                    { new Guid("52ca1dfc-b187-47fc-a379-57af33404b34"), Microsoft.SqlServer.Types.SqlHierarchyId.Parse("/2/3/"), "ItemzType" },
                    { new Guid("5e76f8e8-d3e7-41db-b084-f64c107c6783"), Microsoft.SqlServer.Types.SqlHierarchyId.Parse("/2/2/1/"), "Itemz" },
                    { new Guid("611639db-577a-48f6-9b08-f6aef477368f"), Microsoft.SqlServer.Types.SqlHierarchyId.Parse("/1/2/"), "ItemzType" },
                    { new Guid("8414bf58-6331-4b3e-bbf0-f780950a337b"), Microsoft.SqlServer.Types.SqlHierarchyId.Parse("/2/2/"), "ItemzType" },
                    { new Guid("9153a516-d69e-4364-b17e-03b87442e21c"), Microsoft.SqlServer.Types.SqlHierarchyId.Parse("/1/2/1/"), "Itemz" },
                    { new Guid("b69cf0d7-70ad-4f73-aa4a-8daad5181e1e"), Microsoft.SqlServer.Types.SqlHierarchyId.Parse("/2/"), "Project" }
                });

            migrationBuilder.InsertData(
                table: "Itemzs",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "Name", "Priority", "Severity", "Status" },
                values: new object[,]
                {
                    { new Guid("0850bc8a-84c4-4c52-8ab6-b06e7bc2195b"), "User 4", new DateTimeOffset(new DateTime(2019, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Requirements to be described here.", "Item 4", "Low", "Medium", "New" },
                    { new Guid("4adde56d-8081-45ea-bd37-5c830b63873b"), "User 5", new DateTimeOffset(new DateTime(2019, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Requirements to be described here.", "Item 5", "Low", "Medium", "New" },
                    { new Guid("5e76f8e8-d3e7-41db-b084-f64c107c6783"), "User 2", new DateTimeOffset(new DateTime(2019, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Requirements to be described here.", "Item 2", "Medium", "Medium", "New" },
                    { new Guid("885b8e56-ffe6-4bc9-82e2-ce23230991db"), "User 3", new DateTimeOffset(new DateTime(2019, 7, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Requirements to be described here.", "Item 3", "Low", "Medium", "New" },
                    { new Guid("9153a516-d69e-4364-b17e-03b87442e21c"), "User 1", new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Requirements to be described here.", "Item 1", "High", "Medium", "New" }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "Name", "Status" },
                values: new object[,]
                {
                    { new Guid("42f62a6c-fcda-4dac-a06c-406ac1c17770"), "User 1", new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "This is Project 1", "Project 1", "Active" },
                    { new Guid("b69cf0d7-70ad-4f73-aa4a-8daad5181e1e"), "User 1", new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "This is Project 2", "Project 2", "Active" }
                });

            migrationBuilder.InsertData(
                table: "ItemzTypes",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "IsSystem", "Name", "ProjectId", "Status" },
                values: new object[,]
                {
                    { new Guid("1a069648-6ad6-4c8a-be05-be747bdeb8da"), "User 1", new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "This is Parking Lot system ItemzType", true, "Parking Lot", new Guid("b69cf0d7-70ad-4f73-aa4a-8daad5181e1e"), "Active" },
                    { new Guid("473fe535-1420-42cf-8a40-224388b8df24"), "User 1", new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "This is Parking Lot system ItemzType", true, "Parking Lot", new Guid("42f62a6c-fcda-4dac-a06c-406ac1c17770"), "Active" },
                    { new Guid("52ca1dfc-b187-47fc-a379-57af33404b34"), "User 1", new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "Used for testing that ItemzType Name remains unique within a given Project", false, "TESTING - Unique ItemzType Name", new Guid("b69cf0d7-70ad-4f73-aa4a-8daad5181e1e"), "Active" },
                    { new Guid("611639db-577a-48f6-9b08-f6aef477368f"), "User 1", new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "This is ItemzType 1", false, "ItemzType 1", new Guid("42f62a6c-fcda-4dac-a06c-406ac1c17770"), "Active" },
                    { new Guid("8414bf58-6331-4b3e-bbf0-f780950a337b"), "User 1", new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 1, 0, 0, 0)), "This is ItemzType 2", false, "ItemzType 2", new Guid("b69cf0d7-70ad-4f73-aa4a-8daad5181e1e"), "Active" }
                });

            migrationBuilder.InsertData(
                table: "ItemzTypeJoinItemz",
                columns: new[] { "ItemzId", "ItemzTypeId" },
                values: new object[,]
                {
                    { new Guid("9153a516-d69e-4364-b17e-03b87442e21c"), new Guid("611639db-577a-48f6-9b08-f6aef477368f") },
                    { new Guid("5e76f8e8-d3e7-41db-b084-f64c107c6783"), new Guid("8414bf58-6331-4b3e-bbf0-f780950a337b") }
                });
        }
    }
}
