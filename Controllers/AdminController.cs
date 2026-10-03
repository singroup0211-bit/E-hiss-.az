using EHisseler.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace EHisseler.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

                public async Task<IActionResult> Requests()
        {
            var pendingUsers = await _context.Users
                .Where(u => u.ApprovalStatus == ApprovalStatus.Pending)
                .ToListAsync();

            var pendingAds = await _context.SpareParts
                .Include(p => p.Seller)
                .ThenInclude(s => s.User)
                .Include(p => p.DeviceModel)
                .ThenInclude(m => m.Brand)
                .Include(p => p.PartType)
                .Include(p => p.Reviews)
                .Where(p => p.ModerationStatus == ApprovalStatus.Pending)
                .ToListAsync();
            
            ViewBag.PendingUsers = pendingUsers;
            ViewBag.PendingAds = pendingAds;
            
            return View();
        }

        public async Task<IActionResult> Index()
        {
            var pendingUsers = await _context.Users
                .Where(u => u.ApprovalStatus == ApprovalStatus.Pending)
                .ToListAsync();

            return View(pendingUsers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            var currentAdminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            user.ApprovalStatus = ApprovalStatus.Approved;

            // Avtomatik Satıcı Profili yaradırıq ki, məhsul əlavə edə bilsin
            if (!await _context.SellerProfiles.AnyAsync(s => s.UserId == user.Id))
            {
                var sellerProfile = new SellerProfile
                {
                    UserId = user.Id,
                    CompanyName = $"{user.FirstName} {user.LastName} ({user.Role})",
                    ContactPerson = $"{user.FirstName} {user.LastName}",
                    Phone = user.Phone,
                    WhatsAppNumber = user.Phone, // Default olaraq qeydiyyat nömrəsi WhatsApp sayılır
                    Rating = 5.0m
                };
                _context.SellerProfiles.Add(sellerProfile);
            }

            var history = new ApprovalHistory
            {
                TargetUserId = user.Id,
                AdminId = currentAdminId,
                Status = ApprovalStatus.Approved,
                Reason = "Məlumatlar yoxlanıldı və təsdiqləndi"
            };
            _context.ApprovalHistories.Add(history);

            var log = new ActivityLog
            {
                UserId = currentAdminId,
                Action = "İstifadəçi Təsdiqi",
                EntityName = "User",
                EntityId = user.Id,
                Details = $"{user.Username} adlı istifadəçi təsdiqləndi."
            };
            _context.ActivityLogs.Add(log);

                        SendNotification(user.Phone, user.Email, "Sizin qeydiyyatınız uğurla təsdiqləndi.");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Requests));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectUser(int userId, string reason)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            var currentAdminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            user.ApprovalStatus = ApprovalStatus.Rejected;

            var history = new ApprovalHistory
            {
                TargetUserId = user.Id,
                AdminId = currentAdminId,
                Status = ApprovalStatus.Rejected,
                Reason = string.IsNullOrEmpty(reason) ? "M?lumatlar yet?rsiz v? ya s?hvdir" : reason
            };
            _context.ApprovalHistories.Add(history);

                        SendNotification(user.Phone, user.Email, $"Sizin qeydiyyatınız rədd edildi. Səbəb: {history.Reason}");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Requests));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BlockUser(int userId, string reason)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            user.IsBlocked = true;

            var currentAdminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var log = new ActivityLog
            {
                UserId = currentAdminId,
                Action = "BlockUser",
                EntityName = "User",
                EntityId = user.Id,
                Details = $"İstifadəçi bloklandı. Səbəb: {reason}"
            };
            _context.ActivityLogs.Add(log);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AllUsers));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnblockUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();

            user.IsBlocked = false;

            var currentAdminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var log = new ActivityLog
            {
                UserId = currentAdminId,
                Action = "UnblockUser",
                EntityName = "User",
                EntityId = user.Id,
                Details = "İstifadəçi blokdan çıxarıldı."
            };
            _context.ActivityLogs.Add(log);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AllUsers));
        }

        public async Task<IActionResult> PendingAds()
        {
            var pendingAds = await _context.SpareParts
                .Include(p => p.Seller)
                .Include(p => p.DeviceModel)
                .ThenInclude(m => m.Brand)
                .Include(p => p.PartType)
                .Where(p => p.ModerationStatus == ApprovalStatus.Pending)
                .ToListAsync();

            return View(pendingAds);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveAd(int adId)
        {
            var ad = await _context.SpareParts.Include(p => p.Seller).ThenInclude(s => s.User).FirstOrDefaultAsync(p => p.Id == adId);
            if (ad == null) return NotFound();

            var currentAdminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var oldModerationStatus = ad.ModerationStatus;

            ad.ModerationStatus = ad.StockQuantity > 0 ? ApprovalStatus.Approved : ApprovalStatus.Approved;

            var review = new SparePartReview
            {
                SparePartId = ad.Id,
                AdminId = currentAdminId,
                Status = ad.ModerationStatus,
                Reason = "Təsdiqləndi"
            };
            _context.SparePartReviews.Add(review);
            
            var log = new ActivityLog
            {
                UserId = currentAdminId,
                Action = "Status Change",
                EntityName = "SparePart",
                EntityId = ad.Id,
                Details = $"Status {oldModerationStatus} -> {ad.ModerationStatus} olaraq dəyişdirildi"
            };
            _context.ActivityLogs.Add(log);

                        if (ad.ModerationStatus == ApprovalStatus.Approved && oldModerationStatus != ApprovalStatus.Approved)
            {
                SendMarketingNotification(ad);
            }
            SendNotification(ad.Seller.Phone, ad.Seller.User.Email, $"Sizin '{ad.Title}' adlı elanınız təsdiqləndi.");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Requests));
        }

                        private void SendNotification(string phone, string? email, string message)
        {
            // SMS G?nd?rilm?si (Mock)
            System.Console.WriteLine($"[SMS -> {phone}]: {message}");

            // E-po?t G?nd?rilm?si (Real .NET SMTP kod strukturu haz?r, ancaq detallar ?lav? edilm?lidir)
            if (!string.IsNullOrEmpty(email))
            {
                System.Console.WriteLine($"[EMAIL -> {email}] (from: info@e-hisse.az): {message}");
                
                /* 
                // G?l?c?kd? SMTP m?lumatlar?n? bura yazaraq real email g?nd?r? bil?rsiniz:
                try 
                {
                    using (var client = new System.Net.Mail.SmtpClient("smtp.sizin-server.com", 587))
                    {
                        client.Credentials = new System.Net.NetworkCredential("info@e-hisse.az", "?ifr?niz_burada");
                        client.EnableSsl = true;

                        var mailMessage = new System.Net.Mail.MailMessage
                        {
                            From = new System.Net.Mail.MailAddress("info@e-hisse.az", "e-hisse.az Bildiri�"),
                            Subject = "e-hisse.az - M?raci?tinizin statusu",
                            Body = message,
                            IsBodyHtml = false,
                        };
                        mailMessage.To.Add(email);

                        client.Send(mailMessage);
                    }
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine($"Email g?nd?ril?rk?n x?ta: {ex.Message}");
                }
                */
            }
        }

        private void SendMarketingNotification(SparePart ad)
        {
            // Marketinq bildirişinin göndərilməsi üçün mock məntiq
            System.Console.WriteLine($"[Marketinq] Yeni məhsul satışa çıxarıldı: {ad.Title}");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectAd(int adId, string reason)
        {
            var ad = await _context.SpareParts.Include(p => p.Seller).ThenInclude(s => s.User).FirstOrDefaultAsync(p => p.Id == adId);
            if (ad == null) return NotFound();

            var currentAdminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var oldModerationStatus = ad.ModerationStatus;

            ad.ModerationStatus = ApprovalStatus.Rejected;

            var review = new SparePartReview
            {
                SparePartId = ad.Id,
                AdminId = currentAdminId,
                Status = ApprovalStatus.Rejected,
                Reason = string.IsNullOrEmpty(reason) ? "Qaydalarla ziddiyy?t t?skil edir." : reason
            };
            _context.SparePartReviews.Add(review);
            
            var log = new ActivityLog
            {
                UserId = currentAdminId,
                Action = "Status Change",
                EntityName = "SparePart",
                EntityId = ad.Id,
                Details = $"Status {oldModerationStatus} -> {ad.ModerationStatus} olaraq dəyişdirildi. Səbəb: {review.Reason}"
            };
            _context.ActivityLogs.Add(log);

                        SendNotification(ad.Seller.Phone, ad.Seller.User.Email, $"Sizin '{ad.Title}' adlı elanınız rədd edildi. Səbəb: {review.Reason}");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Requests));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreAd(int adId)
        {
            var ad = await _context.SpareParts.Include(p => p.Seller).ThenInclude(s => s.User).FirstOrDefaultAsync(p => p.Id == adId);
            if (ad == null) return NotFound();

            if (ad.StockQuantity <= 0)
            {
                TempData["Error"] = "Bərpa zamanı stokun müsbət olması tələb olunur.";
                return RedirectToAction(nameof(AllAds));
            }

            var currentAdminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var oldModerationStatus = ad.ModerationStatus;

            ad.ModerationStatus = ApprovalStatus.Pending;
            ad.IsDeleted = false;
            ad.IsActive = true;

            var log = new ActivityLog
            {
                UserId = currentAdminId,
                Action = "RestoreAd",
                EntityName = "SparePart",
                EntityId = ad.Id,
                Details = $"Elan bərpa edildi və yenidən yoxlamaya göndərildi. Status {oldModerationStatus} -> {ad.ModerationStatus}"
            };
            _context.ActivityLogs.Add(log);

            await _context.SaveChangesAsync();
            TempData["Success"] = "Elan bərpa edildi və yoxlamaya göndərildi.";
            return RedirectToAction(nameof(AllAds));
        }

        public async Task<IActionResult> AllUsers()
        {
            return View(await _context.Users.ToListAsync());
        }

        public async Task<IActionResult> AllAds()
        {
            var ads = await _context.SpareParts
                .Include(p => p.Seller)
                .Include(p => p.DeviceModel)
                .ThenInclude(m => m.Brand)
                .ToListAsync();
            return View(ads);
        }

        public async Task<IActionResult> Categories()
        {
            return View(await _context.Categories.ToListAsync());
        }

        public async Task<IActionResult> ModelsAndBrands()
        {
            var models = await _context.DeviceModels
                .Include(m => m.Brand)
                .Include(m => m.Category)
                .ToListAsync();
            return View(models);
        }

        public async Task<IActionResult> ActivityLogs()
        {
            var logs = await _context.ActivityLogs
                .OrderByDescending(l => l.Date)
                .Take(100)
                .ToListAsync();
            return View(logs);
        }
    }
}



