using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pidp.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeletePRPauthorizedlicencetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrpAuthorizedLicence");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrpAuthorizedLicence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Claimed = table.Column<bool>(type: "boolean", nullable: false),
                    LicenceNumber = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrpAuthorizedLicence", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrpAuthorizedLicence_LicenceNumber",
                table: "PrpAuthorizedLicence",
                column: "LicenceNumber",
                unique: true);
        }
    }
}
