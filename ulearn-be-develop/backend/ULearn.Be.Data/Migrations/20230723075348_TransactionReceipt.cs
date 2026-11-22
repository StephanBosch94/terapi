using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class TransactionReceipt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "receipt_url",
                table: "app_transaction",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "receipt_url",
                table: "app_transaction");
        }
    }
}
