using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace Pidp.Migrations
{
    public partial class Auditable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Instant>(
                name: "Created",
                table: "Party",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: NodaTime.Instant.FromUnixTimeTicks(0L));

            migrationBuilder.AddColumn<Instant>(
                name: "Modified",
                table: "Party",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: NodaTime.Instant.FromUnixTimeTicks(0L));

            migrationBuilder.AddColumn<Instant>(
                name: "Created",
                table: "Address",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: NodaTime.Instant.FromUnixTimeTicks(0L));

            migrationBuilder.AddColumn<Instant>(
                name: "Modified",
                table: "Address",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: NodaTime.Instant.FromUnixTimeTicks(0L));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Party");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "Party");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "Address");
        }
    }
}
