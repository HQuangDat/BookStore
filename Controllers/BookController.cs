using BookStore.Data;
using BookStore.DataModels;
using BookStore.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        //Add function
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            var categories = _bookRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Book book)
        {
            if (ModelState.IsValid)
            {
                _bookRepository.AddnewBook(book);
                TempData["success"] = "Book added successfully!";
                return RedirectToAction("List");
            }
            var categories = _bookRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");
            TempData["error"] = "Failed to add book!";
            return View(book);
        }




        //Edit method
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                TempData["error"] = "Book not found!";
                return RedirectToAction("List");
            }
            var book = _bookRepository.findById(id);

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                _bookRepository.EditBook(book);
                TempData["success"] = "Book updated successfully!";
                return RedirectToAction("List");
            }
            TempData["error"] = "Failed to update book!";
            return View(book);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult List(string sortOrder, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["SortByNameParam"] = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewData["SortByAuthorParam"] = sortOrder == "Author" ? "Author_desc" : "Author";
            ViewData["SortByPriceParam"] = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewData["SortByProviderParam"] = sortOrder == "Provider" ? "Provider_desc" : "Provider";
            var books = _bookRepository.getAll();
            switch (sortOrder)
            {
                case "Name_desc":
                    books = books.OrderByDescending(b => b.BookName);
                    break;
                case "Author":
                    books = books.OrderBy(b => b.Author);
                    break;
                case "Author_desc":
                    books = books.OrderByDescending(b => b.Author);
                    break;
                case "Price":
                    books = books.OrderBy(b => b.Price);
                    break;
                case "Price_desc":
                    books = books.OrderByDescending(b => b.Price);
                    break;
                case "Provider":
                    books = books.OrderBy(b => b.Provider);
                    break;
                case "Provider_desc":
                    books = books.OrderByDescending(b => b.Provider);
                    break;
                default:
                    books = books.OrderBy(b => b.BookName);
                    break;
            }
            int pageSize = 5; 
            return View(PaginatedList<Book>.Create(books, pageNumber ?? 1, pageSize));
        }

        //For Book details
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id != null)
            {
                var detailBook = _bookRepository.findById(id);
                return View(detailBook);
            }
            TempData["error"] = "Book not found!";
            return RedirectToAction("List");
        }

        //Delete method
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "Book not found!";
                return RedirectToAction("List");
            }
            var book = _bookRepository.findById(id);
            _bookRepository.RemoveBook(book);
            TempData["success"] = "Book deleted successfully!";
            return RedirectToAction("List");
        }

        //Fetch book for Search function 
        [HttpGet]
        public JsonResult fetchBookTitle()
        {
            var books = _bookRepository.getAll()   
                                                .Select(b => new {
                                                    BookId = b.BookId, 
                                                    BookName = b.BookName,
                                                    ImagePath = b.ImagePath}).ToList();

            return Json(books);
        }

        [HttpGet]
        public IActionResult FindByCategory(int id)
        {
            var books = _bookRepository.getAll()
                .Where(b => b.Categories.Any(c => c.CategoryId == id))
                .ToList();

            return View(books);
        }
    }
}
