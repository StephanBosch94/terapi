using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class Currencies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "currency_id",
                table: "app_transaction",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "currency_id",
                table: "app_reservation_package",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "currency_id",
                table: "app_reservation",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "ix_app_transaction_currency_id",
                table: "app_transaction",
                column: "currency_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_reservation_package_currency_id",
                table: "app_reservation_package",
                column: "currency_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_reservation_currency_id",
                table: "app_reservation",
                column: "currency_id");

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_c_currency_curency_id",
                table: "app_reservation",
                column: "currency_id",
                principalTable: "c_currency",
                principalColumn: "currency_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_package_c_currency_currency_id",
                table: "app_reservation_package",
                column: "currency_id",
                principalTable: "c_currency",
                principalColumn: "currency_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_app_currency_currency_id",
                table: "app_transaction",
                column: "currency_id",
                principalTable: "c_currency",
                principalColumn: "currency_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_c_currency_curency_id",
                table: "app_reservation");

            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_package_c_currency_currency_id",
                table: "app_reservation_package");

            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_app_currency_currency_id",
                table: "app_transaction");

            migrationBuilder.DropIndex(
                name: "ix_app_transaction_currency_id",
                table: "app_transaction");

            migrationBuilder.DropIndex(
                name: "ix_app_reservation_package_currency_id",
                table: "app_reservation_package");

            migrationBuilder.DropIndex(
                name: "ix_app_reservation_currency_id",
                table: "app_reservation");

            migrationBuilder.DropColumn(
                name: "currency_id",
                table: "app_transaction");

            migrationBuilder.DropColumn(
                name: "currency_id",
                table: "app_reservation_package");

            migrationBuilder.DropColumn(
                name: "currency_id",
                table: "app_reservation");

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 1,
                column: "hash",
                value: new Guid("3d48fe5d-10a8-465b-a7a8-f2b8797e6092"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 2,
                column: "hash",
                value: new Guid("a28ffc7d-0436-4733-b514-c5083fdf3f13"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 3,
                column: "hash",
                value: new Guid("32ac562f-dd61-4dd1-8b8e-64c39a9683d8"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 4,
                column: "hash",
                value: new Guid("1b383739-ef83-4ecb-99d2-ae6643c86d1b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 5,
                column: "hash",
                value: new Guid("81b3a077-7e0f-4199-852f-c0bf1916f87f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 6,
                column: "hash",
                value: new Guid("7641537f-cdb4-4b62-b411-e33be051394d"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 7,
                column: "hash",
                value: new Guid("9e108530-e1a4-496a-b55b-5725999fc88a"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 8,
                column: "hash",
                value: new Guid("001dec20-2bd5-4fd4-b800-cfedfe7b0a7b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 9,
                column: "hash",
                value: new Guid("5dc58d80-483e-43e3-aa2c-8b4342619ca3"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 10,
                column: "hash",
                value: new Guid("b12adb19-a6eb-46ad-8712-a3b8a26f2b94"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 11,
                column: "hash",
                value: new Guid("e64911d6-cfe4-45cb-8120-a71382e88a9f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 12,
                column: "hash",
                value: new Guid("9864b8ad-0baf-4dd7-a8dd-7a16909c0808"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 13,
                column: "hash",
                value: new Guid("8cd32ee3-575a-4f8c-8c8c-6153f0c2cfae"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 14,
                column: "hash",
                value: new Guid("7854fcf4-7229-4573-88ab-842ebbe9f9ba"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 15,
                column: "hash",
                value: new Guid("4e152f94-55d8-4b79-bbb7-806d6ca006f9"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 16,
                column: "hash",
                value: new Guid("4c109125-e182-4592-99fe-cf1ff958cb7b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 17,
                column: "hash",
                value: new Guid("4bfee7c2-2b98-4eb5-86e3-d1dbcfc14f43"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 18,
                column: "hash",
                value: new Guid("732db2a3-87ac-4ab1-9337-0a39d85e8178"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 19,
                column: "hash",
                value: new Guid("278c0ad9-b419-4041-a596-9e2dc7a61154"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 20,
                column: "hash",
                value: new Guid("d4cb97b7-4d15-44d8-a9d2-dda61f42149d"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 21,
                column: "hash",
                value: new Guid("3ef11a92-5528-4ff5-9562-3b3388bcb553"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 22,
                column: "hash",
                value: new Guid("14aea27d-3663-4ce0-b4f9-c4cd0580ebe2"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 23,
                column: "hash",
                value: new Guid("6f665586-0f00-4e6f-8872-fcca3b17ea3a"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 24,
                column: "hash",
                value: new Guid("0725f3ce-34dd-4333-b281-4897f3a2e8df"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 25,
                column: "hash",
                value: new Guid("5ba07516-85be-4ff9-8a3f-0c7200fcf0a3"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 26,
                column: "hash",
                value: new Guid("7668980d-c836-48e9-b477-6e25634a15c7"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 27,
                column: "hash",
                value: new Guid("956b2977-eb90-49d6-800b-2a4c656ee4b5"));
        }
    }
}
