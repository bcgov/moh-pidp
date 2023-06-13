using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class UpdateCredentialIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Credential_UserId",
                table: "Credential");

            migrationBuilder.DropCheckConstraint(
                name: "CHK_Credential_AtLeastOneIdentifier",
                table: "Credential");

            migrationBuilder.CreateIndex(
                name: "IX_Credential_UserId",
                table: "Credential",
                column: "UserId",
                unique: true,
                filter: "\"UserId\" != '00000000-0000-0000-0000-000000000000'");

            migrationBuilder.AddCheckConstraint(
                name: "CHK_Credential_AtLeastOneIdentifier",
                table: "Credential",
                sql: "((\"UserId\" != '00000000-0000-0000-0000-000000000000') or (\"IdpId\" is not null))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Credential_UserId",
                table: "Credential");

            migrationBuilder.DropCheckConstraint(
                name: "CHK_Credential_AtLeastOneIdentifier",
                table: "Credential");

            migrationBuilder.CreateIndex(
                name: "IX_Credential_UserId",
                table: "Credential",
                column: "UserId",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CHK_Credential_AtLeastOneIdentifier",
                table: "Credential",
                sql: "((\"UserId\" is not null) or (\"IdpId\" is not null))");
        }
    }
}
