using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PullAt.Interfaces;
using PullAt.Models;
namespace PullAt.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        private readonly IFileService _fileService;
        private readonly string _contentRootPath;
        public FileController(IFileService fileService,IHostEnvironment env)
        {
            _fileService = fileService;
            _contentRootPath = env.ContentRootPath;
        }
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var result = await _fileService.UploadFile(file);
            return result.IsSuccess ? RedirectToAction("Files") : BadRequest(result.Message);
        }
        [HttpGet]
        public IActionResult DownloadFile(string filename){
            var currentDirectory = Directory.GetCurrentDirectory();
            var imagePath = Path.Combine(currentDirectory,"wwwroot","Users",Status.User.Username,filename);
           
            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }
            var contentType = "application/octet-stream";
            var extension = Path.GetExtension(imagePath).ToLowerInvariant();

            if (extension == ".jpg" || extension == ".jpeg")
                contentType = "image/jpeg";
            else if (extension == ".png")
                contentType = "image/png";

            var fileBytes = System.IO.File.ReadAllBytes(imagePath);
            return File(fileBytes, contentType, filename);
        }
        public IActionResult Files()
        {
            var imageUrls = new List<string?>();
            try{
                if(Status.User.Username == null)
                    return RedirectToAction("Login","User");
                
                var folderPath = Path.Combine(_contentRootPath, "users",Status.User.Username);
                if(Directory.Exists(folderPath)){
                    var files = Directory.GetFiles(folderPath);
                    imageUrls = files.Select(filePath => Url.Action("GetImage", "File", new { filePath })).ToList();
                }
            }
            catch(Exception ex){}
            return View(imageUrls);
        }
        public IActionResult GetImage(string filePath)
        {
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var contentType = "image/jpeg";
            return File(fileBytes, contentType);
        }
    }
}