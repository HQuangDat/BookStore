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
                return RedirectToAction("Login");
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
    }
}
