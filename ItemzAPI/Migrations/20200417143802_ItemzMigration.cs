using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItemzApp.API.Migrations
{
    public partial class ItemzMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Itemzs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Status = table.Column<string>(maxLength: 64, nullable: false),
                    Priority = table.Column<string>(maxLength: 64, nullable: true),
                    Description = table.Column<string>(maxLength: 1028, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 128, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itemzs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Itemzs",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "Name", "Priority", "Status" },
                values: new object[,]
                {
                    { new Guid("9153a516-d69e-4364-b17e-03b87442e21c"), "User 1", new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Requirements to be described here.", "Item 1", "High", "New" },
                    { new Guid("5e76f8e8-d3e7-41db-b084-f64c107c6783"), "User 2", new DateTimeOffset(new DateTime(2019, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Requirements to be described here.", "Item 2", "Medium", "New" },
                    { new Guid("b7ee9814-6556-429d-a0a1-69a3add2ddbe"), "User 3", new DateTimeOffset(new DateTime(2019, 7, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Requirements to be described here.", "Item 3", "Low", "New" },
                    { new Guid("3475d7c0-d487-4e38-9b56-08b9e0ff6145"), "User 4", new DateTimeOffset(new DateTime(2019, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Requirements to be described here.", "Item 4", "Low", "New" },
                    { new Guid("05277468-9183-458d-96f3-eecb8d4296c1"), "User 5", new DateTimeOffset(new DateTime(2019, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)), "Requirements to be described here.", "Item 5", "Low", "New" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Itemzs");
        }
    }
}
