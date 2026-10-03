using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EHisseler.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "QualityTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Description",
                value: "Æsas funksiyalar, ən aşağı qiymət");

            migrationBuilder.UpdateData(
                table: "SellerProfiles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ContactPerson",
                value: "Rəşad Æliyev");

            migrationBuilder.UpdateData(
                table: "SellerProfiles",
                keyColumn: "Id",
                keyValue: 3,
                column: "Address",
                value: "Bakı, Nərimanov rayonu, Æhmədli küç. 9");

            migrationBuilder.UpdateData(
                table: "SpareParts",
                keyColumn: "Id",
                keyValue: 5,
                column: "Description",
                value: "6.1 düym AMOLED ekran, B-class keyfiyyət. Æsas funksiyalar tam işləyir, qiymət əlverişlidir.");

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
                columns: new[] { "CreatedDate", "LastName" },
                values: new object[] { new DateTime(2026, 10, 1, 13, 8, 43, 53, DateTimeKind.Local).AddTicks(45), "Æliyev" });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "QualityTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Description",
                value: "Əsas funksiyalar, ən aşağı qiymət");

            migrationBuilder.UpdateData(
                table: "SellerProfiles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ContactPerson",
                value: "Rəşad Əliyev");

            migrationBuilder.UpdateData(
                table: "SellerProfiles",
                keyColumn: "Id",
                keyValue: 3,
                column: "Address",
                value: "Bakı, Nərimanov rayonu, Əhmədli küç. 9");

            migrationBuilder.UpdateData(
                table: "SpareParts",
                keyColumn: "Id",
                keyValue: 5,
                column: "Description",
                value: "6.1 düym AMOLED ekran, B-class keyfiyyət. Əsas funksiyalar tam işləyir, qiymət əlverişlidir.");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 12, 37, 3, 942, DateTimeKind.Local).AddTicks(1999));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "LastName" },
                values: new object[] { new DateTime(2026, 10, 1, 12, 37, 3, 942, DateTimeKind.Local).AddTicks(2019), "Əliyev" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 12, 37, 3, 942, DateTimeKind.Local).AddTicks(2022));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 12, 37, 3, 942, DateTimeKind.Local).AddTicks(2024));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 12, 37, 3, 942, DateTimeKind.Local).AddTicks(2026));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 12, 37, 3, 942, DateTimeKind.Local).AddTicks(2027));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 12, 37, 3, 942, DateTimeKind.Local).AddTicks(2029));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 12, 37, 3, 942, DateTimeKind.Local).AddTicks(2032));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedDate",
                value: new DateTime(2026, 10, 1, 12, 37, 3, 942, DateTimeKind.Local).AddTicks(2034));
        }
    }
}
