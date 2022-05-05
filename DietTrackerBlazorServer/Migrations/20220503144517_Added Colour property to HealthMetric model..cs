using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DietTrackerBlazorServer.Migrations
{
    public partial class AddedColourpropertytoHealthMetricmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "HealthMetrics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "HealthMetrics");
        }
    }
}
