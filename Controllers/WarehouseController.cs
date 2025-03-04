using BookStore.Data;
using BookStore.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class WarehouseController : Controller
    {
        private readonly ApplicationDbContext _db;
        public WarehouseController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult AddWarehouse()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddWarehouse(Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                _db.Warehouses.Add(warehouse);
                _db.SaveChanges();
                TempData["success"] = "Warehouse added successfully!";
                return RedirectToAction("ListWarehouses");
            }
            TempData["error"] = "Failed to add warehouse!";
            return RedirectToAction("ListWarehouses");
        }

        //Edit method
        [HttpGet]
        public IActionResult EditWarehouse(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "Warehouse not found!";
                return RedirectToAction("ListWarehouses");
            }
            var warehouse = _db.Warehouses.Find(id);
            return View(warehouse);
        }

        [HttpPost]
        public IActionResult EditWarehouse(Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                _db.Warehouses.Update(warehouse);
                _db.SaveChanges();
                TempData["success"] = "Warehouse updated successfully!";
                return RedirectToAction("ListWarehouses");
            }
            TempData["error"] = "Failed to update warehouse!";
            return RedirectToAction("ListWarehouses");
        }

        //List method
        [HttpGet]
        public IActionResult ListWarehouses()
        {
            var warehouses = _db.Warehouses.ToList();
            return View(warehouses);
        }
    }
}
