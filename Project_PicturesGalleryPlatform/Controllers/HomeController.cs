using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Project_PicturesGalleryPlatform.Models;
using Project_PicturesGalleryPlatform.Services.ImageService;
using Project_PicturesGalleryPlatform.Repositories.IRatingService;

namespace Project_PicturesGalleryPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IImageService _imageService;
        private readonly IRatingService _ratingService;

        public HomeController(ILogger<HomeController> logger, IImageService imageService, IRatingService ratingService)
        {
            _logger = logger;
            _imageService = imageService;
            _ratingService = ratingService;
        }


        public IActionResult Index()
        {
            var user = HttpContext.Request.Query["user"].ToString();
            if (!string.IsNullOrEmpty(user))
            {
                HttpContext.Session.SetString("UserId", user);
            }
            // 載入資料庫評分表單
            var totalScores = _ratingService.GetUserTotalScore();
            ViewData["TotalScores"] = totalScores;
            return View();
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
