using Blog_API.Utilities;
using Blog_Client.Models.DTO;
using Blog_Client.Service.IService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Blog_Client.Controllers
{

    public class AuthenController : Controller
    {
        private readonly IAuthService _authService;
        public AuthenController(IAuthService authService)
        {
            _authService = authService;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (registerDTO == null)
            {
                ViewBag.message = "Xin nhap vao thong tin";
                return View();
            }
            else if (registerDTO.DateOfBirth > DateTime.UtcNow)
            {
                ViewBag.message = "Ngay sinh khong the lon hon ngay hien tai";
                return View();
            }
            var result = await _authService.Register(registerDTO);
            if (result.success == false)
            {
                ViewBag.message = result.errorList.FirstOrDefault();
                return View();
            }
            ViewBag.message = "Dang ky thanh cong";
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var result = await _authService.Login(username, password);
            if (result.success == true && result != null)
            {
                var stringUSer = result.data.ToString();
                var userIdentity = JsonConvert.DeserializeObject<LoginResponseDTO>(stringUSer);

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(userIdentity.token);
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == "unique_name").Value));
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, jwt.Claims.FirstOrDefault(u => u.Type == "nameid").Value));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);


                HttpContext.Session.SetString(SD.SessionToken, userIdentity.token);
                return RedirectToAction("Index", "Home");

            }
            
                ViewBag.message = result.errorList.FirstOrDefault();
                return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Remove(SD.SessionToken);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
