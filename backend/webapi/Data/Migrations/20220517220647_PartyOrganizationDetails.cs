using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class PartyOrganizationDetails : Migration
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

            migrationBuilder.CreateTable(
                name: "PartyOrgainizationDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PartyId = table.Column<int>(type: "integer", nullable: false),
                    OrganizationCode = table.Column<int>(type: "integer", nullable: false),
                    HealthAuthorityCode = table.Column<int>(type: "integer", nullable: false),
                    EmployeeIdentifier = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyOrgainizationDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartyOrgainizationDetail_Party_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Party",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_PartyOrgainizationDetail_PartyId",
                table: "PartyOrgainizationDetail",
                column: "PartyId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HealthAuthorityLookup");

            migrationBuilder.DropTable(
                name: "OrganizationLookup");

            migrationBuilder.DropTable(
                name: "PartyOrgainizationDetail");
        }
    }
}
