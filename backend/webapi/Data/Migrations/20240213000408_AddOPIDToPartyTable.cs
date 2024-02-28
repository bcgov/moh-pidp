using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class AddOPIDToPartyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OpId",
                table: "Party",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Party_OpId",
                table: "Party",
                column: "OpId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Party_OpId",
                table: "Party");

            migrationBuilder.DropColumn(
                name: "OpId",
                table: "Party");
        }
    }
}
