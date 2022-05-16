using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.DbModels.Migrations
{
    public partial class AddSessionCypherKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserCypherKeys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CryptdedKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCypherKeys", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCypherKeys_SessionId",
                table: "UserCypherKeys",
                column: "SessionId")
                .Annotation("SqlServer:Include", new[] { "CryptdedKey" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCypherKeys");
        }
    }
}
