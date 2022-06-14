using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StreamInstruments.DataAccess.Migrations
{
    public partial class Command_Add_Description : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Commands",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Commands");
        }
    }
}
