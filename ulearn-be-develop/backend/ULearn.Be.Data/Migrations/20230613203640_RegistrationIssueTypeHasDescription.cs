using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class RegistrationIssueTypeHasDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "reservation_state_id",
                table: "c_reservation_issue_type",
                newName: "reservation_issue_type_id");

            migrationBuilder.AddColumn<bool>(
                name: "has_description",
                table: "c_reservation_issue_type",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 1,
                column: "hash",
                value: new Guid("5119a5b7-1b19-4dad-806e-71b2deec4312"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 2,
                column: "hash",
                value: new Guid("eff4ecf2-2a07-47ad-b02d-e7d8b3d02a55"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 3,
                column: "hash",
                value: new Guid("1737a3e7-eb37-49f6-bd0b-24ca3a56f3aa"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 4,
                column: "hash",
                value: new Guid("410f4bce-6d7e-41b0-9857-8f89b23092ff"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 5,
                column: "hash",
                value: new Guid("8e04839b-7c9b-41e8-b46d-6a097ca69ca2"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 6,
                column: "hash",
                value: new Guid("9006d9fe-a1de-4ec7-84ce-c3e31cceefd3"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 7,
                column: "hash",
                value: new Guid("d4676cfa-a989-4848-9130-d267b6a87019"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 8,
                column: "hash",
                value: new Guid("a3018474-6c7b-42db-84a3-bfd488697fb1"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 9,
                column: "hash",
                value: new Guid("2287c8c0-b194-43cd-89ac-985a9aaaa7cc"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 10,
                column: "hash",
                value: new Guid("aacace95-9d25-4928-815f-a52281b10a33"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 11,
                column: "hash",
                value: new Guid("eb213ba2-a6f5-4613-a5b9-fd3a645c43c4"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 12,
                column: "hash",
                value: new Guid("99587ec6-afbe-447a-ae4a-ddb1f76916fb"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 13,
                column: "hash",
                value: new Guid("dae5047d-204b-4da2-84f9-0067411cd2a9"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 14,
                column: "hash",
                value: new Guid("48a010bd-3bff-49f7-ac46-fb3c5178e322"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 15,
                column: "hash",
                value: new Guid("b6ec48ee-b245-4d43-9e1b-37f52b8a8dec"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 16,
                column: "hash",
                value: new Guid("66bd4208-cd54-4a75-b3f8-44a8ce518183"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 17,
                column: "hash",
                value: new Guid("812181d0-5e37-4b29-adb0-c216236666cd"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 18,
                column: "hash",
                value: new Guid("007863b8-5e88-4d55-88a1-aad23e802b92"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 19,
                column: "hash",
                value: new Guid("ded806cc-9988-4ca4-96e1-f829d5dc8b7a"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 20,
                column: "hash",
                value: new Guid("859b57a3-b124-4ca6-9f76-87dedb678ecc"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 21,
                column: "hash",
                value: new Guid("81df0462-94a3-46a5-b26b-352c7ae6f639"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 22,
                column: "hash",
                value: new Guid("7810ca73-2e55-4818-b003-777dec3f623d"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 23,
                column: "hash",
                value: new Guid("e6417258-3588-4e68-9e20-59e7035db2b4"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 24,
                column: "hash",
                value: new Guid("62a751f4-8fdc-42ea-b85f-12709ed137e0"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 25,
                column: "hash",
                value: new Guid("161ba969-800d-4fa2-99f6-302746a3a2b9"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 26,
                column: "hash",
                value: new Guid("0773bd56-db6d-4689-81de-fb9e31b74a35"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 27,
                column: "hash",
                value: new Guid("744392ab-54f5-422a-a1cb-ea58dfa5dd12"));

            migrationBuilder.UpdateData(
                table: "c_reservation_issue_type",
                keyColumn: "reservation_issue_type_id",
                keyValue: 6,
                column: "has_description",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "has_description",
                table: "c_reservation_issue_type");

            migrationBuilder.RenameColumn(
                name: "reservation_issue_type_id",
                table: "c_reservation_issue_type",
                newName: "reservation_state_id");

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 1,
                column: "hash",
                value: new Guid("fde2f7e3-a95a-45f2-9e32-d416235605f3"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 2,
                column: "hash",
                value: new Guid("00bbe500-010c-4132-a0c2-13b7dff13702"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 3,
                column: "hash",
                value: new Guid("a6a91bec-2ab1-45dd-94f5-158c83b448a0"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 4,
                column: "hash",
                value: new Guid("57f039b7-3f49-4287-b71f-ac42602aaa9b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 5,
                column: "hash",
                value: new Guid("0dc74b66-18d4-448f-ad07-b43aa5bcef7a"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 6,
                column: "hash",
                value: new Guid("7f03e63e-7aa4-44f4-a992-e85a86f4df70"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 7,
                column: "hash",
                value: new Guid("5129a9d8-7493-46fd-8fe0-6800281c5ed5"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 8,
                column: "hash",
                value: new Guid("5d532bb5-c35b-4b6f-8851-9e95a6b8530f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 9,
                column: "hash",
                value: new Guid("5c6ecf6c-cceb-488a-8139-dc52c480baae"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 10,
                column: "hash",
                value: new Guid("78aaa236-3f55-408e-b528-3400d98ce3cc"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 11,
                column: "hash",
                value: new Guid("b9aa985b-ebc0-48a3-9937-85c47a760fbf"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 12,
                column: "hash",
                value: new Guid("921f198a-9f76-4665-9783-97cdfba43409"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 13,
                column: "hash",
                value: new Guid("cb60fe98-9227-42b1-a9d4-e73b45d4ff53"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 14,
                column: "hash",
                value: new Guid("b38fa715-c335-4f02-aa53-9babf2d5ff53"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 15,
                column: "hash",
                value: new Guid("f761a61b-e239-4459-9d32-821ff388b8f1"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 16,
                column: "hash",
                value: new Guid("b556c41f-b08d-43ac-8185-b93d0ff8fb9f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 17,
                column: "hash",
                value: new Guid("accd67d4-a129-42d0-b937-07b0cc483cb4"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 18,
                column: "hash",
                value: new Guid("a716bec5-2ea9-424d-a2f6-af22ee0dd15a"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 19,
                column: "hash",
                value: new Guid("45867cc3-8380-42ec-9740-a518b29b5ae1"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 20,
                column: "hash",
                value: new Guid("26abd8a8-c4a4-4df0-ad96-bab1abf6a09c"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 21,
                column: "hash",
                value: new Guid("95245389-8344-4021-9bf0-2f668113b952"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 22,
                column: "hash",
                value: new Guid("91401782-0e42-44aa-bf72-7b6b8780aa8a"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 23,
                column: "hash",
                value: new Guid("2d2491be-710f-4ff5-beff-ff293fcefd9e"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 24,
                column: "hash",
                value: new Guid("1ba08cda-5a40-4099-998b-fdad1eeb77a9"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 25,
                column: "hash",
                value: new Guid("4eb6a903-f156-41a3-86a5-a72b915e41bd"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 26,
                column: "hash",
                value: new Guid("aee2f3aa-046d-4d8d-9f32-dbfc41708b31"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 27,
                column: "hash",
                value: new Guid("739fd20d-1cb8-4f6f-a886-92e18497721e"));
        }
    }
}
