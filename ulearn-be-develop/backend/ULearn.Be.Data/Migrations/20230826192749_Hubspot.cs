using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class Hubspot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "hubspot_contact_id",
                table: "app_user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_sync_to_hubspot",
                table: "app_user",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.InsertData(
                table: "env_configuration",
                columns: new[] { "configuration_id", "admin_configurable", "backend_only", "configuration_key", "configuration_value", "data_type_id", "value_origin" },
                values: new object[] { 54, true, false, "be.hubspot.settings.ApiKey", "", 9, "init_scripts" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "env_configuration",
                keyColumn: "configuration_id",
                keyValue: 54);

            migrationBuilder.DropColumn(
                name: "hubspot_contact_id",
                table: "app_user");

            migrationBuilder.DropColumn(
                name: "last_sync_to_hubspot",
                table: "app_user");
        }
    }
}
