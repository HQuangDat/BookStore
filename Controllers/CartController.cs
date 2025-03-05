using BookStore.Data;
using BookStore.DataModels;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }

        //Add item to Cart
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "Invalid book ID";
                return RedirectToAction("Index");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartBook = _db.Books.Find(id);

            if (userId == null || cartBook == null)
            {
                TempData["error"] = "Invalid book or user";
                return RedirectToAction("Index");
            }

            int accountId = int.Parse(userId);
            var existingCartItem = _db.Carts.FirstOrDefault(c => c.AccountId == accountId && c.BookId == id);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += 1;
                _db.Carts.Update(existingCartItem);
            }
            else
            {
                // Add new item to cart
                var cart = new Cart
                {
                    AccountId = accountId,
                    BookId = id.Value
                };
                _db.Carts.Add(cart);
            }

            _db.SaveChanges();
            TempData["success"] = "Added to cart successfully";

            return RedirectToAction("Index");
        }

        //Show item in Cart
        [HttpGet]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var listCart = _db.Carts.Include(bk=>bk.Book).Where(id=> id.AccountId.ToString() == userId).ToList();
            return View(listCart);
        }

        //Delete item in cart
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if(id != null)
            {
                Cart cart = _db.Carts.FirstOrDefault(bk=>bk.BookId == id);
                _db.Carts.Remove(cart);
                _db.SaveChanges();
                TempData["success"] = "Success";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Remove item failed";
            return RedirectToAction("Index");
        }
    }
}
