using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class OrderNumberFromTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "ix_app_timeslot_owner_id",
                table: "app_timeslot",
                newName: "IX_app_timeslot_owner_id");

            migrationBuilder.RenameIndex(
                name: "ix_app_recurrent_timeslot_owner_id",
                table: "app_recurrent_timeslot",
                newName: "IX_app_recurrent_timeslot_owner_id");

            migrationBuilder.AddColumn<int>(
                name: "order_number",
                table: "app_user",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "from_time",
                table: "app_timeslot",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "c_user_state",
                keyColumn: "user_state_id",
                keyValue: 1,
                columns: new[] { "company_admin_name_key", "description_key", "enum_key", "name_key" },
                values: new object[] { "user_state.email_verification.company_admin_name_key", "user_state.email_verification.description", "email_verification", "user_state.email_verification.name" });

            migrationBuilder.UpdateData(
                table: "c_user_state",
                keyColumn: "user_state_id",
                keyValue: 2,
                columns: new[] { "company_admin_name_key", "description_key", "enum_key", "name_key" },
                values: new object[] { "user_state.password_setup.company_admin_name_key", "user_state.password_setup.description", "password_setup", "user_state.password_setup.name" });

            migrationBuilder.UpdateData(
                table: "c_user_state",
                keyColumn: "user_state_id",
                keyValue: 3,
                columns: new[] { "company_admin_name_key", "description_key", "enum_key", "name_key" },
                values: new object[] { "user_state.registration.company_admin_name_key", "user_state.registration.description", "registration", "user_state.registration.name" });

            migrationBuilder.UpdateData(
                table: "c_user_state",
                keyColumn: "user_state_id",
                keyValue: 4,
                columns: new[] { "company_admin_name_key", "description_key", "enum_key", "name_key" },
                values: new object[] { "user_state.profile_validation.company_admin_name_key", "user_state.profile_validation.description", "profile_validation", "user_state.profile_validation.name" });

            migrationBuilder.UpdateData(
                table: "c_user_state",
                keyColumn: "user_state_id",
                keyValue: 8,
                columns: new[] { "company_admin_name_key", "description_key", "enum_key", "name_key" },
                values: new object[] { "user_state.role_setup.company_admin_name_key", "user_state.role_setup.description", "role_setup", "user_state.role_setup.name" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "order_number",
                table: "app_user");

            migrationBuilder.DropColumn(
                name: "from_time",
                table: "app_timeslot");

            migrationBuilder.RenameIndex(
                name: "IX_app_timeslot_owner_id",
                table: "app_timeslot",
                newName: "ix_app_timeslot_owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_app_recurrent_timeslot_owner_id",
                table: "app_recurrent_timeslot",
                newName: "ix_app_recurrent_timeslot_owner_id");

            migrationBuilder.UpdateData(
                table: "c_user_state",
                keyColumn: "user_state_id",
                keyValue: 1,
                columns: new[] { "company_admin_name_key", "description_key", "enum_key", "name_key" },
                values: new object[] { "user_state.created.company_admin_name_key", "user_state.created.description", "created", "user_state.created.name" });

            migrationBuilder.UpdateData(
                table: "c_user_state",
                keyColumn: "user_state_id",
                keyValue: 2,
                columns: new[] { "company_admin_name_key", "description_key", "enum_key", "name_key" },
                values: new object[] { "user_state.email_verified.company_admin_name_key", "user_state.email_verified.description", "email_verified", "user_state.email_verified.name" });

            migrationBuilder.UpdateData(
                table: "c_user_state",
                keyColumn: "user_state_id",
                keyValue: 3,
                columns: new[] { "company_admin_name_key", "description_key", "enum_key", "name_key" },
                values: new object[] { "user_state.password_set.company_admin_name_key", "user_state.password_set.description", "password_set", "user_state.password_set.name" });

            migrationBuilder.UpdateData(
                table: "c_user_state",
                keyColumn: "user_state_id",
                keyValue: 4,
                columns: new[] { "company_admin_name_key", "description_key", "enum_key", "name_key" },
                values: new object[] { "user_state.profile_in_validation.company_admin_name_key", "user_state.profile_in_validation.description", "profile_in_validation", "user_state.profile_in_validation.name" });

            migrationBuilder.UpdateData(
                table: "c_user_state",
                keyColumn: "user_state_id",
                keyValue: 8,
                columns: new[] { "company_admin_name_key", "description_key", "enum_key", "name_key" },
                values: new object[] { "user_state.created_via_google_one_tap.company_admin_name_key", "user_state.created_via_google_one_tap.description", "created_via_google_one_tap", "user_state.created_via_google_one_tap.name" });
        }
    }
}
