using PullAt.Interfaces;
using PullAt.Models;
using FileInfo = PullAt.Models.FileInfo;

namespace PullAt.Services
{
    public class FileService : IFileService
    {
        private readonly string _usersFolder;
        private string _userFolder;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FileService(IWebHostEnvironment env,IHttpContextAccessor httpContextAccessor)
        {
            _usersFolder = Path.Combine(env.ContentRootPath,"Users");
            _httpContextAccessor = httpContextAccessor;
        }
        public List<FileInfo>? GetFiles(string username)
        {
            var files = new List<FileInfo>();
            try
            {
                var _userFolder = Path.Combine(_usersFolder,username);
                if(!Directory.Exists(_userFolder)){
                    return null;
                }
                var filePaths = Directory.GetFiles(_userFolder);
                foreach (var file in filePaths){
                    files.Add(new Models.FileInfo(){
                        Filename = Path.GetFileName(file),
                        FilePath = file,
                        DateTime = System.IO.File.GetCreationTime(file)
                    });
                }
                files = files.OrderBy(image => image.DateTime).ToList();
            }
            catch{}
            return files;
        }
        public async Task<Result> UploadFileAsync(IFormFile file)
        {
            try{
                if (file == null || file.Length == 0)
                {
                    return Result.Failure("No file is selected.");
                }
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                {
                    return Result.Failure("The uploaded file must be jpg or png.");
                }
                var username = _httpContextAccessor.HttpContext?.User.Identity?.Name;
                var _userFolder = Path.Combine(_usersFolder,username);

                var filePath = PathService.CreateFilePath(_userFolder,file.FileName,extension);
                await SaveFileAsync(file,filePath);

                return Result.Success();
            }
            catch (Exception ex){
                return Result.Failure(ex.Message);
            }
        }
        private async Task SaveFileAsync(IFormFile file,string filePath){
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
        public Task<Result> DeleteFileAsync(string filename,string username){
            try{
                _userFolder = Path.Combine(_usersFolder,username);
                var imagePath = Path.Combine(_userFolder, filename);
                if (!File.Exists(imagePath))
                    return null;
                
                File.Delete(imagePath);
                return Task.FromResult(Result.Success());
            }
            catch(Exception ex){
                return Task.FromResult(Result.Failure(ex));
            }
        }
        public async Task<object> DownloadFileAsync(string filename,string username)
        {
            try{
                _userFolder = Path.Combine(_usersFolder,username);
                var imagePath = Path.Combine(_userFolder, filename);
                
                if (!File.Exists(imagePath))
                    return null;

                var extension = Path.GetExtension(imagePath).ToLower();
                string contentType = extension switch 
                {
                    ".jpg" => "image/jpeg",
                    ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    _ => "application/octet-stream"
                };
                var fileBytes = System.IO.File.ReadAllBytes(imagePath);
                return new { fileBytes, contentType };
            }
            catch (Exception ex){
                return Result.Failure(ex);
            }
        }
    }
}