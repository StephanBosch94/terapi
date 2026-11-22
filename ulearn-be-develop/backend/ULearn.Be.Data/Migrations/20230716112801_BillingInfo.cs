using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class BillingInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "dic",
                table: "app_user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ico",
                table: "app_user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "place_of_business",
                table: "app_user",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 1,
                column: "hash",
                value: new Guid("75f17520-c3d7-4f06-b91c-18bdeb762961"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 2,
                column: "hash",
                value: new Guid("01c8f8cd-508d-4703-af6d-51795466a6ca"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 3,
                column: "hash",
                value: new Guid("2d5c08d2-b804-4ae4-9be7-3095aac2599f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 4,
                column: "hash",
                value: new Guid("641200b1-2df3-487c-a510-bd661f5ef567"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 5,
                column: "hash",
                value: new Guid("59b83a27-33b5-4fdb-a15f-2b79ed71ccc0"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 6,
                column: "hash",
                value: new Guid("8b13cfe0-1986-44e4-b97b-e8c7ff0fe122"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 7,
                column: "hash",
                value: new Guid("b7d4c005-9895-4a3a-b630-ed73ad8a86a3"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 8,
                column: "hash",
                value: new Guid("a9d2213b-9b63-4c63-9330-1dfc87095b2c"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 9,
                column: "hash",
                value: new Guid("9ee65c28-d700-418d-8e7c-7027f20a3025"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 10,
                column: "hash",
                value: new Guid("79086f71-9521-4c57-a06a-7dafe576de45"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 11,
                column: "hash",
                value: new Guid("5cb4707b-10d3-4a6e-9330-3e9414f66188"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 12,
                column: "hash",
                value: new Guid("2c682904-c1f9-4c6b-a0a5-dc46e4af464e"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 13,
                column: "hash",
                value: new Guid("def21cba-8279-4575-b2f1-7cc8c89cda3e"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 14,
                column: "hash",
                value: new Guid("89aa5c36-58f3-488f-aae7-48e9fdb98baf"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 15,
                column: "hash",
                value: new Guid("1aeebdca-4f9a-49d6-a59f-a10fb9855305"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 16,
                column: "hash",
                value: new Guid("d9480a2c-3dc0-48c0-9f93-ef47d26bc334"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 17,
                column: "hash",
                value: new Guid("6a45c7eb-a60d-4f9a-a39b-4be02a839d8a"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 18,
                column: "hash",
                value: new Guid("a51a4fe2-1ae6-4cf6-bbc9-1efea3fc0905"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 19,
                column: "hash",
                value: new Guid("2d0159ec-0b91-4389-b94e-346ff4603ab0"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 20,
                column: "hash",
                value: new Guid("84d2cdaa-ef79-4444-a388-6fc101845578"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 21,
                column: "hash",
                value: new Guid("41f6d551-6382-4728-a2d2-402eada5c1b0"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 22,
                column: "hash",
                value: new Guid("8efb8d35-7153-4217-9b67-987548a3ac31"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 23,
                column: "hash",
                value: new Guid("7b85004b-6812-4763-984c-c9ddbb713ab1"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 24,
                column: "hash",
                value: new Guid("a417d749-0eff-4408-8a74-36f3de24f190"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 25,
                column: "hash",
                value: new Guid("f94614e0-00c0-4823-8a13-003ba745303c"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 26,
                column: "hash",
                value: new Guid("fe1c7df9-2ca0-41a4-9ad8-330cac19ecd5"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 27,
                column: "hash",
                value: new Guid("ee30a0a9-48a2-4bfe-acf5-5feee3dd263c"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dic",
                table: "app_user");

            migrationBuilder.DropColumn(
                name: "ico",
                table: "app_user");

            migrationBuilder.DropColumn(
                name: "place_of_business",
                table: "app_user");

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 1,
                column: "hash",
                value: new Guid("0bdaa541-a4db-4c35-b285-4d4480924bd1"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 2,
                column: "hash",
                value: new Guid("01737702-a030-4d6d-9122-52cacc467d0a"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 3,
                column: "hash",
                value: new Guid("a191b6c6-330e-4362-bc5a-4e3a6fa694fe"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 4,
                column: "hash",
                value: new Guid("4d03e920-24cc-4c30-aa56-7db47c005d44"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 5,
                column: "hash",
                value: new Guid("f9ad6a2a-6316-48a4-8f09-2c51f20a0dde"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 6,
                column: "hash",
                value: new Guid("82cdcbb1-4601-4ef6-8b01-4c683ec3ae84"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 7,
                column: "hash",
                value: new Guid("a1342580-ea94-4d19-9114-9a89426354f7"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 8,
                column: "hash",
                value: new Guid("a8123a5c-cec6-465a-96d3-5e45803f9931"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 9,
                column: "hash",
                value: new Guid("9368f543-7a69-409f-abc5-05abe8cc6496"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 10,
                column: "hash",
                value: new Guid("2f91affd-da32-445f-b41d-f47e2441f9d4"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 11,
                column: "hash",
                value: new Guid("2e16a8de-1684-401e-9d83-31245a780a3f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 12,
                column: "hash",
                value: new Guid("cf7a04e2-71d8-40f1-84cd-212805d4f89e"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 13,
                column: "hash",
                value: new Guid("f3450783-c96c-4448-9e82-6bb6cd4c8302"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 14,
                column: "hash",
                value: new Guid("6ae65f3e-11bf-474e-bfd2-33e5c8d9eb5d"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 15,
                column: "hash",
                value: new Guid("16b520e2-b8bd-4eca-ab7d-5fcd228870bc"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 16,
                column: "hash",
                value: new Guid("7e55a33c-c8f2-4e57-b442-32dbaf01b710"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 17,
                column: "hash",
                value: new Guid("8b9ca74f-962c-439e-a5df-c3118efc2c2a"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 18,
                column: "hash",
                value: new Guid("3087bd30-c549-44bd-be91-838ed88cf82c"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 19,
                column: "hash",
                value: new Guid("7fac385d-dd14-42bc-acac-4463a5f17aa8"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 20,
                column: "hash",
                value: new Guid("48e2bbba-3d74-437d-86d7-53be1fbb7cb3"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 21,
                column: "hash",
                value: new Guid("3fbc2b59-ec78-4284-af49-57df306e4d5b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 22,
                column: "hash",
                value: new Guid("d0544c1e-ca36-442c-88d9-ca62b8280492"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 23,
                column: "hash",
                value: new Guid("4b6e3a17-d301-48a1-969c-f7b833007654"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 24,
                column: "hash",
                value: new Guid("3c294e07-bbe3-4389-862b-603ac8a74245"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 25,
                column: "hash",
                value: new Guid("90590199-d47d-4777-9ef2-97d74b62c4b8"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 26,
                column: "hash",
                value: new Guid("f8a27f33-054f-43ab-8966-26ff0648cb00"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 27,
                column: "hash",
                value: new Guid("c74a8cbf-c527-4a7f-bb4c-1c34c1af25a4"));
        }
    }
}
