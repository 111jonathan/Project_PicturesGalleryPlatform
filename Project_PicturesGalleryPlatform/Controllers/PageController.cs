using Microsoft.AspNetCore.Mvc;
using Project_PicturesGalleryPlatform.Services.ImageAnalysisService;
using Project_PicturesGalleryPlatform.Services.ImageService;
using Project_PicturesGalleryPlatform.Services.MyFavoritesService;
using System.Diagnostics;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Project_PicturesGalleryPlatform.Controllers
{
    public class PageController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IImageService _imageService;
        private readonly IImageAnalysisService _imageAnalysisService;
        private readonly IMyFavoritesService _myFavoritesService;

        public PageController(ILogger<HomeController> logger, IImageService imageService, IImageAnalysisService imageAnalysisService, IMyFavoritesService myFavoritesService)
        {
            _logger = logger;
            _imageService = imageService;
            _imageAnalysisService = imageAnalysisService;
            _myFavoritesService = myFavoritesService;
        }

        //點擊單照片
        public IActionResult PictureInfo(int id)
        {
            var pictures = _imageService.GetImagesById(id);
            ViewData["picture"] = pictures;
            return View();
        }
        public IActionResult Result()
        {
            return View();
        }

        //搜尋類別
        //[HttpPost]
        //public IActionResult GetImagesByFile(IFormFile uploadfile)
        //{
        //    var images =_imageAnalysisService.FindSimilarImagesByImage(uploadfile);
        //    return View("../Page/Pagination");
        //}
        [HttpPost]
        public IActionResult ToggleImageLikeStatus(int id, String category)
        {
            String? userId = HttpContext.Session.GetString("UserId");
            

            if (string.IsNullOrEmpty(userId))
                return Json(new { success = false });

            //if (category.Equals("fas"))
            //    _myFavoritesService.RemoveFavorite(userId, id);
            //else
            //    _myFavoritesService.AddFavorite(userId, id);

            return Json(new { success = true });
        }
    }
}
