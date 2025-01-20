using FileInfo = PullAt.Models.FileInfo;
using PullAt.Models;

namespace PullAt.Interfaces
{
    public interface IFileService
    {
        public Task<Result> UploadFileAsync(IFormFile file);
        public Task<Result> DeleteFileAsync(string filename,string? username);
        public Task<object> DownloadFileAsync(string filename,string username);
        public List<FileInfo?> GetFiles(string? username);
    }
}