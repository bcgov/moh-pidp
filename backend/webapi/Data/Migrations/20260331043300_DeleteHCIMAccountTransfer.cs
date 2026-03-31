using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pidp.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeleteHCIMAccountTransfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HcimAccountTransfer");

            migrationBuilder.DeleteData(
                table: "AccessTypeLookup",
                keyColumn: "Code",
                keyValue: 10);

            migrationBuilder.UpdateData(
                table: "AccessTypeLookup",
                keyColumn: "Code",
                keyValue: 2,
                column: "Name",
                value: "HCIMWeb Enrolment");

            migrationBuilder.UpdateData(
                table: "AccessTypeLookup",
                keyColumn: "Code",
                keyValue: 3,
                column: "Name",
                value: "Driver Medical Fitness");

            migrationBuilder.UpdateData(
                table: "AccessTypeLookup",
                keyColumn: "Code",
                keyValue: 4,
                column: "Name",
                value: "MS Teams for Clinical Use - Privacy Officer");

            migrationBuilder.UpdateData(
                table: "AccessTypeLookup",
                keyColumn: "Code",
                keyValue: 5,
                column: "Name",
                value: "Prescription Refill eForm for Pharmacists");

            migrationBuilder.UpdateData(
                table: "AccessTypeLookup",
                keyColumn: "Code",
                keyValue: 6,
                column: "Name",
                value: "Provider Reporting Portal");

            migrationBuilder.UpdateData(
                table: "AccessTypeLookup",
                keyColumn: "Code",
                keyValue: 7,
                column: "Name",
                value: "MS Teams for Clinical Use - Clinic Member");

            migrationBuilder.UpdateData(
                table: "AccessTypeLookup",
                keyColumn: "Code",
                keyValue: 8,
                column: "Name",
                value: "OneHealthID Service Use Policy Agreement");

            migrationBuilder.UpdateData(
                table: "AccessTypeLookup",
                keyColumn: "Code",
                keyValue: 9,
                column: "Name",
                value: "Immunization Entry eForm");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HcimAccountTransfer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    LdapUsername = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HcimAccountTransfer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HcimAccountTransfer_AccessRequest_Id",
                        column: x => x.Id,
                        principalTable: "AccessRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AccessTypeLookup",
                keyColumn: "Code",
                keyValue: 2,
                column: "Name",
                value: "HCIMWeb Account Transfer");

            migrationBuilder.UpdateData(
                table: "AccessTypeLookup",
                keyColumn: "Code",
                keyValue: 3,
                column: "Name",
                value: "HCIMWeb Enrolment");

            migrationBuilder.UpdateData(
                table: "AccessTypeLookup",
                keyColumn: "Code",
                keyValue: 4,
                column: "Name",
                value: "Driver Medical Fitness");

            migrationBuilder.UpdateData(
                table: "AccessTypeLookup",
                keyColumn: "Code",
                keyValue: 5,
                column: "Name",
                value: "MS Teams for Clinical Use - Privacy Officer");

            migrationBuilder.UpdateData(
                table: "AccessTypeLookup",
                keyColumn: "Code",
                keyValue: 6,
                column: "Name",
                value: "Prescription Refill eForm for Pharmacists");

            migrationBuilder.UpdateData(
                table: "AccessTypeLookup",
                keyColumn: "Code",
                keyValue: 7,
                column: "Name",
                value: "Provider Reporting Portal");

            migrationBuilder.UpdateData(
                table: "AccessTypeLookup",
                keyColumn: "Code",
                keyValue: 8,
                column: "Name",
                value: "MS Teams for Clinical Use - Clinic Member");

            migrationBuilder.UpdateData(
                table: "AccessTypeLookup",
                keyColumn: "Code",
                keyValue: 9,
                column: "Name",
                value: "OneHealthID Service Use Policy Agreement");

            migrationBuilder.InsertData(
                table: "AccessTypeLookup",
                columns: new[] { "Code", "Name" },
                values: new object[] { 10, "Immunization Entry eForm" });
        }
    }
}
