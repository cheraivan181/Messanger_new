using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.DbModels.Migrations
{
    public partial class Addflagindialogrequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsProcessed",
                table: "DialogRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProcessed",
                table: "DialogRequests");
        }
    }
}
