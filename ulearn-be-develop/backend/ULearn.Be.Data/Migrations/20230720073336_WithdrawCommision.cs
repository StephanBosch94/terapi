using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class WithdrawCommision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "source_transaction_id",
                table: "app_transaction",
                type: "integer",
                nullable: true);

            migrationBuilder.InsertData(
                table: "c_transaction_type",
                columns: new[] { "transaction_type_id", "description_key", "enum_key", "name_key" },
                values: new object[] { 13, "transaction_type.withdraw_fee.description", "withdraw_fee", "transaction_type.withdraw_fee.name" });

            migrationBuilder.CreateIndex(
                name: "ix_app_transaction_source_transaction_id",
                table: "app_transaction",
                column: "source_transaction_id");

            migrationBuilder.AddForeignKey(
                name: "fk_app_transaction_app_transaction_source_transaction_id",
                table: "app_transaction",
                column: "source_transaction_id",
                principalTable: "app_transaction",
                principalColumn: "transaction_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_app_transaction_app_transaction_source_transaction_id",
                table: "app_transaction");

            migrationBuilder.DropIndex(
                name: "ix_app_transaction_source_transaction_id",
                table: "app_transaction");

            migrationBuilder.DeleteData(
                table: "c_transaction_type",
                keyColumn: "transaction_type_id",
                keyValue: 13);

            migrationBuilder.DropColumn(
                name: "source_transaction_id",
                table: "app_transaction");
        }
    }
}
