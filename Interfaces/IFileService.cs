using FileInfo = PullAt.Models.FileInfo;
using PullAt.Models;

namespace PullAt.Interfaces
{
    public interface IFileService
    {
        public Task<Result> UploadFile(IFormFile file);
        public List<FileInfo> GetFiles();
    }
}