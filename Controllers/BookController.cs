using BookStore.Data;
using BookStore.Models.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class BookController : Controller
{
    private readonly ApplicationDbContext dbContext;

    public BookController(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    // GET: Book/Add
    [HttpGet]
    public IActionResult Add()
    {
        ViewBag.Authors = new SelectList(dbContext.Authors, "AuthorID", "LastName");
        ViewBag.Categories = new SelectList(dbContext.Categories, "CategoryID", "CategoryName");
        return View();
    }

    // POST: Book/Add
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(Book book)
    {
        if (ModelState.IsValid)
        {
            book.BookID = Guid.NewGuid();
            await dbContext.Books.AddAsync(book);
            await dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }

        ViewBag.Authors = new SelectList(dbContext.Authors, "AuthorID", "LastName", book.AuthorID);
        ViewBag.Categories = new SelectList(dbContext.Categories, "CategoryID", "CategoryName", book.CategoryID);
        return View(book);
    }

    // GET: Book/List
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