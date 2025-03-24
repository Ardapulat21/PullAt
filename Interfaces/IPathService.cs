namespace PullAt.Interfaces {
    public interface IPathService {
        public string GetUserFolderPath { get; }
        public string CreateFilePath(string directoryPath,string fileName,string extension);
    }
}