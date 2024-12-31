using Microsoft.AspNetCore.Mvc;
using Project_PicturesGalleryPlatform.Services.ImageAnalysisService;
using Project_PicturesGalleryPlatform.Services.ImageService;
using System.Diagnostics;
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
            var pictures = _imageService.GetAccountsById(id);
            ViewData["picture"] = pictures;
            return View();
        }

        //搜尋類別
        public IActionResult SearchTag(String tag)
        {
            //if (string.IsNullOrWhiteSpace(tag))
            //{
            //    return View("Index", _imageService.GetRandomImages());
            //}
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
    }
}
