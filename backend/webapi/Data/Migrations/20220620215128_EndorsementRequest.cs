using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class EndorsementRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EndorsementRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RequestingPartyId = table.Column<int>(type: "integer", nullable: false),
                    EndorsingPartyId = table.Column<int>(type: "integer", nullable: true),
                    Token = table.Column<Guid>(type: "uuid", nullable: false),
                    RecipientEmail = table.Column<string>(type: "text", nullable: false),
                    JobTitle = table.Column<string>(type: "text", nullable: false),
                    Approved = table.Column<bool>(type: "boolean", nullable: true),
                    AdjudicatedOn = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    Created = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndorsementRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EndorsementRequest_Party_EndorsingPartyId",
                        column: x => x.EndorsingPartyId,
                        principalTable: "Party",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EndorsementRequest_Party_RequestingPartyId",
                        column: x => x.RequestingPartyId,
                        principalTable: "Party",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EndorsementRequest_EndorsingPartyId",
                table: "EndorsementRequest",
                column: "EndorsingPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_EndorsementRequest_RequestingPartyId",
                table: "EndorsementRequest",
                column: "RequestingPartyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EndorsementRequest");
        }
    }
}
