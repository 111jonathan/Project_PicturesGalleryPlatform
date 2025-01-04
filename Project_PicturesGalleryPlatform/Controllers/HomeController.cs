using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Project_PicturesGalleryPlatform.Models;
using Project_PicturesGalleryPlatform.Services.ImageService;
using Project_PicturesGalleryPlatform.Models.AIPicturesModels;


namespace Project_PicturesGalleryPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IImageService _imageService;

        public HomeController(ILogger<HomeController> logger, IImageService imageService)
        {
            _logger = logger;
            _imageService = imageService;
        }

        public IActionResult Index()
        {
            //var pictures = _imageService.GetImagesByAccountId(1);
            //ViewData["picture"] = pictures;
            //return View("../Page/PictureInfo");

            if (Request.Cookies.ContainsKey("UserAccount"))
            {
                ViewBag.User = Request.Cookies["UserAccount"]; // 從 Cookies 取得使用者名稱
            }
            else
            {
                ViewBag.User = null; // 未登入時，設置為 null
            }

            return View();
        }

        [HttpPost]
        public IActionResult SearchImages(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                ViewData["ErrorMessage"] = "叫块Τ闽龄";
                return View("Index", _imageService.GetRandomImages());
            }

            ViewData["keyword"] = keyword;
            var images = _imageService.SearchImagesByKeyword(keyword);
            return View("../Page/Result");
        }


        [HttpPost]
        public IActionResult AIPictures(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {// 既ぃ钡
                TempData["feedbackMessage"] = "叫块Τ闽龄";
                TempData["action"] = "Index";
                TempData["controller"] = "Home";
                return RedirectToAction("TransitionPage", "Universal");
            }
            Console.WriteLine("钡Μkeyword: {0}", keyword);
            TempData["keyword_AI"] = keyword;
            return View("../Page/Result_AI");
        }


        public JsonResult GetImagesByPageNumber(int page, int pageSize)
        {
            if (page < 0 || pageSize <= 0)
            {
                return Json(new { error = "礚┪–把计" });
            }

            var images = _imageService.GetImagesByPageNumber(page, pageSize);
            return Json(images);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Logout()
        {
            if (Request.Cookies.ContainsKey("UserAccount"))
            {
                Response.Cookies.Delete("UserAccount"); // 刪除 UserAccount 的 Cookie
            }
            return RedirectToAction("Index", "Home"); // 登出後導向首?
        }
    }
}
