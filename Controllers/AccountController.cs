using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PullAt.Interfaces;
using PullAt.Services;

namespace PullAt.Controllers 
{
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
            var absolutePath = _pathService.GetProfilePhotoPath;
            
            var relativePath = String.IsNullOrEmpty(absolutePath) ?
                                "/Assets/Profile_photo.jpg" : 
                                absolutePath.Substring(absolutePath.LastIndexOf("/Users/"));
                                
            ViewBag.RelativePath = relativePath;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangeProfilePhoto([FromForm(Name ="file")] IFormFile image){
            try{
                var profilePhotoPath = _pathService.GetProfilePhotoPath;
                if(!String.IsNullOrEmpty(profilePhotoPath)){
                    await _fileService.DeleteFileAsync(profilePhotoPath);
                }
                
                var extension = Path.GetExtension(image.FileName);
                var path = Path.Join(_pathService.GetUserFolderPath,"Profile_photo" + extension);
                var result = await _fileService.UploadFileAsync(image,path);

                var absolutePath = _pathService.GetProfilePhotoPath;
            
                var relativePath = String.IsNullOrEmpty(absolutePath) ?
                                    "/Assets/Profile_photo.jpg" : 
                                    absolutePath.Substring(absolutePath.LastIndexOf("/Users/"));
                return Json(new { success = true, relativePath});
            }
            catch{}
            return Json(new { success = false});
        }
    }
}
