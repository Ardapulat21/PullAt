using PullAt.Interfaces;
using PullAt.Models;
using FileInfo = PullAt.Models.FileInfo;
using System.IO;

namespace PullAt.Services
{
    public class FileService : IFileService
    {
        private readonly string _uploadFolder;
        public FileService(IWebHostEnvironment env)
        {
            _uploadFolder = Path.Combine(env.WebRootPath,"Uploads");
        }
        public Result GetFiles()
        {
            var _files = new List<FileInfo>();
            string filename;
            string extension;
            if (Directory.Exists(_uploadFolder))
            {
                var fileEntries = Directory.GetFiles(_uploadFolder);
                foreach (var filePath in fileEntries)
                {
                    filename = Path.GetFileName(filePath);
                    extension = Path.GetExtension(filePath);
                    if(extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                        continue;   

                    _files.Add(new FileInfo
                    {
                        Filename = filename,
                        FilePath = "/uploads/"+filename,
                        DateTime = File.GetCreationTime(filePath)
                    });
                }
            }
            _files = _files.OrderBy(f => f.DateTime).ToList(); 
            return Result.Success(_files);
        }
        public async Task<Result> UploadFile(IFormFile file)
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
            var filePath = CreateFilePath(_uploadFolder,file.FileName,extension);
            await SaveFile(file,filePath);

            return Result.Success();
        }
        private string CreateFilePath(string directoryPath,string fileName,string extension){
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
    }
}