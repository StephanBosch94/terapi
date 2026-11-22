using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class UserRefusedState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "c_user_state",
                columns: new[] { "user_state_id", "company_admin_name_key", "description_key", "enum_key", "name_key" },
                values: new object[] { 7, "user_state.refused.company_admin_name_key", "user_state.refused.description", "refused", "user_state.refused.name" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "c_user_state",
                keyColumn: "user_state_id",
                keyValue: 7);
        }
    }
}
