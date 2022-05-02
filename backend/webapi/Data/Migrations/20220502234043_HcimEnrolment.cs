using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class HcimEnrolment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "PartyAccessAdministrator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PartyId = table.Column<int>(type: "integer", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
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

            migrationBuilder.UpdateData(
                table: "CollegeLookup",
                keyColumn: "Code",
                keyValue: 6,
                column: "Name",
                value: "College of Optometrists of British Columbia");

            migrationBuilder.CreateIndex(
                name: "IX_PartyAccessAdministrator_PartyId",
                table: "PartyAccessAdministrator",
                column: "PartyId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HcimEnrolment");

            migrationBuilder.DropTable(
                name: "PartyAccessAdministrator");

            migrationBuilder.UpdateData(
                table: "CollegeLookup",
                keyColumn: "Code",
                keyValue: 6,
                column: "Name",
                value: "College Of Optometrists of British Columbia");
        }
    }
}
