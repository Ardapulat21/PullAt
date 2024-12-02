using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using FileInfo = PullAt.Models.FileInfo;
namespace PullAt.Controllers{
    public class FileController : Controller
    {
        HttpClient _httpClient;
        private readonly string? _folderPath;
        private readonly string? apiUrl;
        public FileController(HttpClient httpClient,IConfiguration configuration)
        {
            _httpClient = httpClient;
            _folderPath = configuration["ResourceFolder"];
            apiUrl = Path.Combine(configuration["ApiSettings:APIUrl"],"File");
        }
        public ActionResult Upload(){
            return View();
        }
        [Authorize]
        public async Task<ActionResult> Files()
        {
            // var url = Path.Combine(apiUrl,"Fetch");
            
            // var response = await _httpClient.GetAsync(url);
            // if (response.IsSuccessStatusCode)
            // {   
            //     var data = await response.Content.ReadAsStringAsync();
            //     var files = JsonConvert.DeserializeObject<List<FileInfo>>(data);
            //     return View(files);
            // }
            return View(new List<FileInfo>());

            // string filename;
            // var files = new List<FileInfo>();
            // if (Directory.Exists(_folderPath))
            // {
            //     var fileEntries = Directory.GetFiles(_folderPath);
            //     foreach (var filePath in fileEntries)
            //     {
            //         filename = Path.GetFileName(filePath);
            //         if(Path.GetExtension(filePath)!= ".jpg")
            //             continue;   

            //         files.Add(new FileInfo
            //         {
            //             Filename = filename,
            //             FilePath = "/uploads/"+filename // URL for accessing the file
            //         });
            //     }
            // }
            // return View(files);
        }
    }
}