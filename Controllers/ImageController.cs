using EHisseler.Web.Models;
using EHisseler.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EHisseler.Web.Controllers
{
    public class ImageController : Controller
    {
        private readonly IBlobStorageService _blobService;
        private readonly AppDbContext _context;

        // Yüklənə bilən fayl növləri
        private static readonly HashSet<string> AllowedExtensions =
            new(System.StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp" };

        private const int MaxFileSizeMb = 5;

        public ImageController(IBlobStorageService blobService, AppDbContext context)
        {
            _blobService = blobService;
            _context = context;
        }

        // ─────────────────────────────────────────────────────────────
        //  Legacy şəkilləri wwwroot-dan qaytarır (köhnə seed məlumatları)
        // ─────────────────────────────────────────────────────────────
        [HttpGet("Image/GetImage")]
        public IActionResult GetImage(string path)
        {
            if (string.IsNullOrEmpty(path))
                return NotFound();

            if (_blobService.IsLegacyPath(path))
            {
                var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path.TrimStart('/'));
                if (System.IO.File.Exists(wwwrootPath))
                    return PhysicalFile(wwwrootPath, GetMimeType(wwwrootPath));
            }

            // Blob path-i üçün redirect
            var url = _blobService.GetUrl(path);
            if (string.IsNullOrEmpty(url))
            {
                var defaultPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "default.png");
                if (System.IO.File.Exists(defaultPath))
                    return PhysicalFile(defaultPath, "image/png");
                return NotFound();
            }

            return Redirect(url);
        }

        // ─────────────────────────────────────────────────────────────
        //  Şəkil yükləmə — Blob Storage-a yükləyir, DB-a path yazır
        // ─────────────────────────────────────────────────────────────
        [HttpPost]
        [Authorize(Roles = "Usta,Anbardar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(int sparePartId, IFormFile file, bool isMain = false)
        {
            // Məhsulun bu istifadəçiyə aid olduğunu yoxla
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var sellerProfile = await _context.SellerProfiles.FirstOrDefaultAsync(s => s.UserId == userId);
            if (sellerProfile == null) return Unauthorized();

            var part = await _context.SpareParts
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == sparePartId && p.SellerId == sellerProfile.Id && !p.IsDeleted);

            if (part == null) return NotFound();

            // Fayl validasiyası
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Fayl seçilməyib.";
                return RedirectToAction("EditPart", "Warehouse", new { id = sparePartId });
            }

            var extension = Path.GetExtension(file.FileName);
            if (!AllowedExtensions.Contains(extension))
            {
                TempData["Error"] = "Yalnız JPG, PNG və WEBP formatları qəbul edilir.";
                return RedirectToAction("EditPart", "Warehouse", new { id = sparePartId });
            }

            if (file.Length > MaxFileSizeMb * 1024 * 1024)
            {
                TempData["Error"] = $"Fayl ölçüsü {MaxFileSizeMb} MB-dan böyük ola bilməz.";
                return RedirectToAction("EditPart", "Warehouse", new { id = sparePartId });
            }

            // Blob Storage-a yüklə
            var folder = $"parts/{sparePartId}";
            var blobPath = await _blobService.UploadAsync(file, folder);

            // Əgər ilk şəkil və ya isMain=true, köhnəsini main-dən çıxar
            if (isMain || !part.Images.Any())
            {
                foreach (var img in part.Images)
                    img.IsMain = false;
            }

            var image = new SparePartImage
            {
                SparePartId = sparePartId,
                ImageUrl = blobPath,           // ← DB-da yalnız blob path saxlanır
                IsMain = isMain || !part.Images.Any(),
                DisplayOrder = part.Images.Count + 1
            };

            _context.SparePartImages.Add(image);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Şəkil uğurla yükləndi.";
            return RedirectToAction("EditPart", "Warehouse", new { id = sparePartId });
        }

        // ─────────────────────────────────────────────────────────────
        //  Şəkil silmə — Blob-dan ve DB-dan silir
        // ─────────────────────────────────────────────────────────────
        [HttpPost]
        [Authorize(Roles = "Usta,Anbardar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int imageId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var sellerProfile = await _context.SellerProfiles.FirstOrDefaultAsync(s => s.UserId == userId);
            if (sellerProfile == null) return Unauthorized();

            var image = await _context.SparePartImages
                .Include(i => i.SparePart)
                .FirstOrDefaultAsync(i => i.Id == imageId && i.SparePart.SellerId == sellerProfile.Id);

            if (image == null) return NotFound();

            var sparePartId = image.SparePartId;
            var wasMain = image.IsMain;

            await _blobService.DeleteAsync(image.ImageUrl);
            _context.SparePartImages.Remove(image);
            await _context.SaveChangesAsync();

            // Silindikdən sonra başqa şəkili main et
            if (wasMain)
            {
                var next = await _context.SparePartImages
                    .Where(i => i.SparePartId == sparePartId)
                    .OrderBy(i => i.DisplayOrder)
                    .FirstOrDefaultAsync();
                if (next != null)
                {
                    next.IsMain = true;
                    await _context.SaveChangesAsync();
                }
            }

            TempData["Success"] = "Şəkil silindi.";
            return RedirectToAction("EditPart", "Warehouse", new { id = sparePartId });
        }

        // ─────────────────────────────────────────────────────────────
        //  Köməkçi metodlar
        // ─────────────────────────────────────────────────────────────
        private static string GetMimeType(string file)
        {
            var ext = Path.GetExtension(file).ToLowerInvariant();
            return ext switch
            {
                ".png"            => "image/png",
                ".gif"            => "image/gif",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".webp"           => "image/webp",
                _                 => "application/octet-stream",
            };
        }
    }
}
