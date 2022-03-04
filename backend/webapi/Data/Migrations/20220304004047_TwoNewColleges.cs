using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class TwoNewColleges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CollegeLookup",
                columns: new[] { "Code", "Acronym", "Name" },
                values: new object[,]
                {
                    { 5, "CDSBC", "College of Dental Surgeons of British Columbia" },
                    { 6, "COBC", "College Of Optometrists of British Columbia" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CollegeLookup",
                keyColumn: "Code",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CollegeLookup",
                keyColumn: "Code",
                keyValue: 6);
        }
    }
}
