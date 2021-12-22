using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pidp.Migrations
{
    public partial class RemoveAddressType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressType",
                table: "Address");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddressType",
                table: "Address",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
