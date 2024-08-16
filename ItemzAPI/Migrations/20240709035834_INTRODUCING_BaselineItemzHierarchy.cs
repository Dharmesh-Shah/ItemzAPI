using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.SqlServer.Types;

#nullable disable

namespace ItemzApp.API.Migrations
{
    /// <inheritdoc />
    public partial class INTRODUCING_BaselineItemzHierarchy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaselineItemzHierarchy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecordType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    BaselineItemzHierarchyId = table.Column<SqlHierarchyId>(type: "hierarchyid", nullable: true),
                    SourceItemzHierarchyId = table.Column<SqlHierarchyId>(type: "hierarchyid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaselineItemzHierarchy", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaselineItemzHierarchy");
        }
    }
}
