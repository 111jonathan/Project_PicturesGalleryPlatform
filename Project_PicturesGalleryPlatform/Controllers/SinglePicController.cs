using Microsoft.AspNetCore.Mvc;
using Project_PicturesGalleryPlatform.Models;
using Project_PicturesGalleryPlatform.Models.FavoriteModel;
using Project_PicturesGalleryPlatform.Services;

namespace Project_PicturesGalleryPlatform.Controllers
{
    public class SinglePicController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IImageService _imageService;

        public SinglePicController(ILogger<HomeController> logger, IImageService imageService)
        {
            _logger = logger;
            _imageService = imageService;
        }

        public IActionResult SinglePic(int id)
        {
            var pictures = _imageService.GetAccountsById(id);
            ViewData["picture"] = pictures;

            // 我的最愛
            // 檢查是否登入
            bool loginCon = Request.Cookies.ContainsKey("UserAccount");
            loginCon = true;//***測試用，之後要刪掉****
            Console.WriteLine("MemberController.Upload() 測試中 loginCon: {0}", loginCon);
            if (loginCon) // 已登入
            {
                FavoriteSetting favoriteSetting = new(Request.Cookies["UserAccount"], id);
                ViewData["favorite"] = favoriteSetting.isFavorite;
                ViewData["favorite"] = true;//***測試用，之後要刪掉****

            }
            else
            {
                ViewData["favorite"] = false;// 預設為未加入我的最愛
            }
            return View();
        }


        public IActionResult FavoriteSetting(int id)
        {
            // 檢查是否登入
            bool loginCon = Request.Cookies.ContainsKey("UserAccount");
            loginCon = true;//***測試用，之後要刪掉****
            Console.WriteLine("MemberController.Upload() 測試中 loginCon: {0}", loginCon);
            if (loginCon) // 已登入
            {
                FavoriteSetting favoriteSetting = new(Request.Cookies["UserAccount"], id);
                favoriteSetting.JudgeState();
                favoriteSetting.AddAndDelete();
            }
            else
            {
                TempData["message"] = "請先登入";
                TempData["action"] = "Login";
                TempData["controller"] = "Login";
                return RedirectToAction("Shared", "TransitionPage");
            }
            return RedirectToAction("SinglePic", new { id = id });
        }


        //public IActionResult SinglePic()
        //{
        //    var pictures = _imageService.GetAccountsById(1);
        //    ViewData["picture"] = pictures;
        //    return View();
        //}
    }
}

