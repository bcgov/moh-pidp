using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class AddMsTeams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AccessTypeLookup",
                columns: new[] { "Code", "Name" },
                values: new object[] { 6, "MS Teams for Clinical Use" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccessTypeLookup",
                keyColumn: "Code",
                keyValue: 6);
        }
    }
}
