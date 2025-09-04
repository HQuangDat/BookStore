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
using BookStore.Repositories;
using Serilog;
using Hangfire;
using BookStore.Service;
using Microsoft.Extensions.Caching.Memory;


namespace BookStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountrepository;
        private readonly IMemoryCache _cache;
        public AccountController(IAccountRepository accountrepository, IMemoryCache cache)
        {
            _accountrepository = accountrepository;
            _cache = cache;
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
            try
            {
                var existUser = await _accountrepository.getByUsernameAsync(username);
                if (existUser != null)
                {
                    PasswordVerificationResult result = _accountrepository.passwordVerificationResult(existUser, existUser.Password, password);
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
                        Log.Information("User {Username} logged in successfully", username);
                        return RedirectToAction("Index", "Home");
                    }
                }
                TempData["error"] = "An error occurred while processing your request. Please try again later.";
                Log.Error("An error occurred during login for user");
                return View();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during login for user {Username}", username);
                return View();
            }
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
                _accountrepository.createNewUser(user);
                _accountrepository.Save();
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
            var user = _accountrepository.getById(id);
            if (user!=null)
            {
                string errorMessage;
                _accountrepository.GrantAdmin(user, out errorMessage);
                if(errorMessage!=null)
                {
                    TempData["error"] = errorMessage;
                    return RedirectToAction("List");
                }
                else
                {
                    _accountrepository.Save();
                    TempData["success"] = "Grant admin role successfully!";
                    return RedirectToAction("List");
                }
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
            bool isDeleted = _accountrepository.Remove(id);
            if (isDeleted)
            {
                _accountrepository.Save();
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
            const string cacheKey = "AccountList";
            if (!_cache.TryGetValue(cacheKey, out List<Account> listAccount))
            {
                listAccount = _accountrepository.GetAllAccounts().ToList();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));
                _cache.Set(cacheKey, listAccount, cacheEntryOptions);
            }
            return View(listAccount);
        }

        //Account details
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Details(int id)
        {
            var user = _accountrepository.Details(id);
            if (user == null)
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
            var user = await _accountrepository.getByEmailAsync(email);
            if (user == null)
            {
                TempData["error"] = "Email not found!";
                return View();
            }
            // Generate a password reset token
            string token = Guid.NewGuid().ToString();
            _accountrepository.createPasswordReset(email, token);
            _accountrepository.Save();

            // Send email 
            var resetLink = Url.Action("ResetPassword", "Account", new { token = token }, Request.Scheme);

            BackgroundJob.Enqueue<SendMailService>(mail => mail.SendEmail(email,resetLink));
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            var passwordReset = _accountrepository.getPasswordReset(token);
            if (passwordReset == null)
            {
                TempData["error"] = "Invalid or expired token!";
                return Content("Invalid token!");
            }
            else if (passwordReset.ExpireDate < DateTime.UtcNow)
            {
                _accountrepository.removePasswordReset(passwordReset);
                _accountrepository.Save();
                TempData["error"] = "Expired token!";
                return Content("Expired token!");
            }   
            return View(model:token);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string token, string newpassword)
        {
            var passwordReset = _accountrepository.getPasswordReset(token);
            var user = await _accountrepository.getByEmailAsync(passwordReset.Email);
            if (user != null)
            {
                _accountrepository.resetPassword(user, newpassword);
                _accountrepository.removePasswordReset(passwordReset);  
                _accountrepository.Save();
                TempData["success"] = "Password changed successfully!";
                return RedirectToAction("Login");
            }
            TempData["error"] = "User not found!";
            return RedirectToAction("Login");
        }

        //Send Email method
        //public async Task<IActionResult> SendEmail(string email, string resetLink)
        //{
        //    var message = new MailMessage();
        //    message.To.Add(new MailAddress(email));
        //    message.Subject = "Reset your account password!";
        //    message.Body = $"<p>Click <a href='{resetLink}'>here</a> to reset your password.</p>";
        //    message.IsBodyHtml = true;
        //    message.From = new MailAddress("hoquangdat123@gmail.com");
        //    using (var smtp = new SmtpClient("smtp.gmail.com", 587))
        //    {
        //        smtp.Credentials = new NetworkCredential("hoquangdat123@gmail.com", "vpkp ssdb coam qfxp");
        //        smtp.EnableSsl = true;
        //        await smtp.SendMailAsync(message);
        //    }
        //    TempData["success"] = "Email sent successfully";
        //    return Content("Email sent successfully!");
        //}
    }
}
