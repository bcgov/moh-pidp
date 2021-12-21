using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pidp.Migrations
{
    public partial class AddedCollegeLookup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CollegeLookup",
                columns: table => new
                {
                    Code = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollegeLookup", x => x.Code);
                });

            migrationBuilder.InsertData(
                table: "CollegeLookup",
                columns: new[] { "Code", "Name" },
                values: new object[,]
                {
                    { 1, "College of Physicians and Surgeons of BC (CPSBC)" },
                    { 2, "College of Pharmacists of BC (CPBC)" },
                    { 3, "BC College of Nurses and Midwives (BCCNM)" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollegeLookup");
        }
    }
}
