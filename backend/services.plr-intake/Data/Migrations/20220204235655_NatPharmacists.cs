using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlrIntake.Migrations
{
    public partial class NatPharmacists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Plr_IdentifierType",
                keyColumn: "Oid",
                keyValue: "2.16.840.1.113883.4.538",
                column: "Name",
                value: "NDID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Plr_IdentifierType",
                keyColumn: "Oid",
                keyValue: "2.16.840.1.113883.4.538",
                column: "Name",
                value: "NAPID");
        }
    }
}
