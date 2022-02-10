using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class AddedCollegeLookupAcronym : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Acronym",
                table: "CollegeLookup",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "CollegeLookup",
                keyColumn: "Code",
                keyValue: 1,
                columns: new[] { "Acronym", "Name" },
                values: new object[] { "CPSBC", "College of Physicians and Surgeons of BC" });

            migrationBuilder.UpdateData(
                table: "CollegeLookup",
                keyColumn: "Code",
                keyValue: 2,
                columns: new[] { "Acronym", "Name" },
                values: new object[] { "CPBC", "College of Pharmacists of BC" });

            migrationBuilder.UpdateData(
                table: "CollegeLookup",
                keyColumn: "Code",
                keyValue: 3,
                columns: new[] { "Acronym", "Name" },
                values: new object[] { "BCCNM", "BC College of Nurses and Midwives" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Acronym",
                table: "CollegeLookup");

            migrationBuilder.UpdateData(
                table: "CollegeLookup",
                keyColumn: "Code",
                keyValue: 1,
                column: "Name",
                value: "College of Physicians and Surgeons of BC (CPSBC)");

            migrationBuilder.UpdateData(
                table: "CollegeLookup",
                keyColumn: "Code",
                keyValue: 2,
                column: "Name",
                value: "College of Pharmacists of BC (CPBC)");

            migrationBuilder.UpdateData(
                table: "CollegeLookup",
                keyColumn: "Code",
                keyValue: 3,
                column: "Name",
                value: "BC College of Nurses and Midwives (BCCNM)");
        }
    }
}
