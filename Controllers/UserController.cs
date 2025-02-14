using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PullAt.Interfaces;
using PullAt.Models;
using System.Net.Http.Headers;
using System.Security.Claims;
namespace PullAt.Controllers
{
    public class UserController : Controller 
    {
        IUserService _userService;
        HttpClient _httpClient;
        public UserController(IUserService userService,HttpClient httpClient) 
        {
            _userService = userService;
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Register()
        {
            return View();
        }
        public async Task<IActionResult> Login()
        {
            if(User.Identity.IsAuthenticated){
                return RedirectToAction("Index","Home");
            }
            return View();
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
            var token = await _userService.Login(user); 
            if(token != null){
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
                    
                HttpContext.Session.SetString("Token", token);//For HTTP Header
                Response.Cookies.Append("AuthToken", token);//For storing JWT Token
                return Redirect("/File/Files");
            }
            return RedirectToAction(nameof(Login));
        }
        #endregion
    }
}