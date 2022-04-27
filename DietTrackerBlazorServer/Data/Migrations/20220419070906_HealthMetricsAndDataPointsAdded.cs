using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietTrackerBlazorServer.Data.Migrations
{
    public partial class HealthMetricsAndDataPointsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HealthMetric",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthMetric", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthMetric_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HealthDataPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HealthMetricId = table.Column<int>(type: "int", nullable: false),
                    IdentityUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthDataPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HealthDataPoints_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HealthDataPoints_HealthMetric_HealthMetricId",
                        column: x => x.HealthMetricId,
                        principalTable: "HealthMetric",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HealthDataPoints_HealthMetricId",
                table: "HealthDataPoints",
                column: "HealthMetricId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthDataPoints_IdentityUserId",
                table: "HealthDataPoints",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_HealthMetric_IdentityUserId",
                table: "HealthMetric",
                column: "IdentityUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HealthDataPoints");

            migrationBuilder.DropTable(
                name: "HealthMetric");
        }
    }
}
