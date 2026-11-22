using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class B2B : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "company_admin_name_key",
                table: "c_user_state",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "billing_email",
                table: "app_user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "company_name",
                table: "app_user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "inviter_id",
                table: "app_user",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_vat_payer",
                table: "app_user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "login",
                table: "app_user",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "planned_completion_time",
                table: "app_transaction",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "c_notification_type",
                columns: new[] { "notification_type_id", "description_key", "enum_key", "name_key", "title_key" },
                values: new object[] { 23, "notification_type.invitation.description", "invitation", "notification_type.invitation.name", "notification_type.invitation.title" });

            migrationBuilder.InsertData(
                table: "c_transaction_template_type",
                columns: new[] { "transaction_template_type_id", "description_key", "enum_key", "name_key" },
                values: new object[] { 7, "transaction_template_type.money_transfer_confirmation.description", "money_transfer_confirmation", "transaction_template_type.money_transfer_confirmation.name" });

            migrationBuilder.InsertData(
                table: "c_transaction_type",
                columns: new[] { "transaction_type_id", "description_key", "enum_key", "name_key" },
                values: new object[] { 11, "transaction_type.money_transfer.description", "money_transfer", "transaction_type.money_transfer.name" });

            migrationBuilder.InsertData(
                table: "c_user_role",
                columns: new[] { "user_role_id", "description_key", "enum_key", "name_key" },
                values: new object[] { 4, "user_role.company.description", "company", "user_role.company.name" });

            migrationBuilder.UpdateData(
                table: "c_user_state",
                keyColumn: "user_state_id",
                keyValue: 1,
                column: "company_admin_name_key",
                value: "user_state.created.company_admin_name_key");

            migrationBuilder.UpdateData(
                table: "c_user_state",
                keyColumn: "user_state_id",
                keyValue: 2,
                column: "company_admin_name_key",
                value: "user_state.email_verified.company_admin_name_key");

            migrationBuilder.UpdateData(
                table: "c_user_state",
                keyColumn: "user_state_id",
                keyValue: 3,
                column: "company_admin_name_key",
                value: "user_state.password_set.company_admin_name_key");

            migrationBuilder.UpdateData(
                table: "c_user_state",
                keyColumn: "user_state_id",
                keyValue: 4,
                column: "company_admin_name_key",
                value: "user_state.profile_in_validation.company_admin_name_key");

            migrationBuilder.UpdateData(
                table: "c_user_state",
                keyColumn: "user_state_id",
                keyValue: 5,
                column: "company_admin_name_key",
                value: "user_state.confirmed.company_admin_name_key");

            migrationBuilder.UpdateData(
                table: "c_user_state",
                keyColumn: "user_state_id",
                keyValue: 6,
                column: "company_admin_name_key",
                value: "user_state.deleted.company_admin_name_key");

            migrationBuilder.UpdateData(
                table: "env_configuration",
                keyColumn: "configuration_id",
                keyValue: 8,
                column: "configuration_value",
                value: "[\"https://www.googleapis.com/auth/userinfo.profile\",\"https://www.googleapis.com/auth/userinfo.email\"]");

            migrationBuilder.InsertData(
                table: "env_configuration",
                columns: new[] { "configuration_id", "admin_configurable", "backend_only", "configuration_key", "configuration_value", "data_type_id", "value_origin" },
                values: new object[,]
                {
                    { 50, true, false, "be.module.google_api.settings.CompanyRedirectUrl", "", 1, "init_scripts" },
                    { 51, true, false, "be.module.google_api.settings.CompanyScopes", "[\"https://www.googleapis.com/auth/userinfo.profile\",\"https://www.googleapis.com/auth/userinfo.email\"]", 2, "init_scripts" },
                    { 52, true, false, "be.module.facebook_api.settings.CompanyRedirectUrl", "", 1, "init_scripts" },
                    { 53, true, false, "be.module.seznam_api.settings.CompanyRedirectUrl", "", 1, "init_scripts" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_app_user_inviter_id",
                table: "app_user",
                column: "inviter_id");

            migrationBuilder.AddForeignKey(
                name: "fk_app_user_app_user_inviter_id",
                table: "app_user",
                column: "inviter_id",
                principalTable: "app_user",
                principalColumn: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_app_user_app_user_inviter_id",
                table: "app_user");

            migrationBuilder.DropIndex(
                name: "ix_app_user_inviter_id",
                table: "app_user");

            migrationBuilder.DeleteData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "c_transaction_template_type",
                keyColumn: "transaction_template_type_id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "c_user_role",
                keyColumn: "user_role_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "env_configuration",
                keyColumn: "configuration_id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "env_configuration",
                keyColumn: "configuration_id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "env_configuration",
                keyColumn: "configuration_id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "env_configuration",
                keyColumn: "configuration_id",
                keyValue: 53);

            migrationBuilder.DropColumn(
                name: "company_admin_name_key",
                table: "c_user_state");

            migrationBuilder.DropColumn(
                name: "billing_email",
                table: "app_user");

            migrationBuilder.DropColumn(
                name: "company_name",
                table: "app_user");

            migrationBuilder.DropColumn(
                name: "inviter_id",
                table: "app_user");

            migrationBuilder.DropColumn(
                name: "is_vat_payer",
                table: "app_user");

            migrationBuilder.DropColumn(
                name: "login",
                table: "app_user");

            migrationBuilder.DropColumn(
                name: "planned_completion_time",
                table: "app_transaction");

            migrationBuilder.UpdateData(
                table: "env_configuration",
                keyColumn: "configuration_id",
                keyValue: 8,
                column: "configuration_value",
                value: "[\"https://www.googleapis.com/auth/userinfo.profile\",\"https://www.googleapis.com/auth/userinfo.email\",\"https://www.googleapis.com/auth/calendar.events\"]");
        }
    }
}
