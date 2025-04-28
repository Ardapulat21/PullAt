namespace PullAt.Interfaces {
    public interface IPathService {
        public string GetUsersFolderPath { get; }
        public string GetUserFolderPath { get; }
        public string GetProfilePhotoPath { get;}
        public string GetRootPath { get; }
        public string CreateFilePath(string directoryPath,string fileName);
    }
}