using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class LessonsForKidsType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_lesson_for_children_type",
                table: "c_specialization",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 1,
                column: "is_lesson_for_children_type",
                value: true);

            migrationBuilder.UpdateData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 25,
                column: "is_lesson_for_children_type",
                value: true);

            migrationBuilder.UpdateData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 37,
                column: "is_lesson_for_children_type",
                value: true);

            migrationBuilder.UpdateData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 45,
                column: "is_lesson_for_children_type",
                value: true);

            migrationBuilder.UpdateData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 53,
                column: "is_lesson_for_children_type",
                value: true);

            migrationBuilder.UpdateData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 62,
                column: "is_lesson_for_children_type",
                value: true);

            migrationBuilder.UpdateData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 67,
                column: "is_lesson_for_children_type",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_lesson_for_children_type",
                table: "c_specialization");
        }
    }
}
