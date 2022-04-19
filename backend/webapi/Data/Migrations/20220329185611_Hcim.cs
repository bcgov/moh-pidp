using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class Hcim : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessRequests_Party_PartyId",
                table: "AccessRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccessRequests",
                table: "AccessRequests");

            migrationBuilder.RenameTable(
                name: "AccessRequests",
                newName: "AccessRequest");

            migrationBuilder.RenameIndex(
                name: "IX_AccessRequests_PartyId",
                table: "AccessRequest",
                newName: "IX_AccessRequest_PartyId");

            migrationBuilder.AlterColumn<string>(
                name: "Hpdid",
                table: "Party",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<LocalDate>(
                name: "Birthdate",
                table: "Party",
                type: "date",
                nullable: true,
                oldClrType: typeof(LocalDate),
                oldType: "date");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccessRequest",
                table: "AccessRequest",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "HcimReEnrolmentAccessRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    LdapUsername = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HcimReEnrolmentAccessRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HcimReEnrolmentAccessRequest_AccessRequest_Id",
                        column: x => x.Id,
                        principalTable: "AccessRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AccessRequest_Party_PartyId",
                table: "AccessRequest",
                column: "PartyId",
                principalTable: "Party",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessRequest_Party_PartyId",
                table: "AccessRequest");

            migrationBuilder.DropTable(
                name: "HcimReEnrolmentAccessRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccessRequest",
                table: "AccessRequest");

            migrationBuilder.RenameTable(
                name: "AccessRequest",
                newName: "AccessRequests");

            migrationBuilder.RenameIndex(
                name: "IX_AccessRequest_PartyId",
                table: "AccessRequests",
                newName: "IX_AccessRequests_PartyId");

            migrationBuilder.AlterColumn<string>(
                name: "Hpdid",
                table: "Party",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<LocalDate>(
                name: "Birthdate",
                table: "Party",
                type: "date",
                nullable: false,
                defaultValue: new NodaTime.LocalDate(1, 1, 1),
                oldClrType: typeof(LocalDate),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccessRequests",
                table: "AccessRequests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessRequests_Party_PartyId",
                table: "AccessRequests",
                column: "PartyId",
                principalTable: "Party",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
