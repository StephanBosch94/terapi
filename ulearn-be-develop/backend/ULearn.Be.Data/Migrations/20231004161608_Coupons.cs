using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class Coupons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "coupon_last_sent_time",
                table: "app_user",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "coupon_id",
                table: "app_transaction",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "coupon_sale_amount",
                table: "app_transaction",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "coupon_id",
                table: "app_reservation",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "coupon_sale_amount",
                table: "app_reservation",
                type: "double precision",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "c_coupon_type",
                columns: table => new
                {
                    coupon_type_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_coupon_type", x => x.coupon_type_id);
                });

            migrationBuilder.CreateTable(
                name: "app_coupon",
                columns: table => new
                {
                    coupon_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    coupon_type_id = table.Column<int>(type: "integer", nullable: false),
                    discount_percentage = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    valid_from = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    valid_to = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    code = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_coupon", x => x.coupon_id);
                    table.ForeignKey(
                        name: "fk_app_coupon_app_user_user_id",
                        column: x => x.user_id,
                        principalTable: "app_user",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "fk_app_coupon_c_coupon_type_coupon_type_id",
                        column: x => x.coupon_type_id,
                        principalTable: "c_coupon_type",
                        principalColumn: "coupon_type_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "r_user_activated_coupon",
                columns: table => new
                {
                    user_activated_coupon_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    coupon_id = table.Column<int>(type: "integer", nullable: false),
                    activated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_r_user_activated_coupon", x => x.user_activated_coupon_id);
                    table.ForeignKey(
                        name: "fk_r_user_activated_coupon_app_coupon_coupon_id",
                        column: x => x.coupon_id,
                        principalTable: "app_coupon",
                        principalColumn: "coupon_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_r_user_activated_coupon_app_user_user_id",
                        column: x => x.user_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "c_coupon_type",
                columns: new[] { "coupon_type_id", "description_key", "enum_key", "name_key" },
                values: new object[] { 1, "coupon_type.one_lesson_sale.description", "one_lesson_sale", "coupon_type.one_lesson_sale.name" });

            migrationBuilder.InsertData(
                table: "c_notification_type",
                columns: new[] { "notification_type_id", "description_key", "enum_key", "name_key", "setting_description_key", "setting_title_key", "title_key" },
                values: new object[] { 29, "notification_type.coupon.description", "coupon", "notification_type.coupon.name", "notification_type.coupon.setting.description", "notification_type.coupon.setting.title", "notification_type.coupon.title" });

            migrationBuilder.InsertData(
                table: "r_notification_type_user_role",
                columns: new[] { "notification_type_user_role_id", "notification_type_id", "user_role_id" },
                values: new object[] { 13, 29, 3 });

            migrationBuilder.CreateIndex(
                name: "ix_app_transaction_coupon_id",
                table: "app_transaction",
                column: "coupon_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_reservation_coupon_id",
                table: "app_reservation",
                column: "coupon_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_coupon_coupon_type_id",
                table: "app_coupon",
                column: "coupon_id");

            migrationBuilder.CreateIndex(
                name: "IX_app_coupon_coupon_type_id",
                table: "app_coupon",
                column: "coupon_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_coupon_user_id",
                table: "app_coupon",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_c_coupon_type_enum_key",
                table: "c_coupon_type",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_r_user_activated_coupon_coupon_id",
                table: "r_user_activated_coupon",
                column: "coupon_id");

            migrationBuilder.CreateIndex(
                name: "ix_r_user_activated_coupon_user_id",
                table: "r_user_activated_coupon",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_app_coupon_coupon_id",
                table: "app_reservation",
                column: "coupon_id",
                principalTable: "app_coupon",
                principalColumn: "coupon_id");

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_app_coupon_coupon_id",
                table: "app_transaction",
                column: "coupon_id",
                principalTable: "app_coupon",
                principalColumn: "coupon_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_app_coupon_coupon_id",
                table: "app_reservation");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_app_coupon_coupon_id",
                table: "app_transaction");

            migrationBuilder.DropTable(
                name: "r_user_activated_coupon");

            migrationBuilder.DropTable(
                name: "app_coupon");

            migrationBuilder.DropTable(
                name: "c_coupon_type");

            migrationBuilder.DropIndex(
                name: "ix_app_transaction_coupon_id",
                table: "app_transaction");

            migrationBuilder.DropIndex(
                name: "ix_app_reservation_coupon_id",
                table: "app_reservation");

            migrationBuilder.DeleteData(
                table: "r_notification_type_user_role",
                keyColumn: "notification_type_user_role_id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "c_notification_type",
                keyColumn: "notification_type_id",
                keyValue: 29);

            migrationBuilder.DropColumn(
                name: "coupon_last_sent_time",
                table: "app_user");

            migrationBuilder.DropColumn(
                name: "coupon_id",
                table: "app_transaction");

            migrationBuilder.DropColumn(
                name: "coupon_sale_amount",
                table: "app_transaction");

            migrationBuilder.DropColumn(
                name: "coupon_id",
                table: "app_reservation");

            migrationBuilder.DropColumn(
                name: "coupon_sale_amount",
                table: "app_reservation");
        }
    }
}
