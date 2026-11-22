using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class Transactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 13);

            migrationBuilder.AddColumn<int>(
                name: "customer_id",
                table: "app_transaction",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "transaction_group_id",
                table: "app_transaction",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "transaction_refund_reason_id",
                table: "app_transaction",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "vendor_id",
                table: "app_transaction",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "class_duration_id",
                table: "app_reservation_package",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "class_duration_id",
                table: "app_reservation",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "expected_end_time",
                table: "app_reservation",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.DropIndex(
                name: "ix_c_transaction_type_enum_key",
                table: "c_transaction_type"
            );
            
            migrationBuilder.CreateTable(
                name: "app_transaction_group",
                columns: table => new
                {
                    transaction_group_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_transaction_group", x => x.transaction_group_id);
                });

            migrationBuilder.CreateTable(
                name: "c_class_duration",
                columns: table => new
                {
                    class_duration_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    duration_with_margin = table.Column<int>(type: "integer", nullable: false),
                    expected_duration = table.Column<int>(type: "integer", nullable: false),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_class_duration", x => x.class_duration_id);
                });

            migrationBuilder.CreateTable(
                name: "c_transaction_refund_reason",
                columns: table => new
                {
                    TransactionRefundReasonId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_transaction_refund_reason", x => x.TransactionRefundReasonId);
                });

            migrationBuilder.CreateTable(
                name: "c_transaction_template_type",
                columns: table => new
                {
                    transaction_template_type_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_transaction_template_type", x => x.transaction_template_type_id);
                });

            migrationBuilder.CreateTable(
                name: "c_transaction_template",
                columns: table => new
                {
                    transaction_template_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    transaction_type_template_id = table.Column<int>(type: "integer", nullable: false),
                    content_type = table.Column<string>(type: "text", nullable: true),
                    data = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_transaction_template", x => x.transaction_template_id);
                    table.ForeignKey(
                        name: "FK_c_transaction_template_c_transaction_template_type_transact~",
                        column: x => x.transaction_type_template_id,
                        principalTable: "c_transaction_template_type",
                        principalColumn: "transaction_template_type_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "c_class_duration",
                columns: new[] { "class_duration_id", "description_key", "duration_with_margin", "enum_key", "expected_duration", "name_key" },
                values: new object[,]
                {
                    { 1, "class_duration.half.description", 30, "half", 25, "class_duration.half.name" },
                    { 2, "class_duration.full.description", 60, "full", 50, "class_duration.full.name" }
                });

            migrationBuilder.InsertData(
                table: "c_transaction_refund_reason",
                columns: new[] { "TransactionRefundReasonId", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "transaction_refund_reason.package_cancellation.description", "package_cancellation", "transaction_refund_reason.package_cancellation.name" },
                    { 2, "transaction_refund_reason.tutor_reschedule.description", "tutor_reschedule", "transaction_refund_reason.tutor_reschedule.name" },
                    { 3, "transaction_refund_reason.student_reschedule.description", "student_reschedule", "transaction_refund_reason.student_reschedule.name" },
                    { 4, "transaction_refund_reason.refund.description", "refund", "transaction_refund_reason.refund.name" }
                });

            migrationBuilder.InsertData(
                table: "c_transaction_state",
                columns: new[] { "transaction_state_id", "description_key", "enum_key", "name_key" },
                values: new object[] { 6, "transaction_state.created.description", "created", "transaction_state.created.name" });

            migrationBuilder.InsertData(
                table: "c_transaction_template_type",
                columns: new[] { "transaction_template_type_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "transaction_template_type.tutors_receipt_with_taxes.description", "tutors_receipt_with_taxes", "transaction_template_type.tutors_receipt_with_taxes.name" },
                    { 2, "transaction_template_type.tutors_receipt_without_taxes.description", "tutors_receipt_without_taxes", "transaction_template_type.tutors_receipt_without_taxes.name" },
                    { 3, "transaction_template_type.tutors_credit_memo.description", "tutors_credit_memo", "transaction_template_type.tutors_credit_memo.name" },
                    { 4, "transaction_template_type.platforms_commission_receipt.description", "platforms_commission_receipt", "transaction_template_type.platforms_commission_receipt.name" },
                    { 5, "transaction_template_type.platforms_withdrawal_confirmation.description", "platforms_withdrawal_confirmation", "transaction_template_type.platforms_withdrawal_confirmation.name" },
                    { 6, "transaction_template_type.platforms_credit_memo.description", "platforms_credit_memo", "transaction_template_type.platforms_credit_memo.name" }
                });

            migrationBuilder.UpdateData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 2,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "transaction_type.package_cancellation.description", "package_cancellation", "transaction_type.package_cancellation.name" });

            migrationBuilder.UpdateData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 3,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "transaction_type.withdrawal.description", "withdrawal", "transaction_type.withdrawal.name" });

            migrationBuilder.UpdateData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 4,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "transaction_type.refund.description", "refund", "transaction_type.refund.name" });

            migrationBuilder.UpdateData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 5,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "transaction_type.lesson_payment.description", "lesson_payment", "transaction_type.lesson_payment.name" });

            migrationBuilder.UpdateData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 6,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "transaction_type.lesson_commission.description", "lesson_commission", "transaction_type.lesson_commission.name" });

            migrationBuilder.UpdateData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 7,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "transaction_type.refund_commission.description", "refund_commission", "transaction_type.refund_commission.name" });

            migrationBuilder.UpdateData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 8,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "transaction_type.stripe_commission.description", "stripe_commission", "transaction_type.stripe_commission.name" });

            migrationBuilder.UpdateData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 9,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "transaction_type.withdraw_commission.description", "withdraw_commission", "transaction_type.withdraw_commission.name" });

            migrationBuilder.UpdateData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 10,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "transaction_type.package_sale_cancellation_commission.description", "package_sale_cancellation_commission", "transaction_type.package_sale_cancellation_commission.name" });

            migrationBuilder.CreateIndex(
                name: "ix_app_transaction_customer_id",
                table: "app_transaction",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_transaction_refund_reason_id",
                table: "app_transaction",
                column: "transaction_refund_reason_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_transaction_transaction_group_id",
                table: "app_transaction",
                column: "transaction_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_transaction_vendor_id",
                table: "app_transaction",
                column: "vendor_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_reservation_package_class_duration_id",
                table: "app_reservation_package",
                column: "class_duration_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_reservation_class_duration_id",
                table: "app_reservation",
                column: "class_duration_id");

            migrationBuilder.CreateIndex(
                name: "ix_c_class_duration_enum_key",
                table: "c_class_duration",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_transaction_refund_reason",
                table: "c_transaction_refund_reason",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uc_c_transaction_template_transaction_template_type_id",
                table: "c_transaction_template",
                column: "transaction_type_template_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_c_class_duration_class_duration_id",
                table: "app_reservation",
                column: "class_duration_id",
                principalTable: "c_class_duration",
                principalColumn: "class_duration_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_package_c_class_duration_class_duration_id",
                table: "app_reservation_package",
                column: "class_duration_id",
                principalTable: "c_class_duration",
                principalColumn: "class_duration_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_app_transaction_group_transaction_group_id",
                table: "app_transaction",
                column: "transaction_group_id",
                principalTable: "app_transaction_group",
                principalColumn: "transaction_group_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_app_user_customer_id",
                table: "app_transaction",
                column: "customer_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_app_user_vendor_id",
                table: "app_transaction",
                column: "vendor_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_c_refund_reason_refund_reason_id",
                table: "app_transaction",
                column: "transaction_refund_reason_id",
                principalTable: "c_transaction_refund_reason",
                principalColumn: "TransactionRefundReasonId");
            
            migrationBuilder.CreateIndex(
                name: "ix_c_transaction_type_enum_key",
                table: "c_transaction_type",
                column: "enum_key",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_c_class_duration_class_duration_id",
                table: "app_reservation");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_package_c_class_duration_class_duration_id",
                table: "app_reservation_package");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_app_transaction_group_transaction_group_id",
                table: "app_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_app_user_customer_id",
                table: "app_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_app_user_vendor_id",
                table: "app_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_c_refund_reason_refund_reason_id",
                table: "app_transaction");

            migrationBuilder.DropTable(
                name: "app_transaction_group");

            migrationBuilder.DropTable(
                name: "c_class_duration");

            migrationBuilder.DropTable(
                name: "c_transaction_refund_reason");

            migrationBuilder.DropTable(
                name: "c_transaction_template");

            migrationBuilder.DropTable(
                name: "c_transaction_template_type");

            migrationBuilder.DropIndex(
                name: "ix_app_transaction_customer_id",
                table: "app_transaction");

            migrationBuilder.DropIndex(
                name: "ix_app_transaction_refund_reason_id",
                table: "app_transaction");

            migrationBuilder.DropIndex(
                name: "ix_app_transaction_transaction_group_id",
                table: "app_transaction");

            migrationBuilder.DropIndex(
                name: "ix_app_transaction_vendor_id",
                table: "app_transaction");

            migrationBuilder.DropIndex(
                name: "ix_app_reservation_package_class_duration_id",
                table: "app_reservation_package");

            migrationBuilder.DropIndex(
                name: "ix_app_reservation_class_duration_id",
                table: "app_reservation");

            migrationBuilder.DeleteData(
                table: "c_transaction_state",
                keyColumn: "transaction_state_id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "customer_id",
                table: "app_transaction");

            migrationBuilder.DropColumn(
                name: "transaction_group_id",
                table: "app_transaction");

            migrationBuilder.DropColumn(
                name: "transaction_refund_reason_id",
                table: "app_transaction");

            migrationBuilder.DropColumn(
                name: "vendor_id",
                table: "app_transaction");

            migrationBuilder.DropColumn(
                name: "class_duration_id",
                table: "app_reservation_package");

            migrationBuilder.DropColumn(
                name: "class_duration_id",
                table: "app_reservation");

            migrationBuilder.DropColumn(
                name: "expected_end_time",
                table: "app_reservation");

            migrationBuilder.UpdateData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 2,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "transaction_type.lesson_package_payment.description", "lesson_package_payment", "transaction_type.lesson_package_payment.name" });

            migrationBuilder.UpdateData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 3,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "transaction_type.package_cancellation.description", "package_cancellation", "transaction_type.package_cancellation.name" });

            migrationBuilder.UpdateData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 4,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "transaction_type.withdrawal.description", "withdrawal", "transaction_type.withdrawal.name" });

            migrationBuilder.UpdateData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 5,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "transaction_type.refund.description", "refund", "transaction_type.refund.name" });

            migrationBuilder.UpdateData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 6,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "transaction_type.lesson_payment.description", "lesson_payment", "transaction_type.lesson_payment.name" });

            migrationBuilder.UpdateData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 7,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "transaction_type.lesson_commission.description", "lesson_commission", "transaction_type.lesson_commission.name" });

            migrationBuilder.UpdateData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 8,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "transaction_type.package_sale_cancellation.description", "package_sale_cancellation", "transaction_type.package_sale_cancellation.name" });

            migrationBuilder.UpdateData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 9,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "transaction_type.lesson_payment_after_refund.description", "lesson_payment_after_refund", "transaction_type.lesson_payment_after_refund.name" });

            migrationBuilder.UpdateData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 10,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "transaction_type.refund_commission.description", "refund_commission", "transaction_type.refund_commission.name" });

            migrationBuilder.InsertData(
                table: "c_transaction_type",
                columns: new[] { "transaction_type_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 11, "transaction_type.refund_loss.description", "refund_loss", "transaction_type.refund_loss.name" },
                    { 12, "transaction_type.stripe_fee.description", "stripe_fee", "transaction_type.stripe_fee.name" },
                    { 13, "transaction_type.withdraw_fee.description", "withdraw_fee", "transaction_type.withdraw_fee.name" }
                });
        }
    }
}
