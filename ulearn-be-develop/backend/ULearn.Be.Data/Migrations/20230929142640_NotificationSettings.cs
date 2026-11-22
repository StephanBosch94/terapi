using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class NotificationSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_app_timeslot_owner_id",
                table: "app_timeslot",
                newName: "ix_app_timeslot_owner_id");

            migrationBuilder.RenameIndex(
                name: "IX_app_recurrent_timeslot_owner_id",
                table: "app_recurrent_timeslot",
                newName: "ix_app_recurrent_timeslot_owner_id");

            migrationBuilder.AddColumn<string>(
                name: "setting_description_key",
                table: "c_notification_type",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "setting_title_key",
                table: "c_notification_type",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "app_user_profile_behaviour",
                columns: table => new
                {
                    user_profile_behaviour_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    visited_user_id = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_user_profile_behaviour", x => x.user_profile_behaviour_id);
                    table.ForeignKey(
                        name: "fk_app_user_profile_behaviour_app_user_user_id",
                        column: x => x.user_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_app_user_profile_behaviour_app_user_visited_user_id",
                        column: x => x.visited_user_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "r_notification_type_user_role",
                columns: table => new
                {
                    notification_type_user_role_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    notification_type_id = table.Column<int>(type: "integer", nullable: false),
                    user_role_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_r_notification_type_user_role", x => x.notification_type_user_role_id);
                    table.ForeignKey(
                        name: "fk_r_notification_type_user_role_c_notification_type_notification_type_id",
                        column: x => x.notification_type_id,
                        principalTable: "c_notification_type",
                        principalColumn: "notification_type_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_r_notification_type_user_role_c_user_role_user_role_id",
                        column: x => x.user_role_id,
                        principalTable: "c_user_role",
                        principalColumn: "user_role_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "r_user_notification_blacklist_setting",
                columns: table => new
                {
                    user_notification_blacklist_setting_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    notification_type_id = table.Column<int>(type: "integer", nullable: false),
                    notification_channel_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_r_user_notification_blacklist_setting", x => x.user_notification_blacklist_setting_id);
                    table.ForeignKey(
                        name: "fk_r_user_notification_blacklist_setting_app_user_user_id",
                        column: x => x.user_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_r_user_notification_blacklist_setting_c_notification_channel_notification_channel_id",
                        column: x => x.notification_channel_id,
                        principalTable: "c_notification_channel",
                        principalColumn: "notification_channel_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_r_user_notification_blacklist_setting_c_notification_type_notification_type_id",
                        column: x => x.notification_type_id,
                        principalTable: "c_notification_type",
                        principalColumn: "notification_type_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 1,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.email_confirmation.setting.description", "notification_type.email_confirmation.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 2,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.profile_verified.setting.description", "notification_type.profile_verified.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 3,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.password_reset.setting.description", "notification_type.password_reset.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 4,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.welcome.setting.description", "notification_type.welcome.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 5,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.package_cancellation.setting.description", "notification_type.package_cancellation.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 6,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.lesson_reminder.setting.description", "notification_type.lesson_reminder.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 7,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.student_rescheduled_lesson.setting.description", "notification_type.student_rescheduled_lesson.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 8,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.new_reservation.setting.description", "notification_type.new_reservation.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 9,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.new_package_reservation.setting.description", "notification_type.new_package_reservation.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 10,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.new_review.setting.description", "notification_type.new_review.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 11,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.new_message.setting.description", "notification_type.new_message.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 12,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.refund_requested_for_your_lesson.setting.description", "notification_type.refund_requested_for_your_lesson.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 13,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.request_refund_approved.setting.description", "notification_type.request_refund_approved.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 14,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.request_refund_declined.setting.description", "notification_type.request_refund_declined.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 15,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.money_sent.setting.description", "notification_type.money_sent.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 16,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.tutor_rescheduled_lesson.setting.description", "notification_type.tutor_rescheduled_lesson.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 17,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.tutor_profile_approval_requested.setting.description", "notification_type.tutor_profile_approval_requested.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 18,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.refund_request_for_admin.setting.description", "notification_type.refund_request_for_admin.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 19,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.money_withdrawal_requested.setting.description", "notification_type.money_withdrawal_requested.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 20,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.student_bought_package.setting.description", "notification_type.student_bought_package.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 21,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.student_deposited_money.setting.description", "notification_type.student_deposited_money.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 22,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.reservation_cancellation.setting.description", "notification_type.reservation_cancellation.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 23,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.invitation.setting.description", "notification_type.invitation.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 24,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.registration_completed.setting.description", "notification_type.registration_completed.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 25,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.account_created.setting.description", "notification_type.account_created.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 26,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.company_invited_colleagues.setting.description", "notification_type.company_invited_colleagues.setting.title" });

            migrationBuilder.UpdateData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 27,
                columns: new[] { "setting_description_key", "setting_title_key" },
                values: new object[] { "notification_type.email_confirmation_2.setting.description", "notification_type.email_confirmation_2.setting.title" });

            migrationBuilder.InsertData(
                table: "c_notification_type",
                columns: new[] { "notification_type_id", "description_key", "enum_key", "name_key", "setting_description_key", "setting_title_key", "title_key" },
                values: new object[] { 28, "notification_type.tutor_profile_visited.description", "tutor_profile_visited", "notification_type.tutor_profile_visited.name", "notification_type.tutor_profile_visited.setting.description", "notification_type.tutor_profile_visited.setting.title", "notification_type.tutor_profile_visited.title" });

            migrationBuilder.InsertData(
                table: "r_notification_type_user_role",
                columns: new[] { "notification_type_user_role_id", "notification_type_id", "user_role_id" },
                values: new object[,]
                {
                    { 1, 5, 2 },
                    { 2, 6, 2 },
                    { 3, 6, 3 },
                    { 4, 7, 2 },
                    { 5, 8, 2 },
                    { 6, 9, 2 },
                    { 7, 10, 2 },
                    { 8, 11, 2 },
                    { 9, 11, 3 },
                    { 10, 16, 3 },
                    { 11, 22, 2 }
                });

            migrationBuilder.InsertData(
                table: "r_notification_type_user_role",
                columns: new[] { "notification_type_user_role_id", "notification_type_id", "user_role_id" },
                values: new object[] { 12, 28, 2 });

            migrationBuilder.CreateIndex(
                name: "ix_app_user_profile_behaviour_user_id",
                table: "app_user_profile_behaviour",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_user_profile_behaviour_visited_user_id",
                table: "app_user_profile_behaviour",
                column: "visited_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_r_notification_type_user_role_notification_type_id",
                table: "r_notification_type_user_role",
                column: "notification_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_r_notification_type_user_role_user_role_id",
                table: "r_notification_type_user_role",
                column: "user_role_id");

            migrationBuilder.CreateIndex(
                name: "ix_r_user_notification_blacklist_setting_notification_channel_id",
                table: "r_user_notification_blacklist_setting",
                column: "notification_channel_id");

            migrationBuilder.CreateIndex(
                name: "ix_r_user_notification_blacklist_setting_notification_type_id",
                table: "r_user_notification_blacklist_setting",
                column: "notification_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_r_user_notification_blacklist_setting_user_id",
                table: "r_user_notification_blacklist_setting",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "app_user_profile_behaviour");

            migrationBuilder.DropTable(
                name: "r_notification_type_user_role");

            migrationBuilder.DropTable(
                name: "r_user_notification_blacklist_setting");

            migrationBuilder.DeleteData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 28);

            migrationBuilder.DropColumn(
                name: "setting_description_key",
                table: "c_notification_type");

            migrationBuilder.DropColumn(
                name: "setting_title_key",
                table: "c_notification_type");

            migrationBuilder.RenameIndex(
                name: "ix_app_timeslot_owner_id",
                table: "app_timeslot",
                newName: "IX_app_timeslot_owner_id");

            migrationBuilder.RenameIndex(
                name: "ix_app_recurrent_timeslot_owner_id",
                table: "app_recurrent_timeslot",
                newName: "IX_app_recurrent_timeslot_owner_id");
        }
    }
}
