using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class UserInterests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "c_interest",
                columns: table => new
                {
                    interest_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    icon_name = table.Column<string>(type: "text", nullable: true),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_interest", x => x.interest_id);
                });

            migrationBuilder.CreateTable(
                name: "r_user_interest",
                columns: table => new
                {
                    user_interest_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    interest_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_r_user_interest", x => x.user_interest_id);
                    table.ForeignKey(
                        name: "fk_r_user_interest_app_user_user_id",
                        column: x => x.user_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_r_user_interest_c_interest_interest_id",
                        column: x => x.interest_id,
                        principalTable: "c_interest",
                        principalColumn: "interest_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "c_interest",
                columns: new[] { "interest_id", "description_key", "enum_key", "icon_name", "name_key" },
                values: new object[,]
                {
                    { 1, "interest.architecture.description", "architecture", "architecture", "interest.architecture.name" },
                    { 2, "interest.basketball.description", "basketball", "basketball", "interest.basketball.name" },
                    { 3, "interest.business.description", "business", "business", "interest.business.name" },
                    { 4, "interest.cars.description", "cars", "cars", "interest.cars.name" },
                    { 5, "interest.chess.description", "chess", "chess", "interest.chess.name" },
                    { 6, "interest.cooking.description", "cooking", "cooking", "interest.cooking.name" },
                    { 7, "interest.ecology.description", "ecology", "ecology", "interest.ecology.name" },
                    { 8, "interest.football.description", "football", "football", "interest.football.name" },
                    { 9, "interest.hiking.description", "hiking", "hiking", "interest.hiking.name" },
                    { 10, "interest.history.description", "history", "history", "interest.history.name" },
                    { 11, "interest.meditation.description", "meditation", "meditation", "interest.meditation.name" },
                    { 12, "interest.movies_and_tv.description", "movies_and_tv", "movies_and_tv", "interest.movies_and_tv.name" },
                    { 13, "interest.music.description", "music", "music", "interest.music.name" },
                    { 14, "interest.nutrition.description", "nutrition", "nutrition", "interest.nutrition.name" },
                    { 15, "interest.personal_growth.description", "personal_growth", "personal_growth", "interest.personal_growth.name" },
                    { 16, "interest.photography.description", "photography", "photography", "interest.photography.name" },
                    { 17, "interest.psychology.description", "psychology", "psychology", "interest.psychology.name" },
                    { 18, "interest.running.description", "running", "running", "interest.running.name" },
                    { 19, "interest.technologies.description", "technologies", "technologies", "interest.technologies.name" },
                    { 20, "interest.theatre.description", "theatre", "theatre", "interest.theatre.name" },
                    { 21, "interest.travel.description", "travel", "travel", "interest.travel.name" },
                    { 22, "interest.workout.description", "workout", "workout", "interest.workout.name" },
                    { 23, "interest.writing.description", "writing", "writing", "interest.writing.name" }
                });

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

            migrationBuilder.CreateIndex(
                name: "ix_r_user_interest_interest_id",
                table: "r_user_interest",
                column: "interest_id");

            migrationBuilder.CreateIndex(
                name: "ix_r_user_interest_user_id",
                table: "r_user_interest",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "uc_r_user_interest_user_id_interest_id",
                table: "r_user_interest",
                columns: new[] { "user_id", "interest_id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "r_user_interest");

            migrationBuilder.DropTable(
                name: "c_interest");

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 1,
                column: "hash",
                value: new Guid("1226ed7f-9ebd-41b0-aa2d-d5d170970d5f"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 2,
                column: "hash",
                value: new Guid("c71cbc8a-eacf-4ecc-b3db-de9eef7bd182"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 3,
                column: "hash",
                value: new Guid("16681f93-36b0-4beb-bb2d-99d27f75f1ca"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 4,
                column: "hash",
                value: new Guid("1b884c72-d343-4bd1-a281-2eff865fcf3d"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 5,
                column: "hash",
                value: new Guid("69e85596-fa5a-46b5-ad1a-0fd53df5363e"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 6,
                column: "hash",
                value: new Guid("0554dd9e-9fad-4821-86b4-274811933540"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 7,
                column: "hash",
                value: new Guid("200fdec9-2f94-43fb-a158-696b2456e9e7"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 8,
                column: "hash",
                value: new Guid("52e71cd5-cc7f-4ccc-8b9b-45b3c083c342"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 9,
                column: "hash",
                value: new Guid("a4e02721-3ae3-423a-9d92-447ae687f109"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 10,
                column: "hash",
                value: new Guid("fce90e25-c25d-4b60-8607-5d4e6cc26583"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 11,
                column: "hash",
                value: new Guid("e14761d6-cbfd-4f29-9893-6d9369d00b61"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 12,
                column: "hash",
                value: new Guid("bbfa8a55-759b-4c3d-baed-c32906033844"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 13,
                column: "hash",
                value: new Guid("4e3644ac-744d-4641-8df5-b23ad822cd37"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 14,
                column: "hash",
                value: new Guid("20cea3b2-2312-457b-af89-2978670aa754"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 15,
                column: "hash",
                value: new Guid("4c501c2b-c6cb-4f66-a596-7d3cf828d485"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 16,
                column: "hash",
                value: new Guid("8996389f-bf0f-46e3-8fe9-15ecf09e1804"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 17,
                column: "hash",
                value: new Guid("e2898e5b-480d-4c3f-8f6f-611d2af01f29"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 18,
                column: "hash",
                value: new Guid("39bf01e8-a996-4a7b-b959-536aee2c6ac9"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 19,
                column: "hash",
                value: new Guid("dd3c334a-47f1-4437-8bb1-8863ccf96693"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 20,
                column: "hash",
                value: new Guid("8902d929-7de9-4dc9-9746-525f9ac55002"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 21,
                column: "hash",
                value: new Guid("95797128-6e66-48d0-8437-27e902a2860e"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 22,
                column: "hash",
                value: new Guid("a0549645-aea5-4fa9-a2f7-c814c51c42a6"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 23,
                column: "hash",
                value: new Guid("4ffe9320-2574-4f32-8e25-27cddd15abff"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 24,
                column: "hash",
                value: new Guid("c4161d2d-a9bf-428b-ba12-f9cb33b43afa"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 25,
                column: "hash",
                value: new Guid("81efd846-fdb0-486e-981f-4962920994a3"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 26,
                column: "hash",
                value: new Guid("af10cf09-6079-432e-ad78-baae7a8d4291"));

            migrationBuilder.UpdateData(
                table: "c_master_hash",
                keyColumn: "master_hash_id",
                keyValue: 27,
                column: "hash",
                value: new Guid("7cc37de9-2170-4300-a577-15f754876c98"));
        }
    }
}
