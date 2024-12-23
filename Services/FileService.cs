using PullAt.Interfaces;
using PullAt.Models;
using FileInfo = PullAt.Models.FileInfo;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace PullAt.Services
{
    public class FileService : IFileService
    {
        private readonly string _usersFolder;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FileService(IWebHostEnvironment env,IHttpContextAccessor httpContextAccessor)
        {
            _usersFolder = Path.Combine(env.ContentRootPath,"Users");
            _httpContextAccessor = httpContextAccessor;
        }
        public List<FileInfo>? GetFiles()
        {
            var files = new List<FileInfo>();
            try
            {
                var username = _httpContextAccessor.HttpContext?.User.Identity?.Name;
                var folderPath = Path.Combine(_usersFolder,username);
                if(!Directory.Exists(folderPath)){
                    return null;
                }
                var filePaths = Directory.GetFiles(folderPath);
                foreach (var file in filePaths){
                    files.Add(new Models.FileInfo(){
                        Filename = Path.GetFileName(file),
                        FilePath = file,
                        DateTime = System.IO.File.GetCreationTime(file)
                    });
                }
                files = files.OrderBy(image => image.DateTime).ToList();
            }
            catch(Exception ex){}
            return files;
        }
        public async Task<Result> UploadFileAsync(IFormFile file)
        {
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

            var filePath = CreateFilePath(_userFolder,file.FileName,extension);
            await SaveFile(file,filePath);

            return Result.Success();
        }
        public static string CreateFilePath(string directoryPath,string fileName,string extension){
            var filePath = Path.Combine(directoryPath,fileName);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            int suffix = 1;
            while (File.Exists(filePath))
            {
                filePath = Path.Combine(directoryPath, $"{fileNameWithoutExtension}_{suffix}{extension}");
                suffix++;
            }
            return filePath;
        }
        private async Task SaveFile(IFormFile file,string filePath){
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }

        public async Task<object> DownloadFileAsync(string filename,string username)
        {
            var folderPath = Path.Combine(_usersFolder,username);
            var imagePath = Path.Combine(folderPath, filename);
            
            if (!System.IO.File.Exists(imagePath))
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
    }
}