using BookStore.Data;
using BookStore.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BookStore.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        public CartRepository(ApplicationDbContext db)
        {
            _db = db;
        } 

        public void Add(Cart cart)
        {
            _db.Carts.Add(cart);
        }

        public Cart existingCartItem(int? accountId, int? bookId)
        {
            var existingCartItem = _db.Carts.FirstOrDefault(c => c.AccountId == accountId && c.BookId == bookId);
            if(existingCartItem == null) {
                return null;
            }
            return existingCartItem;
        }

        public Book findBookbyID(int? id)
        {
            var book = _db.Books.Find(id);
            if (book == null)
            {
                return null;
            }
            return book;
        }

        public Cart findCartbyBookID(int? id)
        {
            var cart = _db.Carts.FirstOrDefault(bk => bk.BookId == id);
            if(cart == null)
            {
                return null;
            }
            return cart;
        }

        public IEnumerable<Cart> getAllByUserID(string userId)
        {
            return _db.Carts.Include(bk => bk.Book).Where(id => id.AccountId.ToString() == userId).ToList();
        }

        public void Remove(Cart cart)
        {
            _db.Carts.Remove(cart);
        }


        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Cart cart)
        {
            _db.Carts.Update(cart);
        }
    }
}
