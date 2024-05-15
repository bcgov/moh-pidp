using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class RemoveOrganization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Facility_FacilityId",
                table: "Address");

            migrationBuilder.DropTable(
                name: "Facility");

            migrationBuilder.DropTable(
                name: "HcimEnrolment");

            migrationBuilder.DropTable(
                name: "HealthAuthorityLookup");

            migrationBuilder.DropTable(
                name: "OrganizationLookup");

            migrationBuilder.DropTable(
                name: "PartyAccessAdministrator");

            migrationBuilder.DropTable(
                name: "PartyOrgainizationDetail");

            migrationBuilder.DropIndex(
                name: "IX_Address_FacilityId",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "Party");

            migrationBuilder.DropColumn(
                name: "FacilityId",
                table: "Address");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "Party",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FacilityId",
                table: "Address",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Facility",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PartyId = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facility", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Facility_Party_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Party",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HcimEnrolment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ManagesTasks = table.Column<bool>(type: "boolean", nullable: false),
                    ModifiesPhns = table.Column<bool>(type: "boolean", nullable: false),
                    RecordsNewborns = table.Column<bool>(type: "boolean", nullable: false),
                    SearchesIdentifiers = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HcimEnrolment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HcimEnrolment_AccessRequest_Id",
                        column: x => x.Id,
                        principalTable: "AccessRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "PartyAccessAdministrator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PartyId = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Modified = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyAccessAdministrator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartyAccessAdministrator_Party_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Party",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartyOrgainizationDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PartyId = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    EmployeeIdentifier = table.Column<string>(type: "text", nullable: false),
                    HealthAuthorityCode = table.Column<int>(type: "integer", nullable: false),
                    Modified = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    OrganizationCode = table.Column<int>(type: "integer", nullable: false)
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
                name: "IX_Address_FacilityId",
                table: "Address",
                column: "FacilityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Facility_PartyId",
                table: "Facility",
                column: "PartyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartyAccessAdministrator_PartyId",
                table: "PartyAccessAdministrator",
                column: "PartyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartyOrgainizationDetail_PartyId",
                table: "PartyOrgainizationDetail",
                column: "PartyId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Facility_FacilityId",
                table: "Address",
                column: "FacilityId",
                principalTable: "Facility",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
