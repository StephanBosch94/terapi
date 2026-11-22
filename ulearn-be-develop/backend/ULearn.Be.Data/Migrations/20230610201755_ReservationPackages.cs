using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class ReservationPackages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "price_without_sales",
                table: "app_reservation",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "reservation_package_id",
                table: "app_reservation",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "c_reservation_package_state",
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
                    table.PrimaryKey("pk_c_reservation_package_state", x => x.reservation_state_id);
                });

            migrationBuilder.CreateTable(
                name: "app_reservation_package",
                columns: table => new
                {
                    reservation_package_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    cancelled = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    owner_id = table.Column<int>(type: "integer", nullable: false),
                    cancellation_reason = table.Column<string>(type: "text", nullable: true),
                    reservation_package_state_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_reservation_package", x => x.reservation_package_id);
                    table.ForeignKey(
                        name: "fk_app_reservation_package_app_reservation_reservation_id",
                        column: x => x.owner_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_reservation_package_c_reservation_package_state_reservation_package_state_id",
                        column: x => x.reservation_package_state_id,
                        principalTable: "c_reservation_package_state",
                        principalColumn: "reservation_state_id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.InsertData(
                table: "c_reservation_package_state",
                columns: new[] { "reservation_state_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "reservation_package_state.created.description", "created", "reservation_package_state.created.name" },
                    { 2, "reservation_package_state.cancelled.description", "cancelled", "reservation_package_state.cancelled.name" }
                });

            migrationBuilder.UpdateData(
                table: "c_reservation_state",
                keyColumn: "reservation_state_id",
                keyValue: 2,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "reservation_state.deleted.description", "deleted", "reservation_state.deleted.name" });

            migrationBuilder.CreateIndex(
                name: "ix_app_reservation_reservation_package_id",
                table: "app_reservation",
                column: "reservation_package_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_reservation_package_owner_id",
                table: "app_reservation_package",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_reservation_package_reservation_package_state_id",
                table: "app_reservation_package",
                column: "reservation_package_state_id");

            migrationBuilder.CreateIndex(
                name: "ix_c_reservation_package_state_enum_key",
                table: "c_reservation_package_state",
                column: "enum_key",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_app_reservation_app_reservation_package_reservation_package_id",
                table: "app_reservation",
                column: "reservation_package_id",
                principalTable: "app_reservation_package",
                principalColumn: "reservation_package_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_app_reservation_app_reservation_package_reservation_package_id",
                table: "app_reservation");

            migrationBuilder.DropTable(
                name: "app_reservation_package");

            migrationBuilder.DropTable(
                name: "c_reservation_package_state");

            migrationBuilder.DropIndex(
                name: "ix_app_reservation_reservation_package_id",
                table: "app_reservation");

            migrationBuilder.DropColumn(
                name: "price_without_sales",
                table: "app_reservation");

            migrationBuilder.DropColumn(
                name: "reservation_package_id",
                table: "app_reservation");

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 1,
                column: "hash",
                value: new Guid("801854ed-684a-458a-89ca-0c4cacf5e20f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 2,
                column: "hash",
                value: new Guid("b8b5feff-bd1e-49da-a625-4ccf469c0f8b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 3,
                column: "hash",
                value: new Guid("aad26f1d-b47b-485b-bfb4-0014b17f9dc6"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 4,
                column: "hash",
                value: new Guid("d8c06dde-eb66-4a82-be8b-c8752dbbd09e"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 5,
                column: "hash",
                value: new Guid("a68e9a38-2350-4871-bf9a-53d2ccd93525"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 6,
                column: "hash",
                value: new Guid("9396a236-ac84-4d4e-ad8c-13dedd99840b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 7,
                column: "hash",
                value: new Guid("a61d4a8d-fdc1-429d-b2d7-306123ba9313"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 8,
                column: "hash",
                value: new Guid("43a490a2-1131-456d-9bce-04265a60199b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 9,
                column: "hash",
                value: new Guid("ae3753de-d9dd-4fb9-a527-cec6d6649bbb"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 10,
                column: "hash",
                value: new Guid("fcaf4dd9-0356-4c49-b4e1-4e8de353cdd4"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 11,
                column: "hash",
                value: new Guid("1237bf20-3fc8-4fd6-8e7e-85ff7754219f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 12,
                column: "hash",
                value: new Guid("780fda04-443e-48ed-b34f-478075a88254"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 13,
                column: "hash",
                value: new Guid("6ff16e8b-32fc-4f67-b7ee-70429a7220c0"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 14,
                column: "hash",
                value: new Guid("eed614e3-7f1f-4c41-a007-c35ed2e6431b"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 15,
                column: "hash",
                value: new Guid("f924df02-5242-4755-8fc5-c9b847a20ad2"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 16,
                column: "hash",
                value: new Guid("f37c30fb-3f01-4df1-aeb3-ea43eeda56c6"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 17,
                column: "hash",
                value: new Guid("0050e7d6-76ef-4679-81ba-3e39541c9e37"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 18,
                column: "hash",
                value: new Guid("06fcdba7-96ba-4983-8ec0-290d19f0d6e1"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 19,
                column: "hash",
                value: new Guid("ddde2664-3b56-4dde-953d-082e55ed55a6"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 20,
                column: "hash",
                value: new Guid("224e92df-96e7-40c3-9ac6-546a889c45a7"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 21,
                column: "hash",
                value: new Guid("a6351529-5750-4625-9513-36378f89ea97"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 22,
                column: "hash",
                value: new Guid("6aaa35fd-9920-4d98-a666-e158a5919538"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 23,
                column: "hash",
                value: new Guid("ea43d805-5993-40f9-8d69-64a773a24848"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 24,
                column: "hash",
                value: new Guid("34333aae-59d6-4446-9cbe-8c3724c3c8fb"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 25,
                column: "hash",
                value: new Guid("ce7ea22b-2387-49d2-9aab-012f6a64ecdd"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 26,
                column: "hash",
                value: new Guid("6d520f5c-b437-48df-96bc-6070f6e21fd0"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 27,
                column: "hash",
                value: new Guid("c270eaff-aa08-4c42-94a7-f7056e804bad"));

            migrationBuilder.UpdateData(
                table: "c_reservation_state",
                keyColumn: "reservation_state_id",
                keyValue: 2,
                columns: new[] { "description_key", "enum_key", "name_key" },
                values: new object[] { "reservation_state.payed.description", "payed", "reservation_state.payed.name" });
        }
    }
}
