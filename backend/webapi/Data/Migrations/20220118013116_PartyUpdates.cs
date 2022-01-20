using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class PartyUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<LocalDate>(
                name: "Birthdate",
                table: "Party",
                type: "date",
                nullable: false,
                defaultValue: new NodaTime.LocalDate(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "Hpdid",
                table: "Party",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Party_Hpdid",
                table: "Party",
                column: "Hpdid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Party_UserId",
                table: "Party",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Party_Hpdid",
                table: "Party");

            migrationBuilder.DropIndex(
                name: "IX_Party_UserId",
                table: "Party");

            migrationBuilder.DropColumn(
                name: "Birthdate",
                table: "Party");

            migrationBuilder.DropColumn(
                name: "Hpdid",
                table: "Party");
        }
    }
}
