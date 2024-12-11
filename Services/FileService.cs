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
        public FileService(IWebHostEnvironment env)
        {
            _usersFolder = Path.Combine(env.ContentRootPath,"Users");
        }
        // LIST -> IEnumerable yapilabilir.
        public Result GetFiles()
        {
            var _files = new List<FileInfo>();
            string filename;
            string extension;
            try
            {
                var _userFolder = Path.Combine(_usersFolder,Status.User.Username);
                var fileEntries = Directory.GetFiles(_userFolder);
                foreach (var filePath in fileEntries)
                {
                    filename = Path.GetFileName(filePath);
                    extension = Path.GetExtension(filePath);
                    if(extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                        continue;   

                    _files.Add(new FileInfo
                    {
                        Filename = filename,
                        FilePath = Path.Combine("Users",Status.User.Username,filename),
                        DateTime = File.GetCreationTime(filePath)
                    });
                }
                return Result.Success(_files.OrderBy(f => f.DateTime).ToList());
            }
            catch(Exception ex)
            {
                return Result.Failure(_files);
            }
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
            var _userFolder = Path.Combine(_usersFolder,Status.User.Username);

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
    }
}