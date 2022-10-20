using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class NewMSTeams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MSTeamsEnrolmentId",
                table: "Address",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MSTeamsEnrolment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ClinicName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MSTeamsEnrolment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MSTeamsEnrolment_AccessRequest_Id",
                        column: x => x.Id,
                        principalTable: "AccessRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_MSTeamsEnrolmentId",
                table: "Address",
                column: "MSTeamsEnrolmentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Address_MSTeamsEnrolment_MSTeamsEnrolmentId",
                table: "Address",
                column: "MSTeamsEnrolmentId",
                principalTable: "MSTeamsEnrolment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_MSTeamsEnrolment_MSTeamsEnrolmentId",
                table: "Address");

            migrationBuilder.DropTable(
                name: "MSTeamsEnrolment");

            migrationBuilder.DropIndex(
                name: "IX_Address_MSTeamsEnrolmentId",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "MSTeamsEnrolmentId",
                table: "Address");
        }
    }
}
