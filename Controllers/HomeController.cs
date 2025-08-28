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
        const string cacheKey = "BookList";
        if (!_cache.TryGetValue(cacheKey, out List<Book> indexBook))
        {
            indexBook = _db.Books.ToList();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5));
            _cache.Set(cacheKey, indexBook, cacheEntryOptions);
        }
        return View(indexBook);
    }
}
