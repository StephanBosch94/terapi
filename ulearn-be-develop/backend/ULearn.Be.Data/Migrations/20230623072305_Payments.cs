using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class Payments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_package_app_reservation_reservation_id",
                table: "app_reservation_package");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_app_user_waller_user_wallet_id",
                table: "app_user_wallet_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_c_payment_service_provider_payment_service_provider_id",
                table: "app_user_wallet_transaction");

            migrationBuilder.DropPrimaryKey(
                name: "pk_app_user_wallet_transaction",
                table: "app_user_wallet_transaction");

            migrationBuilder.RenameTable(
                name: "app_user_wallet_transaction",
                newName: "app_transaction");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "app_reservation_package",
                newName: "host_id");

            migrationBuilder.RenameIndex(
                name: "ix_app_reservation_package_owner_id",
                table: "app_reservation_package",
                newName: "ix_app_reservation_package_host_id");

            migrationBuilder.RenameIndex(
                name: "ix_app_user_wallet_transaction_payment_service_provider_id",
                table: "app_transaction",
                newName: "ix_app_transaction_payment_service_provider_id");

            migrationBuilder.AddColumn<double>(
                name: "scheduled_amount",
                table: "app_user_wallet",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "total_revenue",
                table: "app_user_wallet",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "attender_id",
                table: "app_reservation_package",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "payment_service_provider_id",
                table: "app_transaction",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "reservation_id",
                table: "app_transaction",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_app_transaction",
                table: "app_transaction",
                column: "transaction_id");

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 1,
                column: "hash",
                value: new Guid("6137e3f3-d393-4705-b085-14d92d392623"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 2,
                column: "hash",
                value: new Guid("a58367e5-5f1d-49a5-afc5-72a6123c369d"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 3,
                column: "hash",
                value: new Guid("6f666271-8fec-43c7-b0ea-3d150bf2f615"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 4,
                column: "hash",
                value: new Guid("bb433247-8596-4094-a5ce-4fd499332c1b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 5,
                column: "hash",
                value: new Guid("2a42f0b5-e151-4f18-9b0e-15daf00d0119"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 6,
                column: "hash",
                value: new Guid("4256a509-5dae-4b63-a40d-c601f13e07ea"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 7,
                column: "hash",
                value: new Guid("e7af0ffd-5e91-4552-a0fc-d19a5e1621c7"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 8,
                column: "hash",
                value: new Guid("0874c64b-55ba-4f7e-98e8-adbbbc4503a4"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 9,
                column: "hash",
                value: new Guid("ac10bd05-5e5b-4686-bb09-deb31f5f98b5"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 10,
                column: "hash",
                value: new Guid("192f97f5-1078-4f26-9d00-6edc30f053ae"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 11,
                column: "hash",
                value: new Guid("9bff7750-1533-47be-ad00-87d30807e13e"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 12,
                column: "hash",
                value: new Guid("97f1adcf-1da4-439b-89bc-194f2fc3c3be"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 13,
                column: "hash",
                value: new Guid("e963d990-d794-4546-a2a5-9fe1f4189e9a"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 14,
                column: "hash",
                value: new Guid("5fb0f97c-c9ea-4a85-a5eb-009eb7563aae"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 15,
                column: "hash",
                value: new Guid("49c2e642-65e4-46a0-ad75-1bc5b3a54c50"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 16,
                column: "hash",
                value: new Guid("ae8f40d5-634b-4f3e-8709-12820fd7331d"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 17,
                column: "hash",
                value: new Guid("2d3c69ba-8096-43b3-947f-6b98d2e7ceec"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 18,
                column: "hash",
                value: new Guid("d4c4ba52-b5ae-413f-97dd-6735992e81f3"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 19,
                column: "hash",
                value: new Guid("ec774199-149f-4a82-a851-6fc34fee9a27"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 20,
                column: "hash",
                value: new Guid("0be0abda-928a-40c7-aeb7-5ffa649f3e44"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 21,
                column: "hash",
                value: new Guid("ddce8c92-ea22-44cb-b3c6-9cbf21b94448"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 22,
                column: "hash",
                value: new Guid("2732d372-5ca8-4c9a-9ff0-ca8d126a58dc"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 23,
                column: "hash",
                value: new Guid("a02d7ea3-5120-472f-be8b-de3a85a526e7"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 24,
                column: "hash",
                value: new Guid("4128e67c-7ac6-4a12-bb69-6f8f53dcabb6"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 25,
                column: "hash",
                value: new Guid("6c87200a-16c7-453a-b66d-aee47a23b234"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 26,
                column: "hash",
                value: new Guid("b21b6eae-4cb0-463e-89e2-faf47d27c4b3"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 27,
                column: "hash",
                value: new Guid("9739c05f-71b6-4d38-b2ba-9ca6189a94af"));

            migrationBuilder.InsertData(
                table: "c_reservation_state",
                columns: new[] { "reservation_state_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 5, "reservation_state.scheduled_for_payment.description", "scheduled_for_payment", "reservation_state.scheduled_for_payment.name" },
                    { 6, "reservation_state.payed.description", "payed", "reservation_state.payed.name" },
                    { 7, "reservation_state.refunded.description", "refunded", "reservation_state.refunded.name" },
                    { 8, "reservation_state.waiting_for_review.description", "waiting_for_review", "reservation_state.waiting_for_review.name" }
                });

            migrationBuilder.InsertData(
                table: "c_transaction_state",
                columns: new[] { "transaction_state_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 3, "transaction_state.canceled.description", "canceled", "transaction_state.canceled.name" },
                    { 4, "transaction_state.waiting_for_review.description", "waiting_for_review", "transaction_state.waiting_for_review.name" }
                });

            migrationBuilder.InsertData(
                table: "c_transaction_type",
                columns: new[] { "transaction_type_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 6, "transaction_type.lesson_payment.description", "lesson_payment", "transaction_type.lesson_payment.name" },
                    { 7, "transaction_type.lesson_commission.description", "lesson_commission", "transaction_type.lesson_commission.name" },
                    { 8, "transaction_type.package_sale_cancellation.description", "package_sale_cancellation", "transaction_type.package_sale_cancellation.name" },
                    { 9, "transaction_type.lesson_payment_after_refund.description", "lesson_payment_after_refund", "transaction_type.lesson_payment_after_refund.name" },
                    { 10, "transaction_type.refund_commission.description", "refund_commission", "transaction_type.refund_commission.name" },
                    { 11, "transaction_type.refund_loss.description", "refund_loss", "transaction_type.refund_loss.name" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_app_reservation_package_attender_id",
                table: "app_reservation_package",
                column: "attender_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_transaction_reservation_id",
                table: "app_transaction",
                column: "reservation_id");

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_package_app_user_attender_id",
                table: "app_reservation_package",
                column: "attender_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_package_app_user_host_id",
                table: "app_reservation_package",
                column: "host_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_app_reservation_reservation_id",
                table: "app_transaction",
                column: "reservation_id",
                principalTable: "app_reservation",
                principalColumn: "reservation_id");

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_app_user_wallet_user_wallet_id",
                table: "app_transaction",
                column: "user_wallet_id",
                principalTable: "app_user_wallet",
                principalColumn: "user_wallet_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_c_payment_service_provider_payment_service_provider_id",
                table: "app_transaction",
                column: "payment_service_provider_id",
                principalTable: "c_payment_service_provider",
                principalColumn: "payment_service_provider_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_package_app_user_attender_id",
                table: "app_reservation_package");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_package_app_user_host_id",
                table: "app_reservation_package");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_app_reservation_reservation_id",
                table: "app_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_app_user_wallet_user_wallet_id",
                table: "app_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_c_payment_service_provider_payment_service_provider_id",
                table: "app_transaction");

            migrationBuilder.DropIndex(
                name: "ix_app_reservation_package_attender_id",
                table: "app_reservation_package");

            migrationBuilder.DropPrimaryKey(
                name: "pk_app_transaction",
                table: "app_transaction");

            migrationBuilder.DropIndex(
                name: "ix_app_transaction_reservation_id",
                table: "app_transaction");

            migrationBuilder.DeleteData(
                table: "c_reservation_state",
                keyColumn: "reservation_state_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "c_reservation_state",
                keyColumn: "reservation_state_id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "c_reservation_state",
                keyColumn: "reservation_state_id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "c_reservation_state",
                keyColumn: "reservation_state_id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "c_transaction_state",
                keyColumn: "transaction_state_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "c_transaction_state",
                keyColumn: "transaction_state_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 11);

            migrationBuilder.DropColumn(
                name: "scheduled_amount",
                table: "app_user_wallet");

            migrationBuilder.DropColumn(
                name: "total_revenue",
                table: "app_user_wallet");

            migrationBuilder.DropColumn(
                name: "attender_id",
                table: "app_reservation_package");

            migrationBuilder.DropColumn(
                name: "reservation_id",
                table: "app_transaction");

            migrationBuilder.RenameTable(
                name: "app_transaction",
                newName: "app_user_wallet_transaction");

            migrationBuilder.RenameColumn(
                name: "host_id",
                table: "app_reservation_package",
                newName: "owner_id");

            migrationBuilder.RenameIndex(
                name: "ix_app_reservation_package_host_id",
                table: "app_reservation_package",
                newName: "ix_app_reservation_package_owner_id");

            migrationBuilder.RenameIndex(
                name: "ix_app_transaction_payment_service_provider_id",
                table: "app_user_wallet_transaction",
                newName: "ix_app_user_wallet_transaction_payment_service_provider_id");

            migrationBuilder.AlterColumn<int>(
                name: "payment_service_provider_id",
                table: "app_user_wallet_transaction",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_app_user_wallet_transaction",
                table: "app_user_wallet_transaction",
                column: "transaction_id");

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 1,
                column: "hash",
                value: new Guid("6a430313-2112-4da4-acc1-bd56addac471"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 2,
                column: "hash",
                value: new Guid("2d3387ff-9f9f-4d4f-beaf-0a73ac21df4a"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 3,
                column: "hash",
                value: new Guid("c16a67cb-6433-4d5a-b561-ec2ad312b33a"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 4,
                column: "hash",
                value: new Guid("d3b3cc83-2fb1-4e5e-bd4b-a3be7a2e8261"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 5,
                column: "hash",
                value: new Guid("c916e36b-2d41-423c-8627-78e0efc69ec0"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 6,
                column: "hash",
                value: new Guid("b75e6cf9-b852-47a6-9405-96d69292ad03"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 7,
                column: "hash",
                value: new Guid("fbefc0dd-54f8-4018-b5fa-cb8574857cba"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 8,
                column: "hash",
                value: new Guid("ab1f218e-b265-4503-a2c3-19c54d41f188"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 9,
                column: "hash",
                value: new Guid("c2b4e0cf-5f96-4b7f-92b8-e19c4d775845"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 10,
                column: "hash",
                value: new Guid("7c0d9d92-382c-4b1c-85dd-4928295268bb"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 11,
                column: "hash",
                value: new Guid("d5e568a0-9be5-4272-a38c-91a3dc4b14af"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 12,
                column: "hash",
                value: new Guid("893b3c05-5363-47be-a6e1-8acf6eb03d39"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 13,
                column: "hash",
                value: new Guid("0c506060-4e30-4a9a-a676-ace43f0512a5"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 14,
                column: "hash",
                value: new Guid("379d5323-f46e-4c64-af55-47658680ce2d"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 15,
                column: "hash",
                value: new Guid("75b6a97d-9b8a-4a74-b022-a0b819133ff7"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 16,
                column: "hash",
                value: new Guid("6a6203d7-e770-48c9-b8f6-a1a4518706c4"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 17,
                column: "hash",
                value: new Guid("5cfc42b5-577a-4e4e-b5b9-bc69105ae519"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 18,
                column: "hash",
                value: new Guid("e011b9e4-0488-4db6-ad0f-4d531b34ade3"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 19,
                column: "hash",
                value: new Guid("c0d6f36c-4a84-4ed8-aad8-ad2b977118e8"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 20,
                column: "hash",
                value: new Guid("c20d87c7-c223-4e81-9822-1599da945958"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 21,
                column: "hash",
                value: new Guid("ab760dd1-9fdc-4f2d-a08d-55fa5731e212"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 22,
                column: "hash",
                value: new Guid("58540202-f67d-40d1-97ce-c08a9c77f892"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 23,
                column: "hash",
                value: new Guid("f5b0371a-0132-4b2f-9e60-113814532d5c"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 24,
                column: "hash",
                value: new Guid("1a600931-a0c8-480e-90b9-3e79ad593f80"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 25,
                column: "hash",
                value: new Guid("7d340515-15b3-4842-a470-5b0ca31adbea"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 26,
                column: "hash",
                value: new Guid("0d7fb35f-486f-4a65-b680-81818e4303b8"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 27,
                column: "hash",
                value: new Guid("4c39c53a-4955-4911-8cc5-8ab6cc7ec647"));

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_package_app_reservation_reservation_id",
                table: "app_reservation_package",
                column: "owner_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_app_user_waller_user_wallet_id",
                table: "app_user_wallet_transaction",
                column: "user_wallet_id",
                principalTable: "app_user_wallet",
                principalColumn: "user_wallet_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_c_payment_service_provider_payment_service_provider_id",
                table: "app_user_wallet_transaction",
                column: "payment_service_provider_id",
                principalTable: "c_payment_service_provider",
                principalColumn: "payment_service_provider_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
