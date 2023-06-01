using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlrIntake.Data.Migrations
{
    public partial class Plr_ShouldBeProcessed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShouldBeProcessed",
                table: "Plr_StatusChageLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShouldBeProcessed",
                table: "Plr_StatusChageLog");
        }
    }
}
