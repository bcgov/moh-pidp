using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class AddedEmailLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SendType = table.Column<string>(type: "text", nullable: false),
                    MsgId = table.Column<Guid>(type: "uuid", nullable: true),
                    SentTo = table.Column<string>(type: "text", nullable: false),
                    Cc = table.Column<string>(type: "text", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    DateSent = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    LatestStatus = table.Column<string>(type: "text", nullable: false),
                    StatusMessage = table.Column<string>(type: "text", nullable: false),
                    UpdateCount = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailLog", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailLog");
        }
    }
}
