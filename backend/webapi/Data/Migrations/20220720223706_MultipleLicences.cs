using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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
                newName: "PartyLicenceDeclaration");

            migrationBuilder.RenameIndex(
                name: "IX_PartyCertification_PartyId",
                table: "PartyLicenceDeclaration",
                newName: "IX_PartyLicenceDeclaration_PartyId");

            migrationBuilder.RenameIndex(
                name: "IX_PartyCertification_CollegeCode",
                table: "PartyLicenceDeclaration",
                newName: "IX_PartyLicenceDeclaration_CollegeCode");

            migrationBuilder.AddColumn<string>(
                name: "Cpn",
                table: "Party",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LicenceNumber",
                table: "PartyLicenceDeclaration",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "CollegeCode",
                table: "PartyLicenceDeclaration",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartyLicenceDeclaration",
                table: "PartyLicenceDeclaration",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PartyLicenceDeclaration_CollegeLookup_CollegeCode",
                table: "PartyLicenceDeclaration",
                column: "CollegeCode",
                principalTable: "CollegeLookup",
                principalColumn: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_PartyLicenceDeclaration_Party_PartyId",
                table: "PartyLicenceDeclaration",
                column: "PartyId",
                principalTable: "Party",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.Sql(@"
                UPDATE ""Party"" as p
                SET ""Cpn"" = plr.""Cpn""
                FROM ""Plr_PlrRecord"" as plr
                join ""PartyLicenceDeclaration"" l on l.""Ipc"" = plr.""Ipc""
                WHERE p.""Id"" = l.""PartyId"";
            ");

            migrationBuilder.DropColumn(
                name: "Ipc",
                table: "PartyLicenceDeclaration");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartyLicenceDeclaration_CollegeLookup_CollegeCode",
                table: "PartyLicenceDeclaration");

            migrationBuilder.DropForeignKey(
                name: "FK_PartyLicenceDeclaration_Party_PartyId",
                table: "PartyLicenceDeclaration");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartyLicenceDeclaration",
                table: "PartyLicenceDeclaration");

            migrationBuilder.DropColumn(
                name: "Cpn",
                table: "Party");

            migrationBuilder.RenameTable(
                name: "PartyLicenceDeclaration",
                newName: "PartyCertification");

            migrationBuilder.RenameIndex(
                name: "IX_PartyLicenceDeclaration_PartyId",
                table: "PartyCertification",
                newName: "IX_PartyCertification_PartyId");

            migrationBuilder.RenameIndex(
                name: "IX_PartyLicenceDeclaration_CollegeCode",
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
