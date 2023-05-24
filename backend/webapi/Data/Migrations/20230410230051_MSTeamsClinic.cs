using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class MSTeamsClinic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_MSTeamsEnrolment_MSTeamsEnrolmentId",
                table: "Address");

            migrationBuilder.DropTable(
                name: "MSTeamsEnrolment");

            migrationBuilder.RenameColumn(
                name: "MSTeamsEnrolmentId",
                table: "Address",
                newName: "ClinicId");

            migrationBuilder.RenameIndex(
                name: "IX_Address_MSTeamsEnrolmentId",
                table: "Address",
                newName: "IX_Address_ClinicId");

            migrationBuilder.CreateTable(
                name: "MSTeamsClinic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PrivacyOfficerId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MSTeamsClinic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MSTeamsClinic_Party_PrivacyOfficerId",
                        column: x => x.PrivacyOfficerId,
                        principalTable: "Party",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MSTeamsClinicMemberEnrolment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ClinicId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MSTeamsClinicMemberEnrolment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MSTeamsClinicMemberEnrolment_AccessRequest_Id",
                        column: x => x.Id,
                        principalTable: "AccessRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MSTeamsClinicMemberEnrolment_MSTeamsClinic_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "MSTeamsClinic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AccessTypeLookup",
                keyColumn: "Code",
                keyValue: 5,
                column: "Name",
                value: "MS Teams for Clinical Use - Privacy Officer");

            migrationBuilder.InsertData(
                table: "AccessTypeLookup",
                columns: new[] { "Code", "Name" },
                values: new object[] { 8, "MS Teams for Clinical Use - Clinic Member" });

            migrationBuilder.CreateIndex(
                name: "IX_MSTeamsClinic_PrivacyOfficerId",
                table: "MSTeamsClinic",
                column: "PrivacyOfficerId");

            migrationBuilder.CreateIndex(
                name: "IX_MSTeamsClinicMemberEnrolment_ClinicId",
                table: "MSTeamsClinicMemberEnrolment",
                column: "ClinicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_MSTeamsClinic_ClinicId",
                table: "Address",
                column: "ClinicId",
                principalTable: "MSTeamsClinic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_MSTeamsClinic_ClinicId",
                table: "Address");

            migrationBuilder.DropTable(
                name: "MSTeamsClinicMemberEnrolment");

            migrationBuilder.DropTable(
                name: "MSTeamsClinic");

            migrationBuilder.DeleteData(
                table: "AccessTypeLookup",
                keyColumn: "Code",
                keyValue: 8);

            migrationBuilder.RenameColumn(
                name: "ClinicId",
                table: "Address",
                newName: "MSTeamsEnrolmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Address_ClinicId",
                table: "Address",
                newName: "IX_Address_MSTeamsEnrolmentId");

            migrationBuilder.CreateTable(
                name: "MSTeamsEnrolment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ClinicName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MSTeamsEnrolment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MSTeamsEnrolment_AccessRequest_Id",
                        column: x => x.Id,
                        principalTable: "AccessRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AccessTypeLookup",
                keyColumn: "Code",
                keyValue: 5,
                column: "Name",
                value: "MS Teams for Clinical Use");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_MSTeamsEnrolment_MSTeamsEnrolmentId",
                table: "Address",
                column: "MSTeamsEnrolmentId",
                principalTable: "MSTeamsEnrolment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
