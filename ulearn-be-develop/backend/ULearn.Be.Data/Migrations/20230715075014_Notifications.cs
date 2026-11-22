using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class Notifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_c_currency_curency_id",
                table: "app_reservation");

            migrationBuilder.AddColumn<DateTime>(
                name: "approved",
                table: "app_transaction",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "approver_id",
                table: "app_transaction",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "decision_text",
                table: "app_reservation_issue",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "reservation_issue_state_id",
                table: "app_reservation_issue",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "resolved",
                table: "app_reservation_issue",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "resolver_id",
                table: "app_reservation_issue",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "notified",
                table: "app_reservation",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "source_reservation_id",
                table: "app_reservation",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "notified",
                table: "app_message",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "c_notification_channel",
                columns: table => new
                {
                    notification_channel_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_notification_channel", x => x.notification_channel_id);
                });

            migrationBuilder.CreateTable(
                name: "c_notification_state",
                columns: table => new
                {
                    NotificationStateId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_notification_state", x => x.NotificationStateId);
                });

            migrationBuilder.CreateTable(
                name: "c_notification_type",
                columns: table => new
                {
                    notification_type_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title_key = table.Column<string>(type: "text", nullable: true),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_notification_type", x => x.notification_type_id);
                });

            migrationBuilder.CreateTable(
                name: "c_reservation_issue_state",
                columns: table => new
                {
                    reservation_issue_state_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    icon_name = table.Column<string>(type: "text", nullable: true),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_reservation_issue_state", x => x.reservation_issue_state_id);
                });

            migrationBuilder.CreateTable(
                name: "app_notification",
                columns: table => new
                {
                    notification_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    recipient_id = table.Column<int>(type: "integer", nullable: false),
                    sender_id = table.Column<int>(type: "integer", nullable: false),
                    notification_type_id = table.Column<int>(type: "integer", nullable: false),
                    notification_state_id = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    read_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    json_data = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_notification", x => x.notification_id);
                    table.ForeignKey(
                        name: "fk_app_notification_app_user_recipient_id",
                        column: x => x.recipient_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_app_notification_app_user_sender_id",
                        column: x => x.sender_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_app_notification_c_notification_state_notification_state_id",
                        column: x => x.notification_state_id,
                        principalTable: "c_notification_state",
                        principalColumn: "NotificationStateId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_app_notification_c_notification_type_notification_type_id",
                        column: x => x.notification_type_id,
                        principalTable: "c_notification_type",
                        principalColumn: "notification_type_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 1,
                column: "hash",
                value: new Guid("0bdaa541-a4db-4c35-b285-4d4480924bd1"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 2,
                column: "hash",
                value: new Guid("01737702-a030-4d6d-9122-52cacc467d0a"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 3,
                column: "hash",
                value: new Guid("a191b6c6-330e-4362-bc5a-4e3a6fa694fe"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 4,
                column: "hash",
                value: new Guid("4d03e920-24cc-4c30-aa56-7db47c005d44"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 5,
                column: "hash",
                value: new Guid("f9ad6a2a-6316-48a4-8f09-2c51f20a0dde"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 6,
                column: "hash",
                value: new Guid("82cdcbb1-4601-4ef6-8b01-4c683ec3ae84"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 7,
                column: "hash",
                value: new Guid("a1342580-ea94-4d19-9114-9a89426354f7"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 8,
                column: "hash",
                value: new Guid("a8123a5c-cec6-465a-96d3-5e45803f9931"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 9,
                column: "hash",
                value: new Guid("9368f543-7a69-409f-abc5-05abe8cc6496"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 10,
                column: "hash",
                value: new Guid("2f91affd-da32-445f-b41d-f47e2441f9d4"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 11,
                column: "hash",
                value: new Guid("2e16a8de-1684-401e-9d83-31245a780a3f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 12,
                column: "hash",
                value: new Guid("cf7a04e2-71d8-40f1-84cd-212805d4f89e"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 13,
                column: "hash",
                value: new Guid("f3450783-c96c-4448-9e82-6bb6cd4c8302"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 14,
                column: "hash",
                value: new Guid("6ae65f3e-11bf-474e-bfd2-33e5c8d9eb5d"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 15,
                column: "hash",
                value: new Guid("16b520e2-b8bd-4eca-ab7d-5fcd228870bc"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 16,
                column: "hash",
                value: new Guid("7e55a33c-c8f2-4e57-b442-32dbaf01b710"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 17,
                column: "hash",
                value: new Guid("8b9ca74f-962c-439e-a5df-c3118efc2c2a"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 18,
                column: "hash",
                value: new Guid("3087bd30-c549-44bd-be91-838ed88cf82c"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 19,
                column: "hash",
                value: new Guid("7fac385d-dd14-42bc-acac-4463a5f17aa8"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 20,
                column: "hash",
                value: new Guid("48e2bbba-3d74-437d-86d7-53be1fbb7cb3"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 21,
                column: "hash",
                value: new Guid("3fbc2b59-ec78-4284-af49-57df306e4d5b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 22,
                column: "hash",
                value: new Guid("d0544c1e-ca36-442c-88d9-ca62b8280492"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 23,
                column: "hash",
                value: new Guid("4b6e3a17-d301-48a1-969c-f7b833007654"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 24,
                column: "hash",
                value: new Guid("3c294e07-bbe3-4389-862b-603ac8a74245"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 25,
                column: "hash",
                value: new Guid("90590199-d47d-4777-9ef2-97d74b62c4b8"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 26,
                column: "hash",
                value: new Guid("f8a27f33-054f-43ab-8966-26ff0648cb00"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 27,
                column: "hash",
                value: new Guid("c74a8cbf-c527-4a7f-bb4c-1c34c1af25a4"));

            migrationBuilder.InsertData(
                table: "c_notification_channel",
                columns: new[] { "notification_channel_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "notification_channel.application.description", "application", "notification_channel.application.name" },
                    { 2, "notification_channel.email.description", "email", "notification_channel.email.name" },
                    { 3, "notification_channel.slack.description", "slack", "notification_channel.slack.name" }
                });

            migrationBuilder.InsertData(
                table: "c_notification_state",
                columns: new[] { "NotificationStateId", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "notification_state.created.description", "created", "notification_state.created.name" },
                    { 2, "notification_state.read.description", "read", "notification_state.read.name" }
                });

            migrationBuilder.InsertData(
                table: "c_notification_type",
                columns: new[] { "notification_type_id", "description_key", "enum_key", "name_key", "title_key" },
                values: new object[,]
                {
                    { 1, "notification_type.email_confirmation.description", "email_confirmation", "notification_type.email_confirmation.name", "notification_type.email_confirmation.title" },
                    { 2, "notification_type.profile_verified.description", "profile_verified", "notification_type.profile_verified.name", "notification_type.profile_verified.title" },
                    { 3, "notification_type.password_reset.description", "password_reset", "notification_type.password_reset.name", "notification_type.password_reset.title" },
                    { 4, "notification_type.welcome.description", "welcome", "notification_type.welcome.name", "notification_type.welcome.title" },
                    { 5, "notification_type.package_cancellation.description", "package_cancellation", "notification_type.package_cancellation.name", "notification_type.package_cancellation.title" },
                    { 6, "notification_type.lesson_reminder.description", "lesson_reminder", "notification_type.lesson_reminder.name", "notification_type.lesson_reminder.title" },
                    { 7, "notification_type.student_rescheduled_lesson.description", "student_rescheduled_lesson", "notification_type.student_rescheduled_lesson.name", "notification_type.student_rescheduled_lesson.title" },
                    { 8, "notification_type.new_reservation.description", "new_reservation", "notification_type.new_reservation.name", "notification_type.new_reservation.title" },
                    { 9, "notification_type.new_package_reservation.description", "new_package_reservation", "notification_type.new_package_reservation.name", "notification_type.new_package_reservation.title" },
                    { 10, "notification_type.new_review.description", "new_review", "notification_type.new_review.name", "notification_type.new_review.title" },
                    { 11, "notification_type.new_message.description", "new_message", "notification_type.new_message.name", "notification_type.new_message.title" },
                    { 12, "notification_type.refund_requested_for_your_lesson.description", "refund_requested_for_your_lesson", "notification_type.refund_requested_for_your_lesson.name", "notification_type.refund_requested_for_your_lesson.title" },
                    { 13, "notification_type.request_refund_approved.description", "request_refund_approved", "notification_type.request_refund_approved.name", "notification_type.request_refund_approved.title" },
                    { 14, "notification_type.request_refund_declined.description", "request_refund_declined", "notification_type.request_refund_declined.name", "notification_type.request_refund_declined.title" },
                    { 15, "notification_type.money_sent.description", "money_sent", "notification_type.money_sent.name", "notification_type.money_sent.title" },
                    { 16, "notification_type.tutor_rescheduled_lesson.description", "tutor_rescheduled_lesson", "notification_type.tutor_rescheduled_lesson.name", "notification_type.tutor_rescheduled_lesson.title" },
                    { 17, "notification_type.tutor_profile_approval_requested.description", "tutor_profile_approval_requested", "notification_type.tutor_profile_approval_requested.name", "notification_type.tutor_profile_approval_requested.title" },
                    { 18, "notification_type.refund_request_for_admin.description", "refund_request_for_admin", "notification_type.refund_request_for_admin.name", "notification_type.refund_request_for_admin.title" },
                    { 19, "notification_type.money_withdrawal_requested.description", "money_withdrawal_requested", "notification_type.money_withdrawal_requested.name", "notification_type.money_withdrawal_requested.title" },
                    { 20, "notification_type.student_bought_package.description", "student_bought_package", "notification_type.student_bought_package.name", "notification_type.student_bought_package.title" },
                    { 21, "notification_type.student_deposited_money.description", "student_deposited_money", "notification_type.student_deposited_money.name", "notification_type.student_deposited_money.title" },
                    { 22, "notification_type.reservation_cancellation.description", "reservation_cancellation", "notification_type.reservation_cancellation.name", "notification_type.reservation_cancellation.title" }
                });

            migrationBuilder.InsertData(
                table: "c_reservation_issue_state",
                columns: new[] { "reservation_issue_state_id", "description_key", "enum_key", "icon_name", "name_key" },
                values: new object[,]
                {
                    { 1, "reservation_issue_state.waiting.description", "waiting", "waiting_icon", "reservation_issue_state.waiting.name" },
                    { 2, "reservation_issue_state.approved.description", "approved", "approved_icon", "reservation_issue_state.approved.name" },
                    { 3, "reservation_issue_state.declined.description", "declined", "declined_icon", "reservation_issue_state.declined.name" }
                });

            migrationBuilder.InsertData(
                table: "c_reservation_state",
                columns: new[] { "reservation_state_id", "description_key", "enum_key", "name_key" },
                values: new object[] { 9, "reservation_state.rescheduled.description", "rescheduled", "reservation_state.rescheduled.name" });

            migrationBuilder.InsertData(
                table: "env_configuration",
                columns: new[] { "configuration_id", "admin_configurable", "backend_only", "configuration_key", "configuration_value", "data_type_id", "value_origin" },
                values: new object[] { 49, true, true, "be.slack.settings.WebhookEndpoint", "", 1, "init_scripts" });

            migrationBuilder.CreateIndex(
                name: "ix_app_transaction_approver_id",
                table: "app_transaction",
                column: "approver_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_reservation_reservation_issue_state_id",
                table: "app_reservation_issue",
                column: "reservation_issue_state_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_reservation_updater_id",
                table: "app_reservation_issue",
                column: "resolver_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_reservation_source_reservation_id",
                table: "app_reservation",
                column: "reservation_id");

            migrationBuilder.CreateIndex(
                name: "IX_app_reservation_source_reservation_id",
                table: "app_reservation",
                column: "source_reservation_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_notification_notification_state_id",
                table: "app_notification",
                column: "notification_state_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_notification_notification_type_id",
                table: "app_notification",
                column: "notification_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_notification_recipient_id",
                table: "app_notification",
                column: "recipient_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_notification_sender_id",
                table: "app_notification",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "ix_c_notification_channel_enum_key",
                table: "c_notification_channel",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_notification_state_enum_key",
                table: "c_notification_state",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_notification_type_enum_key",
                table: "c_notification_type",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_reservation_issue_state_enum_key",
                table: "c_reservation_issue_state",
                column: "enum_key",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_app_reservation_source_reservation_id",
                table: "app_reservation",
                column: "source_reservation_id",
                principalTable: "app_reservation",
                principalColumn: "reservation_id");

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_c_currency_currency_id",
                table: "app_reservation",
                column: "currency_id",
                principalTable: "c_currency",
                principalColumn: "currency_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_issue_app_user_resolver_id",
                table: "app_reservation_issue",
                column: "resolver_id",
                principalTable: "app_user",
                principalColumn: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_issue_c_reservation_issue_state_reservation_issue_state_id",
                table: "app_reservation_issue",
                column: "reservation_issue_state_id",
                principalTable: "c_reservation_issue_state",
                principalColumn: "reservation_issue_state_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_app_user_approver_id",
                table: "app_transaction",
                column: "approver_id",
                principalTable: "app_user",
                principalColumn: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_app_reservation_source_reservation_id",
                table: "app_reservation");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_c_currency_currency_id",
                table: "app_reservation");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_issue_app_user_resolver_id",
                table: "app_reservation_issue");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_issue_c_reservation_issue_state_reservation_issue_state_id",
                table: "app_reservation_issue");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_app_user_approver_id",
                table: "app_transaction");

            migrationBuilder.DropTable(
                name: "app_notification");

            migrationBuilder.DropTable(
                name: "c_notification_channel");

            migrationBuilder.DropTable(
                name: "c_reservation_issue_state");

            migrationBuilder.DropTable(
                name: "c_notification_state");

            migrationBuilder.DropTable(
                name: "c_notification_type");

            migrationBuilder.DropIndex(
                name: "ix_app_transaction_approver_id",
                table: "app_transaction");

            migrationBuilder.DropIndex(
                name: "ix_app_reservation_reservation_issue_state_id",
                table: "app_reservation_issue");

            migrationBuilder.DropIndex(
                name: "ix_app_reservation_updater_id",
                table: "app_reservation_issue");

            migrationBuilder.DropIndex(
                name: "ix_app_reservation_source_reservation_id",
                table: "app_reservation");

            migrationBuilder.DropIndex(
                name: "IX_app_reservation_source_reservation_id",
                table: "app_reservation");

            migrationBuilder.DeleteData(
                table: "c_reservation_state",
                keyColumn: "reservation_state_id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "env_configuration",
                keyColumn: "configuration_id",
                keyValue: 49);

            migrationBuilder.DropColumn(
                name: "approved",
                table: "app_transaction");

            migrationBuilder.DropColumn(
                name: "approver_id",
                table: "app_transaction");

            migrationBuilder.DropColumn(
                name: "decision_text",
                table: "app_reservation_issue");

            migrationBuilder.DropColumn(
                name: "reservation_issue_state_id",
                table: "app_reservation_issue");

            migrationBuilder.DropColumn(
                name: "resolved",
                table: "app_reservation_issue");

            migrationBuilder.DropColumn(
                name: "resolver_id",
                table: "app_reservation_issue");

            migrationBuilder.DropColumn(
                name: "notified",
                table: "app_reservation");

            migrationBuilder.DropColumn(
                name: "source_reservation_id",
                table: "app_reservation");

            migrationBuilder.DropColumn(
                name: "notified",
                table: "app_message");

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 1,
                column: "hash",
                value: new Guid("97a2d260-89f0-47d9-8bbf-ee4e07d2aab2"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 2,
                column: "hash",
                value: new Guid("cf2870f6-302a-4f1a-9411-15dfc1946e90"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 3,
                column: "hash",
                value: new Guid("d011ed58-0ac3-42e0-940c-2d3dfb4a7d7b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 4,
                column: "hash",
                value: new Guid("1a8fc8a3-8a3d-460b-baa4-5769d48bcdb1"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 5,
                column: "hash",
                value: new Guid("9f821c14-86ae-483b-8f05-b6daae18485b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 6,
                column: "hash",
                value: new Guid("c207b78a-5e90-40f4-a324-aae09acb3817"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 7,
                column: "hash",
                value: new Guid("969b6d63-b8c2-4f01-be3f-356a4b261d40"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 8,
                column: "hash",
                value: new Guid("f7d69065-5cd8-4818-8ba1-da458968149c"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 9,
                column: "hash",
                value: new Guid("8caf6a03-09df-4168-a910-eafc772e64f4"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 10,
                column: "hash",
                value: new Guid("77cb0bf3-6b28-48c2-8163-d1b6f1532441"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 11,
                column: "hash",
                value: new Guid("ad35bb87-efa4-4097-a8eb-5efe12c5c566"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 12,
                column: "hash",
                value: new Guid("b5a383ae-9b1b-41ca-a435-0ee433dc68a6"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 13,
                column: "hash",
                value: new Guid("906ad122-3751-465b-9eb6-118632173b59"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 14,
                column: "hash",
                value: new Guid("84976e75-acdc-42fd-9440-3fd7c076a9ad"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 15,
                column: "hash",
                value: new Guid("7097245d-6e8d-4fa3-988a-a88d1391888f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 16,
                column: "hash",
                value: new Guid("22d94772-4911-4372-aac4-217efacb55be"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 17,
                column: "hash",
                value: new Guid("d05ea349-d523-4866-b4b0-e2279822846f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 18,
                column: "hash",
                value: new Guid("3a16a255-f2b9-4065-8b4f-e6f3115d7e44"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 19,
                column: "hash",
                value: new Guid("fd35118d-9856-4572-8213-91d3f6a7ebb5"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 20,
                column: "hash",
                value: new Guid("b39124fd-924d-4c3b-bd37-5ace7076f88e"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 21,
                column: "hash",
                value: new Guid("843c5471-016c-4e7e-9fe9-23ab8d67fda7"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 22,
                column: "hash",
                value: new Guid("01888115-a647-4a87-8da7-532e2a2717e7"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 23,
                column: "hash",
                value: new Guid("95e6aeed-9c9a-442d-a557-a96159d75632"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 24,
                column: "hash",
                value: new Guid("a9e208c4-6c1b-4142-ac9d-71a8f30d3020"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 25,
                column: "hash",
                value: new Guid("c5b5b27d-da77-43f9-8be7-77289e2415c8"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 26,
                column: "hash",
                value: new Guid("d2ac12ec-1cee-4b8b-829b-ef4e590e8fc9"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 27,
                column: "hash",
                value: new Guid("f45889c3-9ad9-40f0-b532-156a32878c49"));

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_c_currency_curency_id",
                table: "app_reservation",
                column: "currency_id",
                principalTable: "c_currency",
                principalColumn: "currency_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
