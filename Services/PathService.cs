using PullAt.Interfaces;

namespace PullAt.Services{
    public class PathService : IPathService{
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _usersFolder;
        private readonly IWebHostEnvironment _env;
        public PathService(IHttpContextAccessor httpContextAccessor,IWebHostEnvironment env)
        {
            _httpContextAccessor = httpContextAccessor;
            _env = env;
            _usersFolder = Path.Combine(env.ContentRootPath,"Users");
        }
        public string GetRootPath { get => _env.WebRootPath; }
        public string GetUserFolderPath 
        {
            get{
                var username = _httpContextAccessor.HttpContext?.User.Identity?.Name;
                return Path.Combine(_usersFolder,username);
            } 
        }
        public string GetUsersFolderPath { get => _usersFolder; }
        public string GetProfilePhotoPath { 
            get {
                foreach (var file in Directory.GetFiles(GetUserFolderPath)) {
                    var fileName = Path.GetFileName(file);
                    if(fileName.StartsWith("Profile_photo")){
                        return file;
                    }
                }
                return String.Empty;
            } 
        }
        public string CreateFilePath(string directoryPath,string fileName){
            var filePath = Path.Combine(directoryPath,fileName);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var extension = Path.GetExtension(fileName);
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