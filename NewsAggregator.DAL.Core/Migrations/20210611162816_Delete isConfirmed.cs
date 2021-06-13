using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsAggregator.DAL.Core.Migrations
{
    public partial class DeleteisConfirmed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
