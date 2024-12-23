using FileInfo = PullAt.Models.FileInfo;
using PullAt.Models;

namespace PullAt.Interfaces
{
    public interface IFileService
    {
        public Task<Result> UploadFileAsync(IFormFile file);
        public Task<object> DownloadFileAsync(string filename,string username);
        public List<FileInfo> GetFiles();
    }
}