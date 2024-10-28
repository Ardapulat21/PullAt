using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PullAt.Data;
using PullAt.Interfaces;
using PullAt.Models;
using PullAt.Services;

namespace PullAt.Controllers{
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
            // var user = await _userService.GetById(id);
            // return View(user);
            return View();
        }
        public async Task<IActionResult> UserList()
        {
            var users = await _userService.GetUsers();
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
            return RedirectToAction("UserList");
        }
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
            return RedirectToAction("UserList");
        }
        [HttpPost]
        public async Task<ActionResult> Login(User user)
        {
            if(await _userService.Validate(user) == true)
                return RedirectToAction("UserList");

            return RedirectToAction("Login");
        }
    }
}