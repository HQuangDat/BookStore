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
            ViewBag.Warehouses = new SelectList(_db.Warehouses, "WarehouseId", "Name");
            ViewBag.Authors = new SelectList(_db.Authors, "AuthorId", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Add(Book book)
        {
            if(ModelState.IsValid)
            {
                _db.Books.Add(book);
                _db.SaveChanges();
                TempData["success"] = "Book added successfully!";
                return RedirectToAction("List");
            }

            ViewBag.Categories = new SelectList(_db.Categories, "CategoryId", "Name");
            ViewBag.Warehouses = new SelectList(_db.Warehouses, "WarehouseId", "Name");
            ViewBag.Authors = new SelectList(_db.Authors, "AuthorId", "Name");

            TempData["error"] = "Failed to add book!";
            return RedirectToAction("List");
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
                .Include(au => au.Author)
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
        public IActionResult ListBook()
        {
            var books = _db.Books.Include(ct=>ct.Categories)
                .Include(au=>au.Author)
                .ToList();
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

        //Category Section
        //Add function
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Add(category);
                _db.SaveChanges();
                TempData["success"] = "Category added successfully!";
                return RedirectToAction("List");
            }
            TempData["error"] = "Failed to add category!";
            return RedirectToAction("ListCategory");
        }

        //Edit method
        [HttpGet]
        public IActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "Category not found!";
                return RedirectToAction("ListCategory");
            }
            var category = _db.Categories.FirstOrDefault(y => y.CategoryId == id);

            return View(category);
        }

        public IActionResult EditCategory(Category category)
        {
            if(ModelState.IsValid)
            {
                _db.Categories.Update(category);
                _db.SaveChanges();
                TempData["success"] = "Category updated successfully!";
                return RedirectToAction("ListCategory");
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult ListCategory()
        {
            var categories = _db.Categories.ToList();
            return View(categories);
        }

        //Delete method
        [HttpPost]
        public IActionResult DeleteCategory(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "Category not found!";
                return RedirectToAction("ListCategory");
            }
            var category = _db.Categories.FirstOrDefault(y => y.CategoryId == id);
            _db.Categories.Remove(category);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully!";
            return RedirectToAction("ListCategory");
        }
    }
}
