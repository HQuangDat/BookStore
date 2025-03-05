using BookStore.Data;
using BookStore.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Add(category);
                _db.SaveChanges();
                TempData["success"] = "Category added successfully!";
                return RedirectToAction("List");
            }
            TempData["error"] = "Failed to add category!";
            return RedirectToAction("List");
        }

        //Edit method
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "Category not found!";
                return RedirectToAction("List");
            }
            var category = _db.Categories.FirstOrDefault(y => y.CategoryId == id);

            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(category);
                _db.SaveChanges();
                TempData["success"] = "Category updated successfully!";
                return RedirectToAction("List");
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult List()
        {
            var categories = _db.Categories.ToList();
            return View(categories);
        }

        //Delete method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "Category not found!";
                return RedirectToAction("List");
            }
            var category = _db.Categories.FirstOrDefault(y => y.CategoryId == id);
            _db.Categories.Remove(category);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully!";
            return RedirectToAction("List");
        }
    }
}
