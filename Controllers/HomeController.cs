using System.Diagnostics;
using BookStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;
    public HomeController(ApplicationDbContext db)
    {
        _db = db;
    }


    public IActionResult Index()
    {
        var indexBook = _db.Books.ToList();
        return View(indexBook);
    }
}
