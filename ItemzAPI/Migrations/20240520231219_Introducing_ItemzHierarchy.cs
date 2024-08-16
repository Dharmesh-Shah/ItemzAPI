using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.SqlServer.Types;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ItemzApp.API.Migrations
{
    /// <inheritdoc />
    public partial class Introducing_ItemzHierarchy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemzHierarchy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ItemzHierarchyId = table.Column<SqlHierarchyId>(type: "hierarchyid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemzHierarchy", x => x.Id);
                });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemzHierarchy");
        }
    }
}
