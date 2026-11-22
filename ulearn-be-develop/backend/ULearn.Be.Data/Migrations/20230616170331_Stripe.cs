using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class Stripe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_app_user_wallet_transaction_app_user_waller_user_wallet_id",
                table: "app_user_wallet_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_user_wallet_transaction_c_transaction_state",
                table: "app_user_wallet_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_user_wallet_transaction_c_transaction_type",
                table: "app_user_wallet_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_user_wallet_transaction_payment_service_provider",
                table: "app_user_wallet_transaction");

            migrationBuilder.RenameColumn(
                name: "user_wallet_transaction_id",
                table: "app_user_wallet_transaction",
                newName: "transaction_id");

            migrationBuilder.RenameIndex(
                name: "ix_app_user_wallet_transaction_user_wallet_id",
                table: "app_user_wallet_transaction",
                newName: "ix_app_transaction_user_wallet_id");

            migrationBuilder.RenameIndex(
                name: "ix_app_user_wallet_transaction_transaction_type_id",
                table: "app_user_wallet_transaction",
                newName: "ix_app_transaction_transaction_type_id");

            migrationBuilder.RenameIndex(
                name: "ix_app_user_wallet_transaction_transaction_state_id",
                table: "app_user_wallet_transaction",
                newName: "ix_app_transaction_transaction_state_id");

            migrationBuilder.AddColumn<int>(
                name: "reservation_package_id",
                table: "app_user_wallet_transaction",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "total_price",
                table: "app_reservation_package",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 1,
                column: "hash",
                value: new Guid("197da602-3b5c-4590-9a0e-19d1703574f8"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 2,
                column: "hash",
                value: new Guid("5981a4df-9e5b-444b-b485-dd24e5eb7892"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 3,
                column: "hash",
                value: new Guid("0b7f812d-2499-493d-9f7f-bc9cb7967840"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 4,
                column: "hash",
                value: new Guid("cadc5d33-e0f8-46bf-b251-927e0d1496c3"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 5,
                column: "hash",
                value: new Guid("c5afbfdf-c616-4ee4-867b-759987e269cc"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 6,
                column: "hash",
                value: new Guid("ec241053-d630-409c-b72f-dbf75f0e3ad7"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 7,
                column: "hash",
                value: new Guid("eea5a962-329f-4ba0-bc1e-4dde41e5c113"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 8,
                column: "hash",
                value: new Guid("d70b5b8b-9755-4806-bd06-b874775d2cea"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 9,
                column: "hash",
                value: new Guid("3d338072-6f45-4a96-9049-23361731f9d0"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 10,
                column: "hash",
                value: new Guid("e70442ff-ec2c-482c-a711-fae51c46351f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 11,
                column: "hash",
                value: new Guid("de46d3fa-ccbe-4274-9c58-9e78a509ff41"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 12,
                column: "hash",
                value: new Guid("d2936c6f-0e41-4c10-9aeb-963092690ab2"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 13,
                column: "hash",
                value: new Guid("1358c3db-ddec-469f-843a-23ff6ab6623b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 14,
                column: "hash",
                value: new Guid("7253d3d4-012d-4f48-9c33-01508fd4c558"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 15,
                column: "hash",
                value: new Guid("6654804e-039d-42ef-96c7-aa6a6df2c536"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 16,
                column: "hash",
                value: new Guid("808ba873-8a81-4fbb-9ab0-8db0f700745a"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 17,
                column: "hash",
                value: new Guid("c1d4757b-724f-4b1a-afb5-b6f5ffacc8df"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 18,
                column: "hash",
                value: new Guid("351dce4c-9a0b-4024-b521-a7fdea7074f2"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 19,
                column: "hash",
                value: new Guid("78052d8f-cf02-430d-afe0-a8cd789be594"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 20,
                column: "hash",
                value: new Guid("dd6a422a-c466-4cf1-9bbf-094db6359378"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 21,
                column: "hash",
                value: new Guid("5b943588-d0cf-4bcc-9d7a-f153ae61711e"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 22,
                column: "hash",
                value: new Guid("3ad4caad-ecc4-4ce7-b615-6c41fbf77663"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 23,
                column: "hash",
                value: new Guid("8d5f1145-4090-4f7f-b824-7aac6336809b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 24,
                column: "hash",
                value: new Guid("d94168cf-721f-4d5d-aa4d-18c5d85229a0"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 25,
                column: "hash",
                value: new Guid("0a6cefbd-b25b-4a2f-a0c4-3936b34e2a3d"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 26,
                column: "hash",
                value: new Guid("94b7d699-7531-4d46-a8c2-ac6856fae2c0"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 27,
                column: "hash",
                value: new Guid("bbe22b04-a2a1-4c3f-b03f-b60ea39da5e5"));

            migrationBuilder.UpdateData(
                table: "c_reservation_state",
                keyColumn: "reservation_state_id",
                keyValue: 2,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "reservation_state.canceled_by_user.description", "canceled_by_user", "reservation_state.canceled_by_user.name" });

            migrationBuilder.InsertData(
                table: "c_reservation_state",
                columns: new[] { "reservation_state_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 3, "reservation_state.failed_payment.description", "failed_payment", "reservation_state.failed_payment.name" },
                    { 4, "reservation_state.confirmed.description", "confirmed", "reservation_state.confirmed.name" }
                });

            migrationBuilder.UpdateData(
                table: "c_transaction_state",
                keyColumn: "transaction_state_id",
                keyValue: 1,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "transaction_state.pending.description", "pending", "transaction_state.pending.name" });

            migrationBuilder.UpdateData(
                table: "c_transaction_state",
                keyColumn: "transaction_state_id",
                keyValue: 2,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "transaction_state.completed.description", "completed", "transaction_state.completed.name" });

            migrationBuilder.InsertData(
                table: "c_transaction_type",
                columns: new[] { "transaction_type_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "transaction_type.deposit.description", "deposit", "transaction_type.deposit.name" },
                    { 2, "transaction_type.lesson_package_payment.description", "lesson_package_payment", "transaction_type.lesson_package_payment.name" },
                    { 3, "transaction_type.package_cancellation.description", "package_cancellation", "transaction_type.package_cancellation.name" },
                    { 4, "transaction_type.withdrawal.description", "withdrawal", "transaction_type.withdrawal.name" },
                    { 5, "transaction_type.refund.description", "refund", "transaction_type.refund.name" }
                });

            migrationBuilder.InsertData(
                table: "env_configuration",
                columns: new[] { "configuration_id", "admin_configurable", "backend_only", "configuration_key", "configuration_value", "data_type_id", "value_origin" },
                values: new object[,]
                {
                    { 39, true, false, "be.stripe.settings.PublishableKey", "", 1, "init_scripts" },
                    { 40, true, true, "be.stripe.settings.SecretKey", "", 9, "init_scripts" },
                    { 41, true, true, "be.stripe.settings.SuccessUrl", "", 1, "init_scripts" },
                    { 42, true, true, "be.stripe.settings.CancelUrl", "", 1, "init_scripts" },
                    { 43, true, true, "be.stripe.settings.WebhookKey", "", 9, "init_scripts" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_app_transaction_reservation_package_id",
                table: "app_user_wallet_transaction",
                column: "reservation_package_id");

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_app_reservation_package_reservation_package_id",
                table: "app_user_wallet_transaction",
                column: "reservation_package_id",
                principalTable: "app_reservation_package",
                principalColumn: "reservation_package_id");

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

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_c_transaction_state_transaction_state_id",
                table: "app_user_wallet_transaction",
                column: "transaction_state_id",
                principalTable: "c_transaction_state",
                principalColumn: "transaction_state_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_c_transaction_type_transaction_type_id",
                table: "app_user_wallet_transaction",
                column: "transaction_type_id",
                principalTable: "c_transaction_type",
                principalColumn: "transaction_type_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_app_reservation_package_reservation_package_id",
                table: "app_user_wallet_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_app_user_waller_user_wallet_id",
                table: "app_user_wallet_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_c_payment_service_provider_payment_service_provider_id",
                table: "app_user_wallet_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_c_transaction_state_transaction_state_id",
                table: "app_user_wallet_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_c_transaction_type_transaction_type_id",
                table: "app_user_wallet_transaction");

            migrationBuilder.DropIndex(
                name: "ix_app_transaction_reservation_package_id",
                table: "app_user_wallet_transaction");

            migrationBuilder.DeleteData(
                table: "c_reservation_state",
                keyColumn: "reservation_state_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "c_reservation_state",
                keyColumn: "reservation_state_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "env_configuration",
                keyColumn: "configuration_id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "env_configuration",
                keyColumn: "configuration_id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "env_configuration",
                keyColumn: "configuration_id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "env_configuration",
                keyColumn: "configuration_id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "env_configuration",
                keyColumn: "configuration_id",
                keyValue: 43);

            migrationBuilder.DropColumn(
                name: "reservation_package_id",
                table: "app_user_wallet_transaction");

            migrationBuilder.DropColumn(
                name: "total_price",
                table: "app_reservation_package");

            migrationBuilder.RenameColumn(
                name: "transaction_id",
                table: "app_user_wallet_transaction",
                newName: "user_wallet_transaction_id");

            migrationBuilder.RenameIndex(
                name: "ix_app_transaction_user_wallet_id",
                table: "app_user_wallet_transaction",
                newName: "ix_app_user_wallet_transaction_user_wallet_id");

            migrationBuilder.RenameIndex(
                name: "ix_app_transaction_transaction_type_id",
                table: "app_user_wallet_transaction",
                newName: "ix_app_user_wallet_transaction_transaction_type_id");

            migrationBuilder.RenameIndex(
                name: "ix_app_transaction_transaction_state_id",
                table: "app_user_wallet_transaction",
                newName: "ix_app_user_wallet_transaction_transaction_state_id");

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 1,
                column: "hash",
                value: new Guid("987eb3d9-6def-4598-95fd-252429bc64c1"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 2,
                column: "hash",
                value: new Guid("039dc393-f954-4f0e-b152-7ecb5dcbde47"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 3,
                column: "hash",
                value: new Guid("8459ae77-7cf8-47e8-bf28-fb8bdffaceeb"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 4,
                column: "hash",
                value: new Guid("1028c462-14ac-46fb-827c-a6575ab436ee"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 5,
                column: "hash",
                value: new Guid("38992bd4-eb7a-4a90-9c72-b65985f3767c"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 6,
                column: "hash",
                value: new Guid("fe30aa78-18d7-4dff-8a63-24586d61d3cb"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 7,
                column: "hash",
                value: new Guid("bd23c7ff-8cc6-47ae-a6d5-2abf2901f379"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 8,
                column: "hash",
                value: new Guid("a24a064c-40fb-44c9-82e0-b56e226324ac"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 9,
                column: "hash",
                value: new Guid("9b676613-1d97-459f-957e-6940e8b474d9"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 10,
                column: "hash",
                value: new Guid("3b76ddd4-2919-4bdd-8356-48fa5a9efdd0"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 11,
                column: "hash",
                value: new Guid("c214b071-6f43-4c4e-83de-ccbdf817afbd"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 12,
                column: "hash",
                value: new Guid("08d2f3f8-f610-4616-af37-b84cf18e6b6f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 13,
                column: "hash",
                value: new Guid("50b44f74-11dd-4d21-b20b-b7e00d365957"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 14,
                column: "hash",
                value: new Guid("9d12e430-4c99-43bc-897a-0199c58686c0"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 15,
                column: "hash",
                value: new Guid("b78d15a6-6a08-49b0-b69b-ab6749f91ae4"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 16,
                column: "hash",
                value: new Guid("5b01d149-2d5e-4faa-b378-9bd11437dec3"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 17,
                column: "hash",
                value: new Guid("a2415463-1cf2-4ba3-83e1-4279930e00b5"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 18,
                column: "hash",
                value: new Guid("6f0812d6-2b94-4776-9b33-1b2693414ac0"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 19,
                column: "hash",
                value: new Guid("2b8e3b26-3b8b-4be7-841d-fa7830b6c1bc"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 20,
                column: "hash",
                value: new Guid("abd78044-04a0-4907-9892-f91983d04b6c"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 21,
                column: "hash",
                value: new Guid("0949e846-8b5e-4240-8d54-00887c4a52bd"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 22,
                column: "hash",
                value: new Guid("84e3a065-7658-4537-b564-1b923f1ea285"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 23,
                column: "hash",
                value: new Guid("b0580968-2b18-4813-9b33-304cf993cd76"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 24,
                column: "hash",
                value: new Guid("679ee4ba-17e0-49c3-86d7-cc2f566b3166"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 25,
                column: "hash",
                value: new Guid("282fd534-b10c-4b3d-b5be-6faddf898a10"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 26,
                column: "hash",
                value: new Guid("7d817bbf-91b3-41ff-bff8-caf638048b03"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 27,
                column: "hash",
                value: new Guid("f81be7bd-43ad-448a-9f7a-2070c39c3430"));

            migrationBuilder.UpdateData(
                table: "c_reservation_state",
                keyColumn: "reservation_state_id",
                keyValue: 2,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "reservation_state.deleted.description", "deleted", "reservation_state.deleted.name" });

            migrationBuilder.UpdateData(
                table: "c_transaction_state",
                keyColumn: "transaction_state_id",
                keyValue: 1,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "transaction_state.approved.description", "approved", "transaction_state.approved.name" });

            migrationBuilder.UpdateData(
                table: "c_transaction_state",
                keyColumn: "transaction_state_id",
                keyValue: 2,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "transaction_state.declined.description", "declined", "transaction_state.declined.name" });

            migrationBuilder.AddForeignKey(
                name: "fk_app_user_wallet_transaction_app_user_waller_user_wallet_id",
                table: "app_user_wallet_transaction",
                column: "user_wallet_id",
                principalTable: "app_user_wallet",
                principalColumn: "user_wallet_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_user_wallet_transaction_c_transaction_state",
                table: "app_user_wallet_transaction",
                column: "transaction_state_id",
                principalTable: "c_transaction_state",
                principalColumn: "transaction_state_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_user_wallet_transaction_c_transaction_type",
                table: "app_user_wallet_transaction",
                column: "transaction_type_id",
                principalTable: "c_transaction_type",
                principalColumn: "transaction_type_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_user_wallet_transaction_payment_service_provider",
                table: "app_user_wallet_transaction",
                column: "payment_service_provider_id",
                principalTable: "c_payment_service_provider",
                principalColumn: "payment_service_provider_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
