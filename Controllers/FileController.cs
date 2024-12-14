using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PullAt.Interfaces;
using FileInfo = PullAt.Models.FileInfo;
namespace PullAt.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        private readonly IFileService _fileService;
        private readonly string _usersFolder;
        public FileController(IFileService fileService,IHostEnvironment env)
        {
            _fileService = fileService;
            _usersFolder = Path.Combine(env.ContentRootPath,"users");
        }
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try{
                var result = await _fileService.UploadFile(file);
                return result.IsSuccess ? RedirectToAction("Files") : BadRequest(result.Message);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("DownloadFile/{filename}")]
        public IActionResult DownloadFile(string filename){
            try{
                var folderPath = Path.Combine(_usersFolder,User.Identity.Name);
                var imagePath = Path.Combine(folderPath, filename);
                
                if (!System.IO.File.Exists(imagePath))
                    return NotFound();

                var contentType = "application/octet-stream";
                var extension = Path.GetExtension(imagePath).ToLower();

                if (extension == ".jpg" || extension == ".jpeg")
                    contentType = "image/jpeg";
                else if (extension == ".png")
                    contentType = "image/png";

                var fileBytes = System.IO.File.ReadAllBytes(imagePath);
                return File(fileBytes, contentType, filename);
            }
            catch (Exception ex){
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult Files()
        {
            var fileInfos = new List<FileInfo?>();
            try{
                if(!User.Identity.IsAuthenticated){
                    return RedirectToAction("Login","User");
                }
                fileInfos = _fileService.GetFiles();
                fileInfos.ForEach(file => file.FilePath = Url.Action("GetImage", "File", new { file.FilePath }));
            }
            catch(Exception ex){}
            return View(fileInfos);
        }
        [HttpGet]
        public IActionResult GetImage(string filePath)
        {
            try{
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                var contentType = "image/jpeg";
                return File(fileBytes, contentType);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }
    }
}