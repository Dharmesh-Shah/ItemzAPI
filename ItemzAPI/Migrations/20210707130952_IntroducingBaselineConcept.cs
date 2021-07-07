using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ItemzApp.API.Migrations
{
    public partial class IntroducingBaselineConcept : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Baseline",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1028)", maxLength: 1028, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Baseline", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Baseline_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BaselineItemz",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemzId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Priority = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1028)", maxLength: 1028, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Severity = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaselineItemz", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BaselineItemzType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ItemzTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1028)", maxLength: 1028, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    BaselineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaselineItemzType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaselineItemzType_Baseline_BaselineId",
                        column: x => x.BaselineId,
                        principalTable: "Baseline",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BaselineItemzTypeJoinBaselineItemz",
                columns: table => new
                {
                    BaselineItemzTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BaselineItemzId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaselineItemzTypeJoinBaselineItemz", x => new { x.BaselineItemzTypeId, x.BaselineItemzId });
                    table.ForeignKey(
                        name: "FK_BaselineItemzTypeJoinBaselineItemz_BaselineItemz_BaselineItemzId",
                        column: x => x.BaselineItemzId,
                        principalTable: "BaselineItemz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaselineItemzTypeJoinBaselineItemz_BaselineItemzType_BaselineItemzTypeId",
                        column: x => x.BaselineItemzTypeId,
                        principalTable: "BaselineItemzType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Baseline_Name",
                table: "Baseline",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Baseline_ProjectId",
                table: "Baseline",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_BaselineItemzType_BaselineId",
                table: "BaselineItemzType",
                column: "BaselineId");

            migrationBuilder.CreateIndex(
                name: "IX_BaselineItemzTypeJoinBaselineItemz_BaselineItemzId",
                table: "BaselineItemzTypeJoinBaselineItemz",
                column: "BaselineItemzId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaselineItemzTypeJoinBaselineItemz");

            migrationBuilder.DropTable(
                name: "BaselineItemz");

            migrationBuilder.DropTable(
                name: "BaselineItemzType");

            migrationBuilder.DropTable(
                name: "Baseline");
        }
    }
}
