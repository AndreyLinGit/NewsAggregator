using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsAggregator.DAL.Core.Migrations
{
    public partial class userimages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Users",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageMimeType",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ImageMimeType",
                table: "Users");
        }
    }
}
