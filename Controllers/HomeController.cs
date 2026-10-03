using EHisseler.Web.Extensions;
using EHisseler.Web.Models;
using EHisseler.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EHisseler.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBlobStorageService _blobService;

        public HomeController(AppDbContext context, IBlobStorageService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        public async Task<IActionResult> Index(
            string searchString,
            int? categoryId,
            int? brandId,
            int? modelId,
            int? partTypeId,
            int? qualityTypeId,
            decimal? minPrice,
            decimal? maxPrice,
            int? sellerId,
            bool inStockOnly = true,
            string sortOrder = "",
            int pageNumber = 1)
        {
            int pageSize = 20;

            ViewBag.Categories  = await _context.Categories.ToListAsync();
            ViewBag.Brands      = await _context.DeviceBrands.ToListAsync();
            ViewBag.QualityTypes= await _context.QualityTypes.ToListAsync();

            // Sidebar üçün ümumi statistika (filtrsiz)
            var activeAdsQuery = _context.SpareParts
                .Where(p => p.ModerationStatus == ApprovalStatus.Approved && !p.IsDeleted && p.IsActive);
            if (inStockOnly) activeAdsQuery = activeAdsQuery.Where(p => p.StockQuantity > 0);

            ViewBag.CategoryCounts = await activeAdsQuery
                .GroupBy(p => p.DeviceModel.CategoryId)
                .Select(g => new { CategoryId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(k => k.CategoryId, v => v.Count);

            ViewBag.TotalParts   = await activeAdsQuery.CountAsync();
            ViewBag.TotalSellers = await _context.SellerProfiles.CountAsync();

            // View-ə filter dəyərlərini ötür (form reset üçün)
            ViewData["CurrentFilter"]   = searchString;
            ViewData["CurrentCategory"] = categoryId;
            ViewData["CurrentBrand"]    = brandId;
            ViewData["CurrentModel"]    = modelId;
            ViewData["CurrentPartType"] = partTypeId;
            ViewData["CurrentQuality"]  = qualityTypeId;
            ViewData["MinPrice"]        = minPrice;
            ViewData["MaxPrice"]        = maxPrice;
            ViewData["CurrentSeller"]   = sellerId;
            ViewData["InStockOnly"]     = inStockOnly;
            ViewData["CurrentSort"]     = sortOrder;

            // ── QueryBuilder ilə dynamic search ────────────────────────────────
            var filter = new SparePartSearchQuery
            {
                SearchString   = searchString,
                CategoryId     = categoryId,
                BrandId        = brandId,
                ModelId        = modelId,
                PartTypeId     = partTypeId,
                QualityTypeId  = qualityTypeId,
                MinPrice       = minPrice,
                MaxPrice       = maxPrice,
                SellerId       = sellerId,
                InStockOnly    = inStockOnly,
                SortOrder      = string.IsNullOrEmpty(sortOrder) ? "date_desc" : sortOrder,
                Status         = ApprovalStatus.Approved,
                IncludeDeleted = false,
                PageNumber     = pageNumber,
                PageSize       = pageSize
            };

            var partsQuery = _context.SpareParts
                .Include(p => p.DeviceModel).ThenInclude(m => m.Brand)
                .Include(p => p.DeviceModel).ThenInclude(m => m.Category)
                .Include(p => p.PartType)
                .Include(p => p.QualityType)
                .Include(p => p.Seller)
                .Include(p => p.Images)
                .ApplySearch(filter);           // ← QueryBuilder: bütün filtrləri tətbiq edir
            // ────────────────────────────────────────────────────────────────────

            var totalItems = await partsQuery.CountAsync();
            var parts = await partsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // ── Bulk image URL yükləmə ──────────────────────────────────────────
            var allImagePaths = parts
                .SelectMany(p => p.Images.Select(i => i.ImageUrl))
                .Where(url => !string.IsNullOrEmpty(url))
                .Distinct();
            ViewBag.ImageUrls = _blobService.GetBulkUrls(allImagePaths);
            // ───────────────────────────────────────────────────────────────────

            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return View(parts);
        }

        public async Task<IActionResult> Saticilar(string searchString)
        {
            var query = _context.SellerProfiles
                .Include(s => s.User)
                .Where(s => !s.User.IsBlocked);

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => 
                    s.CompanyName.Contains(searchString) || 
                    s.ContactPerson.Contains(searchString));
            }

            var sellers = await query.ToListAsync();

            var sellerIds = sellers.Select(s => s.Id).ToList();

            var adStats = await _context.SpareParts
                .Where(p => sellerIds.Contains(p.SellerId) && !p.IsDeleted)
                .GroupBy(p => p.SellerId)
                .Select(g => new KeyValuePair<int, (int Total, int Active)>(
                    g.Key,
                    new (g.Count(), g.Count(p => p.ModerationStatus == ApprovalStatus.Approved && p.IsActive))
                ))
                .ToDictionaryAsync(k => k.Key, v => v.Value);

            ViewBag.AdStats = adStats;
            ViewData["CurrentSearch"] = searchString;

            return View(sellers);
        }

        public IActionResult Yardim()
        {
            return View();
        }

        public IActionResult Elaqe()
        {
            return View();
        }
        
        public async Task<IActionResult> Categories()
        {
            ViewBag.CategoryCounts = await _context.SpareParts
                .Where(p => p.ModerationStatus == ApprovalStatus.Approved && !p.IsDeleted && p.IsActive && p.StockQuantity > 0)
                .GroupBy(p => p.DeviceModel.CategoryId)
                .Select(g => new { CategoryId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(k => k.CategoryId, v => v.Count);

            var categories = await _context.Categories.OrderBy(c => c.DisplayOrder).ToListAsync();
            return View(categories);
        }

        public async Task<IActionResult> Details(int id, int? wa)
        {
            if (wa == 1)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Auth", new { returnUrl = Url.Action("Details", "Home", new { id = id, wa = 1 }) });
                }
                
                var userIdStr = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (int.TryParse(userIdStr, out int userId))
                {
                    var sellerProfile = await _context.SellerProfiles.FirstOrDefaultAsync(s => s.UserId == userId);
                    if (sellerProfile != null)
                    {
                        var isOwner = await _context.SpareParts.AnyAsync(p => p.Id == id && p.SellerId == sellerProfile.Id);
                        if (isOwner)
                        {
                            return RedirectToAction("Details", "Warehouse", new { id = id });
                        }
                    }
                }
                
                return RedirectToAction("Details", "Home", new { id = id });
            }

            var part = await _context.SpareParts
                .Include(p => p.DeviceModel).ThenInclude(m => m.Brand)
                .Include(p => p.DeviceModel).ThenInclude(m => m.Category)
                .Include(p => p.PartType)
                .Include(p => p.QualityType)
                .Include(p => p.Seller)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (part == null)
                return NotFound();

            part.ViewCount++;
            _context.Update(part);
            await _context.SaveChangesAsync();

            // ── Bulk image URL yükləmə (GetById) ────────────────────────────────
            var imagePaths = part.Images.Select(i => i.ImageUrl).Where(u => !string.IsNullOrEmpty(u));
            ViewBag.ImageUrls = _blobService.GetBulkUrls(imagePaths);
            // ────────────────────────────────────────────────────────────────────

            return View(part);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
