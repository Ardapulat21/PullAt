using PullAt.Models;

namespace PullAt.Interfaces
{
    public interface IFileService
    {
        public Task<Result> UploadFile(IFormFile file);
        public Result GetFiles();
    }
}