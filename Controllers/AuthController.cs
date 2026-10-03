using EHisseler.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EHisseler.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return LocalRedirect(returnUrl);
                return RedirectToAction("Index", "Home");
            }
                
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, string returnUrl = null)
        {
            // Parol hashinqi
            username = username?.Trim() ?? string.Empty;
            
            // Allow login by Username, Email, or Phone
            string normalizedPhone = username;
            if (PhoneNumberHelper.TryNormalizeAzNumber(username, out string _, out string nTel))
            {
                normalizedPhone = nTel;
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => 
                u.Username.ToLower() == username.ToLower() || 
                (u.Email != null && u.Email.ToLower() == username.ToLower()) ||
                u.Phone == normalizedPhone);

            if (user == null)
            {
                ViewBag.Error = "Mælumatlar yanlışdır.";
                ViewBag.Username = username;
                return View();
            }

            // Geri uyğunluq (legacy plain text passwords) üçün
            bool isPasswordValid = false;
            if (!user.PasswordHash.StartsWith("$2"))
            {
                if (user.PasswordHash == password)
                {
                    isPasswordValid = true;
                    // Hash-i yeniləyək
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            }

            if (!isPasswordValid)
            {
                ViewBag.Error = "İstifadəçi adı və ya şifrə yanlışdır.";
                ViewBag.Username = username;
                return View();
            }

            if (user.IsBlocked)
            {
                var blockLog = await _context.ActivityLogs
                    .Where(l => l.EntityName == "User" && l.EntityId == user.Id && l.Action == "BlockUser")
                    .OrderByDescending(l => l.Date)
                    .FirstOrDefaultAsync();
                var reasonText = blockLog != null ? blockLog.Details : "Zəhmət olmasa adminlə əlaqə saxlayın.";
                ViewBag.Error = $"Hesabınız bloklanıb. {reasonText}";
                ViewBag.Username = username;
                return View();
            }

            if (user.Role != UserRole.Admin && user.ApprovalStatus == ApprovalStatus.Pending)
            {
                ViewBag.Error = "Hesabınız yoxlamadadır. Admin təsdiqlədikdən sonra daxil ola bilərsiniz.";
                ViewBag.Username = username;
                return View();
            }
            
            if (user.Role != UserRole.Admin && user.ApprovalStatus == ApprovalStatus.Rejected)
            {
                var history = await _context.ApprovalHistories
                    .Where(h => h.TargetUserId == user.Id && h.Status == ApprovalStatus.Rejected)
                    .OrderByDescending(h => h.ActionDate)
                    .FirstOrDefaultAsync();

                var reason = history?.Reason ?? "Qeydiyyatınız admin tərəfindən rədd edilib.";
                ViewBag.Error = $"Qeydiyyatınız rədd edilib. Səbəb: {reason}";
                ViewBag.Username = username;
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.GivenName, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            if (user.Role == UserRole.Admin)
                return RedirectToAction("Index", "Admin");
            
            return RedirectToAction("Index", "Warehouse");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string firstName, string lastName, string phone, string email, string password, UserRole role)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || 
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                ViewBag.Error = "Email, Ad, Soyad və Şifrə məcburidir. Şifrə minimum 6 simvol olmalıdır.";
                return View();
            }

            string normalizedTel = null;
            if (!string.IsNullOrWhiteSpace(phone))
            {
                if (!PhoneNumberHelper.TryNormalizeAzNumber(phone, out string _, out normalizedTel))
                {
                    ViewBag.Error = "Mobil nömrə düzgün formatda deyil.";
                    return View();
                }
            }

            email = email?.Trim() ?? string.Empty;
            if (await _context.Users.AnyAsync(u => u.Email != null && u.Email.ToLower() == email.ToLower()))
            {
                ViewBag.Error = "Bu email unvani artıq mövcuddur.";
                return View();
            }

            if (!string.IsNullOrEmpty(normalizedTel) && await _context.Users.AnyAsync(u => u.Phone == normalizedTel))
            {
                ViewBag.Error = "Bu nömrə artıq mövcuddur.";
                return View();
            }

            if (role == UserRole.Admin)
            {
                ViewBag.Error = "Admin kimi qeydiyyatdan keçmək mümkün deyil.";
                return View();
            }

            string generatedUsername = email;
            if (await _context.Users.AnyAsync(u => u.Username.ToLower() == generatedUsername.ToLower()))
            {
                generatedUsername = normalizedTel;
            }

            var newUser = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Phone = normalizedTel,
                Email = email,
                Username = generatedUsername,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = role,
                ApprovalStatus = ApprovalStatus.Pending,
                CreatedDate = System.DateTime.Now
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return RedirectToAction("RegistrationPending");
        }

        public IActionResult RegistrationPending()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string username)
        {
            username = username?.Trim() ?? string.Empty;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
            if (user != null)
            {
                // In a real application, you would generate a token and send an email or SMS here.
                // For now, we'll just redirect to a mock reset page.
                return RedirectToAction("ResetPassword", new { username = user.Username });
            }
            ViewBag.Error = "Belə bir istifadəçi tapılmadı.";
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string username)
        {
            ViewBag.Username = username;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string username, string newPassword)
        {
            username = username?.Trim() ?? string.Empty;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
            if (user != null)
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            ViewBag.Error = "Xəta baş verdi.";
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

