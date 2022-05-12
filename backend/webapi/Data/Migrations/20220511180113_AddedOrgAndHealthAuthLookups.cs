using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class AddedOrgAndHealthAuthLookups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HealthAuthorityLookup",
                columns: table => new
                {
                    Code = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthAuthorityLookup", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationLookup",
                columns: table => new
                {
                    Code = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationLookup", x => x.Code);
                });

            migrationBuilder.InsertData(
                table: "HealthAuthorityLookup",
                columns: new[] { "Code", "Name" },
                values: new object[,]
                {
                    { 1, "Provincial Health Services Authority" },
                    { 2, "Vancouver Island Health Authority" },
                    { 3, "Vancouver Coastal Health Authority" },
                    { 4, "Fraser Health Authority" },
                    { 5, "Interior Health Authority" },
                    { 6, "Northern Health Authority" },
                    { 7, "First Nations Health Authority" }
                });

            migrationBuilder.InsertData(
                table: "OrganizationLookup",
                columns: new[] { "Code", "Name" },
                values: new object[,]
                {
                    { 1, "Health Authority" },
                    { 2, "BC Government Ministry" },
                    { 3, "Maximus" },
                    { 4, "ICBC" },
                    { 5, "Other" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HealthAuthorityLookup");

            migrationBuilder.DropTable(
                name: "OrganizationLookup");
        }
    }
}
