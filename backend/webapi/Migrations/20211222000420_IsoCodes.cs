using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pidp.Migrations
{
    public partial class IsoCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IsoCode",
                table: "ProvinceLookup",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IsoCode",
                table: "CountryLookup",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "CountryLookup",
                keyColumn: "Code",
                keyValue: 1,
                column: "IsoCode",
                value: "CA");

            migrationBuilder.UpdateData(
                table: "CountryLookup",
                keyColumn: "Code",
                keyValue: 2,
                column: "IsoCode",
                value: "US");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 1,
                column: "IsoCode",
                value: "AB");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 2,
                column: "IsoCode",
                value: "BC");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 3,
                column: "IsoCode",
                value: "MB");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 4,
                column: "IsoCode",
                value: "NB");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 5,
                column: "IsoCode",
                value: "NL");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 6,
                column: "IsoCode",
                value: "NS");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 7,
                column: "IsoCode",
                value: "ON");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 8,
                column: "IsoCode",
                value: "PE");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 9,
                column: "IsoCode",
                value: "QC");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 10,
                column: "IsoCode",
                value: "SK");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 11,
                column: "IsoCode",
                value: "NT");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 12,
                column: "IsoCode",
                value: "NU");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 13,
                column: "IsoCode",
                value: "YT");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 14,
                column: "IsoCode",
                value: "AL");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 15,
                column: "IsoCode",
                value: "AK");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 16,
                column: "IsoCode",
                value: "AS");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 17,
                column: "IsoCode",
                value: "AZ");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 18,
                column: "IsoCode",
                value: "AR");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 19,
                column: "IsoCode",
                value: "CA");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 20,
                column: "IsoCode",
                value: "CO");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 21,
                column: "IsoCode",
                value: "CT");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 22,
                column: "IsoCode",
                value: "DE");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 23,
                column: "IsoCode",
                value: "DC");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 24,
                column: "IsoCode",
                value: "FL");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 25,
                column: "IsoCode",
                value: "GA");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 26,
                column: "IsoCode",
                value: "GU");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 27,
                column: "IsoCode",
                value: "HI");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 28,
                column: "IsoCode",
                value: "ID");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 29,
                column: "IsoCode",
                value: "IL");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 30,
                column: "IsoCode",
                value: "IN");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 31,
                column: "IsoCode",
                value: "IA");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 32,
                column: "IsoCode",
                value: "KS");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 33,
                column: "IsoCode",
                value: "KY");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 34,
                column: "IsoCode",
                value: "LA");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 35,
                column: "IsoCode",
                value: "ME");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 36,
                column: "IsoCode",
                value: "MD");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 37,
                column: "IsoCode",
                value: "MA");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 38,
                column: "IsoCode",
                value: "MI");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 39,
                column: "IsoCode",
                value: "MN");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 40,
                column: "IsoCode",
                value: "MS");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 41,
                column: "IsoCode",
                value: "MO");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 42,
                column: "IsoCode",
                value: "MT");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 43,
                column: "IsoCode",
                value: "NE");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 44,
                column: "IsoCode",
                value: "NV");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 45,
                column: "IsoCode",
                value: "NH");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 46,
                column: "IsoCode",
                value: "NJ");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 47,
                column: "IsoCode",
                value: "NM");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 48,
                column: "IsoCode",
                value: "NY");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 49,
                column: "IsoCode",
                value: "NC");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 50,
                column: "IsoCode",
                value: "ND");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 51,
                column: "IsoCode",
                value: "MP");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 52,
                column: "IsoCode",
                value: "OH");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 53,
                column: "IsoCode",
                value: "OK");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 54,
                column: "IsoCode",
                value: "OR");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 55,
                column: "IsoCode",
                value: "PA");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 56,
                column: "IsoCode",
                value: "PR");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 57,
                column: "IsoCode",
                value: "RI");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 58,
                column: "IsoCode",
                value: "SC");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 59,
                column: "IsoCode",
                value: "SD");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 60,
                column: "IsoCode",
                value: "TN");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 61,
                column: "IsoCode",
                value: "TX");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 62,
                column: "IsoCode",
                value: "UM");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 63,
                column: "IsoCode",
                value: "UT");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 64,
                column: "IsoCode",
                value: "VT");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 65,
                column: "IsoCode",
                value: "VI");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 66,
                column: "IsoCode",
                value: "VA");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 67,
                column: "IsoCode",
                value: "WA");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 68,
                column: "IsoCode",
                value: "WV");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 69,
                column: "IsoCode",
                value: "WI");

            migrationBuilder.UpdateData(
                table: "ProvinceLookup",
                keyColumn: "Code",
                keyValue: 70,
                column: "IsoCode",
                value: "WY");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsoCode",
                table: "ProvinceLookup");

            migrationBuilder.DropColumn(
                name: "IsoCode",
                table: "CountryLookup");
        }
    }
}
