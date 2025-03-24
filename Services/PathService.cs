using PullAt.Interfaces;

namespace PullAt.Services{
    public class PathService : IPathService{
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _usersFolder;
        public PathService(IHttpContextAccessor httpContextAccessor,IWebHostEnvironment env)
        {
            _usersFolder = Path.Combine(env.ContentRootPath,"Users");
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetUserFolderPath 
        {
            get{
                var username = _httpContextAccessor.HttpContext?.User.Identity?.Name;
                return Path.Combine(_usersFolder,username);
            } 
        }
        public string CreateFilePath(string directoryPath,string fileName,string extension){
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

    }
}