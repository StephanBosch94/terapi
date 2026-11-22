using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class ReservationIssues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "c_reservation_issue_type",
                columns: table => new
                {
                    reservation_state_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_reservation_issue_type", x => x.reservation_state_id);
                });

            migrationBuilder.CreateTable(
                name: "app_reservation_issue",
                columns: table => new
                {
                    reservation_issue_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    reservation_issue_type_id = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    creator_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_reservation_issue", x => x.reservation_issue_id);
                    table.ForeignKey(
                        name: "fk_app_reservation_issue_app_user_creator_id",
                        column: x => x.creator_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_reservation_issue_c_reservation_issue_type_reservation_issue_type_id",
                        column: x => x.reservation_issue_type_id,
                        principalTable: "c_reservation_issue_type",
                        principalColumn: "reservation_state_id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.InsertData(
                table: "c_reservation_issue_type",
                columns: new[] { "reservation_state_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "reservation_issue_type.agreed_to_reschedule.description", "agreed_to_reschedule", "reservation_issue_type.agreed_to_reschedule.name" },
                    { 2, "reservation_issue_type.technical_problems.description", "technical_problems", "reservation_issue_type.technical_problems.name" },
                    { 3, "reservation_issue_type.absence.description", "absence", "reservation_issue_type.absence.name" },
                    { 4, "reservation_issue_type.tutor_did_not_meet_expectations.description", "tutor_did_not_meet_expectations", "reservation_issue_type.tutor_did_not_meet_expectations.name" },
                    { 5, "reservation_issue_type.tutor_was_absent.description", "tutor_was_absent", "reservation_issue_type.tutor_was_absent.name" },
                    { 6, "reservation_issue_type.other.description", "other", "reservation_issue_type.other.name" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_app_reservation_creator_id",
                table: "app_reservation_issue",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_reservation_reservation_issue_type_id",
                table: "app_reservation_issue",
                column: "reservation_issue_type_id");

            migrationBuilder.CreateIndex(
                name: "uc_c_reservation_issue_type_enum_key",
                table: "c_reservation_issue_type",
                column: "enum_key",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "app_reservation_issue");

            migrationBuilder.DropTable(
                name: "c_reservation_issue_type");

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 1,
                column: "hash",
                value: new Guid("dc4d389e-e704-4b07-b0bf-bc664a157474"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 2,
                column: "hash",
                value: new Guid("b7a6ad2b-b105-45e2-bd77-528e16f7c6ee"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 3,
                column: "hash",
                value: new Guid("eba84a17-77b6-45cb-b6c4-9406f6b52dac"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 4,
                column: "hash",
                value: new Guid("dd88bf13-14da-4c6b-a681-3720b2fc1c44"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 5,
                column: "hash",
                value: new Guid("60aace90-f938-4daf-bc7b-9dd9057a5996"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 6,
                column: "hash",
                value: new Guid("4c192301-aa72-402f-ae0f-af9568d29b1f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 7,
                column: "hash",
                value: new Guid("2fec7652-75ed-4037-9f68-dc34565e357c"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 8,
                column: "hash",
                value: new Guid("f909135e-7973-432a-9307-b884c45c65d1"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 9,
                column: "hash",
                value: new Guid("858b1497-4365-4d42-8ffa-2eae7f24b410"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 10,
                column: "hash",
                value: new Guid("6bfb74c2-9054-4374-8ce8-4f3db89d2cb4"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 11,
                column: "hash",
                value: new Guid("add494ef-f52b-4a9d-81cf-4a40e768910a"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 12,
                column: "hash",
                value: new Guid("45e421c7-8e6a-4573-ba1c-699ff5a37482"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 13,
                column: "hash",
                value: new Guid("a6aa3a83-5f06-4904-b367-17dc1c5be3fb"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 14,
                column: "hash",
                value: new Guid("5e0c0b27-dbb8-4ff5-b61b-ed45859f331b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 15,
                column: "hash",
                value: new Guid("70064794-175f-4782-9be1-f747442a1fcc"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 16,
                column: "hash",
                value: new Guid("a38e365a-002e-4d01-802c-568bfbd65f97"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 17,
                column: "hash",
                value: new Guid("ebbf4f68-c3e6-4938-9d0b-0d4619e14fea"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 18,
                column: "hash",
                value: new Guid("cf9116fc-06a3-4104-8f46-7e57ab3ca2ef"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 19,
                column: "hash",
                value: new Guid("7d7cccad-c818-4ac1-9106-bc301c2910f2"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 20,
                column: "hash",
                value: new Guid("a854e1f8-c99e-4451-baab-ac0d92bfa30a"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 21,
                column: "hash",
                value: new Guid("9d64a07f-e99d-4dd4-a516-233225c56832"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 22,
                column: "hash",
                value: new Guid("9dda523d-d28a-4239-bb3d-3f75892bb2c5"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 23,
                column: "hash",
                value: new Guid("94e1b622-c59e-4df5-b038-356e5c471066"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 24,
                column: "hash",
                value: new Guid("0f2ccebb-bad9-43f3-af74-4131017e8a09"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 25,
                column: "hash",
                value: new Guid("d20313ad-0385-4e35-832f-cedd96c340d1"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 26,
                column: "hash",
                value: new Guid("9b623d47-d210-460f-aadb-780033176c0f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 27,
                column: "hash",
                value: new Guid("17dbb147-79ec-4137-b61d-d2ea12a054ea"));
        }
    }
}
