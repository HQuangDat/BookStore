using BookStore.Data;
using BookStore.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public BookController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //Add Function
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Authors = new SelectList(dbContext.Authors, "AuthorID", "LastName");
            ViewBag.Categories = new SelectList(dbContext.Categories, "CategoryID", "CategoryName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Book book)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Authors = new SelectList(dbContext.Authors, "AuthorID", "FullName");
                ViewBag.Categories = new SelectList(dbContext.Categories, "CategoryID", "CategoryName");
                return View(book);
            }

            book.BookID = Guid.NewGuid();
            await dbContext.Books.AddAsync(book);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("List", "Book");
        }

        //Show list function
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var bookList = await dbContext.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .ToListAsync();
            return View(bookList);
        }
    }
}