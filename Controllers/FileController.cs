using Microsoft.AspNetCore.Mvc;
using FileInfo = PullAt.Models.FileInfo;
namespace PullAt.Controllers{
    public class FileController : Controller
    {
        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","uploads");
        public ActionResult Upload(){
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(folderPath, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                ViewBag.Message = "File uploaded successfully!";
            }
            else
            {
                ViewBag.Message = "Please select a file to upload.";
            }
            return RedirectToAction("Files");
        }

        public IActionResult Files()
        {
            var files = new List<FileInfo>();
            string filename;
            if (Directory.Exists(folderPath))
            {
                var fileEntries = Directory.GetFiles(folderPath);
                foreach (var filePath in fileEntries)
                {
                    filename = Path.GetFileName(filePath);
                    files.Add(new FileInfo
                    {
                        Filename = filename,
                        FilePath = "/uploads/"+filename // URL for accessing the file
                    });
                }
            }

            return View(files);
        }
    }
}