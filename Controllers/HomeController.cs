using System.Diagnostics;
using BookStore.Data;
using BookStore.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BookStore.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IMemoryCache _cache;
    public HomeController(ApplicationDbContext db, IMemoryCache cache)
    {
        _db = db;
        _cache = cache;
    }

    public IActionResult Index()
    {
        const string cacheKey = "NewestBooks";
        if (!_cache.TryGetValue(cacheKey, out List<Book> newestBooks))
        {
            newestBooks = _db.Books.OrderByDescending(b => b.BookId).Take(10).ToList();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));
            _cache.Set(cacheKey, newestBooks, cacheEntryOptions);
        }
        return View(newestBooks);
    }
}
