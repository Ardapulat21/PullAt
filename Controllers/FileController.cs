using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PullAt.Interfaces;
using PullAt.Services;
using FileInfo = PullAt.Models.FileInfo;
namespace PullAt.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class FileController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IPathService _pathService;
        private readonly string _usersFolder;
        public FileController(IFileService fileService,IPathService pathService)
        {
            _fileService = fileService;
            _pathService = pathService;
            _usersFolder = _pathService.GetUserFolderPath;
        }
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try{
                var path = _pathService.CreateFilePath(_pathService.GetUserFolderPath,file.FileName);
                var result = await _fileService.UploadFileAsync(file,path);
                return result.IsSuccess ? Ok("File uploaded successfully") : BadRequest(result.Message);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }
        public List<FileInfo?> FetchFiles(){
            List<FileInfo?> fileInfos = new List<FileInfo?>();
            fileInfos = _fileService.GetFiles(User.Identity?.Name);
            fileInfos.ForEach(file => file.FilePath = Url.Action("GetImage", "File", new { file.FilePath }));
            return fileInfos;
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
        [HttpDelete("DeleteFileAsync/{filename}")]
        public async Task<IActionResult> DeleteFileAsync(string filename){
            try{
                if(String.IsNullOrEmpty(filename))
                    return BadRequest("File name is null");

                var _userFolder = _pathService.GetUserFolderPath;
                var path = Path.Combine(_userFolder, filename);

                var result = await _fileService.DeleteFileAsync(path);
                if(result.IsSuccess){
                    return Ok("Media has been deleted");
                }
                return BadRequest(result.Message);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("DownloadFile/{filename}")]
        public async Task<IActionResult> DownloadFile(string filename){
            try{
                if(String.IsNullOrEmpty(filename))
                    return BadRequest("File name is null");
                var data = await (dynamic)_fileService.DownloadFileAsync(filename,User.Identity.Name);
                return File(data.fileBytes, data.contentType, filename);
            }
            catch (Exception ex){
                return BadRequest(ex.Message);
            }
        }
    }
}