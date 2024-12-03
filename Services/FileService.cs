using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PullAt.Interfaces;
using PullAt.Models;
using FileInfo = PullAt.Models.FileInfo;

namespace PullAt.Services
{
    public class FileService : IFileService
    {
        private readonly string? _uploadFolder;
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
                        FilePath = "/uploads/"+filename
                    });
                }
            }
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
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
            var fileName = String.Concat(fileNameWithoutExtension,extension);
            var filePath = Path.Combine(_uploadFolder,fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Result.Success();
        }
    }
}