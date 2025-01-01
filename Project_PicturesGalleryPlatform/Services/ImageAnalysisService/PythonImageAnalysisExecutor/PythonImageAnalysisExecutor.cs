using System.Diagnostics;

namespace Project_PicturesGalleryPlatform.Services.ImageAnalysisService.PythonImageAnalysisExecutor
{
    public class PythonImageAnalysisExecutor : IPythonImageAnalysisExecutor
    {
        private List<int> ExecutePythonScript(String pythonPath, String args)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = pythonPath,
                Arguments = args,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process proc = Process.Start(startInfo))
            {
                using (System.IO.StreamReader reader = proc.StandardOutput)
                {
                    String result = reader.ReadToEnd().Split("\r")[0];
                    return result.Split(' ').Select(int.Parse).ToList();
                }
            }
        }
        public List<int> FindSimilarImageIds(IFormFile file)
        {
            var rootPath = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(rootPath, "wwwroot", "uploads", file.FileName);
            var pythonExePath = Path.Combine(rootPath, "wwwroot", "exe", "image_similarity_search.exe");
            var testImagesPath = Path.Combine(rootPath, "wwwroot", "testImages");
            var featureCachePath = Path.Combine(rootPath, "wwwroot", "features_cache");
 
            String arguments = $" {filePath} {testImagesPath} {featureCachePath}";
            return ExecutePythonScript(pythonExePath, arguments);
        }

        //public List<int> FindSimilarTextIds(String query)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
