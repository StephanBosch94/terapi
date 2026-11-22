using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class GoogleOneTap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "registration_method_id",
                table: "app_user",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "c_authentication_method",
                columns: table => new
                {
                    AuthenticationMethodId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_authentication_method", x => x.AuthenticationMethodId);
                });

            migrationBuilder.InsertData(
                table: "c_authentication_method",
                columns: new[] { "AuthenticationMethodId", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "authentication_method.internal.description", "internal", "authentication_method.internal.name" },
                    { 2, "authentication_method.google.description", "google", "authentication_method.google.name" },
                    { 3, "authentication_method.facebook.description", "facebook", "authentication_method.facebook.name" },
                    { 4, "authentication_method.seznam.description", "seznam", "authentication_method.seznam.name" },
                    { 5, "authentication_method.google_one_tap.description", "google_one_tap", "authentication_method.google_one_tap.name" }
                });

            migrationBuilder.InsertData(
                table: "c_user_state",
                columns: new[] { "user_state_id", "company_admin_name_key", "description_key", "enum_key", "name_key" },
                values: new object[] { 8, "user_state.created_via_google_one_tap.company_admin_name_key", "user_state.created_via_google_one_tap.description", "created_via_google_one_tap", "user_state.created_via_google_one_tap.name" });

            migrationBuilder.CreateIndex(
                name: "IX_app_user_registration_method_id",
                table: "app_user",
                column: "registration_method_id");

            migrationBuilder.CreateIndex(
                name: "ix_c_authentication_method_enum_key",
                table: "c_authentication_method",
                column: "enum_key",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_app_user_c_authentication_method_registration_method_id",
                table: "app_user",
                column: "registration_method_id",
                principalTable: "c_authentication_method",
                principalColumn: "AuthenticationMethodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_app_user_c_authentication_method_registration_method_id",
                table: "app_user");

            migrationBuilder.DropTable(
                name: "c_authentication_method");

            migrationBuilder.DropIndex(
                name: "IX_app_user_registration_method_id",
                table: "app_user");

            migrationBuilder.DeleteData(
                table: "c_user_state",
                keyColumn: "user_state_id",
                keyValue: 8);

            migrationBuilder.DropColumn(
                name: "registration_method_id",
                table: "app_user");
        }
    }
}
