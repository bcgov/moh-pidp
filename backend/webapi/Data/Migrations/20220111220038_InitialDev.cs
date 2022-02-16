using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class InitialDev : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CollegeLookup",
                columns: table => new
                {
                    Code = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollegeLookup", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "CountryLookup",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryLookup", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Party",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    PreferredFirstName = table.Column<string>(type: "text", nullable: true),
                    PreferredMiddleName = table.Column<string>(type: "text", nullable: true),
                    PreferredLastName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    JobTitle = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Party", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProvinceLookup",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false),
                    CountryCode = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvinceLookup", x => x.Code);
                });

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

            migrationBuilder.CreateTable(
                name: "PartyCertification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PartyId = table.Column<int>(type: "integer", nullable: false),
                    CollegeCode = table.Column<int>(type: "integer", nullable: false),
                    LicenceNumber = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyCertification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartyCertification_CollegeLookup_CollegeCode",
                        column: x => x.CollegeCode,
                        principalTable: "CollegeLookup",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartyCertification_Party_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Party",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryCode = table.Column<string>(type: "text", nullable: false),
                    ProvinceCode = table.Column<string>(type: "text", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    Postal = table.Column<string>(type: "text", nullable: false),
                    Discriminator = table.Column<string>(type: "text", nullable: false),
                    FacilityId = table.Column<int>(type: "integer", nullable: true),
                    Created = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_CountryLookup_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "CountryLookup",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Address_Facility_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "Facility",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Address_ProvinceLookup_ProvinceCode",
                        column: x => x.ProvinceCode,
                        principalTable: "ProvinceLookup",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CollegeLookup",
                columns: new[] { "Code", "Name" },
                values: new object[,]
                {
                    { 1, "College of Physicians and Surgeons of BC (CPSBC)" },
                    { 2, "College of Pharmacists of BC (CPBC)" },
                    { 3, "BC College of Nurses and Midwives (BCCNM)" }
                });

            migrationBuilder.InsertData(
                table: "CountryLookup",
                columns: new[] { "Code", "Name" },
                values: new object[,]
                {
                    { "CA", "Canada" },
                    { "US", "United States" }
                });

            migrationBuilder.InsertData(
                table: "ProvinceLookup",
                columns: new[] { "Code", "CountryCode", "Name" },
                values: new object[,]
                {
                    { "AB", "CA", "Alberta" },
                    { "AK", "US", "Alaska" },
                    { "AL", "US", "Alabama" },
                    { "AR", "US", "Arkansas" },
                    { "AS", "US", "American Samoa" },
                    { "AZ", "US", "Arizona" },
                    { "BC", "CA", "British Columbia" },
                    { "CA", "US", "California" },
                    { "CO", "US", "Colorado" },
                    { "CT", "US", "Connecticut" },
                    { "DC", "US", "District of Columbia" },
                    { "DE", "US", "Delaware" },
                    { "FL", "US", "Florida" },
                    { "GA", "US", "Georgia" },
                    { "GU", "US", "Guam" },
                    { "HI", "US", "Hawaii" },
                    { "IA", "US", "Iowa" },
                    { "ID", "US", "Idaho" },
                    { "IL", "US", "Illinois" },
                    { "IN", "US", "Indiana" },
                    { "KS", "US", "Kansas" },
                    { "KY", "US", "Kentucky" },
                    { "LA", "US", "Louisiana" },
                    { "MA", "US", "Massachusetts" },
                    { "MB", "CA", "Manitoba" },
                    { "MD", "US", "Maryland" },
                    { "ME", "US", "Maine" },
                    { "MI", "US", "Michigan" },
                    { "MN", "US", "Minnesota" },
                    { "MO", "US", "Missouri" },
                    { "MP", "US", "Northern Mariana Islands" },
                    { "MS", "US", "Mississippi" },
                    { "MT", "US", "Montana" },
                    { "NB", "CA", "New Brunswick" },
                    { "NC", "US", "North Carolina" },
                    { "ND", "US", "North Dakota" },
                    { "NE", "US", "Nebraska" },
                    { "NH", "US", "New Hampshire" },
                    { "NJ", "US", "New Jersey" },
                    { "NL", "CA", "Newfoundland and Labrador" },
                    { "NM", "US", "New Mexico" },
                    { "NS", "CA", "Nova Scotia" },
                    { "NT", "CA", "Northwest Territories" },
                    { "NU", "CA", "Nunavut" },
                    { "NV", "US", "Nevada" },
                    { "NY", "US", "New York" },
                    { "OH", "US", "Ohio" },
                    { "OK", "US", "Oklahoma" },
                    { "ON", "CA", "Ontario" },
                    { "OR", "US", "Oregon" },
                    { "PA", "US", "Pennsylvania" },
                    { "PE", "CA", "Prince Edward Island" },
                    { "PR", "US", "Puerto Rico" },
                    { "QC", "CA", "Quebec" },
                    { "RI", "US", "Rhode Island" },
                    { "SC", "US", "South Carolina" },
                    { "SD", "US", "South Dakota" },
                    { "SK", "CA", "Saskatchewan" },
                    { "TN", "US", "Tennessee" },
                    { "TX", "US", "Texas" },
                    { "UM", "US", "United States Minor Outlying Islands" },
                    { "UT", "US", "Utah" },
                    { "VA", "US", "Virginia" },
                    { "VI", "US", "Virgin Islands, U.S." },
                    { "VT", "US", "Vermont" },
                    { "WA", "US", "Washington" },
                    { "WI", "US", "Wisconsin" },
                    { "WV", "US", "West Virginia" },
                    { "WY", "US", "Wyoming" },
                    { "YT", "CA", "Yukon" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_CountryCode",
                table: "Address",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Address_FacilityId",
                table: "Address",
                column: "FacilityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Address_ProvinceCode",
                table: "Address",
                column: "ProvinceCode");

            migrationBuilder.CreateIndex(
                name: "IX_Facility_PartyId",
                table: "Facility",
                column: "PartyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartyCertification_CollegeCode",
                table: "PartyCertification",
                column: "CollegeCode");

            migrationBuilder.CreateIndex(
                name: "IX_PartyCertification_PartyId",
                table: "PartyCertification",
                column: "PartyId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "PartyCertification");

            migrationBuilder.DropTable(
                name: "CountryLookup");

            migrationBuilder.DropTable(
                name: "Facility");

            migrationBuilder.DropTable(
                name: "ProvinceLookup");

            migrationBuilder.DropTable(
                name: "CollegeLookup");

            migrationBuilder.DropTable(
                name: "Party");
        }
    }
}
