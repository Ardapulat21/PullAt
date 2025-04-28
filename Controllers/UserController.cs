using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PullAt.Interfaces;
using PullAt.Models;
using System.Security.Claims;
namespace PullAt.Controllers
{
    public class UserController : Controller 
    {
        IUserService _userService;
        public UserController(IUserService userService) 
        {
            _userService = userService;
        }
        public async Task<IActionResult> Register()
        {
            return View();
        }
        public async Task<IActionResult> Login()
        {
            return User.Identity.IsAuthenticated ? RedirectToAction("Index","Home") : View();
        }
        public async Task<IActionResult> Logout(){
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete("AuthToken");
            return RedirectToAction("Login");
        }
        #region HTTP VERBS
        [HttpPost]
        public async Task<ActionResult> Register(User user)
        {
            bool isValid = await _userService.Register(user);
            if (!isValid){
                ModelState.AddModelError("", "Username or Email already exists.");
                return View(user); 
            }
            return RedirectToAction("Files","File");
        }
        [HttpPost]
        public async Task<IActionResult> Login(User user)   
        {
            var credentials = await _userService.Login(user); 
            if(credentials != null){
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, credentials.Username),
                    new Claim(ClaimTypes.Name, credentials.Email)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
                    
                HttpContext.Session.SetString("Token", credentials.Token);//For HTTP Header
                Response.Cookies.Append("AuthToken", credentials.Token);//For storing JWT Token
                return RedirectToAction("Index","Home");
            }
            return RedirectToAction(nameof(Login));
        }
        #endregion
    }
}