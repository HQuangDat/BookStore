using BookStore.Data;
using BookStore.DataModels;
using BookStore.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
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
                _categoryRepository.Add(category);
                _categoryRepository.Save();
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
            var category = _categoryRepository.GetCategoryById(id);

            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.Update(category);
                _categoryRepository.Save();
                TempData["success"] = "Category updated successfully!";
                return RedirectToAction("List");
            }
            return View(category);
        }

        [HttpGet]
        public IActionResult List()
        {
            var categories = _categoryRepository.GetAll();
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
            var category = _categoryRepository.GetCategoryById(id);
            _categoryRepository.Delete(category);
            _categoryRepository.Save();
            TempData["success"] = "Category deleted successfully!";
            return RedirectToAction("List");
        }

        [HttpGet]
        public JsonResult getAllCategoryName()
        {
            var name = _categoryRepository.GetAll().Select(b => new
            {
               categoryId = b.CategoryId,
               categoryName = b.Name
            }).ToList();
            return Json(name);
        }
    }
}
