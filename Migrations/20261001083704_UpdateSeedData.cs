using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EHisseler.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Action = table.Column<string>(type: "TEXT", nullable: false),
                    EntityName = table.Column<string>(type: "TEXT", nullable: true),
                    EntityId = table.Column<int>(type: "INTEGER", nullable: true),
                    Details = table.Column<string>(type: "TEXT", nullable: true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TargetUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    AdminId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Reason = table.Column<string>(type: "TEXT", nullable: true),
                    ActionDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryBrands",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    BrandId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryBrands", x => new { x.CategoryId, x.BrandId });
                });

            migrationBuilder.CreateTable(
                name: "CategoryPartTypes",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    PartTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryPartTypes", x => new { x.CategoryId, x.PartTypeId });
                });

            migrationBuilder.CreateTable(
                name: "DeviceBrands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QualityTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SparePartTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    IsPortable = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsAccessory = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SparePartTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    ApprovalStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    IsBlocked = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    BrandId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ModelCode = table.Column<string>(type: "TEXT", nullable: false),
                    ReleaseYear = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceModels_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeviceModels_DeviceBrands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "DeviceBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SellerProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CompanyName = table.Column<string>(type: "TEXT", nullable: false),
                    ContactPerson = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    WhatsAppNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    Rating = table.Column<decimal>(type: "TEXT", nullable: false),
                    ShowContactInfoInAds = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsPhoneVerified = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellerProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpareParts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SellerId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeviceModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    PartTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    QualityTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    SKU = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    DiscountPrice = table.Column<decimal>(type: "TEXT", nullable: true),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    StockQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    MinOrderQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    WarrantyMonths = table.Column<int>(type: "INTEGER", nullable: false),
                    ViewCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpareParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpareParts_DeviceModels_DeviceModelId",
                        column: x => x.DeviceModelId,
                        principalTable: "DeviceModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpareParts_QualityTypes_QualityTypeId",
                        column: x => x.QualityTypeId,
                        principalTable: "QualityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpareParts_SellerProfiles_SellerId",
                        column: x => x.SellerId,
                        principalTable: "SellerProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpareParts_SparePartTypes_PartTypeId",
                        column: x => x.PartTypeId,
                        principalTable: "SparePartTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SparePartImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SparePartId = table.Column<int>(type: "INTEGER", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    IsMain = table.Column<bool>(type: "INTEGER", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SparePartImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SparePartImages_SpareParts_SparePartId",
                        column: x => x.SparePartId,
                        principalTable: "SpareParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SparePartReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SparePartId = table.Column<int>(type: "INTEGER", nullable: false),
                    AdminId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Reason = table.Column<string>(type: "TEXT", nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SparePartReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SparePartReviews_SpareParts_SparePartId",
                        column: x => x.SparePartId,
                        principalTable: "SpareParts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SparePartSales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SparePartId = table.Column<int>(type: "INTEGER", nullable: false),
                    SoldByUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    PreviousStock = table.Column<int>(type: "INTEGER", nullable: false),
                    NewStock = table.Column<int>(type: "INTEGER", nullable: false),
                    SaleDate = table.Column<DateTime>(type: "TEXT", nullable: false)
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

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "DisplayOrder", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, 1, true, "Telefon" },
                    { 2, 2, true, "Planşet" },
                    { 3, 3, true, "Noutbuk" }
                });

            migrationBuilder.InsertData(
                table: "CategoryBrands",
                columns: new[] { "BrandId", "CategoryId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 4, 1 },
                    { 1, 2 },
                    { 2, 2 },
                    { 4, 2 },
                    { 1, 3 },
                    { 5, 3 },
                    { 6, 3 },
                    { 7, 3 }
                });

            migrationBuilder.InsertData(
                table: "CategoryPartTypes",
                columns: new[] { "CategoryId", "PartTypeId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 1, 4 },
                    { 1, 5 },
                    { 1, 6 },
                    { 1, 7 },
                    { 1, 8 },
                    { 1, 9 },
                    { 1, 10 },
                    { 1, 16 },
                    { 1, 17 },
                    { 1, 18 },
                    { 1, 19 },
                    { 1, 20 },
                    { 1, 21 },
                    { 1, 22 },
                    { 2, 1 },
                    { 2, 2 },
                    { 2, 3 },
                    { 2, 4 },
                    { 2, 5 },
                    { 2, 6 },
                    { 2, 8 },
                    { 2, 10 },
                    { 3, 1 },
                    { 3, 2 },
                    { 3, 5 },
                    { 3, 11 },
                    { 3, 12 },
                    { 3, 13 },
                    { 3, 14 },
                    { 3, 15 }
                });

            migrationBuilder.InsertData(
                table: "DeviceBrands",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, true, "Apple" },
                    { 2, true, "Samsung" },
                    { 3, true, "Xiaomi" },
                    { 4, true, "Huawei" },
                    { 5, true, "Dell" },
                    { 6, true, "Lenovo" },
                    { 7, true, "Asus" }
                });

            migrationBuilder.InsertData(
                table: "QualityTypes",
                columns: new[] { "Id", "Code", "Description", "DisplayOrder", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "ORIGINAL_NEW", "Rəsmi zavod istehsalı, heç işlənməyib", 1, true, "Zavod istehsalı (yeni)" },
                    { 2, "ORIGINAL_USED", "Rəsmi zavod, başqa telefondan çıxarılıb", 2, true, "Zavod istehsalı (işlənmiş)" },
                    { 3, "A_CLASS", "Yüksək keyfiyyətli alternativ, zavoda yaxın", 3, true, "A-Class (Premium)" },
                    { 4, "B_CLASS", "Orta keyfiyyət, əlverişli qiymət", 4, true, "B-Class (Standart)" },
                    { 5, "C_CLASS", "Əsas funksiyalar, ən aşağı qiymət", 5, true, "C-Class (Ekonom)" }
                });

            migrationBuilder.InsertData(
                table: "SparePartTypes",
                columns: new[] { "Id", "IsAccessory", "IsPortable", "Key", "Name" },
                values: new object[,]
                {
                    { 1, false, false, "SCREEN", "Telefon üçün ekranlar" },
                    { 2, false, false, "BATTERY", "Telefon üçün batareyalar" },
                    { 3, false, false, "CAM_REAR", "Kameralar" },
                    { 4, false, false, "CAM_FRONT", "Kamera şüşələri" },
                    { 5, false, false, "MB", "Motherboard" },
                    { 6, false, false, "BACK_COVER", "Arxa qapaqlar" },
                    { 7, false, false, "SPEAKER", "Dinamiklər, zənglər və vibromotorlar" },
                    { 8, false, false, "CHARGING_PORT", "Charging Port" },
                    { 9, false, false, "SIM_TRAY", "Sim tutucular" },
                    { 10, false, false, "BUTTONS", "Home düymələri və skanerlər" },
                    { 11, false, false, "KEYBOARD", "Keyboard" },
                    { 12, false, false, "TOUCHPAD", "Touchpad" },
                    { 13, false, false, "RAM", "Ram" },
                    { 14, false, false, "STORAGE", "Storage" },
                    { 15, true, false, "ADAPTER", "Adapter" },
                    { 16, false, false, "SENSOR_GLASS", "Telefon üçün sensorlar və ekran şüşələri" },
                    { 17, false, false, "ANTENNA", "Antenlər" },
                    { 18, false, false, "SPEAKER_NET", "Dinamik setkaları" },
                    { 19, false, false, "BODY_PARTS", "Korpus hissələri" },
                    { 20, false, false, "BODY", "Korpuslar" },
                    { 21, false, false, "MIC", "Mikrofonlar" },
                    { 22, false, false, "OCA_POLARIZER", "OCA filmlər, arxa işıqlar və polarizatorlar" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ApprovalStatus", "CreatedDate", "Email", "FirstName", "IsBlocked", "LastName", "PasswordHash", "Phone", "Role", "Username" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2026, 10, 1, 12, 37, 3, 942, DateTimeKind.Local).AddTicks(1999), null, "Elçin", false, "Məmmədov", "123", "+994501234567", 2, "elcin" },
                    { 2, 2, new DateTime(2026, 10, 1, 12, 37, 3, 942, DateTimeKind.Local).AddTicks(2019), null, "Rəşad", false, "Əliyev", "123", "+994551234568", 3, "rashad" },
                    { 3, 2, new DateTime(2026, 10, 1, 12, 37, 3, 942, DateTimeKind.Local).AddTicks(2022), null, "Nigar", false, "Həsənova", "123", "+994701234569", 2, "nigar" },
                    { 4, 2, new DateTime(2026, 10, 1, 12, 37, 3, 942, DateTimeKind.Local).AddTicks(2024), null, "Baş", false, "Administrator", "admin", "+994000000000", 1, "admin" },
                    { 5, 2, new DateTime(2026, 10, 1, 12, 37, 3, 942, DateTimeKind.Local).AddTicks(2026), null, "Samir", false, "Quliyev", "123", "+994501231122", 2, "samir" },
                    { 6, 2, new DateTime(2026, 10, 1, 12, 37, 3, 942, DateTimeKind.Local).AddTicks(2027), null, "Vuqar", false, "Hesenov", "123", "+994551233344", 3, "vuqar" },
                    { 7, 2, new DateTime(2026, 10, 1, 12, 37, 3, 942, DateTimeKind.Local).AddTicks(2029), null, "Leyla", false, "Memmedova", "123", "+994701235566", 2, "leyla" },
                    { 8, 1, new DateTime(2026, 10, 1, 12, 37, 3, 942, DateTimeKind.Local).AddTicks(2032), null, "Taleh", false, "Qarayev", "123", "+994511237788", 3, "taleh" },
                    { 9, 1, new DateTime(2026, 10, 1, 12, 37, 3, 942, DateTimeKind.Local).AddTicks(2034), null, "Rustam", false, "Aliyev", "123", "+994501239900", 2, "rustam" }
                });

            migrationBuilder.InsertData(
                table: "DeviceModels",
                columns: new[] { "Id", "BrandId", "CategoryId", "IsActive", "ModelCode", "Name", "ReleaseYear" },
                values: new object[,]
                {
                    { 1, 1, 1, true, "A2849", "iPhone 15 Pro Max", 2023 },
                    { 2, 1, 1, true, "A2848", "iPhone 15 Pro", 2023 },
                    { 3, 1, 1, true, "A2651", "iPhone 14 Pro Max", 2022 },
                    { 4, 1, 1, true, "A2482", "iPhone 13", 2021 },
                    { 5, 2, 1, true, "SM-S928", "Galaxy S24 Ultra", 2024 },
                    { 6, 2, 1, true, "SM-S911", "Galaxy S23", 2023 },
                    { 7, 2, 1, true, "SM-A546", "Galaxy A54", 2023 },
                    { 8, 3, 1, true, "2210132C", "Xiaomi 13 Pro", 2023 },
                    { 9, 3, 1, true, "2211133C", "Redmi Note 12 Pro", 2023 },
                    { 10, 4, 1, true, "ALN-AL00", "P60 Pro", 2023 },
                    { 11, 1, 2, true, "A2764", "iPad Pro 12.9 (2024)", 2024 },
                    { 12, 1, 2, true, "A2589", "iPad Air (2024)", 2024 },
                    { 13, 2, 2, true, "SM-X916", "Galaxy Tab S9 Ultra", 2023 },
                    { 14, 1, 3, true, "MRX33", "MacBook Pro 14 (M3)", 2023 },
                    { 15, 1, 3, true, "MLXY3", "MacBook Air 13 (M2)", 2022 },
                    { 16, 5, 3, true, "9320", "XPS 13 Plus", 2024 },
                    { 17, 5, 3, true, "9530", "XPS 15", 2023 }
                });

            migrationBuilder.InsertData(
                table: "SellerProfiles",
                columns: new[] { "Id", "Address", "CompanyName", "ContactPerson", "IsPhoneVerified", "Phone", "Rating", "ShowContactInfoInAds", "UserId", "WhatsAppNumber" },
                values: new object[,]
                {
                    { 1, "Bakı, Nəsimi rayonu, 28 May küç. 15", "TechMaster Servis", "Elçin Məmmədov", true, "+994501234567", 4.8m, true, 1, null },
                    { 2, "Bakı, Yasamal rayonu, Cəfər Cabbarlı küç. 44", "Mobile Parts Store", "Rəşad Əliyev", true, "+994551234568", 4.5m, true, 2, null },
                    { 3, "Bakı, Nərimanov rayonu, Əhmədli küç. 9", "SmartFix Təmir Mərkəzi", "Nigar Həsənova", true, "+994701234569", 4.9m, true, 3, null }
                });

            migrationBuilder.InsertData(
                table: "SpareParts",
                columns: new[] { "Id", "CreatedDate", "Currency", "Description", "DeviceModelId", "DiscountPrice", "IsActive", "IsDeleted", "LastModifiedDate", "MinOrderQuantity", "PartTypeId", "Price", "QualityTypeId", "SKU", "SellerId", "Status", "StockQuantity", "Title", "ViewCount", "WarrantyMonths" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "AZN", "6.7 düym OLED ekran, rəsmi Apple zavod istehsalı. Heç işlənməyib, tam komplektdə.", 1, null, true, false, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 850.00m, 1, "SKU-1", 1, 2, 5, "iPhone 15 Pro Max Ekran - Orijinal Zavod", 245, 6 },
                    { 2, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "AZN", "Yüksək keyfiyyətli A-class batareya, 5000mAh tutum. Sürətli şarj dəstəyi.", 5, null, false, false, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 120.00m, 3, "SKU-2", 2, 4, 15, "Samsung S24 Ultra Batareya (5000mAh) - A-Class", 189, 3 },
                    { 3, new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "AZN", "Rəsmi Apple kamera modulu, başqa telefondan çıxarılıb. Test edilib, tam işlək vəziyyətdə.", 4, null, false, false, new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, 280.00m, 2, "SKU-3", 3, 4, 3, "iPhone 13 Arxa Kamera 12MP - Orijinal (işlənmiş)", 156, 1 },
                    { 4, new DateTime(2024, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "AZN", "256GB yaddaş ilə ana plata. Tam işlək, IMEI təmiz. Test edilib.", 8, null, false, false, new DateTime(2024, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 5, 650.00m, 2, "SKU-4", 1, 4, 2, "Xiaomi 13 Pro Ana Plata - 256GB (işlənmiş)", 92, 2 },
                    { 5, new DateTime(2024, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "AZN", "6.1 düym AMOLED ekran, B-class keyfiyyət. Əsas funksiyalar tam işləyir, qiymət əlverişlidir.", 6, null, true, false, new DateTime(2024, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 320.00m, 4, "SKU-5", 2, 2, 10, "Samsung Galaxy S23 Ekran - B-Class (əlverişli)", 203, 2 },
                    { 6, new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "AZN", "70Wh litium-polimer batareya, orijinal Apple istehsalı. Tam yeni, qutuda.", 14, null, false, false, new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, 450.00m, 1, "SKU-6", 3, 4, 4, "MacBook Pro 14 (M3) Batareya - Orijinal Apple", 78, 12 },
                    { 7, new DateTime(2024, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "AZN", "Arxa işıqlandırmalı klaviatura, US layout. Yüksək keyfiyyətli alternativ.", 16, null, false, false, new DateTime(2024, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 11, 180.00m, 3, "SKU-7", 1, 4, 6, "Dell XPS 13 Plus Klaviatura - US Layout (A-Class)", 45, 6 },
                    { 8, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "AZN", "12.9 düym Liquid Retina XDR ekran. Rəsmi Apple istehsalı.", 11, null, true, false, new DateTime(2024, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 1200.00m, 1, "SKU-8", 2, 2, 2, "iPad Pro 12.9 (2024) Ekran - Orijinal", 134, 12 }
                });

            migrationBuilder.InsertData(
                table: "SparePartImages",
                columns: new[] { "Id", "DisplayOrder", "ImageUrl", "IsMain", "SparePartId" },
                values: new object[,]
                {
                    { 1, 1, "https://cdn.ehisse.az/parts/iphone15pm-screen-1.jpg", true, 1 },
                    { 2, 2, "https://cdn.ehisse.az/parts/iphone15pm-screen-2.jpg", false, 1 },
                    { 3, 3, "https://cdn.ehisse.az/parts/iphone15pm-screen-3.jpg", false, 1 },
                    { 4, 1, "https://cdn.ehisse.az/parts/s24u-battery-1.jpg", true, 2 },
                    { 5, 2, "https://cdn.ehisse.az/parts/s24u-battery-2.jpg", false, 2 },
                    { 6, 1, "https://cdn.ehisse.az/parts/iphone13-camera-1.jpg", true, 3 },
                    { 7, 1, "https://cdn.ehisse.az/parts/xiaomi13-mb-1.jpg", true, 4 },
                    { 8, 2, "https://cdn.ehisse.az/parts/xiaomi13-mb-2.jpg", false, 4 },
                    { 9, 1, "https://cdn.ehisse.az/parts/s23-screen-1.jpg", true, 5 },
                    { 10, 1, "https://cdn.ehisse.az/parts/mbp14-battery-1.jpg", true, 6 },
                    { 11, 1, "https://cdn.ehisse.az/parts/xps13-kb-1.jpg", true, 7 },
                    { 12, 1, "https://cdn.ehisse.az/parts/ipadpro-screen-1.jpg", true, 8 },
                    { 13, 2, "https://cdn.ehisse.az/parts/ipadpro-screen-2.jpg", false, 8 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceModels_BrandId",
                table: "DeviceModels",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceModels_CategoryId",
                table: "DeviceModels",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerProfiles_UserId",
                table: "SellerProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SparePartImages_SparePartId",
                table: "SparePartImages",
                column: "SparePartId");

            migrationBuilder.CreateIndex(
                name: "IX_SparePartReviews_SparePartId",
                table: "SparePartReviews",
                column: "SparePartId");

            migrationBuilder.CreateIndex(
                name: "IX_SpareParts_DeviceModelId",
                table: "SpareParts",
                column: "DeviceModelId");

            migrationBuilder.CreateIndex(
                name: "IX_SpareParts_QualityTypeId",
                table: "SpareParts",
                column: "QualityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SpareParts_PartTypeId",
                table: "SpareParts",
                column: "PartTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SpareParts_SellerId",
                table: "SpareParts",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_SparePartSales_SparePartId",
                table: "SparePartSales",
                column: "SparePartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLogs");

            migrationBuilder.DropTable(
                name: "ApprovalHistories");

            migrationBuilder.DropTable(
                name: "CategoryBrands");

            migrationBuilder.DropTable(
                name: "CategoryPartTypes");

            migrationBuilder.DropTable(
                name: "SparePartImages");

            migrationBuilder.DropTable(
                name: "SparePartReviews");

            migrationBuilder.DropTable(
                name: "SparePartSales");

            migrationBuilder.DropTable(
                name: "SpareParts");

            migrationBuilder.DropTable(
                name: "DeviceModels");

            migrationBuilder.DropTable(
                name: "QualityTypes");

            migrationBuilder.DropTable(
                name: "SellerProfiles");

            migrationBuilder.DropTable(
                name: "SparePartTypes");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "DeviceBrands");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
