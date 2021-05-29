using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsAggregator.DAL.Core.Migrations
{
    public partial class Role : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleNumber",
                table: "Roles");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);

            var userRoleId = Guid.NewGuid();
            var adminRoleId = Guid.NewGuid();

            migrationBuilder.Sql($"INSERT INTO [dbo].[Roles] ([Id],[Name]) VALUES ('{userRoleId}','User')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Roles] ([Id],[Name]) VALUES ('{adminRoleId}','Admin')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Roles");

            migrationBuilder.AddColumn<int>(
                name: "RoleNumber",
                table: "Roles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
