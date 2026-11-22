using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class RemoveOnDeleteCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_app_conversation_app_user_initiator_id",
                table: "app_conversation");

            migrationBuilder.DropForeignKey(
                name: "fk_app_conversation_app_user_opponent_id",
                table: "app_conversation");

            migrationBuilder.DropForeignKey(
                name: "fk_app_message_app_conversation_conversation_id",
                table: "app_message");

            migrationBuilder.DropForeignKey(
                name: "fk_app_message_app_user_receiver_id",
                table: "app_message");

            migrationBuilder.DropForeignKey(
                name: "fk_app_message_app_user_sender_id",
                table: "app_message");

            migrationBuilder.DropForeignKey(
                name: "fk_app_message_c_message_state_message_state_id",
                table: "app_message");

            migrationBuilder.DropForeignKey(
                name: "fk_app_message_attachment_app_message_message_id",
                table: "app_message_attachment");

            migrationBuilder.DropForeignKey(
                name: "fk_app_recurrent_timeslot_app_user_owner_id",
                table: "app_recurrent_timeslot");

            migrationBuilder.DropForeignKey(
                name: "fk_app_recurrent_timeslot_blacklist_app_recurrent_timeslot_recurrent_timeslot_id",
                table: "app_recurrent_timeslot_blacklist");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_app_reservation_package_reservation_package_id",
                table: "app_reservation");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_app_room_room_id",
                table: "app_reservation");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_app_user_attender_id",
                table: "app_reservation");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_app_user_host_id",
                table: "app_reservation");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_c_currency_curency_id",
                table: "app_reservation");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_c_reservation_state_reservation_state_id",
                table: "app_reservation");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_c_subject_subject_id",
                table: "app_reservation");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_issue_app_reservation_reservation_id",
                table: "app_reservation_issue");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_issue_app_user_creator_id",
                table: "app_reservation_issue");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_issue_c_reservation_issue_type_reservation_issue_type_id",
                table: "app_reservation_issue");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_package_app_user_attender_id",
                table: "app_reservation_package");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_package_app_user_host_id",
                table: "app_reservation_package");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_package_c_currency_currency_id",
                table: "app_reservation_package");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_package_c_reservation_package_state_reservation_package_state_id",
                table: "app_reservation_package");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_review_app_reservation_reservation_id",
                table: "app_reservation_review");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_review_app_user_creator_id",
                table: "app_reservation_review");

            migrationBuilder.DropForeignKey(
                name: "fk_app_room_app_attender_attender_id",
                table: "app_room");

            migrationBuilder.DropForeignKey(
                name: "fk_app_room_app_user_host_id",
                table: "app_room");

            migrationBuilder.DropForeignKey(
                name: "fk_app_room_c_room_state_room_state_id",
                table: "app_room");

            migrationBuilder.DropForeignKey(
                name: "fk_app_room_activity_app_room_room_id",
                table: "app_room_activity");

            migrationBuilder.DropForeignKey(
                name: "fk_app_room_activity_app_user_user_id",
                table: "app_room_activity");

            migrationBuilder.DropForeignKey(
                name: "fk_app_room_activity_c_room_activity_type_room_activity_type_id",
                table: "app_room_activity");

            migrationBuilder.DropForeignKey(
                name: "fk_app_user_event_app_user_owner_id",
                table: "app_timeslot");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_app_currency_currency_id",
                table: "app_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_app_user_creator_id",
                table: "app_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_app_user_wallet_user_wallet_id",
                table: "app_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_c_transaction_state_transaction_state_id",
                table: "app_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_c_transaction_type_transaction_type_id",
                table: "app_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_user_c_user_role_user_role_id",
                table: "app_user");

            migrationBuilder.DropForeignKey(
                name: "fk_app_user_c_user_state_user_state_id",
                table: "app_user");

            migrationBuilder.DropForeignKey(
                name: "fk_app_user_behaviour_app_user_user_id",
                table: "app_user_behaviour");

            migrationBuilder.DropForeignKey(
                name: "fk_app_user_registration_code_app_user_user_id",
                table: "app_user_registration_code");

            migrationBuilder.DropForeignKey(
                name: "fk_app_user_review_app_user_reviewee_id",
                table: "app_user_review");

            migrationBuilder.DropForeignKey(
                name: "fk_app_user_review_app_user_reviewer_id",
                table: "app_user_review");

            migrationBuilder.DropForeignKey(
                name: "fk_app_user_wallet_app_user_user_id",
                table: "app_user_wallet");

            migrationBuilder.DropForeignKey(
                name: "fk_app_user_wallet_c_currency_currency_id",
                table: "app_user_wallet");

            migrationBuilder.DropForeignKey(
                name: "fk_c_subject_c_category_category_id",
                table: "c_subject");

            migrationBuilder.DropForeignKey(
                name: "fk_c_subject_c_object_state_object_state_id",
                table: "c_subject");

            migrationBuilder.DropForeignKey(
                name: "fk_c_translation_app_user_creator_id",
                table: "c_translation");

            migrationBuilder.DropForeignKey(
                name: "fk_c_translation_c_translation_key_translation_id",
                table: "c_translation");

            migrationBuilder.DropForeignKey(
                name: "fk_c_translation_c_translation_language_translation_language_id",
                table: "c_translation");

            migrationBuilder.DropForeignKey(
                name: "fk_c_translation_key_app_user_creator_id",
                table: "c_translation_key");

            migrationBuilder.DropForeignKey(
                name: "fk_c_translation_key_c_translation_namespace_translation_namespace_id",
                table: "c_translation_key");

            migrationBuilder.DropForeignKey(
                name: "fk_c_translation_namespace_app_user_creator_id",
                table: "c_translation_namespace");

            migrationBuilder.DropForeignKey(
                name: "fk_env_configuration_env_configuration_data_type_data_type_id",
                table: "env_configuration");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_attribute_app_user_user_id",
                table: "r_user_attribute");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_attribute_c_user_attribute_type",
                table: "r_user_attribute");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_file_app_user_user_id",
                table: "r_user_file");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_file_c_file_type_file_type_id",
                table: "r_user_file");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_file_permission_app_user_user_id",
                table: "r_user_file_permission");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_interest_app_user_user_id",
                table: "r_user_interest");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_interest_c_interest_interest_id",
                table: "r_user_interest");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_language_app_user_user_id",
                table: "r_user_language");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_language_c_language_language_id",
                table: "r_user_language");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_language_c_language_level_language_level_id",
                table: "r_user_language");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_room_statistic_app_room_room_id",
                table: "r_user_room_statistic");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_room_statistic_app_user_user_id",
                table: "r_user_room_statistic");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_room_statistic_c_room_statistic_type_room_statistic_type_id",
                table: "r_user_room_statistic");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_specialization_app_user_user_id",
                table: "r_user_specialization");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_specialization_c_specialization_specialization_id",
                table: "r_user_specialization");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_subject_level_app_user_user_id",
                table: "r_user_subject_level");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_subject_level_c_subject_level_subject_level_id",
                table: "r_user_subject_level");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_token_app_user_user_id",
                table: "r_user_token");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_token_c_token_provider_token_provider_id",
                table: "r_user_token");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_token_c_token_type_token_type_id",
                table: "r_user_token");

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 1,
                column: "hash",
                value: new Guid("599eaa8f-c98c-48ad-b9d1-9af4b2c3ec83"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 2,
                column: "hash",
                value: new Guid("46a3a972-01cb-4f05-9955-17728c190fc0"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 3,
                column: "hash",
                value: new Guid("3223de30-f0f5-4491-b7f2-94e29d032ca6"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 4,
                column: "hash",
                value: new Guid("97d80073-90af-4738-995c-c874ed32d384"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 5,
                column: "hash",
                value: new Guid("937c3df0-1677-4176-a78e-d62e0432cb49"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 6,
                column: "hash",
                value: new Guid("2c0ae5d5-62f6-4659-a476-aa0a14a29387"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 7,
                column: "hash",
                value: new Guid("3a7e23cc-7e15-4f31-98cd-32e1a1f0d9b0"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 8,
                column: "hash",
                value: new Guid("b49949ae-68c3-49b6-a98f-1f1ff90edfb7"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 9,
                column: "hash",
                value: new Guid("5ac02ad2-7ceb-4ac6-aef0-a403ec3d72fe"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 10,
                column: "hash",
                value: new Guid("f01b2d99-def0-4ae2-b068-6fb11a5b3088"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 11,
                column: "hash",
                value: new Guid("625fdbc1-72de-4d4d-a1bf-77d26ccab9ac"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 12,
                column: "hash",
                value: new Guid("ed6c1970-44a6-4161-a125-6c83bf65f002"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 13,
                column: "hash",
                value: new Guid("acbd8ff4-82aa-444d-9836-1cb6e402ad19"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 14,
                column: "hash",
                value: new Guid("f6304cb3-6440-4e3a-a294-cc31ab9a3a64"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 15,
                column: "hash",
                value: new Guid("e49be09a-64a5-4f3d-980a-9a3aa111e24b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 16,
                column: "hash",
                value: new Guid("5c3c8111-51a9-4556-b35c-1c387442dd30"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 17,
                column: "hash",
                value: new Guid("8e5aae81-f39b-4c89-b8df-cef5c7f9845a"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 18,
                column: "hash",
                value: new Guid("0aeb3b01-3918-4570-b366-a54ffebe52f8"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 19,
                column: "hash",
                value: new Guid("223a9b71-df84-40bc-ab7d-24debe5e6891"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 20,
                column: "hash",
                value: new Guid("34df7df1-37af-40b9-ae2e-d1cad8f34071"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 21,
                column: "hash",
                value: new Guid("5f8add4a-5ae6-4195-a178-4b9b5e182521"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 22,
                column: "hash",
                value: new Guid("19571c34-5155-41ad-a962-be67d91ed381"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 23,
                column: "hash",
                value: new Guid("e2a5b5da-bcec-486f-b7ac-f65151ebddff"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 24,
                column: "hash",
                value: new Guid("15206f75-7b05-46f6-bbed-9abc87429704"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 25,
                column: "hash",
                value: new Guid("ca851b5d-c889-4ad0-ae41-23c689e258c4"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 26,
                column: "hash",
                value: new Guid("3fa503c4-bf35-4146-bfb9-434a0ec15d86"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 27,
                column: "hash",
                value: new Guid("01cd9530-2b56-444b-81cd-9c4dc97f2b9f"));

            migrationBuilder.AddForeignKey(
                name: "fk_app_conversation_app_user_initiator_id",
                table: "app_conversation",
                column: "initiator_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_conversation_app_user_opponent_id",
                table: "app_conversation",
                column: "opponent_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_message_app_conversation_conversation_id",
                table: "app_message",
                column: "conversation_id",
                principalTable: "app_conversation",
                principalColumn: "conversation_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_message_app_user_receiver_id",
                table: "app_message",
                column: "receiver_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_message_app_user_sender_id",
                table: "app_message",
                column: "sender_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_message_c_message_state_message_state_id",
                table: "app_message",
                column: "message_state_id",
                principalTable: "c_message_state",
                principalColumn: "message_state_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_message_attachment_app_message_message_id",
                table: "app_message_attachment",
                column: "message_id",
                principalTable: "app_message",
                principalColumn: "message_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_recurrent_timeslot_app_user_owner_id",
                table: "app_recurrent_timeslot",
                column: "owner_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_recurrent_timeslot_blacklist_app_recurrent_timeslot_recurrent_timeslot_id",
                table: "app_recurrent_timeslot_blacklist",
                column: "recurrent_timeslot_id",
                principalTable: "app_recurrent_timeslot",
                principalColumn: "recurrent_timeslot_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_app_reservation_package_reservation_package_id",
                table: "app_reservation",
                column: "reservation_package_id",
                principalTable: "app_reservation_package",
                principalColumn: "reservation_package_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_app_room_room_id",
                table: "app_reservation",
                column: "room_id",
                principalTable: "app_room",
                principalColumn: "room_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_app_user_attender_id",
                table: "app_reservation",
                column: "attender_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_app_user_host_id",
                table: "app_reservation",
                column: "host_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_c_currency_curency_id",
                table: "app_reservation",
                column: "currency_id",
                principalTable: "c_currency",
                principalColumn: "currency_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_c_reservation_state_reservation_state_id",
                table: "app_reservation",
                column: "reservation_state_id",
                principalTable: "c_reservation_state",
                principalColumn: "reservation_state_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_c_subject_subject_id",
                table: "app_reservation",
                column: "subject_id",
                principalTable: "c_subject",
                principalColumn: "subject_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_issue_app_reservation_reservation_id",
                table: "app_reservation_issue",
                column: "reservation_id",
                principalTable: "app_reservation",
                principalColumn: "reservation_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_issue_app_user_creator_id",
                table: "app_reservation_issue",
                column: "creator_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_issue_c_reservation_issue_type_reservation_issue_type_id",
                table: "app_reservation_issue",
                column: "reservation_issue_type_id",
                principalTable: "c_reservation_issue_type",
                principalColumn: "reservation_issue_type_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_package_app_user_attender_id",
                table: "app_reservation_package",
                column: "attender_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_package_app_user_host_id",
                table: "app_reservation_package",
                column: "host_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_package_c_currency_currency_id",
                table: "app_reservation_package",
                column: "currency_id",
                principalTable: "c_currency",
                principalColumn: "currency_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_package_c_reservation_package_state_reservation_package_state_id",
                table: "app_reservation_package",
                column: "reservation_package_state_id",
                principalTable: "c_reservation_package_state",
                principalColumn: "reservation_state_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_review_app_reservation_reservation_id",
                table: "app_reservation_review",
                column: "reservation_id",
                principalTable: "app_reservation",
                principalColumn: "reservation_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_review_app_user_creator_id",
                table: "app_reservation_review",
                column: "creator_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_room_app_attender_attender_id",
                table: "app_room",
                column: "attender_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_room_app_user_host_id",
                table: "app_room",
                column: "host_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_room_c_room_state_room_state_id",
                table: "app_room",
                column: "room_state_id",
                principalTable: "c_room_state",
                principalColumn: "room_state_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_room_activity_app_room_room_id",
                table: "app_room_activity",
                column: "room_id",
                principalTable: "app_room",
                principalColumn: "room_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_room_activity_app_user_user_id",
                table: "app_room_activity",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_room_activity_c_room_activity_type_room_activity_type_id",
                table: "app_room_activity",
                column: "room_activity_type_id",
                principalTable: "c_room_activity_type",
                principalColumn: "room_activity_type_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_user_event_app_user_owner_id",
                table: "app_timeslot",
                column: "owner_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_app_currency_currency_id",
                table: "app_transaction",
                column: "currency_id",
                principalTable: "c_currency",
                principalColumn: "currency_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_app_user_creator_id",
                table: "app_transaction",
                column: "creator_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_app_user_wallet_user_wallet_id",
                table: "app_transaction",
                column: "user_wallet_id",
                principalTable: "app_user_wallet",
                principalColumn: "user_wallet_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_c_transaction_state_transaction_state_id",
                table: "app_transaction",
                column: "transaction_state_id",
                principalTable: "c_transaction_state",
                principalColumn: "transaction_state_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_c_transaction_type_transaction_type_id",
                table: "app_transaction",
                column: "transaction_type_id",
                principalTable: "c_transaction_type",
                principalColumn: "transaction_type_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_user_c_user_role_user_role_id",
                table: "app_user",
                column: "user_role_id",
                principalTable: "c_user_role",
                principalColumn: "user_role_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_user_c_user_state_user_state_id",
                table: "app_user",
                column: "user_state_id",
                principalTable: "c_user_state",
                principalColumn: "user_state_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_user_behaviour_app_user_user_id",
                table: "app_user_behaviour",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_user_registration_code_app_user_user_id",
                table: "app_user_registration_code",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_user_review_app_user_reviewee_id",
                table: "app_user_review",
                column: "reviewee_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_user_review_app_user_reviewer_id",
                table: "app_user_review",
                column: "reviewer_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_user_wallet_app_user_user_id",
                table: "app_user_wallet",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_app_user_wallet_c_currency_currency_id",
                table: "app_user_wallet",
                column: "currency_id",
                principalTable: "c_currency",
                principalColumn: "currency_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_c_subject_c_category_category_id",
                table: "c_subject",
                column: "category_id",
                principalTable: "c_category",
                principalColumn: "category_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_c_subject_c_object_state_object_state_id",
                table: "c_subject",
                column: "object_state_id",
                principalTable: "c_object_state",
                principalColumn: "object_state_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_c_translation_app_user_creator_id",
                table: "c_translation",
                column: "creator_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_c_translation_c_translation_key_translation_id",
                table: "c_translation",
                column: "translation_key_id",
                principalTable: "c_translation_key",
                principalColumn: "key_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_c_translation_c_translation_language_translation_language_id",
                table: "c_translation",
                column: "translation_language_id",
                principalTable: "c_translation_language",
                principalColumn: "translation_language_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_c_translation_key_app_user_creator_id",
                table: "c_translation_key",
                column: "creator_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_c_translation_key_c_translation_namespace_translation_namespace_id",
                table: "c_translation_key",
                column: "translation_namespace_id",
                principalTable: "c_translation_namespace",
                principalColumn: "translation_namespace_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_c_translation_namespace_app_user_creator_id",
                table: "c_translation_namespace",
                column: "creator_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_env_configuration_env_configuration_data_type_data_type_id",
                table: "env_configuration",
                column: "data_type_id",
                principalTable: "env_configuration_data_type",
                principalColumn: "data_type_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_attribute_app_user_user_id",
                table: "r_user_attribute",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_attribute_c_user_attribute_type",
                table: "r_user_attribute",
                column: "user_attribute_type_id",
                principalTable: "c_user_attribute_type",
                principalColumn: "user_attribute_type_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_file_app_user_user_id",
                table: "r_user_file",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_file_c_file_type_file_type_id",
                table: "r_user_file",
                column: "file_type_id",
                principalTable: "c_file_type",
                principalColumn: "file_type_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_file_permission_app_user_user_id",
                table: "r_user_file_permission",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_interest_app_user_user_id",
                table: "r_user_interest",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_interest_c_interest_interest_id",
                table: "r_user_interest",
                column: "interest_id",
                principalTable: "c_interest",
                principalColumn: "interest_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_language_app_user_user_id",
                table: "r_user_language",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_language_c_language_language_id",
                table: "r_user_language",
                column: "language_id",
                principalTable: "c_language",
                principalColumn: "language_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_language_c_language_level_language_level_id",
                table: "r_user_language",
                column: "language_level_id",
                principalTable: "c_language_level",
                principalColumn: "language_level_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_room_statistic_app_room_room_id",
                table: "r_user_room_statistic",
                column: "room_id",
                principalTable: "app_room",
                principalColumn: "room_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_room_statistic_app_user_user_id",
                table: "r_user_room_statistic",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_room_statistic_c_room_statistic_type_room_statistic_type_id",
                table: "r_user_room_statistic",
                column: "room_statistics_type_id",
                principalTable: "c_room_statistic_type",
                principalColumn: "room_statistic_type_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_specialization_app_user_user_id",
                table: "r_user_specialization",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_specialization_c_specialization_specialization_id",
                table: "r_user_specialization",
                column: "specialization_id",
                principalTable: "c_specialization",
                principalColumn: "specialization_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_subject_level_app_user_user_id",
                table: "r_user_subject_level",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_subject_level_c_subject_level_subject_level_id",
                table: "r_user_subject_level",
                column: "subject_level_id",
                principalTable: "c_subject_level",
                principalColumn: "subject_level_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_token_app_user_user_id",
                table: "r_user_token",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_token_c_token_provider_token_provider_id",
                table: "r_user_token",
                column: "token_provider_id",
                principalTable: "c_token_provider",
                principalColumn: "token_provider_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_token_c_token_type_token_type_id",
                table: "r_user_token",
                column: "token_type_id",
                principalTable: "c_token_type",
                principalColumn: "token_type_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_app_conversation_app_user_initiator_id",
                table: "app_conversation");

            migrationBuilder.DropForeignKey(
                name: "fk_app_conversation_app_user_opponent_id",
                table: "app_conversation");

            migrationBuilder.DropForeignKey(
                name: "fk_app_message_app_conversation_conversation_id",
                table: "app_message");

            migrationBuilder.DropForeignKey(
                name: "fk_app_message_app_user_receiver_id",
                table: "app_message");

            migrationBuilder.DropForeignKey(
                name: "fk_app_message_app_user_sender_id",
                table: "app_message");

            migrationBuilder.DropForeignKey(
                name: "fk_app_message_c_message_state_message_state_id",
                table: "app_message");

            migrationBuilder.DropForeignKey(
                name: "fk_app_message_attachment_app_message_message_id",
                table: "app_message_attachment");

            migrationBuilder.DropForeignKey(
                name: "fk_app_recurrent_timeslot_app_user_owner_id",
                table: "app_recurrent_timeslot");

            migrationBuilder.DropForeignKey(
                name: "fk_app_recurrent_timeslot_blacklist_app_recurrent_timeslot_recurrent_timeslot_id",
                table: "app_recurrent_timeslot_blacklist");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_app_reservation_package_reservation_package_id",
                table: "app_reservation");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_app_room_room_id",
                table: "app_reservation");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_app_user_attender_id",
                table: "app_reservation");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_app_user_host_id",
                table: "app_reservation");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_c_currency_curency_id",
                table: "app_reservation");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_c_reservation_state_reservation_state_id",
                table: "app_reservation");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_c_subject_subject_id",
                table: "app_reservation");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_issue_app_reservation_reservation_id",
                table: "app_reservation_issue");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_issue_app_user_creator_id",
                table: "app_reservation_issue");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_issue_c_reservation_issue_type_reservation_issue_type_id",
                table: "app_reservation_issue");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_package_app_user_attender_id",
                table: "app_reservation_package");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_package_app_user_host_id",
                table: "app_reservation_package");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_package_c_currency_currency_id",
                table: "app_reservation_package");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_package_c_reservation_package_state_reservation_package_state_id",
                table: "app_reservation_package");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_review_app_reservation_reservation_id",
                table: "app_reservation_review");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_review_app_user_creator_id",
                table: "app_reservation_review");

            migrationBuilder.DropForeignKey(
                name: "fk_app_room_app_attender_attender_id",
                table: "app_room");

            migrationBuilder.DropForeignKey(
                name: "fk_app_room_app_user_host_id",
                table: "app_room");

            migrationBuilder.DropForeignKey(
                name: "fk_app_room_c_room_state_room_state_id",
                table: "app_room");

            migrationBuilder.DropForeignKey(
                name: "fk_app_room_activity_app_room_room_id",
                table: "app_room_activity");

            migrationBuilder.DropForeignKey(
                name: "fk_app_room_activity_app_user_user_id",
                table: "app_room_activity");

            migrationBuilder.DropForeignKey(
                name: "fk_app_room_activity_c_room_activity_type_room_activity_type_id",
                table: "app_room_activity");

            migrationBuilder.DropForeignKey(
                name: "fk_app_user_event_app_user_owner_id",
                table: "app_timeslot");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_app_currency_currency_id",
                table: "app_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_app_user_creator_id",
                table: "app_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_app_user_wallet_user_wallet_id",
                table: "app_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_c_transaction_state_transaction_state_id",
                table: "app_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_c_transaction_type_transaction_type_id",
                table: "app_transaction");

            migrationBuilder.DropForeignKey(
                name: "fk_app_user_c_user_role_user_role_id",
                table: "app_user");

            migrationBuilder.DropForeignKey(
                name: "fk_app_user_c_user_state_user_state_id",
                table: "app_user");

            migrationBuilder.DropForeignKey(
                name: "fk_app_user_behaviour_app_user_user_id",
                table: "app_user_behaviour");

            migrationBuilder.DropForeignKey(
                name: "fk_app_user_registration_code_app_user_user_id",
                table: "app_user_registration_code");

            migrationBuilder.DropForeignKey(
                name: "fk_app_user_review_app_user_reviewee_id",
                table: "app_user_review");

            migrationBuilder.DropForeignKey(
                name: "fk_app_user_review_app_user_reviewer_id",
                table: "app_user_review");

            migrationBuilder.DropForeignKey(
                name: "fk_app_user_wallet_app_user_user_id",
                table: "app_user_wallet");

            migrationBuilder.DropForeignKey(
                name: "fk_app_user_wallet_c_currency_currency_id",
                table: "app_user_wallet");

            migrationBuilder.DropForeignKey(
                name: "fk_c_subject_c_category_category_id",
                table: "c_subject");

            migrationBuilder.DropForeignKey(
                name: "fk_c_subject_c_object_state_object_state_id",
                table: "c_subject");

            migrationBuilder.DropForeignKey(
                name: "fk_c_translation_app_user_creator_id",
                table: "c_translation");

            migrationBuilder.DropForeignKey(
                name: "fk_c_translation_c_translation_key_translation_id",
                table: "c_translation");

            migrationBuilder.DropForeignKey(
                name: "fk_c_translation_c_translation_language_translation_language_id",
                table: "c_translation");

            migrationBuilder.DropForeignKey(
                name: "fk_c_translation_key_app_user_creator_id",
                table: "c_translation_key");

            migrationBuilder.DropForeignKey(
                name: "fk_c_translation_key_c_translation_namespace_translation_namespace_id",
                table: "c_translation_key");

            migrationBuilder.DropForeignKey(
                name: "fk_c_translation_namespace_app_user_creator_id",
                table: "c_translation_namespace");

            migrationBuilder.DropForeignKey(
                name: "fk_env_configuration_env_configuration_data_type_data_type_id",
                table: "env_configuration");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_attribute_app_user_user_id",
                table: "r_user_attribute");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_attribute_c_user_attribute_type",
                table: "r_user_attribute");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_file_app_user_user_id",
                table: "r_user_file");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_file_c_file_type_file_type_id",
                table: "r_user_file");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_file_permission_app_user_user_id",
                table: "r_user_file_permission");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_interest_app_user_user_id",
                table: "r_user_interest");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_interest_c_interest_interest_id",
                table: "r_user_interest");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_language_app_user_user_id",
                table: "r_user_language");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_language_c_language_language_id",
                table: "r_user_language");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_language_c_language_level_language_level_id",
                table: "r_user_language");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_room_statistic_app_room_room_id",
                table: "r_user_room_statistic");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_room_statistic_app_user_user_id",
                table: "r_user_room_statistic");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_room_statistic_c_room_statistic_type_room_statistic_type_id",
                table: "r_user_room_statistic");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_specialization_app_user_user_id",
                table: "r_user_specialization");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_specialization_c_specialization_specialization_id",
                table: "r_user_specialization");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_subject_level_app_user_user_id",
                table: "r_user_subject_level");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_subject_level_c_subject_level_subject_level_id",
                table: "r_user_subject_level");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_token_app_user_user_id",
                table: "r_user_token");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_token_c_token_provider_token_provider_id",
                table: "r_user_token");

            migrationBuilder.DropForeignKey(
                name: "fk_r_user_token_c_token_type_token_type_id",
                table: "r_user_token");

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 1,
                column: "hash",
                value: new Guid("22c958f0-1534-4f08-a363-a8774fd54dfc"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 2,
                column: "hash",
                value: new Guid("5ac926b0-1709-43b8-a983-ee06e1973655"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 3,
                column: "hash",
                value: new Guid("9aefc016-0bd5-4bfc-bfee-53c3dfe5a7a0"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 4,
                column: "hash",
                value: new Guid("1774e3d0-5c9a-40e9-b929-cc4a610df051"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 5,
                column: "hash",
                value: new Guid("0fcb9d26-0aa7-4b11-a80d-7bbf3bc4f41b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 6,
                column: "hash",
                value: new Guid("150d307b-da4b-4e18-96bb-ee551b5967d3"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 7,
                column: "hash",
                value: new Guid("9b25e086-8b63-4acc-b22d-fb1a18259576"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 8,
                column: "hash",
                value: new Guid("35c81782-c1db-4fcb-aa18-6495cbb4294a"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 9,
                column: "hash",
                value: new Guid("99f93522-462d-4f3f-9ddb-6a52dc611965"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 10,
                column: "hash",
                value: new Guid("3e1f2ef5-dd26-4660-8de3-b7494c22f5f3"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 11,
                column: "hash",
                value: new Guid("bb29f0b2-0147-432d-b0a4-ac36a8be8830"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 12,
                column: "hash",
                value: new Guid("5662256d-3056-406b-a280-b0c3c42f92be"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 13,
                column: "hash",
                value: new Guid("27f995e1-ec5b-4828-aeb9-35054830e5d8"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 14,
                column: "hash",
                value: new Guid("98ea9a43-85ae-4f2d-92f3-825e48abdb5d"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 15,
                column: "hash",
                value: new Guid("2b2bd80c-5d83-4225-b878-455b6e2f4249"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 16,
                column: "hash",
                value: new Guid("ad7606ba-1030-45da-aa40-4688522c58df"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 17,
                column: "hash",
                value: new Guid("d82fd47c-810b-4a0e-9cdb-4648678e0a91"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 18,
                column: "hash",
                value: new Guid("d58b9577-f36a-46c9-88a0-22ddfadfbf7b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 19,
                column: "hash",
                value: new Guid("494c4ad4-2837-4008-807c-a7a6090dd75f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 20,
                column: "hash",
                value: new Guid("93309466-e1b5-4f25-9fb3-dcc7f5589b4f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 21,
                column: "hash",
                value: new Guid("e2284a4b-58d0-4771-95b7-dedc1b206058"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 22,
                column: "hash",
                value: new Guid("63566cb4-625f-44f5-a813-6af659b88ad2"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 23,
                column: "hash",
                value: new Guid("421023e6-83b9-404d-af85-c19f5d95a606"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 24,
                column: "hash",
                value: new Guid("4fdaf55f-289c-4ba4-9b75-72c311b30661"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 25,
                column: "hash",
                value: new Guid("500fe2e1-d190-491d-9b71-1e9582d8cf04"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 26,
                column: "hash",
                value: new Guid("36b6c53f-59a4-4d85-b299-d3de284e2d6f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 27,
                column: "hash",
                value: new Guid("62804735-fec8-4e8b-996b-2856f0f38d8d"));

            migrationBuilder.AddForeignKey(
                name: "fk_app_conversation_app_user_initiator_id",
                table: "app_conversation",
                column: "initiator_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_conversation_app_user_opponent_id",
                table: "app_conversation",
                column: "opponent_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_message_app_conversation_conversation_id",
                table: "app_message",
                column: "conversation_id",
                principalTable: "app_conversation",
                principalColumn: "conversation_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_message_app_user_receiver_id",
                table: "app_message",
                column: "receiver_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_message_app_user_sender_id",
                table: "app_message",
                column: "sender_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_message_c_message_state_message_state_id",
                table: "app_message",
                column: "message_state_id",
                principalTable: "c_message_state",
                principalColumn: "message_state_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_message_attachment_app_message_message_id",
                table: "app_message_attachment",
                column: "message_id",
                principalTable: "app_message",
                principalColumn: "message_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_recurrent_timeslot_app_user_owner_id",
                table: "app_recurrent_timeslot",
                column: "owner_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_recurrent_timeslot_blacklist_app_recurrent_timeslot_recurrent_timeslot_id",
                table: "app_recurrent_timeslot_blacklist",
                column: "recurrent_timeslot_id",
                principalTable: "app_recurrent_timeslot",
                principalColumn: "recurrent_timeslot_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_app_reservation_package_reservation_package_id",
                table: "app_reservation",
                column: "reservation_package_id",
                principalTable: "app_reservation_package",
                principalColumn: "reservation_package_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_app_room_room_id",
                table: "app_reservation",
                column: "room_id",
                principalTable: "app_room",
                principalColumn: "room_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_app_user_attender_id",
                table: "app_reservation",
                column: "attender_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_app_user_host_id",
                table: "app_reservation",
                column: "host_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_c_currency_curency_id",
                table: "app_reservation",
                column: "currency_id",
                principalTable: "c_currency",
                principalColumn: "currency_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_c_reservation_state_reservation_state_id",
                table: "app_reservation",
                column: "reservation_state_id",
                principalTable: "c_reservation_state",
                principalColumn: "reservation_state_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_c_subject_subject_id",
                table: "app_reservation",
                column: "subject_id",
                principalTable: "c_subject",
                principalColumn: "subject_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_issue_app_reservation_reservation_id",
                table: "app_reservation_issue",
                column: "reservation_id",
                principalTable: "app_reservation",
                principalColumn: "reservation_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_issue_app_user_creator_id",
                table: "app_reservation_issue",
                column: "creator_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_issue_c_reservation_issue_type_reservation_issue_type_id",
                table: "app_reservation_issue",
                column: "reservation_issue_type_id",
                principalTable: "c_reservation_issue_type",
                principalColumn: "reservation_issue_type_id",
                onDelete: ReferentialAction.Cascade);

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
                name: "fk_app_reservation_package_c_currency_currency_id",
                table: "app_reservation_package",
                column: "currency_id",
                principalTable: "c_currency",
                principalColumn: "currency_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_package_c_reservation_package_state_reservation_package_state_id",
                table: "app_reservation_package",
                column: "reservation_package_state_id",
                principalTable: "c_reservation_package_state",
                principalColumn: "reservation_state_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_review_app_reservation_reservation_id",
                table: "app_reservation_review",
                column: "reservation_id",
                principalTable: "app_reservation",
                principalColumn: "reservation_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_review_app_user_creator_id",
                table: "app_reservation_review",
                column: "creator_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_room_app_attender_attender_id",
                table: "app_room",
                column: "attender_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_room_app_user_host_id",
                table: "app_room",
                column: "host_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_room_c_room_state_room_state_id",
                table: "app_room",
                column: "room_state_id",
                principalTable: "c_room_state",
                principalColumn: "room_state_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_room_activity_app_room_room_id",
                table: "app_room_activity",
                column: "room_id",
                principalTable: "app_room",
                principalColumn: "room_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_room_activity_app_user_user_id",
                table: "app_room_activity",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_room_activity_c_room_activity_type_room_activity_type_id",
                table: "app_room_activity",
                column: "room_activity_type_id",
                principalTable: "c_room_activity_type",
                principalColumn: "room_activity_type_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_user_event_app_user_owner_id",
                table: "app_timeslot",
                column: "owner_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_app_currency_currency_id",
                table: "app_transaction",
                column: "currency_id",
                principalTable: "c_currency",
                principalColumn: "currency_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_app_user_creator_id",
                table: "app_transaction",
                column: "creator_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_app_user_wallet_user_wallet_id",
                table: "app_transaction",
                column: "user_wallet_id",
                principalTable: "app_user_wallet",
                principalColumn: "user_wallet_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_c_transaction_state_transaction_state_id",
                table: "app_transaction",
                column: "transaction_state_id",
                principalTable: "c_transaction_state",
                principalColumn: "transaction_state_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_c_transaction_type_transaction_type_id",
                table: "app_transaction",
                column: "transaction_type_id",
                principalTable: "c_transaction_type",
                principalColumn: "transaction_type_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_user_c_user_role_user_role_id",
                table: "app_user",
                column: "user_role_id",
                principalTable: "c_user_role",
                principalColumn: "user_role_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_user_c_user_state_user_state_id",
                table: "app_user",
                column: "user_state_id",
                principalTable: "c_user_state",
                principalColumn: "user_state_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_user_behaviour_app_user_user_id",
                table: "app_user_behaviour",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_user_registration_code_app_user_user_id",
                table: "app_user_registration_code",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_user_review_app_user_reviewee_id",
                table: "app_user_review",
                column: "reviewee_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_user_review_app_user_reviewer_id",
                table: "app_user_review",
                column: "reviewer_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_user_wallet_app_user_user_id",
                table: "app_user_wallet",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_user_wallet_c_currency_currency_id",
                table: "app_user_wallet",
                column: "currency_id",
                principalTable: "c_currency",
                principalColumn: "currency_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_c_subject_c_category_category_id",
                table: "c_subject",
                column: "category_id",
                principalTable: "c_category",
                principalColumn: "category_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_c_subject_c_object_state_object_state_id",
                table: "c_subject",
                column: "object_state_id",
                principalTable: "c_object_state",
                principalColumn: "object_state_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_c_translation_app_user_creator_id",
                table: "c_translation",
                column: "creator_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_c_translation_c_translation_key_translation_id",
                table: "c_translation",
                column: "translation_key_id",
                principalTable: "c_translation_key",
                principalColumn: "key_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_c_translation_c_translation_language_translation_language_id",
                table: "c_translation",
                column: "translation_language_id",
                principalTable: "c_translation_language",
                principalColumn: "translation_language_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_c_translation_key_app_user_creator_id",
                table: "c_translation_key",
                column: "creator_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_c_translation_key_c_translation_namespace_translation_namespace_id",
                table: "c_translation_key",
                column: "translation_namespace_id",
                principalTable: "c_translation_namespace",
                principalColumn: "translation_namespace_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_c_translation_namespace_app_user_creator_id",
                table: "c_translation_namespace",
                column: "creator_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_env_configuration_env_configuration_data_type_data_type_id",
                table: "env_configuration",
                column: "data_type_id",
                principalTable: "env_configuration_data_type",
                principalColumn: "data_type_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_attribute_app_user_user_id",
                table: "r_user_attribute",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_attribute_c_user_attribute_type",
                table: "r_user_attribute",
                column: "user_attribute_type_id",
                principalTable: "c_user_attribute_type",
                principalColumn: "user_attribute_type_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_file_app_user_user_id",
                table: "r_user_file",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_file_c_file_type_file_type_id",
                table: "r_user_file",
                column: "file_type_id",
                principalTable: "c_file_type",
                principalColumn: "file_type_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_file_permission_app_user_user_id",
                table: "r_user_file_permission",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_interest_app_user_user_id",
                table: "r_user_interest",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_interest_c_interest_interest_id",
                table: "r_user_interest",
                column: "interest_id",
                principalTable: "c_interest",
                principalColumn: "interest_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_language_app_user_user_id",
                table: "r_user_language",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_language_c_language_language_id",
                table: "r_user_language",
                column: "language_id",
                principalTable: "c_language",
                principalColumn: "language_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_language_c_language_level_language_level_id",
                table: "r_user_language",
                column: "language_level_id",
                principalTable: "c_language_level",
                principalColumn: "language_level_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_room_statistic_app_room_room_id",
                table: "r_user_room_statistic",
                column: "room_id",
                principalTable: "app_room",
                principalColumn: "room_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_room_statistic_app_user_user_id",
                table: "r_user_room_statistic",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_room_statistic_c_room_statistic_type_room_statistic_type_id",
                table: "r_user_room_statistic",
                column: "room_statistics_type_id",
                principalTable: "c_room_statistic_type",
                principalColumn: "room_statistic_type_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_specialization_app_user_user_id",
                table: "r_user_specialization",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_specialization_c_specialization_specialization_id",
                table: "r_user_specialization",
                column: "specialization_id",
                principalTable: "c_specialization",
                principalColumn: "specialization_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_subject_level_app_user_user_id",
                table: "r_user_subject_level",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_subject_level_c_subject_level_subject_level_id",
                table: "r_user_subject_level",
                column: "subject_level_id",
                principalTable: "c_subject_level",
                principalColumn: "subject_level_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_token_app_user_user_id",
                table: "r_user_token",
                column: "user_id",
                principalTable: "app_user",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_token_c_token_provider_token_provider_id",
                table: "r_user_token",
                column: "token_provider_id",
                principalTable: "c_token_provider",
                principalColumn: "token_provider_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_r_user_token_c_token_type_token_type_id",
                table: "r_user_token",
                column: "token_type_id",
                principalTable: "c_token_type",
                principalColumn: "token_type_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
