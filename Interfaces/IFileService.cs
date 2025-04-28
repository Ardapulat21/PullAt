using FileInfo = PullAt.Models.FileInfo;
using PullAt.Models;

namespace PullAt.Interfaces
{
    public interface IFileService
    {
        public Task<Result> UploadFileAsync(IFormFile file,string path);
        public Task<Result> DeleteFileAsync(string path);
        public Task<object> DownloadFileAsync(string filename,string username);
        public List<FileInfo?> GetFiles(string? username);
    }
}