using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace StoreApp.Controllers
{
    public class HomeController:Controller
    {
        private readonly IServiceManager _manager;
        public HomeController(IServiceManager manager)
        {
            _manager = manager;
        }
        public IActionResult Index()
        {
            var products = _manager.ProductService.GetAllProducts(false);
            return View(products);
        }
    }
}