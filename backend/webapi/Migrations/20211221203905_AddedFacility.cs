using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pidp.Migrations
{
    public partial class AddedFacility : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    FacilityName = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
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

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Facility_FacilityId",
                table: "Address",
                column: "FacilityId",
                principalTable: "Facility",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Facility_FacilityId",
                table: "Address");

            migrationBuilder.DropTable(
                name: "Facility");

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
    }
}
