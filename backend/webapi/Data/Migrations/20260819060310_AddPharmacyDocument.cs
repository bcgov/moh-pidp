using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace Pidp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPharmacyDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(name: "DocumentId", table: "Pharmacies", type: "uuid", nullable: true);
            migrationBuilder.CreateTable(name: "Documents", columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Data = table.Column<byte[]>(type: "bytea", nullable: false),
                ContentType = table.Column<string>(type: "text", nullable: false),
                FileName = table.Column<string>(type: "text", nullable: false),
                Created = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                Modified = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
            }, constraints: table => { table.PrimaryKey("PK_Documents", x => x.Id); });
            migrationBuilder.CreateIndex( name: "IX_Pharmacies_DocumentId", table: "Pharmacies", column: "DocumentId");
            migrationBuilder.AddForeignKey( name: "FK_Pharmacies_Documents_DocumentId", table: "Pharmacies", column: "DocumentId", principalTable: "Documents", principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Pharmacies_Documents_DocumentId", table: "Pharmacies");
            migrationBuilder.DropTable( name: "Documents");
            migrationBuilder.DropIndex( name: "IX_Pharmacies_DocumentId", table: "Pharmacies");
            migrationBuilder.DropColumn( name: "DocumentId", table: "Pharmacies");
        }
    }
}
