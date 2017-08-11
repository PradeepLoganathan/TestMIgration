using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace testmigration.Data.Migrations
{
    public partial class AddUserWithNewColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DOB",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            migrationBuilder.AddColumn<string>(
               name: "FullName",
               table: "AspNetUsers",
               nullable: true);
            migrationBuilder.AddColumn<string>(
               name: "PictureURL",
               table: "AspNetUsers",
               nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "DOB",
                table: "AspNetRoles");
            migrationBuilder.DropColumn(name: "FullName",
                table: "AspNetRoles");
            migrationBuilder.DropColumn(name: "PictureURL",
                table: "AspNetRoles");
            
        }
    }
}
