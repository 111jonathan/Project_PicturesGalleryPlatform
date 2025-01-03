using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Project_PicturesGalleryPlatform.Models;
using Project_PicturesGalleryPlatform.Services.ImageService;

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
            var user = HttpContext.Request.Query["user"].ToString();
            if (!string.IsNullOrEmpty(user))
            {
                HttpContext.Session.SetString("UserId", user);
            }
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
