using BookStore.Data;
using BookStore.DataModels;
using BookStore.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookStore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartrepo;
        public CartController(ICartRepository cartrepo)
        {
            _cartrepo = cartrepo;
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
            var cartBook =_cartrepo.findBookbyID(id);

            if (userId == null || cartBook == null)
            {
                TempData["error"] = "Invalid book or user";
                return RedirectToAction("Index");
            }

            int accountId = int.Parse(userId);
            var existingCartItem = _cartrepo.existingCartItem(accountId, id);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += 1;
                _cartrepo.Update(existingCartItem);
            }
            else
            {
                // Add new item to cart
                var cart = new Cart
                {
                    AccountId = accountId,
                    BookId = id.Value
                };
                _cartrepo.Add(cart);
            }

            _cartrepo.Save();
            TempData["success"] = "Added to cart successfully";

            return RedirectToAction("Index");
        }

        //Show item in Cart
        [HttpGet]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var listCart = _cartrepo.getAllByUserID(userId);
            return View(listCart);
        }

        //Delete item in cart
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if(id != null)
            {
                Cart cart = _cartrepo.findCartbyBookID(id);
                _cartrepo.Remove(cart);
                _cartrepo.Save();
                TempData["success"] = "Success";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Remove item failed";
            return RedirectToAction("Index");
        }
    }
}
