using Project_PicturesGalleryPlatform.Repositories.ImageRepository;
using System.Diagnostics;
using System.Xml.Linq;

namespace Project_PicturesGalleryPlatform.Services.ImageAnalysisService.PythonImageAnalysisExecutor
{
    public class PythonImageAnalysisExecutor : IPythonImageAnalysisExecutor
    {
        private List<int> ExecutePythonScript(String exeName, String arg)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = exeName,
                Arguments = arg,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startInfo))
            {
                using (System.IO.StreamReader reader = process.StandardOutput)
                {
                    String output = reader.ReadToEnd().Split("\r")[0];
                    return output.Split(' ').Select(int.Parse).ToList();
                }
            }
        }
        public List<int> FindSimilarImageIds(IFormFile file)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", file.FileName);
            return ExecutePythonScript("  ", filePath);
        }

        public List<int> FindSimilarTextIds(String text)
        {
            throw new NotImplementedException();
        }
    }
}
