using BookStore.Data;
using BookStore.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

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
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                _db.Warehouses.Add(warehouse);
                _db.SaveChanges();
                TempData["success"] = "Warehouse added successfully!";
                return RedirectToAction("List");
            }
            TempData["error"] = "Failed to add warehouse!";
            return RedirectToAction("List");
        }

        //Edit method
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "Warehouse not found!";
                return RedirectToAction("List");
            }
            var warehouse = _db.Warehouses.Find(id);
            return View(warehouse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                _db.Warehouses.Update(warehouse);
                _db.SaveChanges();
                TempData["success"] = "Warehouse updated successfully!";
                return RedirectToAction("List");
            }
            TempData["error"] = "Failed to update warehouse!";
            return RedirectToAction("List");
        }

        //List method
        [HttpGet]
        public IActionResult List()
        {
            var warehouses = _db.Warehouses.ToList();
            return View(warehouses);
        }

        //Book and warehouse function
        [HttpGet]
        public IActionResult AddToWarehouse()
        {
            var availBooks = _db.Books.ToList();
            var availWarehouses = _db.Warehouses.ToList();  
            ViewBag.AvailableBooks = new SelectList(availBooks, "BookId", "BookName");
            ViewBag.AvailableWarehouses = new SelectList(availWarehouses, "WarehouseId", "location");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToWarehouse(int? bookId, int? warehouseId,int quantity)
        {
            try
            {
                if (bookId != null && warehouseId != null)
                {
                    BookWarehouse bh = new BookWarehouse
                    {
                        BookId = bookId.Value,
                        WarehouseId = warehouseId.Value,
                        Quantity = quantity
                    };
                    _db.BookWarehouses.Add(bh);
                    _db.SaveChanges();
                    TempData["success"] = "Book added to warehouse successfully!";
                    return RedirectToAction("List");
                }
            }
            catch (Exception e)
            {
                TempData["error"] = $"{e.Message}, please check the information";
            }
            var availBooks = _db.Books.ToList();
            var availWarehouses = _db.Warehouses.ToList();
            ViewBag.AvailableBooks = new SelectList(availBooks, "BookId", "BookName");
            ViewBag.AvailableWarehouses = new SelectList(availWarehouses, "WarehouseId", "location");
            return RedirectToAction("AddToWarehouse");
        }

        [HttpGet]
        public IActionResult ListQuantity()
        {
            var warehouses = _db.BookWarehouses
                .Include(bk=>bk.Book)
                .Include(wh=>wh.Warehouse).ToList();
            return View(warehouses);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var delWare = _db.Warehouses.FirstOrDefault(wh=>wh.WarehouseId == id);
            if(delWare!=null)
            {
               _db.Warehouses.Remove(delWare);
               _db.SaveChanges();
               TempData["success"] = "Delete Warehouse successfully";
               return RedirectToAction("List");
            }
            TempData["error"] = "Warehouse doesn't exist!";
            return RedirectToAction("List");
        }
    }
}
