using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ULearn.Be.Data.Migrations
{
    public partial class PriceLocalization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_base",
                table: "c_currency",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_sync",
                table: "c_currency",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "rate",
                table: "c_currency",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "symbol",
                table: "c_currency",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "hour_rate",
                table: "app_user",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "c_currency",
                keyColumn: "currency_id",
                keyValue: 1,
                column: "symbol",
                value: "Kč");

            migrationBuilder.InsertData(
                table: "c_currency",
                columns: new[] { "currency_id", "description_key", "enum_key", "is_base", "last_sync", "name_key", "rate", "symbol" },
                values: new object[,]
                {
                    { 2, "currency.eur.description", "eur", true, null, "currency.eur.name", 0.0, "€" },
                    { 3, "currency.usd.description", "usd", false, null, "currency.usd.name", 0.0, "$" },
                    { 4, "currency.gbp.description", "gbp", false, null, "currency.gbp.name", 0.0, "£" },
                    { 5, "currency.rsd.description", "rsd", false, null, "currency.rsd.name", 0.0, "Din" },
                    { 6, "currency.pln.description", "pln", false, null, "currency.pln.name", 0.0, "zł" },
                    { 7, "currency.uah.description", "uah", false, null, "currency.uah.name", 0.0, "₴" },
                    { 8, "currency.kzt.description", "kzt", false, null, "currency.kzt.name", 0.0, "₸" },
                    { 9, "currency.rub.description", "rub", false, null, "currency.rub.name", 0.0, "₽" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "c_currency",
                keyColumn: "currency_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "c_currency",
                keyColumn: "currency_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "c_currency",
                keyColumn: "currency_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "c_currency",
                keyColumn: "currency_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "c_currency",
                keyColumn: "currency_id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "c_currency",
                keyColumn: "currency_id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "c_currency",
                keyColumn: "currency_id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "c_currency",
                keyColumn: "currency_id",
                keyValue: 9);

            migrationBuilder.DropColumn(
                name: "is_base",
                table: "c_currency");

            migrationBuilder.DropColumn(
                name: "last_sync",
                table: "c_currency");

            migrationBuilder.DropColumn(
                name: "rate",
                table: "c_currency");

            migrationBuilder.DropColumn(
                name: "symbol",
                table: "c_currency");

            migrationBuilder.AlterColumn<int>(
                name: "hour_rate",
                table: "app_user",
                type: "integer",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);
        }
    }
}
