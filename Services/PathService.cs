namespace PullAt.Services{
    public class PathService {
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
    }
}