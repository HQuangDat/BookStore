using BookStore.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _db;
        public BookController(ApplicationDbContext db)
        {
            _db = db;   
        }
        [HttpGet]
        public IActionResult List()
        {
            return View();
        }
    }
}
