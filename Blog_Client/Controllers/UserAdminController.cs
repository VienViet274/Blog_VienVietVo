using Blog_API.Utilities;
using Blog_Client.Models;
using Blog_Client.Models.DTO;
using Blog_Client.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blog_Client.Controllers
{
    public class UserAdminController : Controller
    {
        private readonly IAuthService _authService;
        public UserAdminController(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _authService.GetAllUser(HttpContext.Session.GetString(SD.SessionToken));
            if (result == null || !result.success || result.data == null)
            {
                ViewBag.Error = "Failed to retrieve users.";
                return View(new List<UserWithRolesDTO>());
            }
            string resultdata = result.data.ToString();
            var listusers = JsonConvert.DeserializeObject<List<UserWithRolesDTO>>(resultdata);
            return View(listusers);
        }
        public async Task<IActionResult> Manage(string id)
        {
            var userOne = await _authService.GetOneUser(id, HttpContext.Session.GetString(SD.SessionToken));
            if (userOne == null || !userOne.success || userOne.data == null)
            {
                ViewBag.Error = "Failed to retrieve user details.";
                return View(new UserWithRolesDTO());
            }
            string resultdata = userOne.data.ToString();
            var user = JsonConvert.DeserializeObject<UserWithRolesDTO>(resultdata);
            //ViewBag.AllRoles = user.Roles;
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Manage(UserWithRolesDTO userWithRolesDTO)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.Error = "Invalid data. Please check the form.";
                return RedirectToAction("Manage");
            }
            var result = await _authService.UpdateUserRoles(userWithRolesDTO, HttpContext.Session.GetString(SD.SessionToken));
            if (result == null || !result.success)
            {
                ViewBag.Error = "Failed to update user roles.";
                return View(userWithRolesDTO);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(string id)
        {
            var userOne = await _authService.GetApplicationUser(id, HttpContext.Session.GetString(SD.SessionToken));
            if (userOne == null || !userOne.success || userOne.data == null)
            {
                ViewBag.Error = "Failed to retrieve user details.";
                return View(new UserWithRolesDTO());
            }
            string resultdata = userOne.data.ToString();
            var user = JsonConvert.DeserializeObject<ApplicationUser>(resultdata);
            return View(user);
        }
    }
}
