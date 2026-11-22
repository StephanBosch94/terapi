using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class UpdateReservationIssueTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "c_reservation_issue_type",
                keyColumn: "reservation_issue_type_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "c_reservation_issue_type",
                keyColumn: "reservation_issue_type_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "c_reservation_issue_type",
                keyColumn: "reservation_issue_type_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "c_reservation_issue_type",
                keyColumn: "reservation_issue_type_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "c_reservation_issue_type",
                keyColumn: "reservation_issue_type_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "c_reservation_issue_type",
                keyColumn: "reservation_issue_type_id",
                keyValue: 6);

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

                migrationBuilder.InsertData(
                table: "c_reservation_issue_type",
                columns: new[] { "reservation_issue_type_id", "description_key", "enum_key", "name_key", "has_description" },
                values: new object[,]
                {
                    { 1, "reservation_issue_type.technical_problems.description", "technical_problems", "reservation_issue_type.technical_problems.name", true },
                    { 2, "reservation_issue_type.tutor_was_absent.description", "tutor_was_absent", "reservation_issue_type.tutor_was_absent.name", true },
                    { 3, "reservation_issue_type.other.description", "other", "reservation_issue_type.other.name", true }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 1,
                column: "hash",
                value: new Guid("6af2e2ab-bf56-4815-bfa1-20587283414b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 2,
                column: "hash",
                value: new Guid("fc11fe63-4cfb-479a-8f0f-f30595c61e0a"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 3,
                column: "hash",
                value: new Guid("de20bf7d-2cc3-4b0a-9030-0b7f917d9867"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 4,
                column: "hash",
                value: new Guid("a8bab58a-3c56-4da2-ac52-d926095fc653"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 5,
                column: "hash",
                value: new Guid("5f6d8d68-609b-4136-ab47-a01b2634597e"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 6,
                column: "hash",
                value: new Guid("664617b6-e0db-4001-bceb-26303a93db48"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 7,
                column: "hash",
                value: new Guid("a338870e-f363-422c-af26-0c396446c9f1"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 8,
                column: "hash",
                value: new Guid("72b399df-c1ac-4954-ad60-96fcf46ce99c"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 9,
                column: "hash",
                value: new Guid("bc8a7243-1851-479a-9185-dd9729b99839"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 10,
                column: "hash",
                value: new Guid("ae7e50a3-9acb-4c70-9833-7e025b179022"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 11,
                column: "hash",
                value: new Guid("0dc65804-f897-480a-81da-081150c85368"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 12,
                column: "hash",
                value: new Guid("868a3ab3-64ca-468e-ade5-fd62cfcccf0f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 13,
                column: "hash",
                value: new Guid("54c7375f-33a8-40c8-8a2d-fb9317502a31"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 14,
                column: "hash",
                value: new Guid("4e007a64-3fc4-42dd-aca3-b42cc559cf38"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 15,
                column: "hash",
                value: new Guid("f45ce3e9-2e48-49f3-b674-17db67956751"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 16,
                column: "hash",
                value: new Guid("8b07343d-1b0a-4c4d-8c3e-ebacb381f107"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 17,
                column: "hash",
                value: new Guid("c768d5f4-dbd3-4da1-a65d-c3d4495df919"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 18,
                column: "hash",
                value: new Guid("f4c7041f-0407-43ea-91ea-74e4258f31fc"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 19,
                column: "hash",
                value: new Guid("858df5e4-bcc9-4f90-9cb0-0a91468f2cc6"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 20,
                column: "hash",
                value: new Guid("84646ea7-de3c-4632-9775-3b4e69ed9794"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 21,
                column: "hash",
                value: new Guid("ba8f3be9-1cfd-4bc4-969c-f533e434c634"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 22,
                column: "hash",
                value: new Guid("e5e64e5b-66aa-4de9-b9a7-6f44e8d26bea"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 23,
                column: "hash",
                value: new Guid("4e242c28-9a8c-42bc-b14b-9235b2aa58a9"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 24,
                column: "hash",
                value: new Guid("a2a66b6e-40b8-42a1-8a50-154ea2d0c2b1"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 25,
                column: "hash",
                value: new Guid("0f26a732-037b-4e60-9207-7750fa9af7fd"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 26,
                column: "hash",
                value: new Guid("504b89cf-9d80-42ec-ae33-b2c531d4c2e0"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 27,
                column: "hash",
                value: new Guid("75d72fd9-681f-449c-b960-f149b157387f"));

            migrationBuilder.UpdateData(
                table: "c_reservation_issue_type",
                keyColumn: "reservation_issue_type_id",
                keyValue: 1,
                columns: new[] { "description_key", "enum_key", "has_description", "name_key" },
                values: new object[] { "reservation_issue_type.agreed_to_reschedule.description", "agreed_to_reschedule", false, "reservation_issue_type.agreed_to_reschedule.name" });

            migrationBuilder.UpdateData(
                table: "c_reservation_issue_type",
                keyColumn: "reservation_issue_type_id",
                keyValue: 2,
                columns: new[] { "description_key", "enum_key", "has_description", "name_key" },
                values: new object[] { "reservation_issue_type.technical_problems.description", "technical_problems", false, "reservation_issue_type.technical_problems.name" });

            migrationBuilder.UpdateData(
                table: "c_reservation_issue_type",
                keyColumn: "reservation_issue_type_id",
                keyValue: 3,
                columns: new[] { "description_key", "enum_key", "has_description", "name_key" },
                values: new object[] { "reservation_issue_type.absence.description", "absence", false, "reservation_issue_type.absence.name" });

            migrationBuilder.InsertData(
                table: "c_reservation_issue_type",
                columns: new[] { "reservation_issue_type_id", "description_key", "enum_key", "has_description", "name_key" },
                values: new object[,]
                {
                    { 4, "reservation_issue_type.tutor_did_not_meet_expectations.description", "tutor_did_not_meet_expectations", false, "reservation_issue_type.tutor_did_not_meet_expectations.name" },
                    { 5, "reservation_issue_type.tutor_was_absent.description", "tutor_was_absent", false, "reservation_issue_type.tutor_was_absent.name" },
                    { 6, "reservation_issue_type.other.description", "other", true, "reservation_issue_type.other.name" }
                });
        }
    }
}
