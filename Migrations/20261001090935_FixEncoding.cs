using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHisseler.Web.Migrations
{
    /// <inheritdoc />
    public partial class FixEncoding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 13, 9, 35, 282, DateTimeKind.Local).AddTicks(3154));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 13, 9, 35, 282, DateTimeKind.Local).AddTicks(3166));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 13, 9, 35, 282, DateTimeKind.Local).AddTicks(3169));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 13, 9, 35, 282, DateTimeKind.Local).AddTicks(3171));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 13, 9, 35, 282, DateTimeKind.Local).AddTicks(3173));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 13, 9, 35, 282, DateTimeKind.Local).AddTicks(3175));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 13, 9, 35, 282, DateTimeKind.Local).AddTicks(3177));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 13, 9, 35, 282, DateTimeKind.Local).AddTicks(3179));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 13, 9, 35, 282, DateTimeKind.Local).AddTicks(3181));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 13, 8, 43, 53, DateTimeKind.Local).AddTicks(32));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 13, 8, 43, 53, DateTimeKind.Local).AddTicks(45));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 13, 8, 43, 53, DateTimeKind.Local).AddTicks(47));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 13, 8, 43, 53, DateTimeKind.Local).AddTicks(49));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 13, 8, 43, 53, DateTimeKind.Local).AddTicks(51));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 13, 8, 43, 53, DateTimeKind.Local).AddTicks(53));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 13, 8, 43, 53, DateTimeKind.Local).AddTicks(54));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 13, 8, 43, 53, DateTimeKind.Local).AddTicks(57));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 13, 8, 43, 53, DateTimeKind.Local).AddTicks(59));
        }
    }
}
