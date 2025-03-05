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
            if(id != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cartBook = _db.Books.Find(id);
                if(userId != null && cartBook != null)
                {
                    var cart = new Cart
                    {
                        AccountId = int.Parse(userId),
                        BookId = id.Value
                    };
                    _db.Carts.Add(cart);
                    TempData["success"] = "Add to cart successfully";
                    _db.SaveChanges();
                }
                else
                {
                    TempData["error"] = "Invalid book or user";
                }
            }
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
