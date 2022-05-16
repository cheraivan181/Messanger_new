using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.DbModels.Migrations
{
    public partial class Rename_cypher_key : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserCypherKeys_SessionId",
                table: "UserCypherKeys");

            migrationBuilder.RenameColumn(
                name: "CryptdedKey",
                table: "UserCypherKeys",
                newName: "CryptedKey");

            migrationBuilder.CreateIndex(
                name: "IX_UserCypherKeys_SessionId",
                table: "UserCypherKeys",
                column: "SessionId")
                .Annotation("SqlServer:Include", new[] { "CryptedKey" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserCypherKeys_SessionId",
                table: "UserCypherKeys");

            migrationBuilder.RenameColumn(
                name: "CryptedKey",
                table: "UserCypherKeys",
                newName: "CryptdedKey");

            migrationBuilder.CreateIndex(
                name: "IX_UserCypherKeys_SessionId",
                table: "UserCypherKeys",
                column: "SessionId")
                .Annotation("SqlServer:Include", new[] { "CryptdedKey" });
        }
    }
}
