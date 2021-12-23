using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace Pidp.Migrations
{
    public partial class RemoveDobAndMailingAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Party_PartyId",
                table: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Address_PartyId",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Party");

            migrationBuilder.DropColumn(
                name: "PartyId",
                table: "Address");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<LocalDate>(
                name: "DateOfBirth",
                table: "Party",
                type: "date",
                nullable: false,
                defaultValue: new NodaTime.LocalDate(1, 1, 1));

            migrationBuilder.AddColumn<int>(
                name: "PartyId",
                table: "Address",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Address_PartyId",
                table: "Address",
                column: "PartyId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Party_PartyId",
                table: "Address",
                column: "PartyId",
                principalTable: "Party",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
