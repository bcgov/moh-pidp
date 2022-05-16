using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class HcimRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "HcimReEnrolmentAccessRequest",
                newName: "HcimAccountTransfer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
               name: "HcimAccountTransfer",
               newName: "HcimReEnrolmentAccessRequest");
        }
    }
}
