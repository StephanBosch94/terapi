using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class SubjectUserGoalKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "user_goal_title_key",
                table: "c_subject",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 1,
                column: "hash",
                value: new Guid("f49795e2-ad7b-4c90-9cd4-01e2fe833dba"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 2,
                column: "hash",
                value: new Guid("cc7a22ad-d02c-4724-88f6-93399a38d7ba"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 3,
                column: "hash",
                value: new Guid("8cf61b5a-d271-4f21-8d7d-6e234f630205"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 4,
                column: "hash",
                value: new Guid("a31faa02-4578-4a77-862e-9fa053841795"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 5,
                column: "hash",
                value: new Guid("c59d3167-0ae7-4c20-9338-78d83284a80b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 6,
                column: "hash",
                value: new Guid("4005f85d-7a35-4804-a6cd-9e4635206d3b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 7,
                column: "hash",
                value: new Guid("93424336-c996-4cfb-9bfa-6d4f63412be2"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 8,
                column: "hash",
                value: new Guid("db08c1f3-bd0e-4894-b156-70d042bf6024"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 9,
                column: "hash",
                value: new Guid("c8d13c01-725f-41bd-8961-41cc14525c09"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 10,
                column: "hash",
                value: new Guid("9b5399de-c222-49be-b076-2dea19a397a4"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 11,
                column: "hash",
                value: new Guid("e0db13ea-9b6a-44ab-9e99-76a6b93ad65b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 12,
                column: "hash",
                value: new Guid("6bc185f9-5d3b-4a73-afb4-7313eba94757"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 13,
                column: "hash",
                value: new Guid("b2a5cc35-15b7-4ec8-8cce-0cdaa3933cf1"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 14,
                column: "hash",
                value: new Guid("5b27327a-9e24-4334-8447-4b016a682c15"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 15,
                column: "hash",
                value: new Guid("76bec1e7-8a3c-4835-846b-ba49e48832fc"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 16,
                column: "hash",
                value: new Guid("dfb798b4-e858-4873-ad8f-8f7dc0109ab3"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 17,
                column: "hash",
                value: new Guid("c2142b7c-b060-4799-858a-3501c7f7df8f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 18,
                column: "hash",
                value: new Guid("72fb8675-7a34-4c07-aab1-8f538e93a728"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 19,
                column: "hash",
                value: new Guid("ee5e8975-7858-4dca-b1ea-49dbafbc6410"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 20,
                column: "hash",
                value: new Guid("c8767962-2108-49c0-94a7-238cdf8aafe1"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 21,
                column: "hash",
                value: new Guid("2da6987e-1a19-4b47-a865-feb9219fbdf8"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 22,
                column: "hash",
                value: new Guid("7cec02f5-c3c4-45d2-8c59-77d8369ef2c5"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 23,
                column: "hash",
                value: new Guid("8b21993e-a6ea-4568-9084-47df9913fee8"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 24,
                column: "hash",
                value: new Guid("15bc9817-bbf5-4191-9156-e7566e5a7491"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 25,
                column: "hash",
                value: new Guid("10637f24-ee4f-4849-aef8-c8e7f206850e"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 26,
                column: "hash",
                value: new Guid("12a63949-95b3-416a-b330-720d6e43fb85"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 27,
                column: "hash",
                value: new Guid("052b0fc1-d82e-427b-b482-593f26acb4d6"));

            migrationBuilder.UpdateData(
                table: "c_subject",
                keyColumn: "subject_id",
                keyValue: 1,
                column: "user_goal_title_key",
                value: "subject.english.user_goal.title");

            migrationBuilder.UpdateData(
                table: "c_subject",
                keyColumn: "subject_id",
                keyValue: 2,
                column: "user_goal_title_key",
                value: "subject.czech.user_goal.title");

            migrationBuilder.UpdateData(
                table: "c_subject",
                keyColumn: "subject_id",
                keyValue: 3,
                column: "user_goal_title_key",
                value: "subject.french.user_goal.title");

            migrationBuilder.UpdateData(
                table: "c_subject",
                keyColumn: "subject_id",
                keyValue: 4,
                column: "user_goal_title_key",
                value: "subject.spanish.user_goal.title");

            migrationBuilder.UpdateData(
                table: "c_subject",
                keyColumn: "subject_id",
                keyValue: 5,
                column: "user_goal_title_key",
                value: "subject.german.user_goal.title");

            migrationBuilder.UpdateData(
                table: "c_subject",
                keyColumn: "subject_id",
                keyValue: 6,
                column: "user_goal_title_key",
                value: "subject.italian.user_goal.title");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user_goal_title_key",
                table: "c_subject");

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 1,
                column: "hash",
                value: new Guid("a8483b52-82e2-4843-9416-bb316a1718e7"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 2,
                column: "hash",
                value: new Guid("87e13176-052c-4a00-b5d4-d3432f832bf4"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 3,
                column: "hash",
                value: new Guid("f95da348-7e76-4f8f-8f66-d3f7fffa9178"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 4,
                column: "hash",
                value: new Guid("be0a8204-c3f8-416a-9348-4b875c006c21"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 5,
                column: "hash",
                value: new Guid("ee8aaef3-136f-41d3-9fd9-e60e8fe69ed5"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 6,
                column: "hash",
                value: new Guid("d1c7e68c-bae8-47c5-814d-d20c9df5a1f0"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 7,
                column: "hash",
                value: new Guid("7d71c78f-dbc1-476e-a14c-82a71a18e28f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 8,
                column: "hash",
                value: new Guid("6c2e0835-baa6-4c0d-a04a-7fe49803af3a"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 9,
                column: "hash",
                value: new Guid("90066180-3efc-414b-b3ab-a4c76358ed2b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 10,
                column: "hash",
                value: new Guid("e9f55644-6e99-4858-b8b6-8342d249d42b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 11,
                column: "hash",
                value: new Guid("61cc7864-05ab-4ac6-8822-330c25ab279b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 12,
                column: "hash",
                value: new Guid("d5b7eba1-2274-45cf-9aa5-3ca7198bd4a4"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 13,
                column: "hash",
                value: new Guid("151a5797-8f94-4d73-be82-8ec79fc845f2"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 14,
                column: "hash",
                value: new Guid("e16e8b3f-251a-4799-acbf-07cc2bae69fc"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 15,
                column: "hash",
                value: new Guid("04b5c9e6-30da-4222-8cb5-ba6203d09d97"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 16,
                column: "hash",
                value: new Guid("eec6ed77-28be-4c67-ae97-383d068c2601"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 17,
                column: "hash",
                value: new Guid("02ac488f-2655-463a-9e14-669fee336f44"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 18,
                column: "hash",
                value: new Guid("1b63247f-aadc-4acd-b4b8-777a312d6e31"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 19,
                column: "hash",
                value: new Guid("d81ba58e-9dc6-47b9-8477-59d8418424f7"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 20,
                column: "hash",
                value: new Guid("c2089b74-3205-426e-a74c-feb8f49c4883"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 21,
                column: "hash",
                value: new Guid("078e2dec-ad2d-4fd4-9bce-88f0d8649e6a"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 22,
                column: "hash",
                value: new Guid("657080ca-ead2-497c-8e30-487726bee11f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 23,
                column: "hash",
                value: new Guid("0573a2be-8c60-4dea-a054-d43d02a806b6"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 24,
                column: "hash",
                value: new Guid("c644b3dd-2d94-4ed0-b147-717999b58581"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 25,
                column: "hash",
                value: new Guid("3b56f993-8871-4b0d-af02-34ce970e61a6"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 26,
                column: "hash",
                value: new Guid("4b5b8030-8170-470a-8c6b-63cb83f3c4b0"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 27,
                column: "hash",
                value: new Guid("120ecb72-024d-46ef-ad3c-bf30e947a2fc"));
        }
    }
}
