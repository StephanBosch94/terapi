using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class Korean : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "c_subject",
                columns: new[] { "subject_id", "category_id", "description_key", "enum_key", "footer_subject_landing_page_link_key", "footer_subject_teachers_page_link_key", "hero_description_key", "hero_title_key", "icon_name", "name_key", "object_state_id", "page_title_key", "user_goal_title_key" },
                values: new object[] { 7, 1, "subject.korean.description", "korean", "subject.korean.footer.lading_page_link", "subject.korean.footer.subject_teachers_link", "subject.korean.hero.description", "subject.korean.hero.title", "korean_flag", "subject.korean.name", 1, "subject.korean.page.title", "subject.korean.user_goal.title" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "c_subject",
                keyColumn: "subject_id",
                keyValue: 7);
        }
    }
}
