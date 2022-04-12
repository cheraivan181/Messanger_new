using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.DbModels.Migrations
{
    public partial class AddIVindialogrequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IV",
                table: "DialogSecrets",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IV",
                table: "DialogSecrets");
        }
    }
}
