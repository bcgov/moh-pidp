using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlrIntake.Data.Migrations
{
    public partial class Plr_CpnIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Plr_PlrRecord_Cpn",
                table: "Plr_PlrRecord",
                column: "Cpn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Plr_PlrRecord_Cpn",
                table: "Plr_PlrRecord");
        }
    }
}
