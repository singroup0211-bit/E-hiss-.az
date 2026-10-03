using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EHisseler.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddModerationStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SparePartSales");

            migrationBuilder.DeleteData(
                table: "SparePartImages",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SparePartImages",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "SparePartImages",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "SparePartImages",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "SpareParts",
                newName: "ModerationStatus");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Users",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "SpareParts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "SpareParts",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "SellerProfiles",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateTable(
                name: "SparePartSaleAudits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SparePartId = table.Column<int>(type: "INTEGER", nullable: false),
                    SellerId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    PreviousStock = table.Column<int>(type: "INTEGER", nullable: false),
                    NewStock = table.Column<int>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SparePartSaleAudits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SparePartSaleAudits_SpareParts_SparePartId",
                        column: x => x.SparePartId,
                        principalTable: "SpareParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "SparePartImages",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "/images/images (1).jpg");

            migrationBuilder.UpdateData(
                table: "SparePartImages",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "/images/images (2).jpg");

            migrationBuilder.UpdateData(
                table: "SparePartImages",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "/images/images (3).jpg");

            migrationBuilder.UpdateData(
                table: "SparePartImages",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "/images/images (4).jpg");

            migrationBuilder.UpdateData(
                table: "SparePartImages",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageUrl",
                value: "/images/images (5).jpg");

            migrationBuilder.UpdateData(
                table: "SparePartImages",
                keyColumn: "Id",
                keyValue: 9,
                column: "ImageUrl",
                value: "/images/images (6).jpg");

            migrationBuilder.UpdateData(
                table: "SparePartImages",
                keyColumn: "Id",
                keyValue: 10,
                column: "ImageUrl",
                value: "/images/images (7).jpg");

            migrationBuilder.UpdateData(
                table: "SparePartImages",
                keyColumn: "Id",
                keyValue: 11,
                column: "ImageUrl",
                value: "/images/images.jpg");

            migrationBuilder.UpdateData(
                table: "SparePartImages",
                keyColumn: "Id",
                keyValue: 12,
                column: "ImageUrl",
                value: "/images/images (1).jpg");

            migrationBuilder.UpdateData(
                table: "SpareParts",
                keyColumn: "Id",
                keyValue: 1,
                column: "RejectionReason",
                value: null);

            migrationBuilder.UpdateData(
                table: "SpareParts",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ModerationStatus", "RejectionReason" },
                values: new object[] { 2, null });

            migrationBuilder.UpdateData(
                table: "SpareParts",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ModerationStatus", "RejectionReason" },
                values: new object[] { 2, null });

            migrationBuilder.UpdateData(
                table: "SpareParts",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ModerationStatus", "RejectionReason" },
                values: new object[] { 2, null });

            migrationBuilder.UpdateData(
                table: "SpareParts",
                keyColumn: "Id",
                keyValue: 5,
                column: "RejectionReason",
                value: null);

            migrationBuilder.UpdateData(
                table: "SpareParts",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ModerationStatus", "RejectionReason" },
                values: new object[] { 2, null });

            migrationBuilder.UpdateData(
                table: "SpareParts",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ModerationStatus", "RejectionReason" },
                values: new object[] { 2, null });

            migrationBuilder.UpdateData(
                table: "SpareParts",
                keyColumn: "Id",
                keyValue: 8,
                column: "RejectionReason",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "Email" },
                values: new object[] { new DateTime(2026, 10, 2, 16, 57, 6, 836, DateTimeKind.Local).AddTicks(4687), "elcin@example.com" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "Email", "LastName" },
                values: new object[] { new DateTime(2026, 10, 2, 16, 57, 6, 836, DateTimeKind.Local).AddTicks(4697), "rashad@example.com", "Æ liyev" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "Email" },
                values: new object[] { new DateTime(2026, 10, 2, 16, 57, 6, 836, DateTimeKind.Local).AddTicks(4699), "nigar@example.com" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "Email" },
                values: new object[] { new DateTime(2026, 10, 2, 16, 57, 6, 836, DateTimeKind.Local).AddTicks(4701), "admin@example.com" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "Email" },
                values: new object[] { new DateTime(2026, 10, 2, 16, 57, 6, 836, DateTimeKind.Local).AddTicks(4702), "samir@example.com" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "Email" },
                values: new object[] { new DateTime(2026, 10, 2, 16, 57, 6, 836, DateTimeKind.Local).AddTicks(4704), "vuqar@example.com" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedDate", "Email" },
                values: new object[] { new DateTime(2026, 10, 2, 16, 57, 6, 836, DateTimeKind.Local).AddTicks(4706), "leyla@example.com" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedDate", "Email" },
                values: new object[] { new DateTime(2026, 10, 2, 16, 57, 6, 836, DateTimeKind.Local).AddTicks(4708), "taleh@example.com" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedDate", "Email" },
                values: new object[] { new DateTime(2026, 10, 2, 16, 57, 6, 836, DateTimeKind.Local).AddTicks(4709), "rustam@example.com" });

            migrationBuilder.CreateIndex(
                name: "IX_SparePartSaleAudits_SparePartId",
                table: "SparePartSaleAudits",
                column: "SparePartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SparePartSaleAudits");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "SpareParts");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "SpareParts");

            migrationBuilder.RenameColumn(
                name: "ModerationStatus",
                table: "SpareParts",
                newName: "Status");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "SellerProfiles",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "SparePartSales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SparePartId = table.Column<int>(type: "INTEGER", nullable: false),
                    NewStock = table.Column<int>(type: "INTEGER", nullable: false),
                    PreviousStock = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    SaleDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SoldByUserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SparePartSales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SparePartSales_SpareParts_SparePartId",
                        column: x => x.SparePartId,
                        principalTable: "SpareParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "SparePartImages",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://cdn.ehisse.az/parts/iphone15pm-screen-1.jpg");

            migrationBuilder.UpdateData(
                table: "SparePartImages",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImageUrl",
                value: "https://cdn.ehisse.az/parts/iphone15pm-screen-2.jpg");

            migrationBuilder.UpdateData(
                table: "SparePartImages",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImageUrl",
                value: "https://cdn.ehisse.az/parts/s24u-battery-1.jpg");

            migrationBuilder.UpdateData(
                table: "SparePartImages",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImageUrl",
                value: "https://cdn.ehisse.az/parts/iphone13-camera-1.jpg");

            migrationBuilder.UpdateData(
                table: "SparePartImages",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImageUrl",
                value: "https://cdn.ehisse.az/parts/xiaomi13-mb-1.jpg");

            migrationBuilder.UpdateData(
                table: "SparePartImages",
                keyColumn: "Id",
                keyValue: 9,
                column: "ImageUrl",
                value: "https://cdn.ehisse.az/parts/s23-screen-1.jpg");

            migrationBuilder.UpdateData(
                table: "SparePartImages",
                keyColumn: "Id",
                keyValue: 10,
                column: "ImageUrl",
                value: "https://cdn.ehisse.az/parts/mbp14-battery-1.jpg");

            migrationBuilder.UpdateData(
                table: "SparePartImages",
                keyColumn: "Id",
                keyValue: 11,
                column: "ImageUrl",
                value: "https://cdn.ehisse.az/parts/xps13-kb-1.jpg");

            migrationBuilder.UpdateData(
                table: "SparePartImages",
                keyColumn: "Id",
                keyValue: 12,
                column: "ImageUrl",
                value: "https://cdn.ehisse.az/parts/ipadpro-screen-1.jpg");

            migrationBuilder.InsertData(
                table: "SparePartImages",
                columns: new[] { "Id", "DisplayOrder", "ImageUrl", "IsMain", "SparePartId" },
                values: new object[,]
                {
                    { 3, 3, "https://cdn.ehisse.az/parts/iphone15pm-screen-3.jpg", false, 1 },
                    { 5, 2, "https://cdn.ehisse.az/parts/s24u-battery-2.jpg", false, 2 },
                    { 8, 2, "https://cdn.ehisse.az/parts/xiaomi13-mb-2.jpg", false, 4 },
                    { 13, 2, "https://cdn.ehisse.az/parts/ipadpro-screen-2.jpg", false, 8 }
                });

            migrationBuilder.UpdateData(
                table: "SpareParts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Status",
                value: 4);

            migrationBuilder.UpdateData(
                table: "SpareParts",
                keyColumn: "Id",
                keyValue: 3,
                column: "Status",
                value: 4);

            migrationBuilder.UpdateData(
                table: "SpareParts",
                keyColumn: "Id",
                keyValue: 4,
                column: "Status",
                value: 4);

            migrationBuilder.UpdateData(
                table: "SpareParts",
                keyColumn: "Id",
                keyValue: 6,
                column: "Status",
                value: 4);

            migrationBuilder.UpdateData(
                table: "SpareParts",
                keyColumn: "Id",
                keyValue: 7,
                column: "Status",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "Email" },
                values: new object[] { new DateTime(2026, 10, 1, 13, 9, 35, 282, DateTimeKind.Local).AddTicks(3154), null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "Email", "LastName" },
                values: new object[] { new DateTime(2026, 10, 1, 13, 9, 35, 282, DateTimeKind.Local).AddTicks(3166), null, "Æliyev" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "Email" },
                values: new object[] { new DateTime(2026, 10, 1, 13, 9, 35, 282, DateTimeKind.Local).AddTicks(3169), null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "Email" },
                values: new object[] { new DateTime(2026, 10, 1, 13, 9, 35, 282, DateTimeKind.Local).AddTicks(3171), null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "Email" },
                values: new object[] { new DateTime(2026, 10, 1, 13, 9, 35, 282, DateTimeKind.Local).AddTicks(3173), null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "Email" },
                values: new object[] { new DateTime(2026, 10, 1, 13, 9, 35, 282, DateTimeKind.Local).AddTicks(3175), null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedDate", "Email" },
                values: new object[] { new DateTime(2026, 10, 1, 13, 9, 35, 282, DateTimeKind.Local).AddTicks(3177), null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedDate", "Email" },
                values: new object[] { new DateTime(2026, 10, 1, 13, 9, 35, 282, DateTimeKind.Local).AddTicks(3179), null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedDate", "Email" },
                values: new object[] { new DateTime(2026, 10, 1, 13, 9, 35, 282, DateTimeKind.Local).AddTicks(3181), null });

            migrationBuilder.CreateIndex(
                name: "IX_SparePartSales_SparePartId",
                table: "SparePartSales",
                column: "SparePartId");
        }
    }
}
