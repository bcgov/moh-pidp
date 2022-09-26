using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class PrescriptionRefilEforms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AccessTypeLookup",
                columns: new[] { "Code", "Name" },
                values: new object[] { 7, "Prescription Refill eForm for Pharmacists" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccessTypeLookup",
                keyColumn: "Code",
                keyValue: 7);
        }
    }
}
