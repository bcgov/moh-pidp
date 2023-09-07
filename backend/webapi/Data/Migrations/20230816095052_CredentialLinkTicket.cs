using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class CredentialLinkTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CredentialLinkTicket",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<Guid>(type: "uuid", nullable: false),
                    PartyId = table.Column<int>(type: "integer", nullable: false),
                    Claimed = table.Column<bool>(type: "boolean", nullable: false),
                    ExpiresAt = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    LinkToIdentityProvider = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredentialLinkTicket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CredentialLinkTicket_Party_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Party",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CredentialLinkTicket_PartyId",
                table: "CredentialLinkTicket",
                column: "PartyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CredentialLinkTicket");
        }
    }
}
