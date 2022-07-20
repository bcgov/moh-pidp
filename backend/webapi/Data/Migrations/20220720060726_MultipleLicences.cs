using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class MultipleLicences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartyCertification_CollegeLookup_CollegeCode",
                table: "PartyCertification");

            migrationBuilder.DropForeignKey(
                name: "FK_PartyCertification_Party_PartyId",
                table: "PartyCertification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartyCertification",
                table: "PartyCertification");

            migrationBuilder.RenameTable(
                name: "PartyCertification",
                newName: "LicenceDeclaration");

            migrationBuilder.RenameIndex(
                name: "IX_PartyCertification_PartyId",
                table: "LicenceDeclaration",
                newName: "IX_LicenceDeclaration_PartyId");

            migrationBuilder.RenameIndex(
                name: "IX_PartyCertification_CollegeCode",
                table: "LicenceDeclaration",
                newName: "IX_LicenceDeclaration_CollegeCode");

            migrationBuilder.AddColumn<string>(
                name: "Cpn",
                table: "Party",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LicenceNumber",
                table: "LicenceDeclaration",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "CollegeCode",
                table: "LicenceDeclaration",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LicenceDeclaration",
                table: "LicenceDeclaration",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LicenceDeclaration_CollegeLookup_CollegeCode",
                table: "LicenceDeclaration",
                column: "CollegeCode",
                principalTable: "CollegeLookup",
                principalColumn: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_LicenceDeclaration_Party_PartyId",
                table: "LicenceDeclaration",
                column: "PartyId",
                principalTable: "Party",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LicenceDeclaration_CollegeLookup_CollegeCode",
                table: "LicenceDeclaration");

            migrationBuilder.DropForeignKey(
                name: "FK_LicenceDeclaration_Party_PartyId",
                table: "LicenceDeclaration");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LicenceDeclaration",
                table: "LicenceDeclaration");

            migrationBuilder.DropColumn(
                name: "Cpn",
                table: "Party");

            migrationBuilder.RenameTable(
                name: "LicenceDeclaration",
                newName: "PartyCertification");

            migrationBuilder.RenameIndex(
                name: "IX_LicenceDeclaration_PartyId",
                table: "PartyCertification",
                newName: "IX_PartyCertification_PartyId");

            migrationBuilder.RenameIndex(
                name: "IX_LicenceDeclaration_CollegeCode",
                table: "PartyCertification",
                newName: "IX_PartyCertification_CollegeCode");

            migrationBuilder.AlterColumn<string>(
                name: "LicenceNumber",
                table: "PartyCertification",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CollegeCode",
                table: "PartyCertification",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartyCertification",
                table: "PartyCertification",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartyCertification_CollegeLookup_CollegeCode",
                table: "PartyCertification",
                column: "CollegeCode",
                principalTable: "CollegeLookup",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartyCertification_Party_PartyId",
                table: "PartyCertification",
                column: "PartyId",
                principalTable: "Party",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
