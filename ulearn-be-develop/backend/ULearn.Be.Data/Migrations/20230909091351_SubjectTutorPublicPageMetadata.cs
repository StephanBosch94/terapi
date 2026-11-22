using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class SubjectTutorPublicPageMetadata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "tutor_public_page_description_key",
                table: "c_subject",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tutor_public_page_title_key",
                table: "c_subject",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "c_subject",
                keyColumn: "subject_id",
                keyValue: 1,
                columns: new[] { "tutor_public_page_description_key", "tutor_public_page_title_key" },
                values: new object[] { "subject.english.tutor_public_page.description", "subject.english.tutor_public_page.title" });

            migrationBuilder.UpdateData(
                table: "c_subject",
                keyColumn: "subject_id",
                keyValue: 2,
                columns: new[] { "tutor_public_page_description_key", "tutor_public_page_title_key" },
                values: new object[] { "subject.czech.tutor_public_page.description", "subject.czech.tutor_public_page.title" });

            migrationBuilder.UpdateData(
                table: "c_subject",
                keyColumn: "subject_id",
                keyValue: 3,
                columns: new[] { "tutor_public_page_description_key", "tutor_public_page_title_key" },
                values: new object[] { "subject.french.tutor_public_page.description", "subject.french.tutor_public_page.title" });

            migrationBuilder.UpdateData(
                table: "c_subject",
                keyColumn: "subject_id",
                keyValue: 4,
                columns: new[] { "tutor_public_page_description_key", "tutor_public_page_title_key" },
                values: new object[] { "subject.spanish.tutor_public_page.description", "subject.spanish.tutor_public_page.title" });

            migrationBuilder.UpdateData(
                table: "c_subject",
                keyColumn: "subject_id",
                keyValue: 5,
                columns: new[] { "tutor_public_page_description_key", "tutor_public_page_title_key" },
                values: new object[] { "subject.german.tutor_public_page.description", "subject.german.tutor_public_page.title" });

            migrationBuilder.UpdateData(
                table: "c_subject",
                keyColumn: "subject_id",
                keyValue: 6,
                columns: new[] { "tutor_public_page_description_key", "tutor_public_page_title_key" },
                values: new object[] { "subject.italian.tutor_public_page.description", "subject.italian.tutor_public_page.title" });

            migrationBuilder.UpdateData(
                table: "c_subject",
                keyColumn: "subject_id",
                keyValue: 7,
                columns: new[] { "tutor_public_page_description_key", "tutor_public_page_title_key" },
                values: new object[] { "subject.korean.tutor_public_page.description", "subject.korean.tutor_public_page.title" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tutor_public_page_description_key",
                table: "c_subject");

            migrationBuilder.DropColumn(
                name: "tutor_public_page_title_key",
                table: "c_subject");
        }
    }
}
