using BookStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    public class Book : Controller
    {
        //Dependency Injection
        private readonly ApplicationDbContext dbContext;

        public Book(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //Show list of Books function
        public async Task<IActionResult> List()
        {
            var bookList = await dbContext.Books.ToListAsync();
            return View(bookList);
        }
    }
}
