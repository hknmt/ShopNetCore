using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessObject.Migrations
{
    public partial class _000520082017 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PageSize",
                table: "ConfigShop");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ConfigShop",
                newName: "ConfigId");

            migrationBuilder.AddColumn<string>(
                name: "ConfigName",
                table: "ConfigShop",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConfigValue",
                table: "ConfigShop",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfigName",
                table: "ConfigShop");

            migrationBuilder.DropColumn(
                name: "ConfigValue",
                table: "ConfigShop");

            migrationBuilder.RenameColumn(
                name: "ConfigId",
                table: "ConfigShop",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "PageSize",
                table: "ConfigShop",
                nullable: false,
                defaultValue: 0);
        }
    }
}
