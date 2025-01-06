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
            if (Request.Cookies.ContainsKey("UserAccount"))
            {
                ViewBag.User = Request.Cookies["UserAccount"]; //  Cookies 腕妏蚚氪靡想
            }
            else
            {
                ViewBag.User = null; // 帤腎ㄛ偞离 null
            }
            // 載入資料庫評分表單
            var totalScores = _ratingService.GetUserTotalScore();
            ViewData["TotalScores"] = totalScores;
            return View();
        }

        public IActionResult Logout()
        {
            if (Request.Cookies.ContainsKey("UserAccount"))
            {
                Response.Cookies.Delete("UserAccount"); // 壺 UserAccount 腔 Cookie
            }
            return RedirectToAction("Index", "Home"); // 腎堤摽砃忑?
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
