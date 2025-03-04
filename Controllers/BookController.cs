using BookStore.Data;
using BookStore.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _db;
        public BookController(ApplicationDbContext db)
        {
            _db = db;   
        }

        //Add function
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Categories = new SelectList(_db.Categories, "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Add(Book book)
        {
            if (ModelState.IsValid)
            {
                book.Categories = _db.Categories.Where(c => book.SelectedCategories.Contains(c.CategoryId)).ToList();

                _db.Books.Add(book);
                _db.SaveChanges();

                TempData["success"] = "Book added successfully!";
                return RedirectToAction("List");
            }

            ViewBag.Categories = new SelectList(_db.Categories, "CategoryId", "Name");
            TempData["error"] = "Failed to add book!";
            return View(book);
        }




        //Edit method
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                TempData["error"] = "Book not found!";
                return RedirectToAction("List");
            }
            var book = _db.Books.Include(ct => ct.Categories)
                //.Include(wh=>wh.Warehouse)
                .FirstOrDefault(y=>y.BookId == id);

            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                _db.Books.Update(book);
                _db.SaveChanges();
                TempData["success"] = "Book updated successfully!";
                return RedirectToAction("List");
            }
            TempData["error"] = "Failed to update book!";
            return View(book);
        }

        [HttpGet]
        public IActionResult List()
        {
            var books = _db.Books.Include(ct=>ct.Categories).ToList();
            return View(books);
        }

        //Delete method
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "Book not found!";
                return RedirectToAction("List");
            }
            var book = _db.Books.FirstOrDefault(y => y.BookId == id);
            _db.Books.Remove(book);
            _db.SaveChanges();
            TempData["success"] = "Book deleted successfully!";
            return RedirectToAction("List");
        }
    }
}
