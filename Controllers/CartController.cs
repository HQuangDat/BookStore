using BookStore.Data;
using BookStore.DataModels;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Add(Cart cart)
        {
            if(ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                cart.AccountId = int.Parse(userId);
                _db.Carts.Add(cart);
                TempData["success"] = "Add to cart successfully";
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        //Show item in Cart
        [HttpGet]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var listCart = _db.Carts.Where(id=> id.AccountId.ToString() == userId).ToList();
            return View(listCart);
        }
    }
}
