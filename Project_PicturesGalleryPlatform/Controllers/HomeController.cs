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
