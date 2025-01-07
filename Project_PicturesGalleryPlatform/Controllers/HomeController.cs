using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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
                ViewBag.User = Request.Cookies["UserAccount"]; // �� Cookies ȡ��ʹ�������Q
            }
            else
            {
                ViewBag.User = null; // δ����r���O�Þ� null
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


        [HttpPost]
        public IActionResult AIPictures(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {// �Ȥ������ŭ�
                TempData["feedbackMessage"] = "�п�J���Ī�����r�C";
                TempData["action"] = "Index";
                TempData["controller"] = "Home";
                return RedirectToAction("TransitionPage", "Universal");
            }
            var temps = (keyword.Trim()).Split(" ");// bug������01/07
            string newKeyword = "";
            foreach (var temp in temps)
            {
                newKeyword += temp;
            }
            Console.WriteLine("Home//AIPictures����&�B�z��keyword: {0}", newKeyword);
            TempData["keyword_AI"] = newKeyword;
            TempData.Keep("keyword_AI");
            return View("~/Views/Page/Result_AI.cshtml");// �ݴ���
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
