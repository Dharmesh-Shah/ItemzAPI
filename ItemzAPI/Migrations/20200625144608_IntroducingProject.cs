using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItemzApp.API.Migrations
{
    public partial class IntroducingProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("05277468-9183-458d-96f3-eecb8d4296c1"));

            migrationBuilder.DeleteData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("3475d7c0-d487-4e38-9b56-08b9e0ff6145"));

            migrationBuilder.DeleteData(
                table: "Itemzs",
                keyColumn: "Id",
                keyValue: new Guid("b7ee9814-6556-429d-a0a1-69a3add2ddbe"));

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Status = table.Column<string>(maxLength: 64, nullable: false),
                    Description = table.Column<string>(maxLength: 1028, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 128, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectJoinItemz",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(nullable: false),
                    ItemzId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectJoinItemz", x => new { x.ProjectId, x.ItemzId });
                    table.ForeignKey(
                        name: "FK_ProjectJoinItemz_Itemzs_ItemzId",
                        column: x => x.ItemzId,
                        principalTable: "Itemzs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectJoinItemz_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Itemzs",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "Name", "Priority", "Status" },
                values: new object[,]
                {
                    { new Guid("e816de73-5c50-4841-944d-c63b651c00e6"), "User 3", new DateTimeOffset(new DateTime(2019, 7, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Requirements to be described here.", "Item 3", "Low", "New" },
                    { new Guid("7eae6d78-36e6-495b-b64f-845b583381a9"), "User 4", new DateTimeOffset(new DateTime(2019, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Requirements to be described here.", "Item 4", "Low", "New" },
                    { new Guid("522eba11-b8b2-40e7-873c-983f1c355c37"), "User 5", new DateTimeOffset(new DateTime(2019, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Requirements to be described here.", "Item 5", "Low", "New" }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "Name", "Status" },
                values: new object[,]
                {
                    { new Guid("42f62a6c-fcda-4dac-a06c-406ac1c17770"), "User 1", new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "This is Project 1", "Project 1", "Active" },
                    { new Guid("b69cf0d7-70ad-4f73-aa4a-8daad5181e1e"), "User 1", new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "This is Project 2", "Project 2", "Active" }
                });

            migrationBuilder.InsertData(
                table: "ProjectJoinItemz",
                columns: new[] { "ProjectId", "ItemzId" },
                values: new object[] { new Guid("42f62a6c-fcda-4dac-a06c-406ac1c17770"), new Guid("9153a516-d69e-4364-b17e-03b87442e21c") });

            migrationBuilder.InsertData(
                table: "ProjectJoinItemz",
                columns: new[] { "ProjectId", "ItemzId" },
                values: new object[] { new Guid("b69cf0d7-70ad-4f73-aa4a-8daad5181e1e"), new Guid("5e76f8e8-d3e7-41db-b084-f64c107c6783") });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectJoinItemz_ItemzId",
                table: "ProjectJoinItemz",
                column: "ItemzId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectJoinItemz");

            migrationBuilder.DropTable(
                name: "Projects");

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

            migrationBuilder.InsertData(
                table: "Itemzs",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "Name", "Priority", "Status" },
                values: new object[] { new Guid("b7ee9814-6556-429d-a0a1-69a3add2ddbe"), "User 3", new DateTimeOffset(new DateTime(2019, 7, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Requirements to be described here.", "Item 3", "Low", "New" });

            migrationBuilder.InsertData(
                table: "Itemzs",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "Name", "Priority", "Status" },
                values: new object[] { new Guid("3475d7c0-d487-4e38-9b56-08b9e0ff6145"), "User 4", new DateTimeOffset(new DateTime(2019, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Requirements to be described here.", "Item 4", "Low", "New" });

            migrationBuilder.InsertData(
                table: "Itemzs",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "Name", "Priority", "Status" },
                values: new object[] { new Guid("05277468-9183-458d-96f3-eecb8d4296c1"), "User 5", new DateTimeOffset(new DateTime(2019, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Requirements to be described here.", "Item 5", "Low", "New" });
        }
    }
}
