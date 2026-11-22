using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class SlackNotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "c_notification_type",
                columns: new[] { "notification_type_id", "description_key", "enum_key", "name_key", "title_key" },
                values: new object[,]
                {
                    { 24, "notification_type.registration_completed.description", "registration_completed", "notification_type.registration_completed.name", "notification_type.registration_completed.title" },
                    { 25, "notification_type.account_created.description", "account_created", "notification_type.account_created.name", "notification_type.account_created.title" },
                    { 26, "notification_type.company_invited_colleagues.description", "company_invited_colleagues", "notification_type.company_invited_colleagues.name", "notification_type.company_invited_colleagues.title" }
                });

            migrationBuilder.InsertData(
                table: "env_configuration",
                columns: new[] { "configuration_id", "admin_configurable", "backend_only", "configuration_key", "configuration_value", "data_type_id", "value_origin" },
                values: new object[] { 55, true, false, "be.slack.settings.LowPriorityWebhookEndpoint", "", 1, "init_scripts" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "env_configuration",
                keyColumn: "configuration_id",
                keyValue: 55);
        }
    }
}
