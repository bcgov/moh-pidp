using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class CredentialLinkErrorLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CredentialLinkErrorLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExistingCredentialId = table.Column<int>(type: "integer", nullable: false),
                    CredentialLinkTicketId = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredentialLinkErrorLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CredentialLinkErrorLog_Credential_ExistingCredentialId",
                        column: x => x.ExistingCredentialId,
                        principalTable: "Credential",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CredentialLinkErrorLog_CredentialLinkTicket_CredentialLinkT~",
                        column: x => x.CredentialLinkTicketId,
                        principalTable: "CredentialLinkTicket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CredentialLinkErrorLog_CredentialLinkTicketId",
                table: "CredentialLinkErrorLog",
                column: "CredentialLinkTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_CredentialLinkErrorLog_ExistingCredentialId",
                table: "CredentialLinkErrorLog",
                column: "ExistingCredentialId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CredentialLinkErrorLog");
        }
    }
}
