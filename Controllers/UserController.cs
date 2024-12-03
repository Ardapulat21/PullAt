using Microsoft.AspNetCore.Authentication;
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
        public async Task<IActionResult> Edit(int id)
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> UserList()
        {
            ViewBag.Message = User.Identity.IsAuthenticated ? "Authenticated" : "Unauthenticated";
            var users = await _userService.GetUsers();
            if(users == null)
                return Unauthorized();

            return View(users);
        }
        public async Task<IActionResult> Register()
        {
            return View();
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }
        public async Task<IActionResult> Delete(int id){
            await _userService.Delete(id);
            return RedirectToAction(nameof(UserList));
        }
        public async Task<IActionResult> Logout(){
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Login");
        }
        #region HTTP VERBS
        [HttpPost]
        public async Task<ActionResult> Register(User user)
        {
            bool condition = await _userService.Add(user);
            if (!condition){
                ModelState.AddModelError("", "Username or Email already exists.");
                return View(user); 
            }
            return RedirectToAction(nameof(UserList));
        }
        [HttpPost]
        public async Task<ActionResult> Edit(User user,int id)
        {
            await _userService.Edit(user,id);
            return RedirectToAction(nameof(UserList));
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
                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync("Cookies", principal);

                return RedirectToAction("Files","File");
            }
            return RedirectToAction(nameof(Login));
        }
        #endregion
    }
}