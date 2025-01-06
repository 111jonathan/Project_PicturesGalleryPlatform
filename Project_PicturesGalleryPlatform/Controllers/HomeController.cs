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

        [HttpPost]
        public IActionResult SearchImages(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                ViewData["ErrorMessage"] = "�п�J���Ī�����r�C";
                return View("Index", _imageService.GetRandomImages());
            }

            ViewData["keyword"] = keyword;
            var images = _imageService.SearchImagesByKeyword(keyword);
            return View("../Page/Result");


        }



        public JsonResult GetImagesByPageNumber(int page, int pageSize)
        {
            if (page < 0 || pageSize <= 0)
            {
                return Json(new { error = "�L�Ī������ΨC���j�p�ѼơC" });
            }

            var images = _imageService.GetImagesByPageNumber(page, pageSize);
            return Json(images);
        }


            //var pictures = _imageService.GetImagesByAccountId(1);
            //ViewData["picture"] = pictures;
            //return View("../Page/PictureInfo");

            if (Request.Cookies.ContainsKey("UserAccount"))
            {
                ViewBag.User = Request.Cookies["UserAccount"]; // �� Cookies ȡ��ʹ�������Q
            }
            else
            {
                ViewBag.User = null; // δ����r���O�Þ� null
            }

            return View();
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
                Response.Cookies.Delete("UserAccount"); // �h�� UserAccount �� Cookie
            }
            return RedirectToAction("Index", "Home"); // �ǳ��ጧ����?
        }

    }
}
