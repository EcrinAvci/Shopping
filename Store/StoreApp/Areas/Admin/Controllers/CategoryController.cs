using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Entities.Models;

namespace StoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IServiceManager _manager;

        public CategoryController(IServiceManager manager)
        {
            _manager = manager;
        }

        public IActionResult Index()
        {
            var categories = _manager.CategoryService.GetAllCategories(true);
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.CategoryName))
            {
                ModelState.AddModelError("CategoryName", "Kategori adı boş olamaz!");
                return View(category);
            }

            if (ModelState.IsValid)
            {
                _manager.CategoryService.CreateCategory(category);
                return RedirectToAction("Index");
            }
            return View(category);
        }
    }
}