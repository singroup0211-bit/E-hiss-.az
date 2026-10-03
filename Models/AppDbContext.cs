using Microsoft.EntityFrameworkCore;
using System;

namespace EHisseler.Web.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<SellerProfile> SellerProfiles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<DeviceBrand> DeviceBrands { get; set; }
        public DbSet<CategoryBrand> CategoryBrands { get; set; }
        public DbSet<DeviceModel> DeviceModels { get; set; }
        public DbSet<SparePartType> SparePartTypes { get; set; }
        public DbSet<CategoryPartType> CategoryPartTypes { get; set; }
        public DbSet<QualityType> QualityTypes { get; set; }
        public DbSet<SparePart> SpareParts { get; set; }
        public DbSet<SparePartImage> SparePartImages { get; set; }
        public DbSet<SparePartSaleAudit> SparePartSaleAudits { get; set; }
        public DbSet<SparePartReview> SparePartReviews { get; set; }
        public DbSet<ApprovalHistory> ApprovalHistories { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryBrand>()
                .HasKey(cb => new { cb.CategoryId, cb.BrandId });

            modelBuilder.Entity<CategoryPartType>()
                .HasKey(cp => new { cp.CategoryId, cp.PartTypeId });

            Seed(modelBuilder);
        }

        public static void Seed(ModelBuilder modelBuilder)
        {
            // ============================================
            // LOOKUPS - Admin paneldən idarə olunan
            // ============================================

            // 1. Categories (Cihaz kateqoriyaları)
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Telefon", DisplayOrder = 1, IsActive = true },
                new Category { Id = 2, Name = "Planşet", DisplayOrder = 2, IsActive = true },
                new Category { Id = 3, Name = "Noutbuk", DisplayOrder = 3, IsActive = true }
            );

            // 2. Brands (Brendlər)
            modelBuilder.Entity<DeviceBrand>().HasData(
                new DeviceBrand { Id = 1, Name = "Apple", IsActive = true },
                new DeviceBrand { Id = 2, Name = "Samsung", IsActive = true },
                new DeviceBrand { Id = 3, Name = "Xiaomi", IsActive = true },
                new DeviceBrand { Id = 4, Name = "Huawei", IsActive = true },
                new DeviceBrand { Id = 5, Name = "Dell", IsActive = true },
                new DeviceBrand { Id = 6, Name = "Lenovo", IsActive = true },
                new DeviceBrand { Id = 7, Name = "Asus", IsActive = true }
            );

            // 3. Category-Brand Mapping (Hər kateqoriyada hansı brendlər var)
            modelBuilder.Entity<CategoryBrand>().HasData(
                // Telefon brendləri
                new { CategoryId = 1, BrandId = 1 }, // Apple
                new { CategoryId = 1, BrandId = 2 }, // Samsung
                new { CategoryId = 1, BrandId = 3 }, // Xiaomi
                new { CategoryId = 1, BrandId = 4 }, // Huawei

                // Planşet brendləri
                new { CategoryId = 2, BrandId = 1 }, // Apple iPad
                new { CategoryId = 2, BrandId = 2 }, // Samsung Tab
                new { CategoryId = 2, BrandId = 4 }, // Huawei

                // Noutbuk brendləri
                new { CategoryId = 3, BrandId = 1 }, // MacBook
                new { CategoryId = 3, BrandId = 5 }, // Dell
                new { CategoryId = 3, BrandId = 6 }, // Lenovo
                new { CategoryId = 3, BrandId = 7 }  // Asus
            );

            // 4. Device Models (Cihaz modelləri)
            modelBuilder.Entity<DeviceModel>().HasData(
                // Apple Telefonlar
                new DeviceModel { Id = 1, CategoryId = 1, BrandId = 1, Name = "iPhone 15 Pro Max", ModelCode = "A2849", ReleaseYear = 2023, IsActive = true },
                new DeviceModel { Id = 2, CategoryId = 1, BrandId = 1, Name = "iPhone 15 Pro", ModelCode = "A2848", ReleaseYear = 2023, IsActive = true },
                new DeviceModel { Id = 3, CategoryId = 1, BrandId = 1, Name = "iPhone 14 Pro Max", ModelCode = "A2651", ReleaseYear = 2022, IsActive = true },
                new DeviceModel { Id = 4, CategoryId = 1, BrandId = 1, Name = "iPhone 13", ModelCode = "A2482", ReleaseYear = 2021, IsActive = true },

                // Samsung Telefonlar
                new DeviceModel { Id = 5, CategoryId = 1, BrandId = 2, Name = "Galaxy S24 Ultra", ModelCode = "SM-S928", ReleaseYear = 2024, IsActive = true },
                new DeviceModel { Id = 6, CategoryId = 1, BrandId = 2, Name = "Galaxy S23", ModelCode = "SM-S911", ReleaseYear = 2023, IsActive = true },
                new DeviceModel { Id = 7, CategoryId = 1, BrandId = 2, Name = "Galaxy A54", ModelCode = "SM-A546", ReleaseYear = 2023, IsActive = true },

                // Xiaomi Telefonlar
                new DeviceModel { Id = 8, CategoryId = 1, BrandId = 3, Name = "Xiaomi 13 Pro", ModelCode = "2210132C", ReleaseYear = 2023, IsActive = true },
                new DeviceModel { Id = 9, CategoryId = 1, BrandId = 3, Name = "Redmi Note 12 Pro", ModelCode = "2211133C", ReleaseYear = 2023, IsActive = true },

                // Huawei Telefonlar
                new DeviceModel { Id = 10, CategoryId = 1, BrandId = 4, Name = "P60 Pro", ModelCode = "ALN-AL00", ReleaseYear = 2023, IsActive = true },

                // Apple Planşetlər
                new DeviceModel { Id = 11, CategoryId = 2, BrandId = 1, Name = "iPad Pro 12.9 (2024)", ModelCode = "A2764", ReleaseYear = 2024, IsActive = true },
                new DeviceModel { Id = 12, CategoryId = 2, BrandId = 1, Name = "iPad Air (2024)", ModelCode = "A2589", ReleaseYear = 2024, IsActive = true },

                // Samsung Planşetlər
                new DeviceModel { Id = 13, CategoryId = 2, BrandId = 2, Name = "Galaxy Tab S9 Ultra", ModelCode = "SM-X916", ReleaseYear = 2023, IsActive = true },

                // Apple Noutbuklar
                new DeviceModel { Id = 14, CategoryId = 3, BrandId = 1, Name = "MacBook Pro 14 (M3)", ModelCode = "MRX33", ReleaseYear = 2023, IsActive = true },
                new DeviceModel { Id = 15, CategoryId = 3, BrandId = 1, Name = "MacBook Air 13 (M2)", ModelCode = "MLXY3", ReleaseYear = 2022, IsActive = true },

                // Dell Noutbuklar
                new DeviceModel { Id = 16, CategoryId = 3, BrandId = 5, Name = "XPS 13 Plus", ModelCode = "9320", ReleaseYear = 2024, IsActive = true },
                new DeviceModel { Id = 17, CategoryId = 3, BrandId = 5, Name = "XPS 15", ModelCode = "9530", ReleaseYear = 2023, IsActive = true }
            );

            // 5. Part Types
            modelBuilder.Entity<SparePartType>().HasData(
                new SparePartType { Id = 1, Name = "Telefon üçün ekranlar", Key = "SCREEN", IsPortable = false, IsAccessory = false },
                new SparePartType { Id = 2, Name = "Telefon üçün batareyalar", Key = "BATTERY", IsPortable = false, IsAccessory = false },
                new SparePartType { Id = 3, Name = "Kameralar", Key = "CAM_REAR", IsPortable = false, IsAccessory = false },
                new SparePartType { Id = 4, Name = "Kamera şüşələri", Key = "CAM_FRONT", IsPortable = false, IsAccessory = false },
                new SparePartType { Id = 5, Name = "Motherboard", Key = "MB", IsPortable = false, IsAccessory = false },
                new SparePartType { Id = 6, Name = "Arxa qapaqlar", Key = "BACK_COVER", IsPortable = false, IsAccessory = false },
                new SparePartType { Id = 7, Name = "Dinamiklər, zənglər və vibromotorlar", Key = "SPEAKER", IsPortable = false, IsAccessory = false },
                new SparePartType { Id = 8, Name = "Charging Port", Key = "CHARGING_PORT", IsPortable = false, IsAccessory = false },
                new SparePartType { Id = 9, Name = "Sim tutucular", Key = "SIM_TRAY", IsPortable = false, IsAccessory = false },
                new SparePartType { Id = 10, Name = "Home düymələri və skanerlər", Key = "BUTTONS", IsPortable = false, IsAccessory = false },
                new SparePartType { Id = 11, Name = "Keyboard", Key = "KEYBOARD", IsPortable = false, IsAccessory = false },
                new SparePartType { Id = 12, Name = "Touchpad", Key = "TOUCHPAD", IsPortable = false, IsAccessory = false },
                new SparePartType { Id = 13, Name = "Ram", Key = "RAM", IsPortable = false, IsAccessory = false },
                new SparePartType { Id = 14, Name = "Storage", Key = "STORAGE", IsPortable = false, IsAccessory = false },
                new SparePartType { Id = 15, Name = "Adapter", Key = "ADAPTER", IsPortable = false, IsAccessory = true },
                new SparePartType { Id = 16, Name = "Telefon üçün sensorlar və ekran şüşələri", Key = "SENSOR_GLASS", IsPortable = false, IsAccessory = false },
                new SparePartType { Id = 17, Name = "Antenlər", Key = "ANTENNA", IsPortable = false, IsAccessory = false },
                new SparePartType { Id = 18, Name = "Dinamik setkaları", Key = "SPEAKER_NET", IsPortable = false, IsAccessory = false },
                new SparePartType { Id = 19, Name = "Korpus hissələri", Key = "BODY_PARTS", IsPortable = false, IsAccessory = false },
                new SparePartType { Id = 20, Name = "Korpuslar", Key = "BODY", IsPortable = false, IsAccessory = false },
                new SparePartType { Id = 21, Name = "Mikrofonlar", Key = "MIC", IsPortable = false, IsAccessory = false },
                new SparePartType { Id = 22, Name = "OCA filmlər, arxa işıqlar və polarizatorlar", Key = "OCA_POLARIZER", IsPortable = false, IsAccessory = false }
            );

            // 6. Category-PartType Mapping
            modelBuilder.Entity<CategoryPartType>().HasData(
                new CategoryPartType { CategoryId = 1, PartTypeId = 1 },
                new CategoryPartType { CategoryId = 1, PartTypeId = 2 },
                new CategoryPartType { CategoryId = 1, PartTypeId = 3 },
                new CategoryPartType { CategoryId = 1, PartTypeId = 4 },
                new CategoryPartType { CategoryId = 1, PartTypeId = 5 },
                new CategoryPartType { CategoryId = 1, PartTypeId = 6 },
                new CategoryPartType { CategoryId = 1, PartTypeId = 7 },
                new CategoryPartType { CategoryId = 1, PartTypeId = 8 },
                new CategoryPartType { CategoryId = 1, PartTypeId = 9 },
                new CategoryPartType { CategoryId = 1, PartTypeId = 10 },
                new CategoryPartType { CategoryId = 1, PartTypeId = 16 },
                new CategoryPartType { CategoryId = 1, PartTypeId = 17 },
                new CategoryPartType { CategoryId = 1, PartTypeId = 18 },
                new CategoryPartType { CategoryId = 1, PartTypeId = 19 },
                new CategoryPartType { CategoryId = 1, PartTypeId = 20 },
                new CategoryPartType { CategoryId = 1, PartTypeId = 21 },
                new CategoryPartType { CategoryId = 1, PartTypeId = 22 },
                new CategoryPartType { CategoryId = 2, PartTypeId = 1 },
                new CategoryPartType { CategoryId = 2, PartTypeId = 2 },
                new CategoryPartType { CategoryId = 2, PartTypeId = 3 },
                new CategoryPartType { CategoryId = 2, PartTypeId = 4 },
                new CategoryPartType { CategoryId = 2, PartTypeId = 5 },
                new CategoryPartType { CategoryId = 2, PartTypeId = 6 },
                new CategoryPartType { CategoryId = 2, PartTypeId = 8 },
                new CategoryPartType { CategoryId = 2, PartTypeId = 10 },
                new CategoryPartType { CategoryId = 3, PartTypeId = 1 },
                new CategoryPartType { CategoryId = 3, PartTypeId = 2 },
                new CategoryPartType { CategoryId = 3, PartTypeId = 5 },
                new CategoryPartType { CategoryId = 3, PartTypeId = 11 },
                new CategoryPartType { CategoryId = 3, PartTypeId = 12 },
                new CategoryPartType { CategoryId = 3, PartTypeId = 13 },
                new CategoryPartType { CategoryId = 3, PartTypeId = 14 },
                new CategoryPartType { CategoryId = 3, PartTypeId = 15 }
            );

            // 7. Quality Types
            modelBuilder.Entity<QualityType>().HasData(
                new QualityType { Id = 1, Name = "Zavod istehsalı (yeni)", Code = "ORIGINAL_NEW", Description = "Rəsmi zavod istehsalı, heç işlənməyib", DisplayOrder = 1, IsActive = true },
                new QualityType { Id = 2, Name = "Zavod istehsalı (işlənmiş)", Code = "ORIGINAL_USED", Description = "Rəsmi zavod, başqa telefondan çıxarılıb", DisplayOrder = 2, IsActive = true },
                new QualityType { Id = 3, Name = "A-Class (Premium)", Code = "A_CLASS", Description = "Yüksək keyfiyyətli alternativ, zavoda yaxın", DisplayOrder = 3, IsActive = true },
                new QualityType { Id = 4, Name = "B-Class (Standart)", Code = "B_CLASS", Description = "Orta keyfiyyət, əlverişli qiymət", DisplayOrder = 4, IsActive = true },
                new QualityType { Id = 5, Name = "C-Class (Ekonom)", Code = "C_CLASS", Description = "Æsas funksiyalar, ən aşağı qiymət", DisplayOrder = 5, IsActive = true }
            );

            // 8. Users & Seller Profiles
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "elcin", Email = "elcin@example.com", PasswordHash = "123", Role = UserRole.Usta, FirstName = "Elçin", LastName = "Məmmədov", Phone = "+994501234567", ApprovalStatus = ApprovalStatus.Approved },
                new User { Id = 2, Username = "rashad", Email = "rashad@example.com", PasswordHash = "123", Role = UserRole.Anbardar, FirstName = "Rəşad", LastName = "Æ liyev", Phone = "+994551234568", ApprovalStatus = ApprovalStatus.Approved },
                new User { Id = 3, Username = "nigar", Email = "nigar@example.com", PasswordHash = "123", Role = UserRole.Usta, FirstName = "Nigar", LastName = "Həsənova", Phone = "+994701234569", ApprovalStatus = ApprovalStatus.Approved },
                new User { Id = 4, Username = "admin", Email = "admin@example.com", PasswordHash = "admin", Role = UserRole.Admin, FirstName = "Baş", LastName = "Administrator", Phone = "+994000000000", ApprovalStatus = ApprovalStatus.Approved },
                new User { Id = 5, Username = "samir", Email = "samir@example.com", PasswordHash = "123", Role = UserRole.Usta, FirstName = "Samir", LastName = "Quliyev", Phone = "+994501231122", ApprovalStatus = ApprovalStatus.Approved },
                new User { Id = 6, Username = "vuqar", Email = "vuqar@example.com", PasswordHash = "123", Role = UserRole.Anbardar, FirstName = "Vuqar", LastName = "Hesenov", Phone = "+994551233344", ApprovalStatus = ApprovalStatus.Approved },
                new User { Id = 7, Username = "leyla", Email = "leyla@example.com", PasswordHash = "123", Role = UserRole.Usta, FirstName = "Leyla", LastName = "Memmedova", Phone = "+994701235566", ApprovalStatus = ApprovalStatus.Approved },
                new User { Id = 8, Username = "taleh", Email = "taleh@example.com", PasswordHash = "123", Role = UserRole.Anbardar, FirstName = "Taleh", LastName = "Qarayev", Phone = "+994511237788", ApprovalStatus = ApprovalStatus.Pending },
                new User { Id = 9, Username = "rustam", Email = "rustam@example.com", PasswordHash = "123", Role = UserRole.Usta, FirstName = "Rustam", LastName = "Aliyev", Phone = "+994501239900", ApprovalStatus = ApprovalStatus.Pending }
            );

            modelBuilder.Entity<SellerProfile>().HasData(
                new SellerProfile {
                    Id = 1,
                    UserId = 1,
                    CompanyName = "TechMaster Servis",
                    ContactPerson = "Elçin Məmmədov",
                    Phone = "+994501234567",
                    Address = "Bakı, Nəsimi rayonu, 28 May küç. 15",
                    Rating = 4.8m,
                    IsPhoneVerified = true
                },
                new SellerProfile {
                    Id = 2,
                    UserId = 2,
                    CompanyName = "Mobile Parts Store",
                    ContactPerson = "Rəşad Æliyev",
                    Phone = "+994551234568",
                    Address = "Bakı, Yasamal rayonu, Cəfər Cabbarlı küç. 44",
                    Rating = 4.5m,
                    IsPhoneVerified = true
                },
                new SellerProfile {
                    Id = 3,
                    UserId = 3,
                    CompanyName = "SmartFix Təmir Mərkəzi",
                    ContactPerson = "Nigar Həsənova",
                    Phone = "+994701234569",
                    Address = "Bakı, Nərimanov rayonu, Æhmədli küç. 9",
                    Rating = 4.9m,
                    IsPhoneVerified = true
                }
            );

            // 9. Spare Parts
            modelBuilder.Entity<SparePart>().HasData(
                new SparePart {
                    Id = 1,
                    SKU = "SKU-1",
                    SellerId = 1,
                    DeviceModelId = 1,
                    PartTypeId = 1,
                    QualityTypeId = 1,
                    Title = "iPhone 15 Pro Max Ekran - Orijinal Zavod",
                    Description = "6.7 düym OLED ekran, rəsmi Apple zavod istehsalı. Heç işlənməyib, tam komplektdə.",
                    Price = 850.00m,
                    Currency = "AZN",
                    StockQuantity = 5,
                    MinOrderQuantity = 1,
                    WarrantyMonths = 6,
                    ViewCount = 245,
                    IsActive = true,
                    ModerationStatus = ApprovalStatus.Approved,
                    CreatedDate = new DateTime(2024, 1, 15),
                    LastModifiedDate = new DateTime(2024, 1, 15)
                },
                new SparePart {
                    Id = 2,
                    SKU = "SKU-2",
                    SellerId = 2,
                    DeviceModelId = 5,
                    PartTypeId = 2,
                    QualityTypeId = 3,
                    Title = "Samsung S24 Ultra Batareya (5000mAh) - A-Class",
                    Description = "Yüksək keyfiyyətli A-class batareya, 5000mAh tutum. Sürətli şarj dəstəyi.",
                    Price = 120.00m,
                    Currency = "AZN",
                    StockQuantity = 15,
                    MinOrderQuantity = 1,
                    WarrantyMonths = 3,
                    ViewCount = 189,
                    IsActive = false,
                    ModerationStatus = ApprovalStatus.Approved,
                    CreatedDate = new DateTime(2024, 1, 20),
                    LastModifiedDate = new DateTime(2024, 1, 20)
                },
                new SparePart {
                    Id = 3,
                    SKU = "SKU-3",
                    SellerId = 3,
                    DeviceModelId = 4,
                    PartTypeId = 3,
                    QualityTypeId = 2,
                    Title = "iPhone 13 Arxa Kamera 12MP - Orijinal (işlənmiş)",
                    Description = "Rəsmi Apple kamera modulu, başqa telefondan çıxarılıb. Test edilib, tam işlək vəziyyətdə.",
                    Price = 280.00m,
                    Currency = "AZN",
                    StockQuantity = 3,
                    MinOrderQuantity = 1,
                    WarrantyMonths = 1,
                    ViewCount = 156,
                    IsActive = false,
                    ModerationStatus = ApprovalStatus.Approved,
                    CreatedDate = new DateTime(2024, 1, 10),
                    LastModifiedDate = new DateTime(2024, 1, 10)
                },
                new SparePart {
                    Id = 4,
                    SKU = "SKU-4",
                    SellerId = 1,
                    DeviceModelId = 8,
                    PartTypeId = 5,
                    QualityTypeId = 2,
                    Title = "Xiaomi 13 Pro Ana Plata - 256GB (işlənmiş)",
                    Description = "256GB yaddaş ilə ana plata. Tam işlək, IMEI təmiz. Test edilib.",
                    Price = 650.00m,
                    Currency = "AZN",
                    StockQuantity = 2,
                    MinOrderQuantity = 1,
                    WarrantyMonths = 2,
                    ViewCount = 92,
                    IsActive = false,
                    ModerationStatus = ApprovalStatus.Approved,
                    CreatedDate = new DateTime(2024, 1, 12),
                    LastModifiedDate = new DateTime(2024, 1, 12)
                },
                new SparePart {
                    Id = 5,
                    SKU = "SKU-5",
                    SellerId = 2,
                    DeviceModelId = 6,
                    PartTypeId = 1,
                    QualityTypeId = 4,
                    Title = "Samsung Galaxy S23 Ekran - B-Class (əlverişli)",
                    Description = "6.1 düym AMOLED ekran, B-class keyfiyyət. Æsas funksiyalar tam işləyir, qiymət əlverişlidir.",
                    Price = 320.00m,
                    Currency = "AZN",
                    StockQuantity = 10,
                    MinOrderQuantity = 1,
                    WarrantyMonths = 2,
                    ViewCount = 203,
                    IsActive = true,
                    ModerationStatus = ApprovalStatus.Approved,
                    CreatedDate = new DateTime(2024, 1, 18),
                    LastModifiedDate = new DateTime(2024, 1, 18)
                },
                new SparePart {
                    Id = 6,
                    SKU = "SKU-6",
                    SellerId = 3,
                    DeviceModelId = 14,
                    PartTypeId = 2,
                    QualityTypeId = 1,
                    Title = "MacBook Pro 14 (M3) Batareya - Orijinal Apple",
                    Description = "70Wh litium-polimer batareya, orijinal Apple istehsalı. Tam yeni, qutuda.",
                    Price = 450.00m,
                    Currency = "AZN",
                    StockQuantity = 4,
                    MinOrderQuantity = 1,
                    WarrantyMonths = 12,
                    ViewCount = 78,
                    IsActive = false,
                    ModerationStatus = ApprovalStatus.Approved,
                    CreatedDate = new DateTime(2024, 1, 8),
                    LastModifiedDate = new DateTime(2024, 1, 8)
                },
                new SparePart {
                    Id = 7,
                    SKU = "SKU-7",
                    SellerId = 1,
                    DeviceModelId = 16,
                    PartTypeId = 11,
                    QualityTypeId = 3,
                    Title = "Dell XPS 13 Plus Klaviatura - US Layout (A-Class)",
                    Description = "Arxa işıqlandırmalı klaviatura, US layout. Yüksək keyfiyyətli alternativ.",
                    Price = 180.00m,
                    Currency = "AZN",
                    StockQuantity = 6,
                    MinOrderQuantity = 1,
                    WarrantyMonths = 6,
                    ViewCount = 45,
                    IsActive = false,
                    ModerationStatus = ApprovalStatus.Approved,
                    CreatedDate = new DateTime(2024, 1, 14),
                    LastModifiedDate = new DateTime(2024, 1, 14)
                },
                new SparePart {
                    Id = 8,
                    SKU = "SKU-8",
                    SellerId = 2,
                    DeviceModelId = 11,
                    PartTypeId = 1,
                    QualityTypeId = 1,
                    Title = "iPad Pro 12.9 (2024) Ekran - Orijinal",
                    Description = "12.9 düym Liquid Retina XDR ekran. Rəsmi Apple istehsalı.",
                    Price = 1200.00m,
                    Currency = "AZN",
                    StockQuantity = 2,
                    MinOrderQuantity = 1,
                    WarrantyMonths = 12,
                    ViewCount = 134,
                    IsActive = true,
                    ModerationStatus = ApprovalStatus.Approved,
                    CreatedDate = new DateTime(2024, 1, 5),
                    LastModifiedDate = new DateTime(2024, 1, 22)
                }
            );

            // ============================================
            // SPARE PART IMAGES
            // ============================================
            modelBuilder.Entity<SparePartImage>().HasData(
                // iPhone 15 Pro Max ekran şəkilləri
                new SparePartImage { Id = 1, SparePartId = 1, ImageUrl = "/images/images (1).jpg", IsMain = true, DisplayOrder = 1 },
                new SparePartImage { Id = 2, SparePartId = 1, ImageUrl = "/images/images (2).jpg", IsMain = false, DisplayOrder = 2 },

                // Samsung S24 Ultra batareya
                new SparePartImage { Id = 4, SparePartId = 2, ImageUrl = "/images/images (3).jpg", IsMain = true, DisplayOrder = 1 },

                // iPhone 13 kamera
                new SparePartImage { Id = 6, SparePartId = 3, ImageUrl = "/images/images (4).jpg", IsMain = true, DisplayOrder = 1 },

                // Xiaomi ana plata
                new SparePartImage { Id = 7, SparePartId = 4, ImageUrl = "/images/images (5).jpg", IsMain = true, DisplayOrder = 1 },

                // S23 ekran
                new SparePartImage { Id = 9, SparePartId = 5, ImageUrl = "/images/images (6).jpg", IsMain = true, DisplayOrder = 1 },

                // MacBook batareya
                new SparePartImage { Id = 10, SparePartId = 6, ImageUrl = "/images/images (7).jpg", IsMain = true, DisplayOrder = 1 },

                // Dell klaviatura
                new SparePartImage { Id = 11, SparePartId = 7, ImageUrl = "/images/images.jpg", IsMain = true, DisplayOrder = 1 },

                // iPad ekran
                new SparePartImage { Id = 12, SparePartId = 8, ImageUrl = "/images/images (1).jpg", IsMain = true, DisplayOrder = 1 }
            );
        }
    }
}



