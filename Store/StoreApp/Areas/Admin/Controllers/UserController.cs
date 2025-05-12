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
            if (!ModelState.IsValid)
                return View(userDto);
            var result = await _manager.AuthService.CreateUser(userDto);
            return result.Succeeded
             ? RedirectToAction("Index")
             : View(userDto);
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
            if(!ModelState.IsValid)
                return View(userDto);
            await _manager.AuthService.Update(userDto);
            return RedirectToAction("Index");
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

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] string userName)
        {
            try
            {
                var result = await _manager.AuthService.DeleteUser(userName);
                if (result.Succeeded)
                {
                    TempData["success"] = "Kullanıcı başarıyla silindi.";
                }
                else
                {
                    TempData["danger"] = "Kullanıcı silinirken bir hata oluştu.";
                }
            }
            catch (Exception ex)
            {
                TempData["danger"] = ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}