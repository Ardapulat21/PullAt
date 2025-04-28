using Microsoft.AspNetCore.Mvc;
using PullAt.Interfaces;
using PullAt.Models;
using FileInfo = PullAt.Models.FileInfo;

namespace PullAt.Services
{
    public class FileService : IFileService
    {
        private string _userFolder;
        private readonly IPathService _pathService;
        public FileService(IPathService pathService)
        {
            _pathService = pathService; 
        }
        public List<FileInfo>? GetFiles(string username)
        {
            _userFolder = _pathService.GetUserFolderPath;
            if(!Directory.Exists(_userFolder)){
                return null;
            }
            var files = new List<FileInfo>();
            var filePaths = Directory.GetFiles(_userFolder);
            foreach (var file in filePaths){
                if(Path.GetFileName(file).StartsWith("Profile_photo"))
                    continue;
                    
                files.Add(new FileInfo(){
                    Filename = Path.GetFileName(file),
                    FilePath = file,
                    DateTime = System.IO.File.GetCreationTime(file)
                });
            }
            files = files.OrderBy(image => image.DateTime).ToList();
            return files;
        }
        public async Task<Result> UploadFileAsync(IFormFile file,string path)
        {
            try{
                if (file == null || file.Length == 0)
                {
                    return Result.Failure("No file is selected.");
                }
                await SaveFileAsync(file,path);
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
        public Task<Result> DeleteFileAsync(string path){
            try{
                if (!File.Exists(path))
                    return null;
                
                File.Delete(path);
                return Task.FromResult(Result.Success());
            }
            catch(Exception ex){
                return Task.FromResult(Result.Failure(ex));
            }
        }
        public async Task<object> DownloadFileAsync(string filename,string username)
        {
            try{
                _userFolder = _pathService.GetUserFolderPath;
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