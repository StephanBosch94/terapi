using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class AppTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "creator_id",
                table: "app_user_wallet_transaction",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "reservation_issue_id",
                table: "app_user_wallet_transaction",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "session_id",
                table: "app_user_wallet_transaction",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated",
                table: "app_user_wallet_transaction",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "updater_id",
                table: "app_user_wallet_transaction",
                type: "integer",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "uc_c_translation_key_key_name",
                table: "c_translation_key",
                column: "key_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_app_transaction_creator_id",
                table: "app_user_wallet_transaction",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_transaction_reservation_reservation_issue_id",
                table: "app_user_wallet_transaction",
                column: "reservation_issue_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_transaction_updater_id",
                table: "app_user_wallet_transaction",
                column: "updater_id");

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_app_reservation_issue_reservation_issue_id",
                table: "app_user_wallet_transaction",
                column: "reservation_issue_id",
                principalTable: "app_reservation_issue",
                principalColumn: "reservation_issue_id");

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_app_user_creator_id",
                table: "app_user_wallet_transaction",
                column: "creator_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_app_user_updater_id",
                table: "app_user_wallet_transaction",
                column: "updater_id",
                principalTable: "app_user",
                principalColumn: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_app_reservation_issue_reservation_issue_id",
                table: "app_user_wallet_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_app_user_creator_id",
                table: "app_user_wallet_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_app_user_updater_id",
                table: "app_user_wallet_transaction");

            migrationBuilder.DropIndex(
                name: "uc_c_translation_key_key_name",
                table: "c_translation_key");

            migrationBuilder.DropIndex(
                name: "ix_app_transaction_creator_id",
                table: "app_user_wallet_transaction");

            migrationBuilder.DropIndex(
                name: "ix_app_transaction_reservation_reservation_issue_id",
                table: "app_user_wallet_transaction");

            migrationBuilder.DropIndex(
                name: "ix_app_transaction_updater_id",
                table: "app_user_wallet_transaction");

            migrationBuilder.DropColumn(
                name: "creator_id",
                table: "app_user_wallet_transaction");

            migrationBuilder.DropColumn(
                name: "reservation_issue_id",
                table: "app_user_wallet_transaction");

            migrationBuilder.DropColumn(
                name: "session_id",
                table: "app_user_wallet_transaction");

            migrationBuilder.DropColumn(
                name: "updated",
                table: "app_user_wallet_transaction");

            migrationBuilder.DropColumn(
                name: "updater_id",
                table: "app_user_wallet_transaction");

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 1,
                column: "hash",
                value: new Guid("490f8010-e5fa-46ba-bc12-d3212fc8cf77"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 2,
                column: "hash",
                value: new Guid("8463b1ef-13a1-47f2-8875-5c0aefbb1635"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 3,
                column: "hash",
                value: new Guid("2a9d0ff6-9a47-4737-bcd2-2ab87459222f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 4,
                column: "hash",
                value: new Guid("dc2fec26-b3df-45c0-aa5c-66daf5b3d5a0"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 5,
                column: "hash",
                value: new Guid("bd062ff2-a7da-4501-b2aa-e51a55f0e660"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 6,
                column: "hash",
                value: new Guid("c28e1b78-b0b2-4cbb-8319-12cf6f5079be"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 7,
                column: "hash",
                value: new Guid("112d0265-115f-40e1-a4ca-83c92b7778db"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 8,
                column: "hash",
                value: new Guid("3009b0de-678e-4124-89e6-9916292bf2a5"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 9,
                column: "hash",
                value: new Guid("5e11bc51-9fe3-45ef-9237-0a3404818695"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 10,
                column: "hash",
                value: new Guid("5d3123c6-84f0-4b14-9a2a-91d8ad03e15d"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 11,
                column: "hash",
                value: new Guid("8cd93c6d-ce4e-4f6b-813d-bdff3757cd83"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 12,
                column: "hash",
                value: new Guid("db56971a-a04d-47f8-91d5-f84df007a95b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 13,
                column: "hash",
                value: new Guid("086c96a0-7edd-44e7-90a9-c8b78f7d7782"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 14,
                column: "hash",
                value: new Guid("76910300-8042-4b00-90af-d5b5699b82a5"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 15,
                column: "hash",
                value: new Guid("86c10dc0-b012-4670-868c-19e2d06c658c"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 16,
                column: "hash",
                value: new Guid("272be0f4-0530-414e-8131-1f0821b4e8ea"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 17,
                column: "hash",
                value: new Guid("d4ca7f23-54c6-4e88-935d-1978f0166454"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 18,
                column: "hash",
                value: new Guid("a7c23a88-3ecd-477c-98ec-2f5bbd1bb98a"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 19,
                column: "hash",
                value: new Guid("912de632-6213-4320-b241-833fed06e7b9"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 20,
                column: "hash",
                value: new Guid("84834312-e6f2-47b1-a8ed-b307b35a2e16"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 21,
                column: "hash",
                value: new Guid("49938e54-178c-456d-85d3-53413aae15bb"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 22,
                column: "hash",
                value: new Guid("15266172-74a3-4c3e-93f8-5c74992322b8"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 23,
                column: "hash",
                value: new Guid("2fb4e22b-1eb7-4527-859b-4f353e25e611"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 24,
                column: "hash",
                value: new Guid("97679d80-0365-4626-ad24-15a24528bb70"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 25,
                column: "hash",
                value: new Guid("5b2d491f-4e1f-4780-9ae0-dc146383b34e"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 26,
                column: "hash",
                value: new Guid("9234b5c3-e5f8-4669-a18c-10cc7fa9145f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 27,
                column: "hash",
                value: new Guid("7f2951a3-6ba5-43f4-8475-2ab97f500a6d"));
        }
    }
}
