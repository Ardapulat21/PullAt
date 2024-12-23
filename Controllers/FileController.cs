using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PullAt.Interfaces;
using FileInfo = PullAt.Models.FileInfo;
namespace PullAt.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class FileController : Controller
    {
        private readonly IFileService _fileService;
        private readonly string _usersFolder;
        public FileController(IFileService fileService,IHostEnvironment env)
        {
            _fileService = fileService;
            _usersFolder = Path.Combine(env.ContentRootPath,"users");
        }
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try{
                var result = await _fileService.UploadFileAsync(file);
                return result.IsSuccess ? RedirectToAction("Files") : BadRequest(result.Message);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("DownloadFile/{filename}")]
        public async Task<IActionResult> DownloadFile(string filename){
            try{
                var data = await (dynamic)_fileService.DownloadFileAsync(filename,User.Identity.Name);
                return File(data.fileBytes, data.contentType, filename);
            }
            catch (Exception ex){
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("Files")]
        public async Task<IActionResult> Files()
        {
            if(!User.Identity.IsAuthenticated){
                return RedirectToAction("Login","User");
            }
            var fileInfos = FetchFiles();
            return View(fileInfos);
        }
        [HttpGet("GetFiles")]
        public IActionResult GetFiles()
        {
            var fileInfos = FetchFiles();
            return Json(fileInfos);
        }
        [HttpGet("GetImage")]
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
        public List<FileInfo?> FetchFiles(){
            var fileInfos = new List<FileInfo?>();
            fileInfos = _fileService.GetFiles();
            fileInfos.ForEach(file => file.FilePath = Url.Action("GetImage", "File", new { file.FilePath }));
            return fileInfos;
        }
    }
}