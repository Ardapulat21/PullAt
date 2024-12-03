using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PullAt.Interfaces;
namespace PullAt.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        private readonly IFileService _fileService;
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var result = await _fileService.UploadFile(file);
            return result.IsSuccess ? RedirectToAction("Files") : BadRequest(result.Message);
        }
        [HttpGet]
        public ActionResult Files(){
            var _files = _fileService.GetFiles().Data;
            return View(_files);
        }
    }
}