using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PullAt.Interfaces;
using PullAt.Services;

namespace PullAt.Controllers {
    [Authorize]
    public class AccountController : Controller 
    {
        private IPathService _pathService;
        private IFileService _fileService;
        public AccountController(IFileService fileService, IPathService pathService) 
        {
            _fileService = fileService;
            _pathService = pathService;
        }
        public async Task<IActionResult> Account(){
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangeProfilePhoto(IFormFile image){
            var result = await _fileService.UploadFileAsync(image,_pathService.GetUserFolderPath);
            return result.IsSuccess ? Ok("File uploaded successfully") : BadRequest(result.Message);
        }
    }
}
