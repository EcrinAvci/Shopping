
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Services.Contracts;

namespace StoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IServiceManager _manager;

        public UserController(IServiceManager manager)
        {
            _manager = manager;
        }

        public IActionResult Index()
        {
            var users = _manager.AuthService.GetAllUsers();
            return View(users);
        }

        public IActionResult Create()
        {
            return View(new UserDtoForCreation()
            {
                Roles = new HashSet<string>(_manager
                .AuthService
                .Roles
                .Select(r => r.Name)
                .ToList())
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] UserDtoForCreation userDto)
        {
            var result = await _manager.AuthService.CreateUser(userDto);
            return result.Succeeded
             ? RedirectToAction("Index")
             : View();
        }

        public async Task<IActionResult> Update([FromRoute(Name ="id")]string id)
        {
            var user = await _manager.AuthService.GetOneUserForUpdate(id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromForm] UserDtoForUpdate userDto)
        {
            if(ModelState.IsValid)
            {
            await _manager.AuthService.Update(userDto);
            return RedirectToAction("Index");
            }
            return View();
        }
        
        public async Task<ActionResult> ResetPassword([FromRoute(Name ="id")] string id)
        {
            return View(new ResetPasswordDto()
            {
                UserName= id
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> ResetPassword([FromForm] ResetPasswordDto model)
        {
            var result =await _manager.AuthService.ResetPassword(model);
            return result.Succeeded
                ? RedirectToAction("Index")
                : View();
        }
    }
}