using BookStore.Data;
using BookStore.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookStore.Controllers
{
    [Authorize]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmCheckout(List<Cart> CartItems)
        {
            if (CartItems == null || !CartItems.Any())
            {
                TempData["error"] = "Your cart is empty!";
                return RedirectToAction("Index", "Cart");
            }

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var checkOut = new Receipt
            {
                AccountId = userId,
                TotalAmount = CartItems.Sum(c => _db.Books.First(b => b.BookId == c.BookId).Price * c.Quantity),
            };
            _db.Receipts.Add(checkOut);
            _db.SaveChanges(); 

            var receiptItems = CartItems.Select(c => new ReceiptItem
            {
                ReceiptId = checkOut.ReceiptId,
                BookId = c.BookId,
                Quantity = c.Quantity,
            }).ToList();

            _db.ReceiptItems.AddRange(receiptItems);

            var cartItemsToRemove = _db.Carts.Where(c => c.AccountId == userId).ToList();
            _db.Carts.RemoveRange(cartItemsToRemove);

            _db.SaveChanges();
            TempData["success"] = "Checkout successfully!";
            return RedirectToAction("Index", "Home");
        }

    }
}
