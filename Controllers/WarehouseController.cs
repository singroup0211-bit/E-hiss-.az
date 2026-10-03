using EHisseler.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EHisseler.Web.Controllers
{
    [Authorize(Roles = "Usta,Anbardar")]
    public class WarehouseController : Controller
    {
        private readonly AppDbContext _context;

        public WarehouseController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var sellerProfile = await _context.SellerProfiles.FirstOrDefaultAsync(s => s.UserId == userId);

            if (sellerProfile == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var myParts = await _context.SpareParts
                .Include(p => p.DeviceModel)
                .Include(p => p.PartType)
                .Include(p => p.QualityType)
                .Include(p => p.Reviews)
                .Where(p => p.SellerId == sellerProfile.Id)
                .ToListAsync();

            return View(myParts);
        }

        public async Task<IActionResult> AddPart()
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            ViewBag.QualityTypes = new SelectList(await _context.QualityTypes.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPart(SparePart part, int? CategoryId, int? BrandId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var sellerProfile = await _context.SellerProfiles.FirstOrDefaultAsync(s => s.UserId == userId);

            if (sellerProfile == null) return Unauthorized();

            ModelState.Remove("Seller"); ModelState.Remove("DeviceModel"); ModelState.Remove("PartType"); ModelState.Remove("QualityType"); 
            ModelState.Remove("Currency"); ModelState.Remove("Images"); ModelState.Remove("SKU");
            
            if (part.DeviceModelId <= 0)
            {
                ModelState.AddModelError("DeviceModelId", "Zəhmət olmasa cihaz modelini seçin.");
            }

            if (!string.IsNullOrEmpty(part.SKU))
            {
                var skuExists = await _context.SpareParts.AnyAsync(p => p.SKU == part.SKU && p.SellerId == sellerProfile.Id && !p.IsDeleted);
                if (skuExists)
                {
                    ModelState.AddModelError("SKU", "Bu SKU artıq istifadə olunub.");
                }
            }

            if (part.Price <= 0)
            {
                ModelState.AddModelError("Price", "Qiymət sıfırdan böyük olmalıdır.");
            }
            if (part.WarrantyMonths < 0)
            {
                ModelState.AddModelError("WarrantyMonths", "Zəmanət müddəti mənfi ola bilməz.");
            }

            if (!ModelState.IsValid) { 
                ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name"); 
                ViewBag.QualityTypes = new SelectList(await _context.QualityTypes.ToListAsync(), "Id", "Name"); 
                ViewBag.SelectedCategoryId = CategoryId;
                ViewBag.SelectedBrandId = BrandId;
                return View(part); 
            } 
            
            if (string.IsNullOrEmpty(part.SKU))
            {
                part.SKU = $"SKU-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
            }
            
            part.SellerId = sellerProfile.Id;
            part.CreatedDate = DateTime.Now;
            part.LastModifiedDate = DateTime.Now;
            part.ModerationStatus = ApprovalStatus.Pending;
            part.IsActive = true;
            part.IsDeleted = false;
            
            _context.Add(part);
            
            var log = new ActivityLog
            {
                UserId = userId,
                Action = "CreateAd",
                EntityName = "SparePart",
                Details = "Yeni elan yaradıldı",
                Date = DateTime.Now
            };
            _context.ActivityLogs.Add(log);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditPart(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var sellerProfile = await _context.SellerProfiles.FirstOrDefaultAsync(s => s.UserId == userId);
            if (sellerProfile == null) return Unauthorized();

            var part = await _context.SpareParts.FirstOrDefaultAsync(p => p.Id == id && p.SellerId == sellerProfile.Id && !p.IsDeleted);
            if (part == null) return NotFound();

            var deviceModel = await _context.DeviceModels.FirstOrDefaultAsync(m => m.Id == part.DeviceModelId);
            if (deviceModel != null) {
                ViewBag.SelectedCategoryId = deviceModel.CategoryId;
                ViewBag.SelectedBrandId = deviceModel.BrandId;
            }
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            ViewBag.QualityTypes = new SelectList(await _context.QualityTypes.ToListAsync(), "Id", "Name");
            return View(part);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPart(int id, SparePart updatedPart)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var sellerProfile = await _context.SellerProfiles.FirstOrDefaultAsync(s => s.UserId == userId);
            if (sellerProfile == null) return Unauthorized();

            var part = await _context.SpareParts.FirstOrDefaultAsync(p => p.Id == id && p.SellerId == sellerProfile.Id && !p.IsDeleted);
            if (part == null) return NotFound();

            if (updatedPart.Price <= 0) ModelState.AddModelError("Price", "Qiymət sıfırdan böyük olmalıdır.");
            if (updatedPart.WarrantyMonths < 0) ModelState.AddModelError("WarrantyMonths", "Zəmanət müddəti mənfi ola bilməz.");
            if (updatedPart.StockQuantity < 0) ModelState.AddModelError("StockQuantity", "Stok miqdarı mənfi ola bilməz.");
            if (!string.IsNullOrEmpty(updatedPart.SKU) && updatedPart.SKU != part.SKU)
            {
                var skuExists = await _context.SpareParts.AnyAsync(p => p.SKU == updatedPart.SKU && p.SellerId == sellerProfile.Id && !p.IsDeleted);
                if (skuExists) ModelState.AddModelError("SKU", "Bu SKU artıq istifadə olunub.");
            }

            ModelState.Remove("Seller"); ModelState.Remove("DeviceModel"); ModelState.Remove("PartType"); ModelState.Remove("QualityType"); 
            ModelState.Remove("Images"); ModelState.Remove("SKU"); ModelState.Remove("Currency");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
                ViewBag.QualityTypes = new SelectList(await _context.QualityTypes.ToListAsync(), "Id", "Name");
                return View(updatedPart);
            }

            bool requiresApproval = false;
            
            if (part.Title != updatedPart.Title ||
                part.DeviceModelId != updatedPart.DeviceModelId ||
                part.PartTypeId != updatedPart.PartTypeId ||
                part.QualityTypeId != updatedPart.QualityTypeId ||
                part.Description != updatedPart.Description ||
                part.Price != updatedPart.Price)
            {
                requiresApproval = true;
            }

            part.DeviceModelId = updatedPart.DeviceModelId;
            part.PartTypeId = updatedPart.PartTypeId;
            part.QualityTypeId = updatedPart.QualityTypeId;
            part.Title = updatedPart.Title;
            part.Description = updatedPart.Description;
            part.Price = updatedPart.Price;
            part.Currency = updatedPart.Currency ?? "AZN";
            part.StockQuantity = updatedPart.StockQuantity;
            part.MinOrderQuantity = updatedPart.MinOrderQuantity;
            part.WarrantyMonths = updatedPart.WarrantyMonths;
            if (!string.IsNullOrEmpty(updatedPart.SKU))
            {
                part.SKU = updatedPart.SKU;
            }
            part.DiscountPrice = updatedPart.DiscountPrice;
            part.LastModifiedDate = DateTime.Now;

            if (requiresApproval)
            {
                part.ModerationStatus = ApprovalStatus.Pending;
                part.IsActive = false;
            }

            if (part.StockQuantity <= 0 && part.IsActive)
            {
                part.IsActive = false;
                part.ModerationStatus = ApprovalStatus.Approved;
            }

            try
            {
                _context.Update(part);
                
                var log = new ActivityLog
                {
                    UserId = userId,
                    Action = "UpdateAd",
                    EntityName = "SparePart",
                    EntityId = part.Id,
                    Details = "Elan yeniləndi",
                    Date = DateTime.Now
                };
                _context.ActivityLogs.Add(log);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "Eyni vaxtda başqa istifadəçi tərəfindən dəyişiklik edilib. Zəhmət olmasa səhifəni yeniləyin və yenidən cəhd edin.");
                ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
                ViewBag.QualityTypes = new SelectList(await _context.QualityTypes.ToListAsync(), "Id", "Name");
                return View(updatedPart);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePrice(int id, decimal newPrice)
        {
            if (newPrice <= 0) return BadRequest("Mənfi və ya sıfır qiymət qəbul edilmir.");

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var sellerProfile = await _context.SellerProfiles.FirstOrDefaultAsync(s => s.UserId == userId);
            if (sellerProfile == null) return Unauthorized();

            var part = await _context.SpareParts.FirstOrDefaultAsync(p => p.Id == id && p.SellerId == sellerProfile.Id && !p.IsDeleted);
            if (part == null) return NotFound();

            part.Price = newPrice;
            part.LastModifiedDate = DateTime.Now;

            _context.Update(part);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var sellerProfile = await _context.SellerProfiles.FirstOrDefaultAsync(s => s.UserId == userId);
            if (sellerProfile == null) return Unauthorized();

            var part = await _context.SpareParts.FirstOrDefaultAsync(p => p.Id == id && p.SellerId == sellerProfile.Id && !p.IsDeleted);
            if (part == null) return NotFound();

            if (part.IsActive)
            {
                part.IsActive = false;
                part.ModerationStatus = ApprovalStatus.Approved;
                part.LastModifiedDate = DateTime.Now;

                _context.Update(part);
                await _context.SaveChangesAsync();
            }
            else
            {
                TempData["Error"] = "Elanı yenidən aktivləşdirmək yalnız administrator tərəfindən edilə bilər.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePart(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var sellerProfile = await _context.SellerProfiles.FirstOrDefaultAsync(s => s.UserId == userId);
            if (sellerProfile == null) return Unauthorized();

            var part = await _context.SpareParts.FirstOrDefaultAsync(p => p.Id == id && p.SellerId == sellerProfile.Id && !p.IsDeleted);
            if (part == null) return NotFound();

            part.IsDeleted = true;
            part.IsActive = false; // also deactivate if deleted
            part.LastModifiedDate = DateTime.Now;

            _context.Update(part);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecordSale(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var sellerProfile = await _context.SellerProfiles.FirstOrDefaultAsync(s => s.UserId == userId);

            if (sellerProfile == null) return Unauthorized();

            try 
            {
                var part = await _context.SpareParts.FirstOrDefaultAsync(p => p.Id == id && p.SellerId == sellerProfile.Id && !p.IsDeleted);
                
                if (part == null || part.StockQuantity <= 0)
                {
                    TempData["Error"] = "Məhsul tapılmadı və ya stok bitib.";
                    return RedirectToAction(nameof(Index));
                }

                var previousStock = part.StockQuantity;
                part.StockQuantity -= 1;

                if (part.StockQuantity == 0)
                {
                    part.ModerationStatus = ApprovalStatus.Approved;
                    part.IsActive = false;
                }

                var sale = new SparePartSaleAudit {
                    SparePartId = part.Id,
                    SellerId = userId,
                    Quantity = 1,
                    PreviousStock = previousStock,
                    NewStock = part.StockQuantity,
                    Timestamp = DateTime.Now
                };

                _context.SparePartSaleAudits.Add(sale);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["Error"] = "Eyni vaxtda başqa istifadəçi tərəfindən satış edilib. Zəhmət olmasa səhifəni yeniləyin və yenidən cəhd edin.";
            }
            catch
            {
                TempData["Error"] = "Satış zamanı xəta baş verdi.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _context.Users
                .Include(u => u.SellerProfile)
                .FirstOrDefaultAsync(u => u.Id == userId);
            
            if (user == null || user.SellerProfile == null) return NotFound();
            
            return View(user.SellerProfile);
        }

        [HttpPost]
        public async Task<IActionResult> Settings(SellerProfile updatedProfile)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _context.Users
                .Include(u => u.SellerProfile)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.SellerProfile == null) return NotFound();

            if (!string.IsNullOrWhiteSpace(updatedProfile.WhatsAppNumber))
            {
                if (!EHisseler.Web.Models.PhoneNumberHelper.TryNormalizeAzNumber(updatedProfile.WhatsAppNumber, out string _, out string _))
                {
                    ModelState.AddModelError("WhatsAppNumber", "WhatsApp nömrəsi düzgün formatda deyil.");
                    return View(updatedProfile);
                }
                user.SellerProfile.WhatsAppNumber = updatedProfile.WhatsAppNumber;
            }
            else
            {
                user.SellerProfile.WhatsAppNumber = null;
            }

            user.SellerProfile.ShowContactInfoInAds = updatedProfile.ShowContactInfoInAds;
            
            if (user.SellerProfile.Phone != updatedProfile.Phone)
            {
                if (!EHisseler.Web.Models.PhoneNumberHelper.TryNormalizeAzNumber(updatedProfile.Phone, out string _, out string normalizedTel))
                {
                    ModelState.AddModelError("Phone", "Mobil nömrə düzgün formatda deyil.");
                    return View(updatedProfile);
                }
                user.SellerProfile.Phone = normalizedTel;
                user.SellerProfile.IsPhoneVerified = false;
            }
            
            _context.Update(user.SellerProfile);
            await _context.SaveChangesAsync();
            
            TempData["Success"] = "T?nziml?m?l?r ugurla yadda saxlanildi.";
            return RedirectToAction(nameof(Settings));
        }
    }
}


