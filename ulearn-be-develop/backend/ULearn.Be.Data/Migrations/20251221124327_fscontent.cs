using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class fscontent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "content",
                table: "r_user_file",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "content",
                table: "app_message_attachment",
                type: "bytea",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "content",
                table: "r_user_file");

            migrationBuilder.DropColumn(
                name: "content",
                table: "app_message_attachment");
        }
    }
}
