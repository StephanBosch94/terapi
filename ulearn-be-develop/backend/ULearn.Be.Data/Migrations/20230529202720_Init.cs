using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "c_category",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_category", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "c_country",
                columns: table => new
                {
                    country_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    country_code = table.Column<string>(type: "text", nullable: true),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_country", x => x.country_id);
                });

            migrationBuilder.CreateTable(
                name: "c_currency",
                columns: table => new
                {
                    currency_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_currency", x => x.currency_id);
                });

            migrationBuilder.CreateTable(
                name: "c_file_type",
                columns: table => new
                {
                    file_type_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_file_type", x => x.file_type_id);
                });

            migrationBuilder.CreateTable(
                name: "c_language",
                columns: table => new
                {
                    language_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_language", x => x.language_id);
                });

            migrationBuilder.CreateTable(
                name: "c_language_level",
                columns: table => new
                {
                    language_level_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_language_level", x => x.language_level_id);
                });

            migrationBuilder.CreateTable(
                name: "c_master_hash",
                columns: table => new
                {
                    master_hash_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    table_name = table.Column<string>(type: "text", nullable: true),
                    hash = table.Column<Guid>(type: "uuid", nullable: false),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_master_hash", x => x.master_hash_id);
                });

            migrationBuilder.CreateTable(
                name: "c_message_state",
                columns: table => new
                {
                    message_state_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_message_state", x => x.message_state_id);
                });

            migrationBuilder.CreateTable(
                name: "c_object_state",
                columns: table => new
                {
                    object_state_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("object_state_id", x => x.object_state_id);
                });

            migrationBuilder.CreateTable(
                name: "c_payment_service_provider",
                columns: table => new
                {
                    payment_service_provider_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_payment_service_provider", x => x.payment_service_provider_id);
                });

            migrationBuilder.CreateTable(
                name: "c_reservation_state",
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
                    table.PrimaryKey("pk_c_reservation_state", x => x.reservation_state_id);
                });

            migrationBuilder.CreateTable(
                name: "c_room_activity_type",
                columns: table => new
                {
                    room_activity_type_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_room_activity_type", x => x.room_activity_type_id);
                });

            migrationBuilder.CreateTable(
                name: "c_room_state",
                columns: table => new
                {
                    room_state_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_room_state", x => x.room_state_id);
                });

            migrationBuilder.CreateTable(
                name: "c_room_statistic_type",
                columns: table => new
                {
                    room_statistic_type_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_room_statistic_type", x => x.room_statistic_type_id);
                });

            migrationBuilder.CreateTable(
                name: "c_specialization",
                columns: table => new
                {
                    specialization_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    icon = table.Column<string>(type: "text", nullable: true),
                    alternative_name_key = table.Column<string>(type: "text", nullable: true),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_specialization", x => x.specialization_id);
                });

            migrationBuilder.CreateTable(
                name: "c_subject_level",
                columns: table => new
                {
                    subject_level_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    tutor_name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    student_name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    icon_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    inverted_icon_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_subject_level", x => x.subject_level_id);
                });

            migrationBuilder.CreateTable(
                name: "c_teaching_experience",
                columns: table => new
                {
                    teaching_experience_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_teaching_experience", x => x.teaching_experience_id);
                });

            migrationBuilder.CreateTable(
                name: "c_token_provider",
                columns: table => new
                {
                    token_provider_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_token_provider", x => x.token_provider_id);
                });

            migrationBuilder.CreateTable(
                name: "c_token_type",
                columns: table => new
                {
                    token_type_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_token_type", x => x.token_type_id);
                });

            migrationBuilder.CreateTable(
                name: "c_transaction_state",
                columns: table => new
                {
                    transaction_state_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_transaction_state", x => x.transaction_state_id);
                });

            migrationBuilder.CreateTable(
                name: "c_transaction_type",
                columns: table => new
                {
                    transaction_type_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_transaction_type", x => x.transaction_type_id);
                });

            migrationBuilder.CreateTable(
                name: "c_translation_language",
                columns: table => new
                {
                    translation_language_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    language_code = table.Column<string>(type: "text", nullable: true),
                    language = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_translation_language", x => x.translation_language_id);
                });

            migrationBuilder.CreateTable(
                name: "c_user_attribute_type",
                columns: table => new
                {
                    user_attribute_type_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_user_attribute", x => x.user_attribute_type_id);
                });

            migrationBuilder.CreateTable(
                name: "c_user_role",
                columns: table => new
                {
                    user_role_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_user_role", x => x.user_role_id);
                });

            migrationBuilder.CreateTable(
                name: "c_user_state",
                columns: table => new
                {
                    user_state_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_user_state", x => x.user_state_id);
                });

            migrationBuilder.CreateTable(
                name: "c_weekly_goal",
                columns: table => new
                {
                    weekly_goal_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_weekly_goal", x => x.weekly_goal_id);
                });

            migrationBuilder.CreateTable(
                name: "env_configuration_data_type",
                columns: table => new
                {
                    data_type_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_env_configuration_data_type", x => x.data_type_id);
                });

            migrationBuilder.CreateTable(
                name: "c_subject",
                columns: table => new
                {
                    subject_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    icon_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    hero_title_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    hero_description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    page_title_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    footer_subject_teachers_page_link_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    footer_subject_landing_page_link_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    object_state_id = table.Column<int>(type: "integer", nullable: false),
                    enum_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    name_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description_key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_subject", x => x.subject_id);
                    table.ForeignKey(
                        name: "fk_c_subject_c_category_category_id",
                        column: x => x.category_id,
                        principalTable: "c_category",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_c_subject_c_object_state_object_state_id",
                        column: x => x.object_state_id,
                        principalTable: "c_object_state",
                        principalColumn: "object_state_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "env_configuration",
                columns: table => new
                {
                    configuration_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    data_type_id = table.Column<int>(type: "integer", nullable: false),
                    configuration_key = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    configuration_value = table.Column<string>(type: "text", nullable: true),
                    admin_configurable = table.Column<bool>(type: "boolean", nullable: false),
                    backend_only = table.Column<bool>(type: "boolean", nullable: false),
                    value_origin = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_env_configuration", x => x.configuration_id);
                    table.ForeignKey(
                        name: "fk_env_configuration_env_configuration_data_type_data_type_id",
                        column: x => x.data_type_id,
                        principalTable: "env_configuration_data_type",
                        principalColumn: "data_type_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "app_user",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    first_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    last_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_state_id = table.Column<int>(type: "integer", nullable: false),
                    phone = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    user_role_id = table.Column<int>(type: "integer", nullable: false),
                    password = table.Column<string>(type: "text", nullable: true),
                    iana_timezone = table.Column<string>(type: "text", nullable: true),
                    country_id = table.Column<int>(type: "integer", nullable: true),
                    teaching_experience_id = table.Column<int>(type: "integer", nullable: true),
                    currency_id = table.Column<int>(type: "integer", nullable: true),
                    hour_rate = table.Column<int>(type: "integer", nullable: true),
                    subject_id = table.Column<int>(type: "integer", nullable: true),
                    excluded = table.Column<bool>(type: "boolean", nullable: false),
                    translation_language_id = table.Column<int>(type: "integer", nullable: true),
                    weekly_goal_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_user", x => x.user_id);
                    table.ForeignKey(
                        name: "fk_app_user_c_country_country_id",
                        column: x => x.country_id,
                        principalTable: "c_country",
                        principalColumn: "country_id");
                    table.ForeignKey(
                        name: "fk_app_user_c_currency_currency_id",
                        column: x => x.currency_id,
                        principalTable: "c_currency",
                        principalColumn: "currency_id");
                    table.ForeignKey(
                        name: "fk_app_user_c_subject_subject_id",
                        column: x => x.subject_id,
                        principalTable: "c_subject",
                        principalColumn: "subject_id");
                    table.ForeignKey(
                        name: "fk_app_user_c_teaching_experience_teaching_experience_id",
                        column: x => x.teaching_experience_id,
                        principalTable: "c_teaching_experience",
                        principalColumn: "teaching_experience_id");
                    table.ForeignKey(
                        name: "fk_app_user_c_translation_language_translation_language_id",
                        column: x => x.translation_language_id,
                        principalTable: "c_translation_language",
                        principalColumn: "translation_language_id");
                    table.ForeignKey(
                        name: "fk_app_user_c_user_role_user_role_id",
                        column: x => x.user_role_id,
                        principalTable: "c_user_role",
                        principalColumn: "user_role_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_user_c_user_state_user_state_id",
                        column: x => x.user_state_id,
                        principalTable: "c_user_state",
                        principalColumn: "user_state_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_user_c_weekly_goal_weekly_goal_id",
                        column: x => x.weekly_goal_id,
                        principalTable: "c_weekly_goal",
                        principalColumn: "weekly_goal_id");
                });

            migrationBuilder.CreateTable(
                name: "app_conversation",
                columns: table => new
                {
                    conversation_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    initiator_id = table.Column<int>(type: "integer", nullable: false),
                    opponent_id = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_conversation", x => x.conversation_id);
                    table.ForeignKey(
                        name: "fk_app_conversation_app_user_initiator_id",
                        column: x => x.initiator_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_conversation_app_user_opponent_id",
                        column: x => x.opponent_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "app_recurrent_timeslot",
                columns: table => new
                {
                    recurrent_timeslot_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    owner_id = table.Column<int>(type: "integer", nullable: false),
                    local_iso_weekday = table.Column<int>(type: "integer", nullable: false),
                    from_local_hour = table.Column<int>(type: "integer", nullable: false),
                    from_local_minute = table.Column<int>(type: "integer", nullable: false),
                    duration_in_minutes = table.Column<int>(type: "integer", nullable: false),
                    iana_timezone = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_recurrent_timeslot", x => x.recurrent_timeslot_id);
                    table.ForeignKey(
                        name: "fk_app_recurrent_timeslot_app_user_owner_id",
                        column: x => x.owner_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "app_room",
                columns: table => new
                {
                    room_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    room_key = table.Column<string>(type: "text", nullable: true),
                    room_state_id = table.Column<int>(type: "integer", nullable: false),
                    host_id = table.Column<int>(type: "integer", nullable: false),
                    attender_id = table.Column<int>(type: "integer", nullable: false),
                    scheduled_from_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    scheduled_to_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_room", x => x.room_id);
                    table.ForeignKey(
                        name: "fk_app_room_app_attender_attender_id",
                        column: x => x.attender_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_room_app_user_host_id",
                        column: x => x.host_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_room_c_room_state_room_state_id",
                        column: x => x.room_state_id,
                        principalTable: "c_room_state",
                        principalColumn: "room_state_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "app_timeslot",
                columns: table => new
                {
                    timeslot_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    owner_id = table.Column<int>(type: "integer", nullable: false),
                    from_local_year = table.Column<int>(type: "integer", nullable: false),
                    from_local_month = table.Column<int>(type: "integer", nullable: false),
                    from_local_day = table.Column<int>(type: "integer", nullable: false),
                    from_local_hour = table.Column<int>(type: "integer", nullable: false),
                    from_local_minute = table.Column<int>(type: "integer", nullable: false),
                    duration_in_minutes = table.Column<int>(type: "integer", nullable: false),
                    iana_timezone = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_timeslot", x => x.timeslot_id);
                    table.ForeignKey(
                        name: "fk_app_user_event_app_user_owner_id",
                        column: x => x.owner_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "app_user_behaviour",
                columns: table => new
                {
                    user_behaviour_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    connection_uuid = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    connected = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    disconnected = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_user_behaviour", x => x.user_behaviour_id);
                    table.ForeignKey(
                        name: "fk_app_user_behaviour_app_user_user_id",
                        column: x => x.user_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "app_user_registration_code",
                columns: table => new
                {
                    user_registration_code_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    registration_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    used = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_user_registration_code", x => x.user_registration_code_id);
                    table.ForeignKey(
                        name: "fk_app_user_registration_code_app_user_user_id",
                        column: x => x.user_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "app_user_review",
                columns: table => new
                {
                    user_review_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    reviewer_id = table.Column<int>(type: "integer", nullable: false),
                    reviewee_id = table.Column<int>(type: "integer", nullable: false),
                    rate = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_user_review", x => x.user_review_id);
                    table.ForeignKey(
                        name: "fk_app_user_review_app_user_reviewee_id",
                        column: x => x.reviewee_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_user_review_app_user_reviewer_id",
                        column: x => x.reviewer_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "app_user_wallet",
                columns: table => new
                {
                    user_wallet_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    currency_id = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<double>(type: "double precision", nullable: false),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_user_wallet", x => x.user_wallet_id);
                    table.ForeignKey(
                        name: "fk_app_user_wallet_app_user_user_id",
                        column: x => x.user_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_user_wallet_c_currency_currency_id",
                        column: x => x.currency_id,
                        principalTable: "c_currency",
                        principalColumn: "currency_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "c_translation_namespace",
                columns: table => new
                {
                    translation_namespace_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creator_id = table.Column<int>(type: "integer", nullable: false),
                    updater_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_translation_namespace", x => x.translation_namespace_id);
                    table.ForeignKey(
                        name: "fk_c_translation_namespace_app_user_creator_id",
                        column: x => x.creator_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_c_translation_namespace_app_user_updater_id",
                        column: x => x.updater_id,
                        principalTable: "app_user",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "r_user_attribute",
                columns: table => new
                {
                    user_attribute_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    user_attribute_type_id = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_r_user_attribute", x => x.user_attribute_id);
                    table.ForeignKey(
                        name: "fk_r_user_attribute_app_user_user_id",
                        column: x => x.user_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_r_user_attribute_c_user_attribute_type",
                        column: x => x.user_attribute_type_id,
                        principalTable: "c_user_attribute_type",
                        principalColumn: "user_attribute_type_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "r_user_file",
                columns: table => new
                {
                    user_file_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    file_type_id = table.Column<int>(type: "integer", nullable: false),
                    file_name = table.Column<string>(type: "text", nullable: true),
                    content_type = table.Column<string>(type: "text", nullable: true),
                    file_path = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_r_user_file", x => x.user_file_id);
                    table.ForeignKey(
                        name: "fk_r_user_file_app_user_user_id",
                        column: x => x.user_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_r_user_file_c_file_type_file_type_id",
                        column: x => x.file_type_id,
                        principalTable: "c_file_type",
                        principalColumn: "file_type_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "r_user_file_permission",
                columns: table => new
                {
                    user_file_permission_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    file_path = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_r_user_file_permission", x => x.user_file_permission_id);
                    table.ForeignKey(
                        name: "fk_r_user_file_permission_app_user_user_id",
                        column: x => x.user_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "r_user_language",
                columns: table => new
                {
                    user_language_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    language_id = table.Column<int>(type: "integer", nullable: false),
                    language_level_id = table.Column<int>(type: "integer", nullable: false),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_r_user_language", x => x.user_language_id);
                    table.ForeignKey(
                        name: "fk_r_user_language_app_user_user_id",
                        column: x => x.user_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_r_user_language_c_language_language_id",
                        column: x => x.language_id,
                        principalTable: "c_language",
                        principalColumn: "language_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_r_user_language_c_language_level_language_level_id",
                        column: x => x.language_level_id,
                        principalTable: "c_language_level",
                        principalColumn: "language_level_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "r_user_specialization",
                columns: table => new
                {
                    user_specialization_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    specialization_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_r_user_specialization", x => x.user_specialization_id);
                    table.ForeignKey(
                        name: "fk_r_user_specialization_app_user_user_id",
                        column: x => x.user_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_r_user_specialization_c_specialization_specialization_id",
                        column: x => x.specialization_id,
                        principalTable: "c_specialization",
                        principalColumn: "specialization_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "r_user_subject_level",
                columns: table => new
                {
                    user_subject_level_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    subject_level_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_r_user_subject_level", x => x.user_subject_level_id);
                    table.ForeignKey(
                        name: "fk_r_user_subject_level_app_user_user_id",
                        column: x => x.user_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_r_user_subject_level_c_subject_level_subject_level_id",
                        column: x => x.subject_level_id,
                        principalTable: "c_subject_level",
                        principalColumn: "subject_level_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "r_user_token",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    token_type_id = table.Column<int>(type: "integer", nullable: false),
                    token_provider_id = table.Column<int>(type: "integer", nullable: false),
                    token_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_r_user_token", x => new { x.user_id, x.token_type_id });
                    table.ForeignKey(
                        name: "fk_r_user_token_app_user_user_id",
                        column: x => x.user_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_r_user_token_c_token_provider_token_provider_id",
                        column: x => x.token_provider_id,
                        principalTable: "c_token_provider",
                        principalColumn: "token_provider_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_r_user_token_c_token_type_token_type_id",
                        column: x => x.token_type_id,
                        principalTable: "c_token_type",
                        principalColumn: "token_type_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "app_message",
                columns: table => new
                {
                    message_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    conversation_id = table.Column<int>(type: "integer", nullable: false),
                    sender_id = table.Column<int>(type: "integer", nullable: false),
                    receiver_id = table.Column<int>(type: "integer", nullable: false),
                    message_state_id = table.Column<int>(type: "integer", nullable: false),
                    text_value = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_message_message_id", x => x.message_id);
                    table.ForeignKey(
                        name: "fk_app_message_app_conversation_conversation_id",
                        column: x => x.conversation_id,
                        principalTable: "app_conversation",
                        principalColumn: "conversation_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_message_app_user_receiver_id",
                        column: x => x.receiver_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_message_app_user_sender_id",
                        column: x => x.sender_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_message_c_message_state_message_state_id",
                        column: x => x.message_state_id,
                        principalTable: "c_message_state",
                        principalColumn: "message_state_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "app_recurrent_timeslot_blacklist",
                columns: table => new
                {
                    recurrent_timeslot_blacklist_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    recurrent_timeslot_id = table.Column<int>(type: "integer", nullable: false),
                    local_year = table.Column<int>(type: "integer", nullable: false),
                    local_month = table.Column<int>(type: "integer", nullable: false),
                    local_day = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_recurrent_timeslot_blacklist", x => x.recurrent_timeslot_blacklist_id);
                    table.ForeignKey(
                        name: "fk_app_recurrent_timeslot_blacklist_app_recurrent_timeslot_recurrent_timeslot_id",
                        column: x => x.recurrent_timeslot_id,
                        principalTable: "app_recurrent_timeslot",
                        principalColumn: "recurrent_timeslot_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "app_reservation",
                columns: table => new
                {
                    reservation_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    subject_id = table.Column<int>(type: "integer", nullable: false),
                    host_id = table.Column<int>(type: "integer", nullable: false),
                    attender_id = table.Column<int>(type: "integer", nullable: false),
                    reservation_state_id = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    room_id = table.Column<int>(type: "integer", nullable: false),
                    host_event_iana_timezone = table.Column<string>(type: "text", nullable: true),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_reservation", x => x.reservation_id);
                    table.ForeignKey(
                        name: "fk_app_reservation_app_room_room_id",
                        column: x => x.room_id,
                        principalTable: "app_room",
                        principalColumn: "room_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_reservation_app_user_attender_id",
                        column: x => x.attender_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_reservation_app_user_host_id",
                        column: x => x.host_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_reservation_c_reservation_state_reservation_state_id",
                        column: x => x.reservation_state_id,
                        principalTable: "c_reservation_state",
                        principalColumn: "reservation_state_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_reservation_c_subject_subject_id",
                        column: x => x.subject_id,
                        principalTable: "c_subject",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "app_room_activity",
                columns: table => new
                {
                    room_activity_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    room_id = table.Column<int>(type: "integer", nullable: false),
                    room_activity_type_id = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_room_activity", x => x.room_activity_id);
                    table.ForeignKey(
                        name: "fk_app_room_activity_app_room_room_id",
                        column: x => x.room_id,
                        principalTable: "app_room",
                        principalColumn: "room_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_room_activity_app_user_user_id",
                        column: x => x.user_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_room_activity_c_room_activity_type_room_activity_type_id",
                        column: x => x.room_activity_type_id,
                        principalTable: "c_room_activity_type",
                        principalColumn: "room_activity_type_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "r_user_room_statistic",
                columns: table => new
                {
                    user_room_statistic_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    room_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    room_statistics_type_id = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    value = table.Column<double>(type: "double precision", nullable: false),
                    text_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_r_user_room_statistic", x => x.user_room_statistic_id);
                    table.ForeignKey(
                        name: "fk_r_user_room_statistic_app_room_room_id",
                        column: x => x.room_id,
                        principalTable: "app_room",
                        principalColumn: "room_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_r_user_room_statistic_app_user_user_id",
                        column: x => x.user_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_r_user_room_statistic_c_room_statistic_type_room_statistic_type_id",
                        column: x => x.room_statistics_type_id,
                        principalTable: "c_room_statistic_type",
                        principalColumn: "room_statistic_type_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "app_user_wallet_transaction",
                columns: table => new
                {
                    user_wallet_transaction_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_wallet_id = table.Column<int>(type: "integer", nullable: false),
                    transaction_type_id = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<double>(type: "double precision", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    transaction_state_id = table.Column<int>(type: "integer", nullable: false),
                    payment_service_provider_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_user_wallet_transaction", x => x.user_wallet_transaction_id);
                    table.ForeignKey(
                        name: "fk_app_user_wallet_transaction_app_user_waller_user_wallet_id",
                        column: x => x.user_wallet_id,
                        principalTable: "app_user_wallet",
                        principalColumn: "user_wallet_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_user_wallet_transaction_c_transaction_state",
                        column: x => x.transaction_state_id,
                        principalTable: "c_transaction_state",
                        principalColumn: "transaction_state_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_user_wallet_transaction_c_transaction_type",
                        column: x => x.transaction_type_id,
                        principalTable: "c_transaction_type",
                        principalColumn: "transaction_type_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_user_wallet_transaction_payment_service_provider",
                        column: x => x.payment_service_provider_id,
                        principalTable: "c_payment_service_provider",
                        principalColumn: "payment_service_provider_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "c_translation_key",
                columns: table => new
                {
                    key_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    key_name = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creator_id = table.Column<int>(type: "integer", nullable: false),
                    updater_id = table.Column<int>(type: "integer", nullable: true),
                    translation_namespace_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_translation_key", x => x.key_id);
                    table.ForeignKey(
                        name: "fk_c_translation_key_app_user_creator_id",
                        column: x => x.creator_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_c_translation_key_app_user_updater_id",
                        column: x => x.updater_id,
                        principalTable: "app_user",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "fk_c_translation_key_c_translation_namespace_translation_namespace_id",
                        column: x => x.translation_namespace_id,
                        principalTable: "c_translation_namespace",
                        principalColumn: "translation_namespace_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "app_message_attachment",
                columns: table => new
                {
                    message_attachment_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    message_id = table.Column<int>(type: "integer", nullable: false),
                    content_type = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    file_path = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_message_attachment", x => x.message_attachment_id);
                    table.ForeignKey(
                        name: "fk_app_message_attachment_app_message_message_id",
                        column: x => x.message_id,
                        principalTable: "app_message",
                        principalColumn: "message_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "c_translation",
                columns: table => new
                {
                    translation_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    translation_key_id = table.Column<int>(type: "integer", nullable: false),
                    translation_language_id = table.Column<int>(type: "integer", nullable: false),
                    text_value = table.Column<string>(type: "text", nullable: true),
                    is_protected = table.Column<bool>(type: "boolean", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    creator_id = table.Column<int>(type: "integer", nullable: false),
                    updater_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_translation", x => x.translation_id);
                    table.ForeignKey(
                        name: "fk_c_translation_app_user_creator_id",
                        column: x => x.creator_id,
                        principalTable: "app_user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_c_translation_app_user_updater_id",
                        column: x => x.updater_id,
                        principalTable: "app_user",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "fk_c_translation_c_translation_key_translation_id",
                        column: x => x.translation_key_id,
                        principalTable: "c_translation_key",
                        principalColumn: "key_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_c_translation_c_translation_language_translation_language_id",
                        column: x => x.translation_language_id,
                        principalTable: "c_translation_language",
                        principalColumn: "translation_language_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "c_category",
                columns: new[] { "category_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "category.languages.description", "languages", "category.languages.name" },
                    { 2, "category.music.description", "music", "category.music.name" },
                    { 3, "category.school.description", "school", "category.school.name" },
                    { 4, "category.programming.description", "programming", "category.programming.name" }
                });

            migrationBuilder.InsertData(
                table: "c_country",
                columns: new[] { "country_id", "country_code", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "AD", "country.andorra.description", "andorra", "country.andorra.name" },
                    { 2, "AE", "country.united_arab_emirates.description", "united_arab_emirates", "country.united_arab_emirates.name" },
                    { 3, "AF", "country.afghanistan.description", "afghanistan", "country.afghanistan.name" },
                    { 4, "AG", "country.antigua_barbuda.description", "antigua_barbuda", "country.antigua_barbuda.name" },
                    { 5, "AI", "country.anguilla.description", "anguilla", "country.anguilla.name" },
                    { 6, "AL", "country.albania.description", "albania", "country.albania.name" },
                    { 7, "AM", "country.armenia.description", "armenia", "country.armenia.name" },
                    { 8, "AO", "country.angola.description", "angola", "country.angola.name" },
                    { 9, "AR", "country.argentina.description", "argentina", "country.argentina.name" },
                    { 10, "AS", "country.american_samoa.description", "american_samoa", "country.american_samoa.name" },
                    { 11, "AT", "country.austria.description", "austria", "country.austria.name" },
                    { 12, "AU", "country.australia.description", "australia", "country.australia.name" },
                    { 13, "AW", "country.aruba.description", "aruba", "country.aruba.name" },
                    { 14, "AX", "country.aland_islands.description", "aland_islands", "country.aland_islands.name" },
                    { 15, "AZ", "country.azerbaijan.description", "azerbaijan", "country.azerbaijan.name" },
                    { 16, "BA", "country.bosnia_herzegovina.description", "bosnia_herzegovina", "country.bosnia_herzegovina.name" },
                    { 17, "BB", "country.barbados.description", "barbados", "country.barbados.name" },
                    { 18, "BD", "country.bangladesh.description", "bangladesh", "country.bangladesh.name" },
                    { 19, "BE", "country.belgium.description", "belgium", "country.belgium.name" },
                    { 20, "BF", "country.burkina_faso.description", "burkina_faso", "country.burkina_faso.name" },
                    { 21, "BG", "country.bulgaria.description", "bulgaria", "country.bulgaria.name" },
                    { 22, "BH", "country.bahrain.description", "bahrain", "country.bahrain.name" },
                    { 23, "BI", "country.burundi.description", "burundi", "country.burundi.name" },
                    { 24, "BJ", "country.benin.description", "benin", "country.benin.name" },
                    { 25, "BL", "country.st_barthelemy.description", "st_barthelemy", "country.st_barthelemy.name" },
                    { 26, "BM", "country.bermuda.description", "bermuda", "country.bermuda.name" },
                    { 27, "BN", "country.brunei.description", "brunei", "country.brunei.name" },
                    { 28, "BO", "country.bolivia.description", "bolivia", "country.bolivia.name" },
                    { 29, "BR", "country.brazil.description", "brazil", "country.brazil.name" },
                    { 30, "BS", "country.bahamas.description", "bahamas", "country.bahamas.name" },
                    { 31, "BT", "country.bhutan.description", "bhutan", "country.bhutan.name" },
                    { 32, "BV", "country.bouvet_island.description", "bouvet_island", "country.bouvet_island.name" },
                    { 33, "BW", "country.botswana.description", "botswana", "country.botswana.name" },
                    { 34, "BY", "country.belarus.description", "belarus", "country.belarus.name" },
                    { 35, "BZ", "country.belize.description", "belize", "country.belize.name" },
                    { 36, "CA", "country.canada.description", "canada", "country.canada.name" },
                    { 37, "CC", "country.cocos_keeling_islands.description", "cocos_keeling_islands", "country.cocos_keeling_islands.name" },
                    { 38, "CD", "country.congo_kinshasa.description", "congo_kinshasa", "country.congo_kinshasa.name" },
                    { 39, "CF", "country.central_african_republic.description", "central_african_republic", "country.central_african_republic.name" },
                    { 40, "CG", "country.congo_brazzaville.description", "congo_brazzaville", "country.congo_brazzaville.name" },
                    { 41, "CH", "country.switzerland.description", "switzerland", "country.switzerland.name" },
                    { 42, "CI", "country.cote_d_ivoire.description", "cote_d_ivoire", "country.cote_d_ivoire.name" },
                    { 43, "CK", "country.cook_islands.description", "cook_islands", "country.cook_islands.name" },
                    { 44, "CL", "country.chile.description", "chile", "country.chile.name" },
                    { 45, "CM", "country.cameroon.description", "cameroon", "country.cameroon.name" },
                    { 46, "CN", "country.china.description", "china", "country.china.name" },
                    { 47, "CO", "country.colombia.description", "colombia", "country.colombia.name" },
                    { 48, "CR", "country.costa_rica.description", "costa_rica", "country.costa_rica.name" },
                    { 49, "CU", "country.cuba.description", "cuba", "country.cuba.name" },
                    { 50, "CV", "country.cape_verde.description", "cape_verde", "country.cape_verde.name" },
                    { 51, "CW", "country.curacao.description", "curacao", "country.curacao.name" },
                    { 52, "CX", "country.christmas_island.description", "christmas_island", "country.christmas_island.name" },
                    { 53, "CY", "country.cyprus.description", "cyprus", "country.cyprus.name" },
                    { 54, "CZ", "country.czech_republic.description", "czech_republic", "country.czech_republic.name" },
                    { 55, "DE", "country.germany.description", "germany", "country.germany.name" },
                    { 56, "DJ", "country.djibouti.description", "djibouti", "country.djibouti.name" },
                    { 57, "DK", "country.denmark.description", "denmark", "country.denmark.name" },
                    { 58, "DM", "country.dominica.description", "dominica", "country.dominica.name" },
                    { 59, "DO", "country.dominican_republic.description", "dominican_republic", "country.dominican_republic.name" },
                    { 60, "DZ", "country.algeria.description", "algeria", "country.algeria.name" },
                    { 61, "EC", "country.ecuador.description", "ecuador", "country.ecuador.name" },
                    { 62, "EE", "country.estonia.description", "estonia", "country.estonia.name" },
                    { 63, "EG", "country.egypt.description", "egypt", "country.egypt.name" },
                    { 64, "ER", "country.eritrea.description", "eritrea", "country.eritrea.name" },
                    { 65, "ES", "country.spain.description", "spain", "country.spain.name" },
                    { 66, "ET", "country.ethiopia.description", "ethiopia", "country.ethiopia.name" },
                    { 67, "EU", "country.european_union.description", "european_union", "country.european_union.name" },
                    { 68, "FI", "country.finland.description", "finland", "country.finland.name" },
                    { 69, "FJ", "country.fiji.description", "fiji", "country.fiji.name" },
                    { 70, "FK", "country.falkland_islands.description", "falkland_islands", "country.falkland_islands.name" },
                    { 71, "FM", "country.micronesia.description", "micronesia", "country.micronesia.name" },
                    { 72, "FO", "country.faroe_islands.description", "faroe_islands", "country.faroe_islands.name" },
                    { 73, "FR", "country.france.description", "france", "country.france.name" },
                    { 74, "GA", "country.gabon.description", "gabon", "country.gabon.name" },
                    { 75, "GB", "country.united_kingdom.description", "united_kingdom", "country.united_kingdom.name" },
                    { 76, "GB-ENG", "country.england.description", "england", "country.england.name" },
                    { 77, "GB-NIR", "country.northern_ireland.description", "northern_ireland", "country.northern_ireland.name" },
                    { 78, "GB-SCT", "country.scotland.description", "scotland", "country.scotland.name" },
                    { 79, "GB-WLS", "country.wales.description", "wales", "country.wales.name" },
                    { 80, "GB-ZET", "country.shetland.description", "shetland", "country.shetland.name" },
                    { 81, "GD", "country.grenada.description", "grenada", "country.grenada.name" },
                    { 82, "GE", "country.georgia.description", "georgia", "country.georgia.name" },
                    { 83, "GF", "country.french_guiana.description", "french_guiana", "country.french_guiana.name" },
                    { 84, "GG", "country.guernsey.description", "guernsey", "country.guernsey.name" },
                    { 85, "GH", "country.ghana.description", "ghana", "country.ghana.name" },
                    { 86, "GI", "country.gibraltar.description", "gibraltar", "country.gibraltar.name" },
                    { 87, "GL", "country.greenland.description", "greenland", "country.greenland.name" },
                    { 88, "GM", "country.gambia.description", "gambia", "country.gambia.name" },
                    { 89, "GN", "country.guinea.description", "guinea", "country.guinea.name" },
                    { 90, "GP", "country.guadeloupe.description", "guadeloupe", "country.guadeloupe.name" },
                    { 91, "GQ", "country.equatorial_guinea.description", "equatorial_guinea", "country.equatorial_guinea.name" },
                    { 92, "GR", "country.greece.description", "greece", "country.greece.name" },
                    { 93, "GS", "country.so_georgia_so_sandwich_isl.description", "so_georgia_so_sandwich_isl", "country.so_georgia_so_sandwich_isl.name" },
                    { 94, "GT", "country.guatemala.description", "guatemala", "country.guatemala.name" },
                    { 95, "GU", "country.guam.description", "guam", "country.guam.name" },
                    { 96, "GW", "country.guinea_bissau.description", "guinea_bissau", "country.guinea_bissau.name" },
                    { 97, "GY", "country.guyana.description", "guyana", "country.guyana.name" },
                    { 98, "HK", "country.hong_kong_china.description", "hong_kong_china", "country.hong_kong_china.name" },
                    { 99, "HM", "country.heard_mcdonald_islands.description", "heard_mcdonald_islands", "country.heard_mcdonald_islands.name" },
                    { 100, "HN", "country.honduras.description", "honduras", "country.honduras.name" },
                    { 101, "HR", "country.croatia.description", "croatia", "country.croatia.name" },
                    { 102, "HT", "country.haiti.description", "haiti", "country.haiti.name" },
                    { 103, "HU", "country.hungary.description", "hungary", "country.hungary.name" },
                    { 104, "ID", "country.indonesia.description", "indonesia", "country.indonesia.name" },
                    { 105, "IE", "country.ireland.description", "ireland", "country.ireland.name" },
                    { 106, "IL", "country.israel.description", "israel", "country.israel.name" },
                    { 107, "IM", "country.isle_of_man.description", "isle_of_man", "country.isle_of_man.name" },
                    { 108, "IN", "country.india.description", "india", "country.india.name" },
                    { 109, "IO", "country.british_indian_ocean_territory.description", "british_indian_ocean_territory", "country.british_indian_ocean_territory.name" },
                    { 110, "IQ", "country.iraq.description", "iraq", "country.iraq.name" },
                    { 111, "IR", "country.iran.description", "iran", "country.iran.name" },
                    { 112, "IS", "country.iceland.description", "iceland", "country.iceland.name" },
                    { 113, "IT", "country.italy.description", "italy", "country.italy.name" },
                    { 114, "JE", "country.jersey.description", "jersey", "country.jersey.name" },
                    { 115, "JM", "country.jamaica.description", "jamaica", "country.jamaica.name" },
                    { 116, "JO", "country.jordan.description", "jordan", "country.jordan.name" },
                    { 117, "JP", "country.japan.description", "japan", "country.japan.name" },
                    { 118, "KE", "country.kenya.description", "kenya", "country.kenya.name" },
                    { 119, "KG", "country.kyrgyzstan.description", "kyrgyzstan", "country.kyrgyzstan.name" },
                    { 120, "KH", "country.cambodia.description", "cambodia", "country.cambodia.name" },
                    { 121, "KI", "country.kiribati.description", "kiribati", "country.kiribati.name" },
                    { 122, "KM", "country.comoros.description", "comoros", "country.comoros.name" },
                    { 123, "KN", "country.st_kitts_nevis.description", "st_kitts_nevis", "country.st_kitts_nevis.name" },
                    { 124, "KP", "country.north_korea.description", "north_korea", "country.north_korea.name" },
                    { 125, "KR", "country.south_korea.description", "south_korea", "country.south_korea.name" },
                    { 126, "KW", "country.kuwait.description", "kuwait", "country.kuwait.name" },
                    { 127, "KY", "country.cayman_islands.description", "cayman_islands", "country.cayman_islands.name" },
                    { 128, "KZ", "country.kazakhstan.description", "kazakhstan", "country.kazakhstan.name" },
                    { 129, "LA", "country.laos.description", "laos", "country.laos.name" },
                    { 130, "LB", "country.lebanon.description", "lebanon", "country.lebanon.name" },
                    { 131, "LC", "country.st_lucia.description", "st_lucia", "country.st_lucia.name" },
                    { 132, "LGBT", "country.pride.description", "pride", "country.pride.name" },
                    { 133, "LI", "country.liechtenstein.description", "liechtenstein", "country.liechtenstein.name" },
                    { 134, "LK", "country.sri_lanka.description", "sri_lanka", "country.sri_lanka.name" },
                    { 135, "LR", "country.liberia.description", "liberia", "country.liberia.name" },
                    { 136, "LS", "country.lesotho.description", "lesotho", "country.lesotho.name" },
                    { 137, "LT", "country.lithuania.description", "lithuania", "country.lithuania.name" },
                    { 138, "LU", "country.luxembourg.description", "luxembourg", "country.luxembourg.name" },
                    { 139, "LV", "country.latvia.description", "latvia", "country.latvia.name" },
                    { 140, "LY", "country.libya.description", "libya", "country.libya.name" },
                    { 141, "MA", "country.morocco.description", "morocco", "country.morocco.name" },
                    { 142, "MC", "country.monaco.description", "monaco", "country.monaco.name" },
                    { 143, "MD", "country.moldova.description", "moldova", "country.moldova.name" },
                    { 144, "ME", "country.montenegro.description", "montenegro", "country.montenegro.name" },
                    { 145, "MF", "country.st_martin.description", "st_martin", "country.st_martin.name" },
                    { 146, "MG", "country.madagascar.description", "madagascar", "country.madagascar.name" },
                    { 147, "MH", "country.marshall_islands.description", "marshall_islands", "country.marshall_islands.name" },
                    { 148, "MK", "country.macedonia.description", "macedonia", "country.macedonia.name" },
                    { 149, "ML", "country.mali.description", "mali", "country.mali.name" },
                    { 150, "MM", "country.myanmar_burma.description", "myanmar_burma", "country.myanmar_burma.name" },
                    { 151, "MN", "country.mongolia.description", "mongolia", "country.mongolia.name" },
                    { 152, "MO", "country.macau_china.description", "macau_china", "country.macau_china.name" },
                    { 153, "MP", "country.northern_mariana_islands.description", "northern_mariana_islands", "country.northern_mariana_islands.name" },
                    { 154, "MQ", "country.martinique.description", "martinique", "country.martinique.name" },
                    { 155, "MR", "country.mauritania.description", "mauritania", "country.mauritania.name" },
                    { 156, "MS", "country.montserrat.description", "montserrat", "country.montserrat.name" },
                    { 157, "MT", "country.malta.description", "malta", "country.malta.name" },
                    { 158, "MU", "country.mauritius.description", "mauritius", "country.mauritius.name" },
                    { 159, "MV", "country.maldives.description", "maldives", "country.maldives.name" },
                    { 160, "MW", "country.malawi.description", "malawi", "country.malawi.name" },
                    { 161, "MX", "country.mexico.description", "mexico", "country.mexico.name" },
                    { 162, "MY", "country.malaysia.description", "malaysia", "country.malaysia.name" },
                    { 163, "MZ", "country.mozambique.description", "mozambique", "country.mozambique.name" },
                    { 164, "NA", "country.namibia.description", "namibia", "country.namibia.name" },
                    { 165, "NC", "country.new_caledonia.description", "new_caledonia", "country.new_caledonia.name" },
                    { 166, "NE", "country.niger.description", "niger", "country.niger.name" },
                    { 167, "NF", "country.norfolk_island.description", "norfolk_island", "country.norfolk_island.name" },
                    { 168, "NG", "country.nigeria.description", "nigeria", "country.nigeria.name" },
                    { 169, "NI", "country.nicaragua.description", "nicaragua", "country.nicaragua.name" },
                    { 170, "NL", "country.netherlands.description", "netherlands", "country.netherlands.name" },
                    { 171, "NO", "country.norway.description", "norway", "country.norway.name" },
                    { 172, "NP", "country.nepal.description", "nepal", "country.nepal.name" },
                    { 173, "NR", "country.nauru.description", "nauru", "country.nauru.name" },
                    { 174, "NU", "country.niue.description", "niue", "country.niue.name" },
                    { 175, "NZ", "country.new_zealand.description", "new_zealand", "country.new_zealand.name" },
                    { 176, "OM", "country.oman.description", "oman", "country.oman.name" },
                    { 177, "PA", "country.panama.description", "panama", "country.panama.name" },
                    { 178, "PE", "country.peru.description", "peru", "country.peru.name" },
                    { 179, "PF", "country.french_polynesia.description", "french_polynesia", "country.french_polynesia.name" },
                    { 180, "PG", "country.papua_new_guinea.description", "papua_new_guinea", "country.papua_new_guinea.name" },
                    { 181, "PH", "country.philippines.description", "philippines", "country.philippines.name" },
                    { 182, "PK", "country.pakistan.description", "pakistan", "country.pakistan.name" },
                    { 183, "PL", "country.poland.description", "poland", "country.poland.name" },
                    { 184, "PM", "country.st_pierre_miquelon.description", "st_pierre_miquelon", "country.st_pierre_miquelon.name" },
                    { 185, "PN", "country.pitcairn_islands.description", "pitcairn_islands", "country.pitcairn_islands.name" },
                    { 186, "PR", "country.puerto_rico.description", "puerto_rico", "country.puerto_rico.name" },
                    { 187, "PS", "country.palestinian_territories.description", "palestinian_territories", "country.palestinian_territories.name" },
                    { 188, "PT", "country.portugal.description", "portugal", "country.portugal.name" },
                    { 189, "PW", "country.palau.description", "palau", "country.palau.name" },
                    { 190, "PY", "country.paraguay.description", "paraguay", "country.paraguay.name" },
                    { 191, "QA", "country.qatar.description", "qatar", "country.qatar.name" },
                    { 192, "RE", "country.reunion.description", "reunion", "country.reunion.name" },
                    { 193, "RO", "country.romania.description", "romania", "country.romania.name" },
                    { 194, "RS", "country.serbia.description", "serbia", "country.serbia.name" },
                    { 195, "RU", "country.russia.description", "russia", "country.russia.name" },
                    { 196, "RW", "country.rwanda.description", "rwanda", "country.rwanda.name" },
                    { 197, "SA", "country.saudi_arabia.description", "saudi_arabia", "country.saudi_arabia.name" },
                    { 198, "SB", "country.solomon_islands.description", "solomon_islands", "country.solomon_islands.name" },
                    { 199, "SC", "country.seychelles.description", "seychelles", "country.seychelles.name" },
                    { 200, "SD", "country.sudan.description", "sudan", "country.sudan.name" },
                    { 201, "SE", "country.sweden.description", "sweden", "country.sweden.name" },
                    { 202, "SG", "country.singapore.description", "singapore", "country.singapore.name" },
                    { 203, "SH", "country.st_helena.description", "st_helena", "country.st_helena.name" },
                    { 204, "SI", "country.slovenia.description", "slovenia", "country.slovenia.name" },
                    { 205, "SJ", "country.svalbard_jan_mayen.description", "svalbard_jan_mayen", "country.svalbard_jan_mayen.name" },
                    { 206, "SK", "country.slovakia.description", "slovakia", "country.slovakia.name" },
                    { 207, "SL", "country.sierra_leone.description", "sierra_leone", "country.sierra_leone.name" },
                    { 208, "SM", "country.san_marino.description", "san_marino", "country.san_marino.name" },
                    { 209, "SN", "country.senegal.description", "senegal", "country.senegal.name" },
                    { 210, "SO", "country.somalia.description", "somalia", "country.somalia.name" },
                    { 211, "SR", "country.suriname.description", "suriname", "country.suriname.name" },
                    { 212, "SS", "country.south_sudan.description", "south_sudan", "country.south_sudan.name" },
                    { 213, "ST", "country.sao_tome_principe.description", "sao_tome_principe", "country.sao_tome_principe.name" },
                    { 214, "SV", "country.el_salvador.description", "el_salvador", "country.el_salvador.name" },
                    { 215, "SX", "country.sint_maarten.description", "sint_maarten", "country.sint_maarten.name" },
                    { 216, "SY", "country.syria.description", "syria", "country.syria.name" },
                    { 217, "SZ", "country.swaziland.description", "swaziland", "country.swaziland.name" },
                    { 218, "TC", "country.turks_caicos_islands.description", "turks_caicos_islands", "country.turks_caicos_islands.name" },
                    { 219, "TD", "country.chad.description", "chad", "country.chad.name" },
                    { 220, "TF", "country.french_southern_territories.description", "french_southern_territories", "country.french_southern_territories.name" },
                    { 221, "TG", "country.togo.description", "togo", "country.togo.name" },
                    { 222, "TH", "country.thailand.description", "thailand", "country.thailand.name" },
                    { 223, "TJ", "country.tajikistan.description", "tajikistan", "country.tajikistan.name" },
                    { 224, "TK", "country.tokelau.description", "tokelau", "country.tokelau.name" },
                    { 225, "TL", "country.timor_leste.description", "timor_leste", "country.timor_leste.name" },
                    { 226, "TM", "country.turkmenistan.description", "turkmenistan", "country.turkmenistan.name" },
                    { 227, "TN", "country.tunisia.description", "tunisia", "country.tunisia.name" },
                    { 228, "TO", "country.tonga.description", "tonga", "country.tonga.name" },
                    { 229, "TR", "country.turkey.description", "turkey", "country.turkey.name" },
                    { 230, "TT", "country.trinidad_tobago.description", "trinidad_tobago", "country.trinidad_tobago.name" },
                    { 231, "TV", "country.tuvalu.description", "tuvalu", "country.tuvalu.name" },
                    { 232, "TW", "country.taiwan.description", "taiwan", "country.taiwan.name" },
                    { 233, "TZ", "country.tanzania.description", "tanzania", "country.tanzania.name" },
                    { 234, "UA", "country.ukraine.description", "ukraine", "country.ukraine.name" },
                    { 235, "UG", "country.uganda.description", "uganda", "country.uganda.name" },
                    { 236, "UM", "country.us_outlying_islands.description", "us_outlying_islands", "country.us_outlying_islands.name" },
                    { 237, "US", "country.united_states.description", "united_states", "country.united_states.name" },
                    { 238, "US-CA", "country.california.description", "california", "country.california.name" },
                    { 239, "UY", "country.uruguay.description", "uruguay", "country.uruguay.name" },
                    { 240, "UZ", "country.uzbekistan.description", "uzbekistan", "country.uzbekistan.name" },
                    { 241, "VA", "country.vatican_city.description", "vatican_city", "country.vatican_city.name" },
                    { 242, "VC", "country.st_vincent_grenadines.description", "st_vincent_grenadines", "country.st_vincent_grenadines.name" },
                    { 243, "VE", "country.venezuela.description", "venezuela", "country.venezuela.name" },
                    { 244, "VG", "country.british_virgin_islands.description", "british_virgin_islands", "country.british_virgin_islands.name" },
                    { 245, "VI", "country.us_virgin_islands.description", "us_virgin_islands", "country.us_virgin_islands.name" },
                    { 246, "VN", "country.vietnam.description", "vietnam", "country.vietnam.name" },
                    { 247, "VU", "country.vanuatu.description", "vanuatu", "country.vanuatu.name" },
                    { 248, "WF", "country.wallis_futuna.description", "wallis_futuna", "country.wallis_futuna.name" },
                    { 249, "WS", "country.samoa.description", "samoa", "country.samoa.name" },
                    { 250, "XK", "country.kosovo.description", "kosovo", "country.kosovo.name" },
                    { 251, "YE", "country.yemen.description", "yemen", "country.yemen.name" },
                    { 252, "YT", "country.mayotte.description", "mayotte", "country.mayotte.name" },
                    { 253, "ZA", "country.south_africa.description", "south_africa", "country.south_africa.name" },
                    { 254, "ZM", "country.zambia.description", "zambia", "country.zambia.name" },
                    { 255, "ZW", "country.zimbabwe.description", "zimbabwe", "country.zimbabwe.name" }
                });

            migrationBuilder.InsertData(
                table: "c_currency",
                columns: new[] { "currency_id", "description_key", "enum_key", "name_key" },
                values: new object[] { 1, "currency.czk.description", "czk", "currency.czk.name" });

            migrationBuilder.InsertData(
                table: "c_file_type",
                columns: new[] { "file_type_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "file_type.profile_photo.description", "profile_photo", "file_type.profile_photo.name" },
                    { 2, "file_type.profile_photo_thumbnail.description", "profile_photo_thumbnail", "file_type.profile_photo_thumbnail.name" },
                    { 3, "file_type.certificate.description", "certificate", "file_type.certificate.name" }
                });

            migrationBuilder.InsertData(
                table: "c_language",
                columns: new[] { "language_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "language.afar.description", "afar", "language.afar.name" },
                    { 2, "language.abkhaz.description", "abkhaz", "language.abkhaz.name" },
                    { 3, "language.avestan.description", "avestan", "language.avestan.name" },
                    { 4, "language.afrikaans.description", "afrikaans", "language.afrikaans.name" },
                    { 5, "language.akan.description", "akan", "language.akan.name" },
                    { 6, "language.amharic.description", "amharic", "language.amharic.name" },
                    { 7, "language.aragonese.description", "aragonese", "language.aragonese.name" },
                    { 8, "language.arabic.description", "arabic", "language.arabic.name" },
                    { 9, "language.assamese.description", "assamese", "language.assamese.name" },
                    { 10, "language.avaric.description", "avaric", "language.avaric.name" },
                    { 11, "language.aymara.description", "aymara", "language.aymara.name" },
                    { 12, "language.azerbaijani.description", "azerbaijani", "language.azerbaijani.name" },
                    { 13, "language.south_azerbaijani.description", "south_azerbaijani", "language.south_azerbaijani.name" },
                    { 14, "language.bashkir.description", "bashkir", "language.bashkir.name" },
                    { 15, "language.belarusian.description", "belarusian", "language.belarusian.name" },
                    { 16, "language.bulgarian.description", "bulgarian", "language.bulgarian.name" },
                    { 17, "language.bihari.description", "bihari", "language.bihari.name" },
                    { 18, "language.bislama.description", "bislama", "language.bislama.name" },
                    { 19, "language.bambara.description", "bambara", "language.bambara.name" },
                    { 20, "language.bengali.description", "bengali", "language.bengali.name" },
                    { 21, "language.tibetan_standard.description", "tibetan_standard", "language.tibetan_standard.name" },
                    { 22, "language.breton.description", "breton", "language.breton.name" },
                    { 23, "language.bosnian.description", "bosnian", "language.bosnian.name" },
                    { 24, "language.catalan.description", "catalan", "language.catalan.name" },
                    { 25, "language.chechen.description", "chechen", "language.chechen.name" },
                    { 26, "language.chamorro.description", "chamorro", "language.chamorro.name" },
                    { 27, "language.corsican.description", "corsican", "language.corsican.name" },
                    { 28, "language.cree.description", "cree", "language.cree.name" },
                    { 29, "language.czech.description", "czech", "language.czech.name" },
                    { 30, "language.old_church_slavonic.description", "old_church_slavonic", "language.old_church_slavonic.name" },
                    { 31, "language.chuvash.description", "chuvash", "language.chuvash.name" },
                    { 32, "language.welsh.description", "welsh", "language.welsh.name" },
                    { 33, "language.danish.description", "danish", "language.danish.name" },
                    { 34, "language.german.description", "german", "language.german.name" },
                    { 35, "language.divehi.description", "divehi", "language.divehi.name" },
                    { 36, "language.dzongkha.description", "dzongkha", "language.dzongkha.name" },
                    { 37, "language.ewe.description", "ewe", "language.ewe.name" },
                    { 38, "language.greek.description", "greek", "language.greek.name" },
                    { 39, "language.english.description", "english", "language.english.name" },
                    { 40, "language.esperanto.description", "esperanto", "language.esperanto.name" },
                    { 41, "language.spanish.description", "spanish", "language.spanish.name" },
                    { 42, "language.estonian.description", "estonian", "language.estonian.name" },
                    { 43, "language.basque.description", "basque", "language.basque.name" },
                    { 44, "language.persian.description", "persian", "language.persian.name" },
                    { 45, "language.fula.description", "fula", "language.fula.name" },
                    { 46, "language.finnish.description", "finnish", "language.finnish.name" },
                    { 47, "language.fijian.description", "fijian", "language.fijian.name" },
                    { 48, "language.faroese.description", "faroese", "language.faroese.name" },
                    { 49, "language.french.description", "french", "language.french.name" },
                    { 50, "language.western_frisian.description", "western_frisian", "language.western_frisian.name" },
                    { 51, "language.irish.description", "irish", "language.irish.name" },
                    { 52, "language.scottish_gaelic.description", "scottish_gaelic", "language.scottish_gaelic.name" },
                    { 53, "language.galician.description", "galician", "language.galician.name" },
                    { 54, "language.guaraní.description", "guaraní", "language.guaraní.name" },
                    { 55, "language.gujarati.description", "gujarati", "language.gujarati.name" },
                    { 56, "language.manx.description", "manx", "language.manx.name" },
                    { 57, "language.hausa.description", "hausa", "language.hausa.name" },
                    { 58, "language.hebrew.description", "hebrew", "language.hebrew.name" },
                    { 59, "language.hindi.description", "hindi", "language.hindi.name" },
                    { 60, "language.hiri_motu.description", "hiri_motu", "language.hiri_motu.name" },
                    { 61, "language.croatian.description", "croatian", "language.croatian.name" },
                    { 62, "language.haitian.description", "haitian", "language.haitian.name" },
                    { 63, "language.hungarian.description", "hungarian", "language.hungarian.name" },
                    { 64, "language.armenian.description", "armenian", "language.armenian.name" },
                    { 65, "language.herero.description", "herero", "language.herero.name" },
                    { 66, "language.interlingua.description", "interlingua", "language.interlingua.name" },
                    { 67, "language.indonesian.description", "indonesian", "language.indonesian.name" },
                    { 68, "language.interlingue.description", "interlingue", "language.interlingue.name" },
                    { 69, "language.igbo.description", "igbo", "language.igbo.name" },
                    { 70, "language.nuosu.description", "nuosu", "language.nuosu.name" },
                    { 71, "language.inupiaq.description", "inupiaq", "language.inupiaq.name" },
                    { 72, "language.ido.description", "ido", "language.ido.name" },
                    { 73, "language.icelandic.description", "icelandic", "language.icelandic.name" },
                    { 74, "language.italian.description", "italian", "language.italian.name" },
                    { 75, "language.inuktitut.description", "inuktitut", "language.inuktitut.name" },
                    { 76, "language.japanese.description", "japanese", "language.japanese.name" },
                    { 77, "language.javanese.description", "javanese", "language.javanese.name" },
                    { 78, "language.georgian.description", "georgian", "language.georgian.name" },
                    { 79, "language.kongo.description", "kongo", "language.kongo.name" },
                    { 80, "language.kikuyu.description", "kikuyu", "language.kikuyu.name" },
                    { 81, "language.kwanyama.description", "kwanyama", "language.kwanyama.name" },
                    { 82, "language.kazakh.description", "kazakh", "language.kazakh.name" },
                    { 83, "language.kalaallisut.description", "kalaallisut", "language.kalaallisut.name" },
                    { 84, "language.khmer.description", "khmer", "language.khmer.name" },
                    { 85, "language.kannada.description", "kannada", "language.kannada.name" },
                    { 86, "language.korean.description", "korean", "language.korean.name" },
                    { 87, "language.kanuri.description", "kanuri", "language.kanuri.name" },
                    { 88, "language.kashmiri.description", "kashmiri", "language.kashmiri.name" },
                    { 89, "language.kurdish.description", "kurdish", "language.kurdish.name" },
                    { 90, "language.komi.description", "komi", "language.komi.name" },
                    { 91, "language.cornish.description", "cornish", "language.cornish.name" },
                    { 92, "language.kyrgyz.description", "kyrgyz", "language.kyrgyz.name" },
                    { 93, "language.latin.description", "latin", "language.latin.name" },
                    { 94, "language.luxembourgish.description", "luxembourgish", "language.luxembourgish.name" },
                    { 95, "language.ganda.description", "ganda", "language.ganda.name" },
                    { 96, "language.limburgish.description", "limburgish", "language.limburgish.name" },
                    { 97, "language.lingala.description", "lingala", "language.lingala.name" },
                    { 98, "language.lao.description", "lao", "language.lao.name" },
                    { 99, "language.lithuanian.description", "lithuanian", "language.lithuanian.name" },
                    { 100, "language.luba_katanga.description", "luba_katanga", "language.luba_katanga.name" },
                    { 101, "language.latvian.description", "latvian", "language.latvian.name" },
                    { 102, "language.malagasy.description", "malagasy", "language.malagasy.name" },
                    { 103, "language.marshallese.description", "marshallese", "language.marshallese.name" },
                    { 104, "language.maori.description", "maori", "language.maori.name" },
                    { 105, "language.macedonian.description", "macedonian", "language.macedonian.name" },
                    { 106, "language.malayalam.description", "malayalam", "language.malayalam.name" },
                    { 107, "language.mongolian.description", "mongolian", "language.mongolian.name" },
                    { 108, "language.marathi.description", "marathi", "language.marathi.name" },
                    { 109, "language.malay.description", "malay", "language.malay.name" },
                    { 110, "language.maltese.description", "maltese", "language.maltese.name" },
                    { 111, "language.burmese.description", "burmese", "language.burmese.name" },
                    { 112, "language.nauru.description", "nauru", "language.nauru.name" },
                    { 113, "language.norwegian_bokmal.description", "norwegian_bokmal", "language.norwegian_bokmal.name" },
                    { 114, "language.north_ndebele.description", "north_ndebele", "language.north_ndebele.name" },
                    { 115, "language.nepali.description", "nepali", "language.nepali.name" },
                    { 116, "language.ndonga.description", "ndonga", "language.ndonga.name" },
                    { 117, "language.dutch.description", "dutch", "language.dutch.name" },
                    { 118, "language.norwegian_nynorsk.description", "norwegian_nynorsk", "language.norwegian_nynorsk.name" },
                    { 119, "language.norwegian.description", "norwegian", "language.norwegian.name" },
                    { 120, "language.south_ndebele.description", "south_ndebele", "language.south_ndebele.name" },
                    { 121, "language.navajo.description", "navajo", "language.navajo.name" },
                    { 122, "language.chichewa.description", "chichewa", "language.chichewa.name" },
                    { 123, "language.occitan.description", "occitan", "language.occitan.name" },
                    { 124, "language.ojibwe.description", "ojibwe", "language.ojibwe.name" },
                    { 125, "language.oromo.description", "oromo", "language.oromo.name" },
                    { 126, "language.oriya.description", "oriya", "language.oriya.name" },
                    { 127, "language.ossetian.description", "ossetian", "language.ossetian.name" },
                    { 128, "language.panjabi.description", "panjabi", "language.panjabi.name" },
                    { 129, "language.pali.description", "pali", "language.pali.name" },
                    { 130, "language.polish.description", "polish", "language.polish.name" },
                    { 131, "language.pashto.description", "pashto", "language.pashto.name" },
                    { 132, "language.portuguese.description", "portuguese", "language.portuguese.name" },
                    { 133, "language.quechua.description", "quechua", "language.quechua.name" },
                    { 134, "language.romansh.description", "romansh", "language.romansh.name" },
                    { 135, "language.kirundi.description", "kirundi", "language.kirundi.name" },
                    { 136, "language.romanian.description", "romanian", "language.romanian.name" },
                    { 137, "language.russian.description", "russian", "language.russian.name" },
                    { 138, "language.kinyarwanda.description", "kinyarwanda", "language.kinyarwanda.name" },
                    { 139, "language.sanskrit.description", "sanskrit", "language.sanskrit.name" },
                    { 140, "language.sardinian.description", "sardinian", "language.sardinian.name" },
                    { 141, "language.sindhi.description", "sindhi", "language.sindhi.name" },
                    { 142, "language.northern_sami.description", "northern_sami", "language.northern_sami.name" },
                    { 143, "language.sango.description", "sango", "language.sango.name" },
                    { 144, "language.sinhala.description", "sinhala", "language.sinhala.name" },
                    { 145, "language.slovak.description", "slovak", "language.slovak.name" },
                    { 146, "language.slovene.description", "slovene", "language.slovene.name" },
                    { 147, "language.samoan.description", "samoan", "language.samoan.name" },
                    { 148, "language.shona.description", "shona", "language.shona.name" },
                    { 149, "language.somali.description", "somali", "language.somali.name" },
                    { 150, "language.albanian.description", "albanian", "language.albanian.name" },
                    { 151, "language.serbian.description", "serbian", "language.serbian.name" },
                    { 152, "language.swati.description", "swati", "language.swati.name" },
                    { 153, "language.southern_sotho.description", "southern_sotho", "language.southern_sotho.name" },
                    { 154, "language.sundanese.description", "sundanese", "language.sundanese.name" },
                    { 155, "language.swedish.description", "swedish", "language.swedish.name" },
                    { 156, "language.swahili.description", "swahili", "language.swahili.name" },
                    { 157, "language.tamil.description", "tamil", "language.tamil.name" },
                    { 158, "language.telugu.description", "telugu", "language.telugu.name" },
                    { 159, "language.tajik.description", "tajik", "language.tajik.name" },
                    { 160, "language.thai.description", "thai", "language.thai.name" },
                    { 161, "language.tigrinya.description", "tigrinya", "language.tigrinya.name" },
                    { 162, "language.turkmen.description", "turkmen", "language.turkmen.name" },
                    { 163, "language.tagalog.description", "tagalog", "language.tagalog.name" },
                    { 164, "language.tswana.description", "tswana", "language.tswana.name" },
                    { 165, "language.tonga.description", "tonga", "language.tonga.name" },
                    { 166, "language.turkish.description", "turkish", "language.turkish.name" },
                    { 167, "language.tsonga.description", "tsonga", "language.tsonga.name" },
                    { 168, "language.tatar.description", "tatar", "language.tatar.name" },
                    { 169, "language.twi.description", "twi", "language.twi.name" },
                    { 170, "language.tahitian.description", "tahitian", "language.tahitian.name" },
                    { 171, "language.uyghur.description", "uyghur", "language.uyghur.name" },
                    { 172, "language.ukrainian.description", "ukrainian", "language.ukrainian.name" },
                    { 173, "language.urdu.description", "urdu", "language.urdu.name" },
                    { 174, "language.uzbek.description", "uzbek", "language.uzbek.name" },
                    { 175, "language.venda.description", "venda", "language.venda.name" },
                    { 176, "language.vietnamese.description", "vietnamese", "language.vietnamese.name" },
                    { 177, "language.volapük.description", "volapük", "language.volapük.name" },
                    { 178, "language.walloon.description", "walloon", "language.walloon.name" },
                    { 179, "language.wolof.description", "wolof", "language.wolof.name" },
                    { 180, "language.xhosa.description", "xhosa", "language.xhosa.name" },
                    { 181, "language.yiddish.description", "yiddish", "language.yiddish.name" },
                    { 182, "language.yoruba.description", "yoruba", "language.yoruba.name" },
                    { 183, "language.zhuang.description", "zhuang", "language.zhuang.name" },
                    { 184, "language.chinese.description", "chinese", "language.chinese.name" },
                    { 185, "language.zulu.description", "zulu", "language.zulu.name" }
                });

            migrationBuilder.InsertData(
                table: "c_language_level",
                columns: new[] { "language_level_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "language_level.a1.description", "a1", "language_level.a1.name" },
                    { 2, "language_level.a2.description", "a2", "language_level.a2.name" },
                    { 3, "language_level.b1.description", "b1", "language_level.b1.name" },
                    { 4, "language_level.b2.description", "b2", "language_level.b2.name" },
                    { 5, "language_level.c1.description", "c1", "language_level.c1.name" },
                    { 6, "language_level.c2.description", "c2", "language_level.c2.name" },
                    { 7, "language_level.native.description", "native", "language_level.native.name" }
                });

            migrationBuilder.InsertData(
                table: "c_master_hash",
                columns: new[] { "master_hash_id", "description_key", "enum_key", "hash", "name_key", "table_name", "updated" },
                values: new object[,]
                {
                    { 1, "master_hash.c_age_range.description", "c_age_range", new Guid("59cd5fd0-49f0-424c-a46d-b1a0c640acf2"), "master_hash.c_age_range.name", "c_age_range", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "master_hash.c_category.description", "c_category", new Guid("72ab8c1d-4a03-4d7f-8d95-a5480bc64b8d"), "master_hash.c_category.name", "c_category", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "master_hash.c_country.description", "c_country", new Guid("503be24a-afbb-4cfe-a1bb-6a33b9581708"), "master_hash.c_country.name", "c_country", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "master_hash.c_currency.description", "c_currency", new Guid("b65447b9-0f0f-4657-b689-ad40c100089b"), "master_hash.c_currency.name", "c_currency", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "master_hash.c_current_situation.description", "c_current_situation", new Guid("a5b30134-7fd2-40fd-ab7c-fb32c37140fa"), "master_hash.c_current_situation.name", "c_current_situation", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "master_hash.c_day_of_week.description", "c_day_of_week", new Guid("6c599d2b-6d43-42b2-a567-ec989b39a8ab"), "master_hash.c_day_of_week.name", "c_day_of_week", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "master_hash.c_file_type.description", "c_file_type", new Guid("86894a7a-1030-4fb8-ad8d-7a1beab0e221"), "master_hash.c_file_type.name", "c_file_type", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, "master_hash.c_language.description", "c_language", new Guid("638e3bba-8070-4c87-bea1-08872388ea42"), "master_hash.c_language.name", "c_language", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, "master_hash.c_language_level.description", "c_language_level", new Guid("59e6c51a-1ba9-4871-ba23-66907283e6da"), "master_hash.c_language_level.name", "c_language_level", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, "master_hash.c_master_hash.description", "c_master_hash", new Guid("267cb91f-2033-4903-917c-418de315bc6d"), "master_hash.c_master_hash.name", "c_master_hash", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, "master_hash.c_payment_service_provider.description", "c_payment_service_provider", new Guid("862e45c1-c00f-4f1e-83e7-4b71c2b62d36"), "master_hash.c_payment_service_provider.name", "c_payment_service_provider", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, "master_hash.c_reservation_state.description", "c_reservation_state", new Guid("7225489e-d0c3-4b04-a1f5-f122e1dc1364"), "master_hash.c_reservation_state.name", "c_reservation_state", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, "master_hash.c_subject.description", "c_subject", new Guid("9ff3f4b3-1aa5-4da7-91ab-86031eb53395"), "master_hash.c_subject.name", "c_subject", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, "master_hash.c_subject_level.description", "c_subject_level", new Guid("94f85bf9-5afb-4e17-ac2d-caa942bfbaa4"), "master_hash.c_subject_level.name", "c_subject_level", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, "master_hash.c_timestamp.description", "c_timestamp", new Guid("4e014fad-8694-4059-a2b5-c545463fac79"), "master_hash.c_timestamp.name", "c_timestamp", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, "master_hash.c_timezone.description", "c_timezone", new Guid("9c8975bf-d842-4a56-90c4-a2fcc2dcbe25"), "master_hash.c_timezone.name", "c_timezone", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17, "master_hash.c_token_provider.description", "c_token_provider", new Guid("376b8bfa-670d-44f2-86de-480cd0262cbb"), "master_hash.c_token_provider.name", "c_token_provider", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 18, "master_hash.c_token_type.description", "c_token_type", new Guid("321059c0-6f1e-4210-8bcc-11602afb9c38"), "master_hash.c_token_type.name", "c_token_type", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 19, "master_hash.c_transaction_state.description", "c_transaction_state", new Guid("e5bee690-392e-4df1-89ee-2a28d702a849"), "master_hash.c_transaction_state.name", "c_transaction_state", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 20, "master_hash.c_transaction_type.description", "c_transaction_type", new Guid("c226eda7-57a7-4338-98ed-d5d137190e6f"), "master_hash.c_transaction_type.name", "c_transaction_type", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 21, "master_hash.c_user_attribute_type.description", "c_user_attribute_type", new Guid("b0121f2c-7a4f-4932-9871-f65a2d27162c"), "master_hash.c_user_attribute_type.name", "c_user_attribute_type", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 22, "master_hash.c_user_role.description", "c_user_role", new Guid("f3560a72-7788-456f-916d-d32adae46fd4"), "master_hash.c_user_role.name", "c_user_role", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 23, "master_hash.c_user_state.description", "c_user_state", new Guid("5345321e-e985-445b-b3f0-385d5050feb0"), "master_hash.c_user_state.name", "c_user_state", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 24, "master_hash.env_configuration.description", "env_configuration", new Guid("340bedd6-b76a-4281-ac34-275706eea17e"), "master_hash.env_configuration.name", "env_configuration", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 25, "master_hash.c_translation_language.description", "c_translation_language", new Guid("64fc38b7-8471-4786-bedf-349901d43983"), "master_hash.c_translation_language.name", "c_translation_language", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 26, "master_hash.c_translation.description", "c_translation", new Guid("553b73de-022b-43c9-aa66-50e2c581cda4"), "master_hash.c_translation.name", "c_translation", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 27, "master_hash.c_specialization.description", "c_specialization", new Guid("041f769c-1651-4f83-b317-3d168f22bb84"), "master_hash.c_specialization.name", "c_specialization", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "c_message_state",
                columns: new[] { "message_state_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "message_state.created.description", "created", "message_state.created.name" },
                    { 2, "message_state.read.description", "read", "message_state.read.name" }
                });

            migrationBuilder.InsertData(
                table: "c_object_state",
                columns: new[] { "object_state_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "object_state.active.description", "active", "object_state.active.name" },
                    { 2, "object_state.deleted.description", "deleted", "object_state.deleted.name" }
                });

            migrationBuilder.InsertData(
                table: "c_payment_service_provider",
                columns: new[] { "payment_service_provider_id", "description_key", "enum_key", "name_key" },
                values: new object[] { 1, "payment_service_provider.stripe.description", "stripe", "payment_service_provider.stripe.name" });

            migrationBuilder.InsertData(
                table: "c_reservation_state",
                columns: new[] { "reservation_state_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "reservation_state.created.description", "created", "reservation_state.created.name" },
                    { 2, "reservation_state.payed.description", "payed", "reservation_state.payed.name" }
                });

            migrationBuilder.InsertData(
                table: "c_room_activity_type",
                columns: new[] { "room_activity_type_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "room_activity.disconnected.description", "disconnected", "room_activity.disconnected.name" },
                    { 2, "room_activity.joined.description", "joined", "room_activity.joined.name" },
                    { 3, "room_activity.started.description", "started", "room_activity.started.name" },
                    { 4, "room_activity.signal.description", "signal", "room_activity.signal.name" },
                    { 5, "room_activity.connected.description", "connected", "room_activity.connected.name" }
                });

            migrationBuilder.InsertData(
                table: "c_room_state",
                columns: new[] { "room_state_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "room_state.created.description", "created", "room_state.created.name" },
                    { 2, "room_state.started.description", "started", "room_state.started.name" }
                });

            migrationBuilder.InsertData(
                table: "c_room_statistic_type",
                columns: new[] { "room_statistic_type_id", "description_key", "enum_key", "name_key" },
                values: new object[] { 1, "room_statistic_type.room_quality_rating.description", "room_quality_rating", "room_statistic_type.room_quality_rating.name" });

            migrationBuilder.InsertData(
                table: "c_specialization",
                columns: new[] { "specialization_id", "alternative_name_key", "description_key", "enum_key", "icon", "name_key" },
                values: new object[,]
                {
                    { 1, "specialization.support_education.alternative", "specialization.support_education.description", "support_education", "education", "specialization.support_education.name" },
                    { 2, "specialization.boost_career.alternative", "specialization.boost_career.description", "boost_career", "career", "specialization.boost_career.name" },
                    { 3, "specialization.move_abroad.alternative", "specialization.move_abroad.description", "move_abroad", "abroad", "specialization.move_abroad.name" },
                    { 4, "specialization.connect_with_people.alternative", "specialization.connect_with_people.description", "connect_with_people", "connect_people", "specialization.connect_with_people.name" },
                    { 5, "specialization.language_for_kids.alternative", "specialization.language_for_kids.description", "language_for_kids", "kids", "specialization.language_for_kids.name" },
                    { 6, "specialization.self_improvement.alternative", "specialization.self_improvement.description", "self_improvement", "productivity", "specialization.self_improvement.name" },
                    { 7, "specialization.just_for_fun.alternative", "specialization.just_for_fun.description", "just_for_fun", "fun", "specialization.just_for_fun.name" }
                });

            migrationBuilder.InsertData(
                table: "c_subject_level",
                columns: new[] { "subject_level_id", "description_key", "enum_key", "icon_name", "inverted_icon_name", "student_name_key", "tutor_name_key" },
                values: new object[,]
                {
                    { 1, "subject.level.beginner.description", "beginner", "level_one", "level_one_inverted", "subject.level.beginner.student.name", "subject.level.beginner.tutor.name" },
                    { 2, "subject.level.intermediate.description", "intermediate", "level_two", "level_two_inverted", "subject.level.intermediate.student.name", "subject.level.intermediate.tutor.name" },
                    { 3, "subject.level.advanced.description", "advanced", "level_three", "level_three_inverted", "subject.level.advanced.student.name", "subject.level.advanced.tutor.name" }
                });

            migrationBuilder.InsertData(
                table: "c_teaching_experience",
                columns: new[] { "teaching_experience_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "teaching_experience.formal_teaching_experience.description", "formal_teaching_experience", "teaching_experience.formal_teaching_experience.name" },
                    { 2, "teaching_experience.informal_teaching_experience.description", "informal_teaching_experience", "teaching_experience.informal_teaching_experience.name" },
                    { 3, "teaching_experience.none.description", "none", "teaching_experience.none.name" }
                });

            migrationBuilder.InsertData(
                table: "c_token_provider",
                columns: new[] { "token_provider_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "token_provider.google_api.description", "google_api", "token_provider.google_api.name" },
                    { 2, "token_provider.seznam_api.description", "seznam_api", "token_provider.seznam_api.name" }
                });

            migrationBuilder.InsertData(
                table: "c_token_type",
                columns: new[] { "token_type_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "token_type.refresh_token.description", "refresh_token", "token_type.refresh_token.name" },
                    { 2, "token_type.id_token.description", "id_token", "token_type.id_token.name" },
                    { 3, "token_type.access_token.description", "access_token", "token_type.access_token.name" },
                    { 4, "token_type.bearer.description", "bearer", "token_type.bearer.name" }
                });

            migrationBuilder.InsertData(
                table: "c_transaction_state",
                columns: new[] { "transaction_state_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "transaction_state.approved.description", "approved", "transaction_state.approved.name" },
                    { 2, "transaction_state.declined.description", "declined", "transaction_state.declined.name" }
                });

            migrationBuilder.InsertData(
                table: "c_user_attribute_type",
                columns: new[] { "user_attribute_type_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "user_attribute_type.description.description", "description", "user_attribute_type.description.name" },
                    { 2, "user_attribute_type.video_url.description", "video_url", "user_attribute_type.video_url.name" }
                });

            migrationBuilder.InsertData(
                table: "c_user_role",
                columns: new[] { "user_role_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "user_role.admin.description", "admin", "user_role.admin.name" },
                    { 2, "user_role.tutor.description", "tutor", "user_role.tutor.name" },
                    { 3, "user_role.student.description", "student", "user_role.student.name" }
                });

            migrationBuilder.InsertData(
                table: "c_user_state",
                columns: new[] { "user_state_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "user_state.created.description", "created", "user_state.created.name" },
                    { 2, "user_state.email_verified.description", "email_verified", "user_state.email_verified.name" },
                    { 3, "user_state.password_set.description", "password_set", "user_state.password_set.name" },
                    { 4, "user_state.profile_in_validation.description", "profile_in_validation", "user_state.profile_in_validation.name" },
                    { 5, "user_state.confirmed.description", "confirmed", "user_state.confirmed.name" },
                    { 6, "user_state.deleted.description", "deleted", "user_state.deleted.name" }
                });

            migrationBuilder.InsertData(
                table: "c_weekly_goal",
                columns: new[] { "weekly_goal_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "weekly_goal.one_lesson_per_week.description", "one_lesson_per_week", "weekly_goal.one_lesson_per_week.name" },
                    { 2, "weekly_goal.two_lessons_per_week.description", "two_lessons_per_week", "weekly_goal.two_lessons_per_week.name" },
                    { 3, "weekly_goal.three_lessons_per_week.description", "three_lessons_per_week", "weekly_goal.three_lessons_per_week.name" },
                    { 4, "weekly_goal.four_lessons_per_week.description", "four_lessons_per_week", "weekly_goal.four_lessons_per_week.name" },
                    { 5, "weekly_goal.five_and_more_lessons_per_week.description", "five_and_more_lessons_per_week", "weekly_goal.five_and_more_lessons_per_week.name" }
                });

            migrationBuilder.InsertData(
                table: "env_configuration_data_type",
                columns: new[] { "data_type_id", "description_key", "enum_key", "name_key" },
                values: new object[,]
                {
                    { 1, "env_configuration_data_type.string.description", "string", "env_configuration_data_type.string.name" },
                    { 2, "env_configuration_data_type.string_array.description", "string_array", "env_configuration_data_type.string_array.name" },
                    { 3, "env_configuration_data_type.integer.description", "integer", "env_configuration_data_type.integer.name" },
                    { 4, "env_configuration_data_type.object.description", "object", "env_configuration_data_type.object.name" },
                    { 5, "env_configuration_data_type.object_array.description", "object_array", "env_configuration_data_type.object_array.name" },
                    { 6, "env_configuration_data_type.boolean.description", "boolean", "env_configuration_data_type.boolean.name" },
                    { 7, "env_configuration_data_type.object_map.description", "object_map", "env_configuration_data_type.object_map.name" },
                    { 8, "env_configuration_data_type.integer_array.description", "integer_array", "env_configuration_data_type.integer_array.name" },
                    { 9, "env_configuration_data_type.password.description", "password", "env_configuration_data_type.password.name" },
                    { 10, "env_configuration_data_type.long.description", "long", "env_configuration_data_type.long.name" }
                });

            migrationBuilder.InsertData(
                table: "c_subject",
                columns: new[] { "subject_id", "category_id", "description_key", "enum_key", "footer_subject_landing_page_link_key", "footer_subject_teachers_page_link_key", "hero_description_key", "hero_title_key", "icon_name", "name_key", "object_state_id", "page_title_key" },
                values: new object[,]
                {
                    { 1, 1, "subject.english.description", "english", "subject.english.footer.lading_page_link", "subject.english.footer.subject_teachers_link", "subject.english.hero.description", "subject.english.hero.title", "british_flag", "subject.english.name", 1, "subject.english.page.title" },
                    { 2, 1, "subject.czech.description", "czech", "subject.czech.footer.lading_page_link", "subject.czech.footer.subject_teachers_link", "subject.czech.hero.description", "subject.czech.hero.title", "czech_flag", "subject.czech.name", 1, "subject.czech.page.title" },
                    { 3, 1, "subject.french.description", "french", "subject.french.footer.lading_page_link", "subject.french.footer.subject_teachers_link", "subject.french.hero.description", "subject.french.hero.title", "french_flag", "subject.french.name", 1, "subject.french.page.title" },
                    { 4, 1, "subject.spanish.description", "spanish", "subject.spanish.footer.lading_page_link", "subject.spanish.footer.subject_teachers_link", "subject.spanish.hero.description", "subject.spanish.hero.title", "spanish_flag", "subject.spanish.name", 1, "subject.spanish.page.title" },
                    { 5, 1, "subject.german.description", "german", "subject.german.footer.lading_page_link", "subject.german.footer.subject_teachers_link", "subject.german.hero.description", "subject.german.hero.title", "german_flag", "subject.german.name", 1, "subject.german.page.title" },
                    { 6, 1, "subject.italian.description", "italian", "subject.italian.footer.lading_page_link", "subject.italian.footer.subject_teachers_link", "subject.italian.hero.description", "subject.italian.hero.title", "italian_flag", "subject.italian.name", 1, "subject.italian.page.title" }
                });

            migrationBuilder.InsertData(
                table: "env_configuration",
                columns: new[] { "configuration_id", "admin_configurable", "backend_only", "configuration_key", "configuration_value", "data_type_id", "value_origin" },
                values: new object[,]
                {
                    { 1, true, true, "be.module.auth.settings.Secret", "", 9, "init_scripts" },
                    { 2, false, true, "be.module.auth.settings.ExpirationMs", "31104000000", 10, "init_scripts" },
                    { 3, true, true, "be.module.auth.settings.Issuer", "", 1, "init_scripts" },
                    { 4, true, false, "be.module.google_api.settings.ClientId", "", 1, "init_scripts" },
                    { 5, true, true, "be.module.google_api.settings.ClientSecret", "", 9, "init_scripts" },
                    { 6, true, false, "be.module.google_api.settings.TutorRedirectUrl", "", 1, "init_scripts" },
                    { 7, false, false, "be.module.google_api.settings.OauthUrl", "https://accounts.google.com/o/oauth2/v2/auth", 1, "init_scripts" },
                    { 8, false, false, "be.module.google_api.settings.TutorScopes", "[\"https://www.googleapis.com/auth/userinfo.profile\",\"https://www.googleapis.com/auth/userinfo.email\",\"https://www.googleapis.com/auth/calendar.events\"]", 2, "init_scripts" },
                    { 9, false, false, "be.module.google_api.settings.StudentScopes", "[\"https://www.googleapis.com/auth/userinfo.profile\",\"https://www.googleapis.com/auth/userinfo.email\"]", 2, "init_scripts" },
                    { 10, true, false, "be.module.seznam_api.settings.ClientId", "", 1, "init_scripts" },
                    { 11, true, true, "be.module.seznam_api.settings.ClientSecret", "", 9, "init_scripts" },
                    { 12, true, false, "be.module.seznam_api.settings.StudentRedirectUrl", "", 1, "init_scripts" },
                    { 13, false, false, "be.module.seznam_api.settings.OauthUrl", "https://login.szn.cz/api/v1/oauth/auth", 1, "init_scripts" },
                    { 14, false, false, "be.module.seznam_api.settings.Scopes", "[\"identity\",\"contact-phone\",\"avatar\"]", 2, "init_scripts" },
                    { 15, true, false, "be.module.facebook_api.settings.ClientId", "", 1, "init_scripts" },
                    { 16, true, true, "be.module.facebook_api.settings.ClientSecret", "", 9, "init_scripts" },
                    { 17, true, false, "be.module.facebook_api.settings.StudentRedirectUrl", "", 1, "init_scripts" },
                    { 18, false, false, "be.module.facebook_api.settings.OauthUrl", "https://www.facebook.com/v14.0/dialog/oauth", 1, "init_scripts" },
                    { 19, false, false, "be.module.facebook_api.settings.Scopes", "[\"email\",\"public_profile\",\"user_birthday\"]", 2, "init_scripts" },
                    { 20, true, true, "be.module.email.settings.ApiKey", null, 9, "init_scripts" },
                    { 21, true, false, "be.module.registration.settings.RedirectUrl", null, 1, "init_scripts" },
                    { 22, true, true, "be.module.email.settings.SenderEmail", "noreply@ulearn.cz", 1, "init_scripts" },
                    { 23, true, true, "be.module.email.settings.SenderName", "ULearn.cz s.r.o.", 1, "init_scripts" },
                    { 24, true, false, "be.module.google_api.settings.StudentRedirectUrl", null, 1, "init_scripts" },
                    { 25, true, false, "be.module.fe.settings.FeUrl", null, 1, "init_scripts" },
                    { 26, true, false, "be.fileservice.disk.settings.FileStoragePath", "C:\\ServerData\\ulearn-test\\", 1, "init_scripts" },
                    { 27, true, false, "be.fileservice.azureBlob.settings.ContainerName", null, 1, "init_scripts" },
                    { 28, true, false, "be.fileservice.azureBlob.settings.Connection", null, 1, "init_scripts" },
                    { 29, true, false, "be.fileservice.azureBlob.settings.Password", null, 9, "init_scripts" },
                    { 30, true, false, "be.instance.settings.InstanceName", "", 1, "init_scripts" },
                    { 31, true, false, "be.fileservice.auth.settings.ApiUrl", "", 1, "init_scripts" },
                    { 32, true, false, "be.fileservice.auth.settings.AccessToken", "", 1, "init_scripts" },
                    { 33, true, false, "be.videocall.settings.Servers", "[]", 5, "init_scripts" },
                    { 34, true, false, "be.videocall.settings.Trickle", "true", 6, "init_scripts" },
                    { 35, true, true, "be.module.auth.settings.Domain", "localhost", 1, "init_scripts" },
                    { 36, true, true, "be.module.registration.settings.ResetPasswordRedirectUrl", "", 1, "init_scripts" },
                    { 37, true, false, "be.module.facebook_api.settings.TutorRedirectUrl", "", 1, "init_scripts" },
                    { 38, true, false, "be.module.seznam_api.settings.TutorRedirectUrl", "", 1, "init_scripts" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_app_conversation_initiator_id",
                table: "app_conversation",
                column: "initiator_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_conversation_opponent_id",
                table: "app_conversation",
                column: "opponent_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_message_conversation_id",
                table: "app_message",
                column: "conversation_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_message_message_state_id",
                table: "app_message",
                column: "message_state_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_message_receiver_id",
                table: "app_message",
                column: "receiver_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_message_sender_id",
                table: "app_message",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_message_attachment_message_id",
                table: "app_message_attachment",
                column: "message_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_recurrent_timeslot_owner_id",
                table: "app_recurrent_timeslot",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_recurrent_timeslot_blacklist_user_recurrent_event_id",
                table: "app_recurrent_timeslot_blacklist",
                column: "recurrent_timeslot_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_reservation_attender_id",
                table: "app_reservation",
                column: "attender_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_reservation_host_id",
                table: "app_reservation",
                column: "host_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_reservation_host_id_attender_id_start_local_time_end_local_time",
                table: "app_reservation",
                columns: new[] { "host_id", "attender_id", "start_time", "end_time" });

            migrationBuilder.CreateIndex(
                name: "ix_app_reservation_reservation_state_id",
                table: "app_reservation",
                column: "reservation_state_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_reservation_room_id",
                table: "app_reservation",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_reservation_subject_id",
                table: "app_reservation",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_room_room_attender_id",
                table: "app_room",
                column: "attender_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_room_room_host_id",
                table: "app_room",
                column: "host_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_room_room_state_id",
                table: "app_room",
                column: "room_state_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_room_activity_room_activity_id",
                table: "app_room_activity",
                column: "room_activity_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_room_activity_room_id",
                table: "app_room_activity",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_room_activity_user_id",
                table: "app_room_activity",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_room_activity_user_id_room_id_created",
                table: "app_room_activity",
                columns: new[] { "user_id", "room_id", "created" });

            migrationBuilder.CreateIndex(
                name: "ix_app_timeslot_owner_id",
                table: "app_timeslot",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_user_country_id",
                table: "app_user",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_user_currency_id",
                table: "app_user",
                column: "currency_id");

            migrationBuilder.CreateIndex(
                name: "IX_app_user_subject_id",
                table: "app_user",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_user_taught_subject_id",
                table: "app_user",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_user_teaching_experience_id",
                table: "app_user",
                column: "teaching_experience_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_user_translation_language_id",
                table: "app_user",
                column: "translation_language_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_user_user_role_id",
                table: "app_user",
                column: "user_role_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_user_user_state_id",
                table: "app_user",
                column: "user_state_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_user_weekly_goal_id",
                table: "app_user",
                column: "weekly_goal_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_user_behaviour_connected_disconnected",
                table: "app_user_behaviour",
                columns: new[] { "connected", "disconnected" });

            migrationBuilder.CreateIndex(
                name: "ix_app_user_behaviour_connection_uuid",
                table: "app_user_behaviour",
                column: "connection_uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_app_user_behaviour_user_id",
                table: "app_user_behaviour",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_user_registration_code_user_id",
                table: "app_user_registration_code",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_user_registration_registration_code",
                table: "app_user_registration_code",
                column: "registration_code");

            migrationBuilder.CreateIndex(
                name: "ix_app_user_review_reviewee_id",
                table: "app_user_review",
                column: "reviewee_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_user_review_reviewer_id",
                table: "app_user_review",
                column: "reviewer_id");

            migrationBuilder.CreateIndex(
                name: "IX_app_user_wallet_currency_id",
                table: "app_user_wallet",
                column: "currency_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_user_wallet_user_id",
                table: "app_user_wallet",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_user_wallet_transaction_payment_service_provider_id",
                table: "app_user_wallet_transaction",
                column: "payment_service_provider_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_user_wallet_transaction_transaction_state_id",
                table: "app_user_wallet_transaction",
                column: "transaction_state_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_user_wallet_transaction_transaction_type_id",
                table: "app_user_wallet_transaction",
                column: "transaction_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_user_wallet_transaction_user_wallet_id",
                table: "app_user_wallet_transaction",
                column: "user_wallet_id");

            migrationBuilder.CreateIndex(
                name: "ix_c_category_enum_key",
                table: "c_category",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_country_enum_key",
                table: "c_country",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_currency_enum_key",
                table: "c_currency",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_file_type_enum_key",
                table: "c_file_type",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_language_enum_key",
                table: "c_language",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_language_level_enum_key",
                table: "c_language_level",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_master_hash_enum_key",
                table: "c_master_hash",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_master_hash_table_name",
                table: "c_master_hash",
                column: "table_name");

            migrationBuilder.CreateIndex(
                name: "ix_c_message_state_enum_key",
                table: "c_message_state",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_object_state_enum_key",
                table: "c_object_state",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_object_state_object_state_id",
                table: "c_object_state",
                column: "object_state_id");

            migrationBuilder.CreateIndex(
                name: "ix_c_payment_service_provider_enum_key",
                table: "c_payment_service_provider",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_reservation_state_enum_key",
                table: "c_reservation_state",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_subject_category_id",
                table: "c_subject",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_c_subject_enum_key",
                table: "c_subject",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_c_subject_object_state_id",
                table: "c_subject",
                column: "object_state_id");

            migrationBuilder.CreateIndex(
                name: "ix_c_subject_level_enum_key",
                table: "c_subject_level",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_teaching_experience_enum_key",
                table: "c_teaching_experience",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_token_provider_enum_key",
                table: "c_token_provider",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_token_type_enum_key",
                table: "c_token_type",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_transaction_state_enum_key",
                table: "c_transaction_state",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_transaction_type_enum_key",
                table: "c_transaction_type",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_translation_creator_id",
                table: "c_translation",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "ix_c_translation_translation_key_id",
                table: "c_translation",
                column: "translation_key_id");

            migrationBuilder.CreateIndex(
                name: "IX_c_translation_translation_language_id",
                table: "c_translation",
                column: "translation_language_id");

            migrationBuilder.CreateIndex(
                name: "ix_c_translation_updater_id",
                table: "c_translation",
                column: "updater_id");

            migrationBuilder.CreateIndex(
                name: "ix_c_translation_key_creator_id",
                table: "c_translation_key",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "ix_c_translation_key_translation_namespace_id",
                table: "c_translation_key",
                column: "translation_namespace_id");

            migrationBuilder.CreateIndex(
                name: "ix_c_translation_key_updater_id",
                table: "c_translation_key",
                column: "updater_id");

            migrationBuilder.CreateIndex(
                name: "ix_c_translation_language_language_code",
                table: "c_translation_language",
                column: "language_code");

            migrationBuilder.CreateIndex(
                name: "ix_c_translation_namespace_creator_id",
                table: "c_translation_namespace",
                column: "creator_id");

            migrationBuilder.CreateIndex(
                name: "ix_c_translation_namespace_updater_id",
                table: "c_translation_namespace",
                column: "updater_id");

            migrationBuilder.CreateIndex(
                name: "ix_c_attribute_type_enum_key",
                table: "c_user_attribute_type",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_user_profile_enum_key",
                table: "c_user_role",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_user_state_enum_key",
                table: "c_user_state",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_weekly_goal_enum_key",
                table: "c_weekly_goal",
                column: "enum_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_env_configuration_backend_only",
                table: "env_configuration",
                column: "backend_only");

            migrationBuilder.CreateIndex(
                name: "ix_env_configuration_data_type_data_type_id",
                table: "env_configuration",
                column: "data_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_r_user_attribute_user_attribute_type_id",
                table: "r_user_attribute",
                column: "user_attribute_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_r_user_attribute_user_id",
                table: "r_user_attribute",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "uc_r_user_attribute_user_id_user_attribute_type_id",
                table: "r_user_attribute",
                columns: new[] { "user_id", "user_attribute_type_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_r_user_file_file_type_id",
                table: "r_user_file",
                column: "file_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_r_user_file_user_id",
                table: "r_user_file",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_r_user_file_permission_user_id",
                table: "r_user_file_permission",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "uc_r_user_file_permission_user_id_file_path",
                table: "r_user_file_permission",
                columns: new[] { "user_id", "file_path" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_r_user_language_language_id",
                table: "r_user_language",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "ix_r_user_language_language_level_id",
                table: "r_user_language",
                column: "language_level_id");

            migrationBuilder.CreateIndex(
                name: "ix_r_user_language_user_id",
                table: "r_user_language",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "uc_r_user_language_user_id_language_id",
                table: "r_user_language",
                columns: new[] { "user_id", "language_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_r_user_room_statistic_room_id",
                table: "r_user_room_statistic",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "ix_r_user_room_statistic_room_statistics_type_id",
                table: "r_user_room_statistic",
                column: "room_statistics_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_r_user_room_statistic_user_id",
                table: "r_user_room_statistic",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_r_user_specialization_specialization_id",
                table: "r_user_specialization",
                column: "specialization_id");

            migrationBuilder.CreateIndex(
                name: "ix_r_user_specialization_user_id",
                table: "r_user_specialization",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "uc_r_user_specialization_user_id_specialization",
                table: "r_user_specialization",
                columns: new[] { "user_id", "specialization_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_r_user_subject_level_subject_level_id",
                table: "r_user_subject_level",
                column: "subject_level_id");

            migrationBuilder.CreateIndex(
                name: "ix_r_user_subject_level_user_id",
                table: "r_user_subject_level",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "uc_r_user_subject_level_user_id_subject_level_id",
                table: "r_user_subject_level",
                columns: new[] { "user_id", "subject_level_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_c_token_type_token_provider_id",
                table: "r_user_token",
                column: "token_provider_id");

            migrationBuilder.CreateIndex(
                name: "ix_r_user_token_token_type_id",
                table: "r_user_token",
                column: "token_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_r_user_token_user_id",
                table: "r_user_token",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "app_message_attachment");

            migrationBuilder.DropTable(
                name: "app_recurrent_timeslot_blacklist");

            migrationBuilder.DropTable(
                name: "app_reservation");

            migrationBuilder.DropTable(
                name: "app_room_activity");

            migrationBuilder.DropTable(
                name: "app_timeslot");

            migrationBuilder.DropTable(
                name: "app_user_behaviour");

            migrationBuilder.DropTable(
                name: "app_user_registration_code");

            migrationBuilder.DropTable(
                name: "app_user_review");

            migrationBuilder.DropTable(
                name: "app_user_wallet_transaction");

            migrationBuilder.DropTable(
                name: "c_master_hash");

            migrationBuilder.DropTable(
                name: "c_translation");

            migrationBuilder.DropTable(
                name: "env_configuration");

            migrationBuilder.DropTable(
                name: "r_user_attribute");

            migrationBuilder.DropTable(
                name: "r_user_file");

            migrationBuilder.DropTable(
                name: "r_user_file_permission");

            migrationBuilder.DropTable(
                name: "r_user_language");

            migrationBuilder.DropTable(
                name: "r_user_room_statistic");

            migrationBuilder.DropTable(
                name: "r_user_specialization");

            migrationBuilder.DropTable(
                name: "r_user_subject_level");

            migrationBuilder.DropTable(
                name: "r_user_token");

            migrationBuilder.DropTable(
                name: "app_message");

            migrationBuilder.DropTable(
                name: "app_recurrent_timeslot");

            migrationBuilder.DropTable(
                name: "c_reservation_state");

            migrationBuilder.DropTable(
                name: "c_room_activity_type");

            migrationBuilder.DropTable(
                name: "app_user_wallet");

            migrationBuilder.DropTable(
                name: "c_transaction_state");

            migrationBuilder.DropTable(
                name: "c_transaction_type");

            migrationBuilder.DropTable(
                name: "c_payment_service_provider");

            migrationBuilder.DropTable(
                name: "c_translation_key");

            migrationBuilder.DropTable(
                name: "env_configuration_data_type");

            migrationBuilder.DropTable(
                name: "c_user_attribute_type");

            migrationBuilder.DropTable(
                name: "c_file_type");

            migrationBuilder.DropTable(
                name: "c_language");

            migrationBuilder.DropTable(
                name: "c_language_level");

            migrationBuilder.DropTable(
                name: "app_room");

            migrationBuilder.DropTable(
                name: "c_room_statistic_type");

            migrationBuilder.DropTable(
                name: "c_specialization");

            migrationBuilder.DropTable(
                name: "c_subject_level");

            migrationBuilder.DropTable(
                name: "c_token_provider");

            migrationBuilder.DropTable(
                name: "c_token_type");

            migrationBuilder.DropTable(
                name: "app_conversation");

            migrationBuilder.DropTable(
                name: "c_message_state");

            migrationBuilder.DropTable(
                name: "c_translation_namespace");

            migrationBuilder.DropTable(
                name: "c_room_state");

            migrationBuilder.DropTable(
                name: "app_user");

            migrationBuilder.DropTable(
                name: "c_country");

            migrationBuilder.DropTable(
                name: "c_currency");

            migrationBuilder.DropTable(
                name: "c_subject");

            migrationBuilder.DropTable(
                name: "c_teaching_experience");

            migrationBuilder.DropTable(
                name: "c_translation_language");

            migrationBuilder.DropTable(
                name: "c_user_role");

            migrationBuilder.DropTable(
                name: "c_user_state");

            migrationBuilder.DropTable(
                name: "c_weekly_goal");

            migrationBuilder.DropTable(
                name: "c_category");

            migrationBuilder.DropTable(
                name: "c_object_state");
        }
    }
}
