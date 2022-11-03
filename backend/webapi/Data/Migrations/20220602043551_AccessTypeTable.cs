using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class AccessTypeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccessType",
                table: "AccessRequest",
                newName: "AccessTypeCode");

            migrationBuilder.CreateTable(
                name: "AccessTypeLookup",
                columns: table => new
                {
                    Code = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessTypeLookup", x => x.Code);
                });

            migrationBuilder.InsertData(
                table: "AccessTypeLookup",
                columns: new[] { "Code", "Name" },
                values: new object[,]
                {
                    { 1, "Special Authority eForms" },
                    { 2, "HCIMWeb Account Transfer" },
                    { 3, "HCIMWeb Enrolment" },
                    { 4, "Driver Medical Fitness" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessTypeLookup");

            migrationBuilder.RenameColumn(
                name: "AccessTypeCode",
                table: "AccessRequest",
                newName: "AccessType");
        }
    }
}
