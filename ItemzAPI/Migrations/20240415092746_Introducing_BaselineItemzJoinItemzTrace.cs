using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItemzApp.API.Migrations
{
    public partial class Introducing_BaselineItemzJoinItemzTrace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaselineItemzJoinItemzTrace",
                columns: table => new
                {
                    BaselineFromItemzId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BaselineToItemzId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaselineItemzJoinItemzTrace", x => new { x.BaselineFromItemzId, x.BaselineToItemzId });
                    table.ForeignKey(
                        name: "FK_BaselineItemzJoinItemzTrace_BaselineItemz_BaselineFromItemzId",
                        column: x => x.BaselineFromItemzId,
                        principalTable: "BaselineItemz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaselineItemzJoinItemzTrace_BaselineItemz_BaselineToItemzId",
                        column: x => x.BaselineToItemzId,
                        principalTable: "BaselineItemz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaselineItemzJoinItemzTrace_BaselineToItemzId",
                table: "BaselineItemzJoinItemzTrace",
                column: "BaselineToItemzId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaselineItemzJoinItemzTrace");
        }
    }
}
