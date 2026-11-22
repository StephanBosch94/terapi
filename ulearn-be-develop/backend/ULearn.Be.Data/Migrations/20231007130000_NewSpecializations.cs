using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class NewSpecializations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "alternative_name_key",
                table: "c_specialization");

            migrationBuilder.AddColumn<string>(
                name: "hero_description_key",
                table: "c_specialization",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "hero_title_key",
                table: "c_specialization",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "page_description_key",
                table: "c_specialization",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "page_title_key",
                table: "c_specialization",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "specialization_category_id",
                table: "c_specialization",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "subject_id",
                table: "c_specialization",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "c_specialization_category",
                columns: table => new
                {
                    specialization_category_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_specialization_category", x => x.specialization_category_id);
                });

            migrationBuilder.InsertData(
                table: "c_specialization_category",
                columns: new[] { "specialization_category_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "specialization_category.popular.description", "popular", "specialization_category.popular.name" },
                    { 2, "specialization_category.other.description", "other", "specialization_category.other.name" },
                    { 3, "specialization_category.accent.description", "accent", "specialization_category.accent.name" },
                    { 4, "specialization_category.tests.description", "tests", "specialization_category.tests.name" }
                });

            migrationBuilder.UpdateData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 1,
                columns: new[] { "description_key", "enum_key", "hero_description_key", "hero_title_key", "icon", "name_key", "page_description_key", "page_title_key", "specialization_category_id", "subject_id" },
                values: new object[] { "specialization.english_for_children.description", "english_for_children", "specialization.english_for_children.hero.description", "specialization.english_for_children.hero.title", "kids", "specialization.english_for_children.name", "specialization.english_for_children.page.description", "specialization.english_for_children.page.title", 1, 1 });

            migrationBuilder.UpdateData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 2,
                columns: new[] { "description_key", "enum_key", "hero_description_key", "hero_title_key", "name_key", "page_description_key", "page_title_key", "specialization_category_id", "subject_id" },
                values: new object[] { "specialization.business_english.description", "business_english", "specialization.business_english.hero.description", "specialization.business_english.hero.title", "specialization.business_english.name", "specialization.business_english.page.description", "specialization.business_english.page.title", 1, 1 });

            migrationBuilder.UpdateData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 3,
                columns: new[] { "description_key", "enum_key", "hero_description_key", "hero_title_key", "icon", "name_key", "page_description_key", "page_title_key", "specialization_category_id", "subject_id" },
                values: new object[] { "specialization.conversational_english.description", "conversational_english", "specialization.conversational_english.hero.description", "specialization.conversational_english.hero.title", "connect_people", "specialization.conversational_english.name", "specialization.conversational_english.page.description", "specialization.conversational_english.page.title", 1, 1 });

            migrationBuilder.UpdateData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 4,
                columns: new[] { "description_key", "enum_key", "hero_description_key", "hero_title_key", "icon", "name_key", "page_description_key", "page_title_key", "specialization_category_id", "subject_id" },
                values: new object[] { "specialization.english_intensive_course.description", "english_intensive_course", "specialization.english_intensive_course.hero.description", "specialization.english_intensive_course.hero.title", "woman_superhero", "specialization.english_intensive_course.name", "specialization.english_intensive_course.page.description", "specialization.english_intensive_course.page.title", 1, 1 });

            migrationBuilder.UpdateData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 5,
                columns: new[] { "description_key", "enum_key", "hero_description_key", "hero_title_key", "icon", "name_key", "page_description_key", "page_title_key", "specialization_category_id", "subject_id" },
                values: new object[] { "specialization.english_from_zero.description", "english_from_zero", "specialization.english_from_zero.hero.description", "specialization.english_from_zero.hero.title", "chart_up", "specialization.english_from_zero.name", "specialization.english_from_zero.page.description", "specialization.english_from_zero.page.title", 1, 1 });

            migrationBuilder.UpdateData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 6,
                columns: new[] { "description_key", "enum_key", "hero_description_key", "hero_title_key", "icon", "name_key", "page_description_key", "page_title_key", "specialization_category_id", "subject_id" },
                values: new object[] { "specialization.english_for_travelling.description", "english_for_travelling", "specialization.english_for_travelling.hero.description", "specialization.english_for_travelling.hero.title", "airplane", "specialization.english_for_travelling.name", "specialization.english_for_travelling.page.description", "specialization.english_for_travelling.page.title", 2, 1 });

            migrationBuilder.UpdateData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 7,
                columns: new[] { "description_key", "enum_key", "hero_description_key", "hero_title_key", "icon", "name_key", "page_description_key", "page_title_key", "specialization_category_id", "subject_id" },
                values: new object[] { "specialization.english_for_high_school_students.description", "english_for_high_school_students", "specialization.english_for_high_school_students.hero.description", "specialization.english_for_high_school_students.hero.title", "education", "specialization.english_for_high_school_students.name", "specialization.english_for_high_school_students.page.description", "specialization.english_for_high_school_students.page.title", 2, 1 });

            migrationBuilder.InsertData(
                table: "c_specialization",
                columns: new[] { "specialization_id", "description_key", "enum_key", "hero_description_key", "hero_title_key", "icon", "name_key", "page_description_key", "page_title_key", "specialization_category_id", "subject_id" },
                values: new object[,]
                {
                    { 8, "specialization.toeic_preparation.description", "toeic_preparation", "specialization.toeic_preparation.hero.description", "specialization.toeic_preparation.hero.title", "writing_hand", "specialization.toeic_preparation.name", "specialization.toeic_preparation.page.description", "specialization.toeic_preparation.page.title", 4, 1 },
                    { 9, "specialization.toefl_preparation.description", "toefl_preparation", "specialization.toefl_preparation.hero.description", "specialization.toefl_preparation.hero.title", "writing_hand", "specialization.toefl_preparation.name", "specialization.toefl_preparation.page.description", "specialization.toefl_preparation.page.title", 4, 1 },
                    { 10, "specialization.ielts_preparation.description", "ielts_preparation", "specialization.ielts_preparation.hero.description", "specialization.ielts_preparation.hero.title", "writing_hand", "specialization.ielts_preparation.name", "specialization.ielts_preparation.page.description", "specialization.ielts_preparation.page.title", 4, 1 },
                    { 11, "specialization.cambridge_ket_preparation.description", "cambridge_ket_preparation", "specialization.cambridge_ket_preparation.hero.description", "specialization.cambridge_ket_preparation.hero.title", "writing_hand", "specialization.cambridge_ket_preparation.name", "specialization.cambridge_ket_preparation.page.description", "specialization.cambridge_ket_preparation.page.title", 4, 1 },
                    { 12, "specialization.cambridge_pet_preparation.description", "cambridge_pet_preparation", "specialization.cambridge_pet_preparation.hero.description", "specialization.cambridge_pet_preparation.hero.title", "writing_hand", "specialization.cambridge_pet_preparation.name", "specialization.cambridge_pet_preparation.page.description", "specialization.cambridge_pet_preparation.page.title", 4, 1 },
                    { 13, "specialization.cambridge_fce_preparation.description", "cambridge_fce_preparation", "specialization.cambridge_fce_preparation.hero.description", "specialization.cambridge_fce_preparation.hero.title", "writing_hand", "specialization.cambridge_fce_preparation.name", "specialization.cambridge_fce_preparation.page.description", "specialization.cambridge_fce_preparation.page.title", 4, 1 },
                    { 14, "specialization.cambridge_cae_preparation.description", "cambridge_cae_preparation", "specialization.cambridge_cae_preparation.hero.description", "specialization.cambridge_cae_preparation.hero.title", "writing_hand", "specialization.cambridge_cae_preparation.name", "specialization.cambridge_cae_preparation.page.description", "specialization.cambridge_cae_preparation.page.title", 4, 1 },
                    { 15, "specialization.cambridge_cpe_preparation.description", "cambridge_cpe_preparation", "specialization.cambridge_cpe_preparation.hero.description", "specialization.cambridge_cpe_preparation.hero.title", "writing_hand", "specialization.cambridge_cpe_preparation.name", "specialization.cambridge_cpe_preparation.page.description", "specialization.cambridge_cpe_preparation.page.title", 4, 1 },
                    { 16, "specialization.british_accent.description", "british_accent", "specialization.british_accent.hero.description", "specialization.british_accent.hero.title", "british_flag", "specialization.british_accent.name", "specialization.british_accent.page.description", "specialization.british_accent.page.title", 3, 1 },
                    { 17, "specialization.american_accent.description", "american_accent", "specialization.american_accent.hero.description", "specialization.american_accent.hero.title", "usa_flag", "specialization.american_accent.name", "specialization.american_accent.page.description", "specialization.american_accent.page.title", 3, 1 },
                    { 18, "specialization.australian_accent.description", "australian_accent", "specialization.australian_accent.hero.description", "specialization.australian_accent.hero.title", "australia_flag", "specialization.australian_accent.name", "specialization.australian_accent.page.description", "specialization.australian_accent.page.title", 3, 1 },
                    { 19, "specialization.new_zealand_accent.description", "new_zealand_accent", "specialization.new_zealand_accent.hero.description", "specialization.new_zealand_accent.hero.title", "new_zeland_flag", "specialization.new_zealand_accent.name", "specialization.new_zealand_accent.page.description", "specialization.new_zealand_accent.page.title", 3, 1 },
                    { 20, "specialization.czech_for_foreigners.description", "czech_for_foreigners", "specialization.czech_for_foreigners.hero.description", "specialization.czech_for_foreigners.hero.title", "passport_control", "specialization.czech_for_foreigners.name", "specialization.czech_for_foreigners.page.description", "specialization.czech_for_foreigners.page.title", 1, 2 },
                    { 21, "specialization.czech_for_beginners.description", "czech_for_beginners", "specialization.czech_for_beginners.hero.description", "specialization.czech_for_beginners.hero.title", "chart_up", "specialization.czech_for_beginners.name", "specialization.czech_for_beginners.page.description", "specialization.czech_for_beginners.page.title", 1, 2 },
                    { 22, "specialization.business_czech.description", "business_czech", "specialization.business_czech.hero.description", "specialization.business_czech.hero.title", "career", "specialization.business_czech.name", "specialization.business_czech.page.description", "specialization.business_czech.page.title", 1, 2 },
                    { 23, "specialization.conversational_czech.description", "conversational_czech", "specialization.conversational_czech.hero.description", "specialization.conversational_czech.hero.title", "connect_people", "specialization.conversational_czech.name", "specialization.conversational_czech.page.description", "specialization.conversational_czech.page.title", 1, 2 },
                    { 24, "specialization.czech_intensive_course.description", "czech_intensive_course", "specialization.czech_intensive_course.hero.description", "specialization.czech_intensive_course.hero.title", "woman_superhero", "specialization.czech_intensive_course.name", "specialization.czech_intensive_course.page.description", "specialization.czech_intensive_course.page.title", 1, 2 },
                    { 25, "specialization.czech_for_children.description", "czech_for_children", "specialization.czech_for_children.hero.description", "specialization.czech_for_children.hero.title", "kids", "specialization.czech_for_children.name", "specialization.czech_for_children.page.description", "specialization.czech_for_children.page.title", 1, 2 },
                    { 26, "specialization.czech_language_exam_for_permanent_residence.description", "czech_language_exam_for_permanent_residence", "specialization.czech_language_exam_for_permanent_residence.hero.description", "specialization.czech_language_exam_for_permanent_residence.hero.title", "writing_hand", "specialization.czech_language_exam_for_permanent_residence.name", "specialization.czech_language_exam_for_permanent_residence.page.description", "specialization.czech_language_exam_for_permanent_residence.page.title", 4, 2 },
                    { 27, "specialization.czech_language_exam_for_citizenship.description", "czech_language_exam_for_citizenship", "specialization.czech_language_exam_for_citizenship.hero.description", "specialization.czech_language_exam_for_citizenship.hero.title", "writing_hand", "specialization.czech_language_exam_for_citizenship.name", "specialization.czech_language_exam_for_citizenship.page.description", "specialization.czech_language_exam_for_citizenship.page.title", 4, 2 },
                    { 28, "specialization.czech_state_examination.description", "czech_state_examination", "specialization.czech_state_examination.hero.description", "specialization.czech_state_examination.hero.title", "writing_hand", "specialization.czech_state_examination.name", "specialization.czech_state_examination.page.description", "specialization.czech_state_examination.page.title", 4, 2 },
                    { 29, "specialization.czech_for_foreigners_a2.description", "czech_for_foreigners_a2", "specialization.czech_for_foreigners_a2.hero.description", "specialization.czech_for_foreigners_a2.hero.title", "writing_hand", "specialization.czech_for_foreigners_a2.name", "specialization.czech_for_foreigners_a2.page.description", "specialization.czech_for_foreigners_a2.page.title", 4, 2 },
                    { 30, "specialization.czech_for_foreigners_b1.description", "czech_for_foreigners_b1", "specialization.czech_for_foreigners_b1.hero.description", "specialization.czech_for_foreigners_b1.hero.title", "writing_hand", "specialization.czech_for_foreigners_b1.name", "specialization.czech_for_foreigners_b1.page.description", "specialization.czech_for_foreigners_b1.page.title", 4, 2 },
                    { 31, "specialization.czech_for_foreigners_b2.description", "czech_for_foreigners_b2", "specialization.czech_for_foreigners_b2.hero.description", "specialization.czech_for_foreigners_b2.hero.title", "writing_hand", "specialization.czech_for_foreigners_b2.name", "specialization.czech_for_foreigners_b2.page.description", "specialization.czech_for_foreigners_b2.page.title", 4, 2 },
                    { 32, "specialization.czech_for_foreigners_c1.description", "czech_for_foreigners_c1", "specialization.czech_for_foreigners_c1.hero.description", "specialization.czech_for_foreigners_c1.hero.title", "writing_hand", "specialization.czech_for_foreigners_c1.name", "specialization.czech_for_foreigners_c1.page.description", "specialization.czech_for_foreigners_c1.page.title", 4, 2 },
                    { 33, "specialization.spanish_for_beginners.description", "spanish_for_beginners", "specialization.spanish_for_beginners.hero.description", "specialization.spanish_for_beginners.hero.title", "chart_up", "specialization.spanish_for_beginners.name", "specialization.spanish_for_beginners.page.description", "specialization.spanish_for_beginners.page.title", 1, 4 },
                    { 34, "specialization.business_spanish.description", "business_spanish", "specialization.business_spanish.hero.description", "specialization.business_spanish.hero.title", "career", "specialization.business_spanish.name", "specialization.business_spanish.page.description", "specialization.business_spanish.page.title", 1, 4 },
                    { 35, "specialization.conversational_spanish.description", "conversational_spanish", "specialization.conversational_spanish.hero.description", "specialization.conversational_spanish.hero.title", "connect_people", "specialization.conversational_spanish.name", "specialization.conversational_spanish.page.description", "specialization.conversational_spanish.page.title", 1, 4 },
                    { 36, "specialization.spanish_intensive_course.description", "spanish_intensive_course", "specialization.spanish_intensive_course.hero.description", "specialization.spanish_intensive_course.hero.title", "woman_superhero", "specialization.spanish_intensive_course.name", "specialization.spanish_intensive_course.page.description", "specialization.spanish_intensive_course.page.title", 1, 4 },
                    { 37, "specialization.spanish_for_children.description", "spanish_for_children", "specialization.spanish_for_children.hero.description", "specialization.spanish_for_children.hero.title", "kids", "specialization.spanish_for_children.name", "specialization.spanish_for_children.page.description", "specialization.spanish_for_children.page.title", 1, 4 },
                    { 38, "specialization.european_spanish.description", "european_spanish", "specialization.european_spanish.hero.description", "specialization.european_spanish.hero.title", "europe_flag", "specialization.european_spanish.name", "specialization.european_spanish.page.description", "specialization.european_spanish.page.title", 1, 4 },
                    { 39, "specialization.dele_preparation.description", "dele_preparation", "specialization.dele_preparation.hero.description", "specialization.dele_preparation.hero.title", "writing_hand", "specialization.dele_preparation.name", "specialization.dele_preparation.page.description", "specialization.dele_preparation.page.title", 4, 4 },
                    { 40, "specialization.siele_preparation.description", "siele_preparation", "specialization.siele_preparation.hero.description", "specialization.siele_preparation.hero.title", "writing_hand", "specialization.siele_preparation.name", "specialization.siele_preparation.page.description", "specialization.siele_preparation.page.title", 4, 4 },
                    { 41, "specialization.german_for_beginners.description", "german_for_beginners", "specialization.german_for_beginners.hero.description", "specialization.german_for_beginners.hero.title", "chart_up", "specialization.german_for_beginners.name", "specialization.german_for_beginners.page.description", "specialization.german_for_beginners.page.title", 1, 5 },
                    { 42, "specialization.business_german.description", "business_german", "specialization.business_german.hero.description", "specialization.business_german.hero.title", "career", "specialization.business_german.name", "specialization.business_german.page.description", "specialization.business_german.page.title", 1, 5 },
                    { 43, "specialization.conversational_german.description", "conversational_german", "specialization.conversational_german.hero.description", "specialization.conversational_german.hero.title", "connect_people", "specialization.conversational_german.name", "specialization.conversational_german.page.description", "specialization.conversational_german.page.title", 1, 5 },
                    { 44, "specialization.german_intensive_course.description", "german_intensive_course", "specialization.german_intensive_course.hero.description", "specialization.german_intensive_course.hero.title", "woman_superhero", "specialization.german_intensive_course.name", "specialization.german_intensive_course.page.description", "specialization.german_intensive_course.page.title", 1, 5 },
                    { 45, "specialization.german_for_children.description", "german_for_children", "specialization.german_for_children.hero.description", "specialization.german_for_children.hero.title", "kids", "specialization.german_for_children.name", "specialization.german_for_children.page.description", "specialization.german_for_children.page.title", 1, 5 },
                    { 46, "specialization.dsh_preparation.description", "dsh_preparation", "specialization.dsh_preparation.hero.description", "specialization.dsh_preparation.hero.title", "writing_hand", "specialization.dsh_preparation.name", "specialization.dsh_preparation.page.description", "specialization.dsh_preparation.page.title", 4, 5 },
                    { 47, "specialization.goethe_preparation.description", "goethe_preparation", "specialization.goethe_preparation.hero.description", "specialization.goethe_preparation.hero.title", "writing_hand", "specialization.goethe_preparation.name", "specialization.goethe_preparation.page.description", "specialization.goethe_preparation.page.title", 4, 5 },
                    { 48, "specialization.testdaf_preparation.description", "testdaf_preparation", "specialization.testdaf_preparation.hero.description", "specialization.testdaf_preparation.hero.title", "writing_hand", "specialization.testdaf_preparation.name", "specialization.testdaf_preparation.page.description", "specialization.testdaf_preparation.page.title", 4, 5 },
                    { 49, "specialization.french_for_beginners.description", "french_for_beginners", "specialization.french_for_beginners.hero.description", "specialization.french_for_beginners.hero.title", "chart_up", "specialization.french_for_beginners.name", "specialization.french_for_beginners.page.description", "specialization.french_for_beginners.page.title", 1, 3 },
                    { 50, "specialization.business_french.description", "business_french", "specialization.business_french.hero.description", "specialization.business_french.hero.title", "career", "specialization.business_french.name", "specialization.business_french.page.description", "specialization.business_french.page.title", 1, 3 },
                    { 51, "specialization.conversational_french.description", "conversational_french", "specialization.conversational_french.hero.description", "specialization.conversational_french.hero.title", "connect_people", "specialization.conversational_french.name", "specialization.conversational_french.page.description", "specialization.conversational_french.page.title", 1, 3 },
                    { 52, "specialization.french_intensive_course.description", "french_intensive_course", "specialization.french_intensive_course.hero.description", "specialization.french_intensive_course.hero.title", "woman_superhero", "specialization.french_intensive_course.name", "specialization.french_intensive_course.page.description", "specialization.french_intensive_course.page.title", 1, 3 },
                    { 53, "specialization.french_for_children.description", "french_for_children", "specialization.french_for_children.hero.description", "specialization.french_for_children.hero.title", "kids", "specialization.french_for_children.name", "specialization.french_for_children.page.description", "specialization.french_for_children.page.title", 1, 3 },
                    { 54, "specialization.delf_preparation.description", "delf_preparation", "specialization.delf_preparation.hero.description", "specialization.delf_preparation.hero.title", "writing_hand", "specialization.delf_preparation.name", "specialization.delf_preparation.page.description", "specialization.delf_preparation.page.title", 4, 3 },
                    { 55, "specialization.tfi_preparation.description", "tfi_preparation", "specialization.tfi_preparation.hero.description", "specialization.tfi_preparation.hero.title", "writing_hand", "specialization.tfi_preparation.name", "specialization.tfi_preparation.page.description", "specialization.tfi_preparation.page.title", 4, 3 },
                    { 56, "specialization.dalf_preparation.description", "dalf_preparation", "specialization.dalf_preparation.hero.description", "specialization.dalf_preparation.hero.title", "writing_hand", "specialization.dalf_preparation.name", "specialization.dalf_preparation.page.description", "specialization.dalf_preparation.page.title", 4, 3 },
                    { 57, "specialization.dfp_preparation.description", "dfp_preparation", "specialization.dfp_preparation.hero.description", "specialization.dfp_preparation.hero.title", "writing_hand", "specialization.dfp_preparation.name", "specialization.dfp_preparation.page.description", "specialization.dfp_preparation.page.title", 4, 3 },
                    { 58, "specialization.italian_for_beginners.description", "italian_for_beginners", "specialization.italian_for_beginners.hero.description", "specialization.italian_for_beginners.hero.title", "chart_up", "specialization.italian_for_beginners.name", "specialization.italian_for_beginners.page.description", "specialization.italian_for_beginners.page.title", 1, 6 },
                    { 59, "specialization.business_italian.description", "business_italian", "specialization.business_italian.hero.description", "specialization.business_italian.hero.title", "career", "specialization.business_italian.name", "specialization.business_italian.page.description", "specialization.business_italian.page.title", 1, 6 },
                    { 60, "specialization.conversational_italian.description", "conversational_italian", "specialization.conversational_italian.hero.description", "specialization.conversational_italian.hero.title", "connect_people", "specialization.conversational_italian.name", "specialization.conversational_italian.page.description", "specialization.conversational_italian.page.title", 1, 6 },
                    { 61, "specialization.italian_intensive_course.description", "italian_intensive_course", "specialization.italian_intensive_course.hero.description", "specialization.italian_intensive_course.hero.title", "woman_superhero", "specialization.italian_intensive_course.name", "specialization.italian_intensive_course.page.description", "specialization.italian_intensive_course.page.title", 1, 6 },
                    { 62, "specialization.italian_for_children.description", "italian_for_children", "specialization.italian_for_children.hero.description", "specialization.italian_for_children.hero.title", "kids", "specialization.italian_for_children.name", "specialization.italian_for_children.page.description", "specialization.italian_for_children.page.title", 1, 6 },
                    { 63, "specialization.korean_for_beginners.description", "korean_for_beginners", "specialization.korean_for_beginners.hero.description", "specialization.korean_for_beginners.hero.title", "chart_up", "specialization.korean_for_beginners.name", "specialization.korean_for_beginners.page.description", "specialization.korean_for_beginners.page.title", 1, 7 },
                    { 64, "specialization.business_korean.description", "business_korean", "specialization.business_korean.hero.description", "specialization.business_korean.hero.title", "career", "specialization.business_korean.name", "specialization.business_korean.page.description", "specialization.business_korean.page.title", 1, 7 },
                    { 65, "specialization.conversational_korean.description", "conversational_korean", "specialization.conversational_korean.hero.description", "specialization.conversational_korean.hero.title", "connect_people", "specialization.conversational_korean.name", "specialization.conversational_korean.page.description", "specialization.conversational_korean.page.title", 1, 7 },
                    { 66, "specialization.korean_intensive_course.description", "korean_intensive_course", "specialization.korean_intensive_course.hero.description", "specialization.korean_intensive_course.hero.title", "woman_superhero", "specialization.korean_intensive_course.name", "specialization.korean_intensive_course.page.description", "specialization.korean_intensive_course.page.title", 1, 7 },
                    { 67, "specialization.korean_for_children.description", "korean_for_children", "specialization.korean_for_children.hero.description", "specialization.korean_for_children.hero.title", "kids", "specialization.korean_for_children.name", "specialization.korean_for_children.page.description", "specialization.korean_for_children.page.title", 1, 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_c_specialization_specialization_category_id",
                table: "c_specialization",
                column: "specialization_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_c_specialization_subject_id",
                table: "c_specialization",
                column: "subject_id");

            migrationBuilder.AddForeignKey(
                name: "fk_c_specialization_c_specialization_category_specialization_category_id",
                table: "c_specialization",
                column: "specialization_category_id",
                principalTable: "c_specialization_category",
                principalColumn: "specialization_category_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_c_specialization_c_subject_subject_id",
                table: "c_specialization",
                column: "subject_id",
                principalTable: "c_subject",
                principalColumn: "subject_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_c_specialization_c_specialization_category_specialization_category_id",
                table: "c_specialization");

            migrationBuilder.DropForeignKey(
                name: "fk_c_specialization_c_subject_subject_id",
                table: "c_specialization");

            migrationBuilder.DropTable(
                name: "c_specialization_category");

            migrationBuilder.DropIndex(
                name: "IX_c_specialization_specialization_category_id",
                table: "c_specialization");

            migrationBuilder.DropIndex(
                name: "IX_c_specialization_subject_id",
                table: "c_specialization");

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 67);

            migrationBuilder.DropColumn(
                name: "hero_description_key",
                table: "c_specialization");

            migrationBuilder.DropColumn(
                name: "hero_title_key",
                table: "c_specialization");

            migrationBuilder.DropColumn(
                name: "page_description_key",
                table: "c_specialization");

            migrationBuilder.DropColumn(
                name: "page_title_key",
                table: "c_specialization");

            migrationBuilder.DropColumn(
                name: "specialization_category_id",
                table: "c_specialization");

            migrationBuilder.DropColumn(
                name: "subject_id",
                table: "c_specialization");

            migrationBuilder.AddColumn<string>(
                name: "alternative_name_key",
                table: "c_specialization",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 1,
                columns: new[] { "alternative_name_key", "description_key", "enum_key", "icon", "name_key" },
                values: new object[] { "specialization.support_education.alternative", "specialization.support_education.description", "support_education", "education", "specialization.support_education.name" });

            migrationBuilder.UpdateData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 2,
                columns: new[] { "alternative_name_key", "description_key", "enum_key", "name_key" },
                values: new object[] { "specialization.boost_career.alternative", "specialization.boost_career.description", "boost_career", "specialization.boost_career.name" });

            migrationBuilder.UpdateData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 3,
                columns: new[] { "alternative_name_key", "description_key", "enum_key", "icon", "name_key" },
                values: new object[] { "specialization.move_abroad.alternative", "specialization.move_abroad.description", "move_abroad", "abroad", "specialization.move_abroad.name" });

            migrationBuilder.UpdateData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 4,
                columns: new[] { "alternative_name_key", "description_key", "enum_key", "icon", "name_key" },
                values: new object[] { "specialization.connect_with_people.alternative", "specialization.connect_with_people.description", "connect_with_people", "connect_people", "specialization.connect_with_people.name" });

            migrationBuilder.UpdateData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 5,
                columns: new[] { "alternative_name_key", "description_key", "enum_key", "icon", "name_key" },
                values: new object[] { "specialization.language_for_kids.alternative", "specialization.language_for_kids.description", "language_for_kids", "kids", "specialization.language_for_kids.name" });

            migrationBuilder.UpdateData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 6,
                columns: new[] { "alternative_name_key", "description_key", "enum_key", "icon", "name_key" },
                values: new object[] { "specialization.self_improvement.alternative", "specialization.self_improvement.description", "self_improvement", "productivity", "specialization.self_improvement.name" });

            migrationBuilder.UpdateData(
                table: "c_specialization",
                keyColumn: "specialization_id",
                keyValue: 7,
                columns: new[] { "alternative_name_key", "description_key", "enum_key", "icon", "name_key" },
                values: new object[] { "specialization.just_for_fun.alternative", "specialization.just_for_fun.description", "just_for_fun", "fun", "specialization.just_for_fun.name" });
        }
    }
}
