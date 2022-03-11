using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PlrIntake.Data.Migrations
{
    public partial class Plr_Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plr_IdentifierType",
                columns: table => new
                {
                    Oid = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plr_IdentifierType", x => x.Oid);
                });

            migrationBuilder.CreateTable(
                name: "Plr_PlrRecord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ipc = table.Column<string>(type: "text", nullable: false),
                    Cpn = table.Column<string>(type: "text", nullable: true),
                    IdentifierType = table.Column<string>(type: "text", nullable: true),
                    CollegeId = table.Column<string>(type: "text", nullable: true),
                    ProviderRoleType = table.Column<string>(type: "text", nullable: true),
                    MspId = table.Column<string>(type: "text", nullable: true),
                    NamePrefix = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    SecondName = table.Column<string>(type: "text", nullable: true),
                    ThirdName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Suffix = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    StatusCode = table.Column<string>(type: "text", nullable: true),
                    StatusReasonCode = table.Column<string>(type: "text", nullable: true),
                    StatusStartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    StatusExpiryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Address1Line1 = table.Column<string>(type: "text", nullable: true),
                    Address1Line2 = table.Column<string>(type: "text", nullable: true),
                    Address1Line3 = table.Column<string>(type: "text", nullable: true),
                    City1 = table.Column<string>(type: "text", nullable: true),
                    Province1 = table.Column<string>(type: "text", nullable: true),
                    Country1 = table.Column<string>(type: "text", nullable: true),
                    PostalCode1 = table.Column<string>(type: "text", nullable: true),
                    Address1StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    TelephoneAreaCode = table.Column<string>(type: "text", nullable: true),
                    TelephoneNumber = table.Column<string>(type: "text", nullable: true),
                    FaxAreaCode = table.Column<string>(type: "text", nullable: true),
                    FaxNumber = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    ConditionCode = table.Column<string>(type: "text", nullable: true),
                    ConditionStartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ConditionEndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plr_PlrRecord", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plr_Credential",
                columns: table => new
                {
                    PlrRecordId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plr_Credential", x => new { x.PlrRecordId, x.Id });
                    table.ForeignKey(
                        name: "FK_Plr_Credential_Plr_PlrRecord_PlrRecordId",
                        column: x => x.PlrRecordId,
                        principalTable: "Plr_PlrRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Plr_Expertise",
                columns: table => new
                {
                    PlrRecordId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plr_Expertise", x => new { x.PlrRecordId, x.Id });
                    table.ForeignKey(
                        name: "FK_Plr_Expertise_Plr_PlrRecord_PlrRecordId",
                        column: x => x.PlrRecordId,
                        principalTable: "Plr_PlrRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Plr_IdentifierType",
                columns: new[] { "Oid", "Name" },
                values: new object[,]
                {
                    { "2.16.840.1.113883.3.40.2.10", "LPNID" },
                    { "2.16.840.1.113883.3.40.2.14", "PHID" },
                    { "2.16.840.1.113883.3.40.2.18", "RMID" },
                    { "2.16.840.1.113883.3.40.2.19", "RNID" },
                    { "2.16.840.1.113883.3.40.2.20", "RNPID" },
                    { "2.16.840.1.113883.3.40.2.4", "CPSID" },
                    { "2.16.840.1.113883.3.40.2.44", "PPID" },
                    { "2.16.840.1.113883.3.40.2.46", "MOAID" },
                    { "2.16.840.1.113883.3.40.2.6", "DENID" },
                    { "2.16.840.1.113883.4.361", "SWID" },
                    { "2.16.840.1.113883.4.362", "PSYCHID" },
                    { "2.16.840.1.113883.4.363", "CCID" },
                    { "2.16.840.1.113883.4.364", "OTID" },
                    { "2.16.840.1.113883.4.401", "PHTID" },
                    { "2.16.840.1.113883.4.414", "PHYSIOID" },
                    { "2.16.840.1.113883.4.422", "CHIROID" },
                    { "2.16.840.1.113883.4.429", "OPTID" },
                    { "2.16.840.1.113883.4.433", "RMTID" },
                    { "2.16.840.1.113883.4.439", "KNID" },
                    { "2.16.840.1.113883.4.452", "MFTID" },
                    { "2.16.840.1.113883.4.454", "RACID" },
                    { "2.16.840.1.113883.4.477", "COUNID" },
                    { "2.16.840.1.113883.4.530", "RDID" },
                    { "2.16.840.1.113883.4.538", "NDID" },
                    { "2.16.840.1.113883.4.608", "RPNRC" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plr_PlrRecord_Ipc",
                table: "Plr_PlrRecord",
                column: "Ipc",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Plr_Credential");

            migrationBuilder.DropTable(
                name: "Plr_Expertise");

            migrationBuilder.DropTable(
                name: "Plr_IdentifierType");

            migrationBuilder.DropTable(
                name: "Plr_PlrRecord");
        }
    }
}
