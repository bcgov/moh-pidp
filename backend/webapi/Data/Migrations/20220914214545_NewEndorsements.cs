using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class NewEndorsements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EndorsementRequest_Party_EndorsingPartyId",
                table: "EndorsementRequest");

            migrationBuilder.DropColumn(
                name: "AdjudicatedOn",
                table: "EndorsementRequest");

            migrationBuilder.DropColumn(
                name: "Approved",
                table: "EndorsementRequest");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "EndorsementRequest");

            migrationBuilder.RenameColumn(
                name: "EndorsingPartyId",
                table: "EndorsementRequest",
                newName: "ReceivingPartyId");

            migrationBuilder.RenameIndex(
                name: "IX_EndorsementRequest_EndorsingPartyId",
                table: "EndorsementRequest",
                newName: "IX_EndorsementRequest_ReceivingPartyId");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInformation",
                table: "EndorsementRequest",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "EndorsementRequest",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Instant>(
                name: "StatusDate",
                table: "EndorsementRequest",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: NodaTime.Instant.FromUnixTimeTicks(0L));

            migrationBuilder.CreateTable(
                name: "Endorsement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    Created = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Endorsement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EndorsementRelationship",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PartyId = table.Column<int>(type: "integer", nullable: false),
                    EndorsementId = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndorsementRelationship", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EndorsementRelationship_Endorsement_EndorsementId",
                        column: x => x.EndorsementId,
                        principalTable: "Endorsement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EndorsementRelationship_Party_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Party",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EndorsementRelationship_EndorsementId",
                table: "EndorsementRelationship",
                column: "EndorsementId");

            migrationBuilder.CreateIndex(
                name: "IX_EndorsementRelationship_PartyId",
                table: "EndorsementRelationship",
                column: "PartyId");

            migrationBuilder.AddForeignKey(
                name: "FK_EndorsementRequest_Party_ReceivingPartyId",
                table: "EndorsementRequest",
                column: "ReceivingPartyId",
                principalTable: "Party",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EndorsementRequest_Party_ReceivingPartyId",
                table: "EndorsementRequest");

            migrationBuilder.DropTable(
                name: "EndorsementRelationship");

            migrationBuilder.DropTable(
                name: "Endorsement");

            migrationBuilder.DropColumn(
                name: "AdditionalInformation",
                table: "EndorsementRequest");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EndorsementRequest");

            migrationBuilder.DropColumn(
                name: "StatusDate",
                table: "EndorsementRequest");

            migrationBuilder.RenameColumn(
                name: "ReceivingPartyId",
                table: "EndorsementRequest",
                newName: "EndorsingPartyId");

            migrationBuilder.RenameIndex(
                name: "IX_EndorsementRequest_ReceivingPartyId",
                table: "EndorsementRequest",
                newName: "IX_EndorsementRequest_EndorsingPartyId");

            migrationBuilder.AddColumn<Instant>(
                name: "AdjudicatedOn",
                table: "EndorsementRequest",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "EndorsementRequest",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "EndorsementRequest",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_EndorsementRequest_Party_EndorsingPartyId",
                table: "EndorsementRequest",
                column: "EndorsingPartyId",
                principalTable: "Party",
                principalColumn: "Id");
        }
    }
}
