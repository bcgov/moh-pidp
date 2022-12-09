﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pidp.Data.Migrations
{
    public partial class MultipleCredentials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "Credential",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IdpId = table.Column<string>(type: "text", nullable: false),
                    PartyId = table.Column<int>(type: "integer", nullable: false),
                    CredentialType = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Created = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credential", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Credential_Party_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Party",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Credential_PartyId",
                table: "Credential",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Credential_UserId",
                table: "Credential",
                column: "UserId",
                unique: true);

            migrationBuilder.Sql(@"
                INSERT INTO ""Credential"" (""UserId"", ""PartyId"", ""IdpId"")
                SELECT ""UserId"", ""Id"" AS ""PartyId"", ""Hpdid"" AS ""IdpId""
                FROM ""Party"";
            ");

            migrationBuilder.Sql(@"
                UPDATE ""Credential""
                SET ""CredentialType"" = 1
                WHERE ""IdpId"" IS NOT NULL;
            ");

            migrationBuilder.DropIndex(
                name: "IX_Party_Hpdid",
                table: "Party");

            migrationBuilder.DropIndex(
                name: "IX_Party_UserId",
                table: "Party");

            migrationBuilder.DropColumn(
                name: "Hpdid",
                table: "Party");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Party");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Credential");

            migrationBuilder.AddColumn<string>(
                name: "Hpdid",
                table: "Party",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Party",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Party_Hpdid",
                table: "Party",
                column: "Hpdid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Party_UserId",
                table: "Party",
                column: "UserId",
                unique: true);
        }
    }
}
