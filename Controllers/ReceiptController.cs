using BookStore.Data;
using BookStore.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookStore.Controllers
{
    public class ReceiptController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ReceiptController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartCheckout = _db.Carts
                .Include(c => c.Book)
                .Include(c => c.Account)
                .Where(c => c.AccountId == int.Parse(userId))
                .ToList();
            ViewBag.Address = _db.Accounts.Find(int.Parse(userId)).Address;
            ViewBag.Email = _db.Accounts.Find(int.Parse(userId)).Email;
            if (cartCheckout == null)
            {
                TempData["error"] = "Cart is empty!";
                return RedirectToAction("Index", "Home");
            }
            return View(cartCheckout);
        }
    }
}
