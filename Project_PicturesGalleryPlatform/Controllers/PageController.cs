using Microsoft.AspNetCore.Mvc;
using Project_PicturesGalleryPlatform.Models.AIPicturesModels;
using Project_PicturesGalleryPlatform.Services.ImageAnalysisService;
using Project_PicturesGalleryPlatform.Services.ImageService;
using System.Diagnostics;
using System.Text.Json;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Project_PicturesGalleryPlatform.Controllers
{
    public class PageController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IImageService _imageService;
        private readonly IImageAnalysisService _imageAnalysisService;

        public PageController(ILogger<HomeController> logger, IImageService imageService, IImageAnalysisService imageAnalysisService)
        {
            _logger = logger;
            _imageService = imageService;
            _imageAnalysisService = imageAnalysisService;
        }

        //點擊單照片
        public IActionResult PictureInfo(int id)
        {
            var pictures = _imageService.GetImagesByAccountId(id);
            ViewData["picture"] = pictures;
            return View();
        }

        //搜尋類別
        public IActionResult SearchTag(String tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                return View("Index", _imageService.GetRandomImages());
            }
            ViewData["tag"] = tag;
            var images = _imageService.GetAccountsByTag(tag);
            return View("../Page/Pagination");
        }
        [HttpPost]
        public IActionResult GetImagesByFile(IFormFile uploadfile)
        {
            var images =_imageAnalysisService.FindSimilarImagesByImage(uploadfile);
            return View("../Page/Pagination");
        }

        public JsonResult GenPic()
        {
            Console.WriteLine("進入GenPic");
            string keyword = TempData["keyword"]+"";
            Console.WriteLine("keyword: " + keyword);
            TempData["keyword"] = keyword;// 刷新TempData存活時間?
            // test
            string currentDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine("當前GenPic()工作目錄: " + currentDirectory);
            PYProcess_AI genPic = new PYProcess_AI(keyword, 1);// *測試 僅生成一張圖
            var picturesData = genPic.Generate();
            // 判斷是否成功生成圖片
            if (picturesData == null)
            {
                return Json(new { status = "fail", message = "生成圖片失敗" });
            }
            else
            {
                return Json(new { status = "success", message = "生成圖片成功", data = picturesData });
            }
        }
    }
}
