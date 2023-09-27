using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class ImmsBC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AccessTypeLookup",
                columns: new[] { "Code", "Name" },
                values: new object[] { 10, "Immunization Entry eForm" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccessTypeLookup",
                keyColumn: "Code",
                keyValue: 10);
        }
    }
}
