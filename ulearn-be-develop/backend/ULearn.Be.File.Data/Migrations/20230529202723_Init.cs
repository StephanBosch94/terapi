using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ULearn.Be.File.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sf");

            migrationBuilder.CreateTable(
                name: "c_compression_type",
                schema: "sf",
                columns: table => new
                {
                    compression_type_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_Key = table.Column<string>(type: "text", nullable: true),
                    name_Key = table.Column<string>(type: "text", nullable: true),
                    description_Key = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_compression_type", x => x.compression_type_id);
                });

            migrationBuilder.CreateTable(
                name: "c_file_format",
                schema: "sf",
                columns: table => new
                {
                    file_format_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    extension = table.Column<string>(type: "text", nullable: true),
                    enum_key = table.Column<string>(type: "text", nullable: true),
                    name_key = table.Column<string>(type: "text", nullable: true),
                    description_key = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_file_format", x => x.file_format_id);
                });

            migrationBuilder.CreateTable(
                name: "c_file_state",
                schema: "sf",
                columns: table => new
                {
                    file_state_Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enum_key2 = table.Column<string>(type: "text", nullable: true),
                    name_key = table.Column<string>(type: "text", nullable: true),
                    description_Key = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_c_file_state", x => x.file_state_Id);
                });

            migrationBuilder.CreateTable(
                name: "r_file_format_compression_type",
                schema: "sf",
                columns: table => new
                {
                    file_format_id = table.Column<int>(type: "integer", nullable: false),
                    compression_type_id2 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_r_file_compression_type", x => new { x.file_format_id, x.compression_type_id2 });
                    table.ForeignKey(
                        name: "fk_r_file_format_compression_type_c_compression_type_compression_type_id",
                        column: x => x.compression_type_id2,
                        principalSchema: "sf",
                        principalTable: "c_compression_type",
                        principalColumn: "compression_type_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_r_file_format_compression_type_c_file_format_file_format_id",
                        column: x => x.file_format_id,
                        principalSchema: "sf",
                        principalTable: "c_file_format",
                        principalColumn: "file_format_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "app_file",
                schema: "sf",
                columns: table => new
                {
                    file_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    unique_key = table.Column<string>(type: "text", nullable: true),
                    content_type = table.Column<string>(type: "text", nullable: true),
                    path = table.Column<string>(type: "text", nullable: true),
                    file_name = table.Column<string>(type: "text", nullable: true),
                    environment = table.Column<string>(type: "text", nullable: true),
                    creator_id = table.Column<int>(type: "integer", nullable: false),
                    updater_id = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    state_id = table.Column<int>(type: "integer", nullable: false),
                    compression_type_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_file", x => x.file_id);
                    table.ForeignKey(
                        name: "fk_app_file_c_compression_type_compression_type_id",
                        column: x => x.compression_type_id,
                        principalSchema: "sf",
                        principalTable: "c_compression_type",
                        principalColumn: "compression_type_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_app_file_c_file_state_file_state_id",
                        column: x => x.state_id,
                        principalSchema: "sf",
                        principalTable: "c_file_state",
                        principalColumn: "file_state_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "sf",
                table: "c_compression_type",
                columns: new[] { "compression_type_id", "description_Key", "enum_Key", "name_Key" },
                values: new object[,]
                {
                    { 1, "file_state.none.description", "none", "file_state.none.name" },
                    { 2, "file_state.zip.description", "zip", "file_state.zip.name" }
                });

            migrationBuilder.InsertData(
                schema: "sf",
                table: "c_file_state",
                columns: new[] { "file_state_Id", "description_Key", "enum_key2", "name_key" },
                values: new object[,]
                {
                    { 10, "file_state.active.description", "active", "file_state.active.name" },
                    { 20, "file_state.inactive.description", "inactive", "file_state.inactive.name" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_app_file_c_compression_type_compression_type_id",
                schema: "sf",
                table: "app_file",
                column: "compression_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_app_file_c_state_state_id",
                schema: "sf",
                table: "app_file",
                column: "state_id");

            migrationBuilder.CreateIndex(
                name: "ix_r_file_format_compression_type_c_compression_type_compression_type_id",
                schema: "sf",
                table: "r_file_format_compression_type",
                column: "compression_type_id2");

            migrationBuilder.CreateIndex(
                name: "ix_r_file_format_compression_type_c_file_format_file_format_id",
                schema: "sf",
                table: "r_file_format_compression_type",
                column: "file_format_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "app_file",
                schema: "sf");

            migrationBuilder.DropTable(
                name: "r_file_format_compression_type",
                schema: "sf");

            migrationBuilder.DropTable(
                name: "c_file_state",
                schema: "sf");

            migrationBuilder.DropTable(
                name: "c_compression_type",
                schema: "sf");

            migrationBuilder.DropTable(
                name: "c_file_format",
                schema: "sf");
        }
    }
}
