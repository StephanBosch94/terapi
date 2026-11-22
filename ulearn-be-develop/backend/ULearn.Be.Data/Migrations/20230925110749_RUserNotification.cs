using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class RUserNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "r_user_notification",
                columns: table => new
                {
                    User_Notification_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    User_Id = table.Column<int>(type: "integer", nullable: false),
                    Notification_Type_Id = table.Column<int>(type: "integer", nullable: false),
                    Notification_Channel_Id = table.Column<int>(type: "integer", nullable: false),
                    Notification_State_Id = table.Column<int>(type: "integer", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_r_user_notification", x => x.User_Notification_Id);
                    table.ForeignKey(
                        name: "fk_r_user_notification_app_user_user_id",
                        column: x => x.User_Id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_r_user_notification_c_notification_channel_notification_channel_id",
                        column: x => x.Notification_Channel_Id,
                        principalTable: "c_notification_channel",
                        principalColumn: "notification_channel_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_r_user_notification_c_notification_state_notification_state_id",
                        column: x => x.Notification_State_Id,
                        principalTable: "c_notification_state",
                        principalColumn: "NotificationStateId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_r_user_notification_c_notification_type_notification_type_id",
                        column: x => x.Notification_Type_Id,
                        principalTable: "c_notification_type",
                        principalColumn: "notification_type_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "c_notification_state",
                columns: new[] { "NotificationStateId", "description_key", "enum_key", "name_key" },
                values: new object[] { 3, "notification_state.failed_to_send.description", "failed_to_send", "notification_state.failed_to_send.name" });

            migrationBuilder.InsertData(
                table: "c_notification_type",
                columns: new[] { "notification_type_id", "description_key", "enum_key", "name_key", "title_key" },
                values: new object[] { 27, "notification_type.email_confirmation_2.description", "email_confirmation_2", "notification_type.email_confirmation_2.name", "notification_type.email_confirmation_2.title" });

            migrationBuilder.CreateIndex(
                name: "ix_r_user_notification_notification_channel_id",
                table: "r_user_notification",
                column: "Notification_Channel_Id");

            migrationBuilder.CreateIndex(
                name: "ix_r_user_notification_notification_state_id",
                table: "r_user_notification",
                column: "Notification_State_Id");

            migrationBuilder.CreateIndex(
                name: "ix_r_user_notification_notification_type_id",
                table: "r_user_notification",
                column: "Notification_Type_Id");

            migrationBuilder.CreateIndex(
                name: "ix_r_user_notification_user_id",
                table: "r_user_notification",
                column: "User_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "r_user_notification");

            migrationBuilder.DeleteData(
                table: "c_notification_state",
                keyColumn: "NotificationStateId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 27);
        }
    }
}
