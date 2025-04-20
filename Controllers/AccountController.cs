using BookStore.Data;
using BookStore.DataModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Google;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;


namespace BookStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IPasswordHasher<Account> _passwordHasher;
        public AccountController(ApplicationDbContext db, IPasswordHasher<Account> passwordHasher)
        {
            _db = db;
            _passwordHasher = passwordHasher;
        }

        //For Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        //OAuth2 Login
        public async Task GoogleLogin()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claims => new
            {
                claims.Issuer,
                claims.OriginalIssuer,
                claims.Type,
                claims.Value
            });

            return Json(claims);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            var existUser = await _db.Accounts.Include(role => role.Roles).
                FirstOrDefaultAsync(name => name.Username == username);

            if (existUser != null)
            {
                PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(existUser, existUser.Password, password);
                if (result == PasswordVerificationResult.Success)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, existUser.Email),
                        new Claim(ClaimTypes.Role, existUser.Roles.FirstOrDefault().RoleName),
                        new Claim(ClaimTypes.NameIdentifier, existUser.AccountId.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    TempData["success"] = "Login successful";
                    return RedirectToAction("Index", "Home");
                }
            }
            TempData["error"] = "Invalid username or password!";
            return View();
        }

        //For Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Account user)
        {
            if (ModelState.IsValid)
            {
                user.Password = _passwordHasher.HashPassword(user, user.Password);
                user.Roles.Add(_db.Roles.Find(2));
                _db.Accounts.Add(user);
                _db.SaveChanges();
                TempData["success"] = "Create account success!";
                return RedirectToAction("Login");
            }
            TempData["error"] = "Please check the information!";
            return View();
        }


        //Grant Admin Role for User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GrantAdminRole(int id)
        {
            var user = _db.Accounts.Include(rl=>rl.Roles).FirstOrDefault(u => u.AccountId == id);
            if (user!=null)
            {
                var adminRole = _db.Roles.Find(1);
                if(adminRole == null)
                {
                    TempData["error"] = "Admin role not found!";
                    return RedirectToAction("List");
                }
                if (user.Roles.Any(r => r.RoleId == 1 || r.RoleName == "Admin"))
                {
                    TempData["error"] = "User already has admin role!";
                    return RedirectToAction("List");
                }
                user.Roles.Add(adminRole);
                _db.SaveChanges();
                TempData["success"] = "Grant admin role successfully!";
                return RedirectToAction("List");
            }
            TempData["error"] = "User not found!";
            return RedirectToAction("List");
        }

        //For Forgot Password
        [HttpGet]
        public IActionResult Forgot()
        {
            return View();
        }

        //Loguot
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        //For Delete
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            Account user = _db.Accounts.FirstOrDefault(us => us.AccountId == id);
            if (user != null)
            {
                _db.Accounts.Remove(user);
                _db.SaveChanges();
                TempData["success"] = "Delete successfully!";
                return RedirectToAction("List");
            }
            TempData["error"] = "User not found";
            return NotFound();
        }

        //For list
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult List()
        {
            var listAccount = _db.Accounts.ToList();
            return View(listAccount);
        }

        //Account details
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Details(int id)
        {
            var user = _db.Accounts.Include(rol=>rol.Roles).FirstOrDefault(u => u.AccountId == id);
            if(user == null)
            {
                TempData["error"] = "User not found!";
                return RedirectToAction("List");
            }
            return View(user);
        }

        //Forgot Password
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _db.Accounts.FirstOrDefaultAsync(u => u.Email == email);
            if(user == null)
            {
                TempData["error"] = "Email not found!";
                return View();
            }
            // Generate a password reset token
            string token = Guid.NewGuid().ToString();
            _db.PasswordReset.Add(new PasswordReset
            {
                Email = email,
                Token = token,
                ExpireDate = DateTime.UtcNow.AddHours(1)
            });
            _db.SaveChanges();

            // Send email 
            var resetLink = Url.Action("ResetPassword", "Account", new { token = token }, Request.Scheme);
            await SendEmail(email, resetLink);

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            var passwordReset = _db.PasswordReset.FirstOrDefault(p => p.Token == token);
            if(passwordReset == null || passwordReset.ExpireDate < DateTime.UtcNow)
            {
                TempData["error"] = "Invalid or expired token!";
                return Content("Invalid or expired token!");
            }
            return View(model:token);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string token, string newpassword)
        {
            var passwordReset = _db.PasswordReset.FirstOrDefault(p => p.Token == token);
            if (passwordReset == null || passwordReset.ExpireDate < DateTime.UtcNow)
            {
                TempData["error"] = "Invalid or expired token!";
                return Content("Invalid or expired token!");
            }
            var user = await _db.Accounts.FirstOrDefaultAsync(u => u.Email == passwordReset.Email);
            if(user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, newpassword);
                _db.PasswordReset.Remove(passwordReset);
                _db.SaveChanges();
                TempData["success"] = "Password changed successfully!";
                return RedirectToAction("Login");
            }
            TempData["error"] = "User not found!";
            return RedirectToAction("Login");
            
        }

        //Send Email method
        public async Task<IActionResult> SendEmail(string email, string resetLink)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(email));
            message.Subject = "Reset your account password!";
            message.Body = $"<p>Click <a href='{resetLink}'>here</a> to reset your password.</p>";
            message.IsBodyHtml = true;
            message.From = new MailAddress("hoquangdat123@gmail.com");
            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential("hoquangdat123@gmail.com", "vpkp ssdb coam qfxp");
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }
            TempData["success"] = "Email sent successfully";
            return Content("Email sent successfully!");
        }
    }
}
