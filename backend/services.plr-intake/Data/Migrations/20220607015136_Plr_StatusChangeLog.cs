using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PlrIntake.Data.Migrations
{
    public partial class Plr_StatusChangeLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plr_StatusChageLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlrRecordId = table.Column<int>(type: "integer", nullable: false),
                    OldStatusCode = table.Column<string>(type: "text", nullable: true),
                    OldStatusReasonCode = table.Column<string>(type: "text", nullable: true),
                    NewStatusCode = table.Column<string>(type: "text", nullable: true),
                    NewStatusReasonCode = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plr_StatusChageLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plr_StatusChageLog_Plr_PlrRecord_PlrRecordId",
                        column: x => x.PlrRecordId,
                        principalTable: "Plr_PlrRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plr_StatusChageLog_PlrRecordId",
                table: "Plr_StatusChageLog",
                column: "PlrRecordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Plr_StatusChageLog");
        }
    }
}
