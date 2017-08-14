using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace testmigration.Data.Migrations
{
    public partial class Response : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Response",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Identifier",
                table: "AspNetUsers",
                nullable: true);

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
