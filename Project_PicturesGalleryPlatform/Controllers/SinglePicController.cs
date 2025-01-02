using Microsoft.AspNetCore.Mvc;
using Project_PicturesGalleryPlatform.Services.ImageService;
using Project_PicturesGalleryPlatform.Services.MyFavoritesService;
using System.Security.Claims;

namespace Project_PicturesGalleryPlatform.Controllers
{
    public class SinglePicController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IImageService _imageService;
        private readonly IMyFavoritesService _myFavoritesService;

        public SinglePicController(ILogger<HomeController> logger, IImageService imageService, IMyFavoritesService myFavoritesService)
        {
            _logger = logger;
            _imageService = imageService;
            _myFavoritesService = myFavoritesService;
        }

        public IActionResult SinglePic(int id, string user)
        {

            if (!string.IsNullOrEmpty(user))
            {
                // 當用戶資料不為空，設置 ViewData["user"] 為 true
                ViewData["user"] = true;
            }
            else
            {
                // 如果用戶資料為空，設置 ViewData["user"] 為 false
                ViewData["user"] = false;
            }

            var pictures = _imageService.GetImagesByAccountId(id);
            if (pictures == null)
            {
                return NotFound();
            }
            ViewData["picture"] = pictures;
            return View();
        }

        // 檢查是否登入
        [HttpGet]
        public IActionResult IsUserLoggedIn()
        {
            bool isLoggedIn = User.Identity.IsAuthenticated; // 檢查是否已登入
            return Json(new { isLoggedIn });
        }

        // 提交評分
        [HttpPost]
        public IActionResult SubmitRating([FromBody] SinglePicViewModel model)
        {
            // 檢查用戶是否已登入
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "您必須先登入才能進行評分。" });
            }

            // 驗證評分數據的有效性
            if (model == null || model.ProductId <= 0 || model.Rating < 1 || model.Rating > 5)
            {
                return Json(new { success = false, message = "無效的評分數據。" });
            }

            try
            {
                // 處理評分邏輯
                //bool result = _ratingService.SaveRating(User.Identity.Name, model.ProductId, model.Rating);
                bool result = false;

                if (result)
                {
                    return Json(new { success = true, message = "評分成功！" });
                }
                else
                {
                    return Json(new { success = false, message = "儲存評分失敗，請稍後再試。" });
                }
            }
            catch (Exception ex)
            {
                // 紀錄錯誤
                _logger.LogError(ex, "評分過程中發生錯誤");

                return StatusCode(500, new { success = false, message = "伺服器內部錯誤，請稍後再試。" });
            }
        }


        public class SinglePicViewModel
        {
            public int ProductId { get; set; }
            public int Rating { get; set; }
        }

        // 檢查圖片是否已經被用戶喜愛
        public bool IsImageLiked(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // 用戶ID

            if (string.IsNullOrEmpty(userId))
            {
                return false; // 如果用戶尚未登入，返回 false
            }

            return _myFavoritesService.IsPictureInFavorites(userId, id); // 檢查用戶是否將圖片添加到收藏
        }



        //public IActionResult SinglePic()
        //{
        //    var pictures = _imageService.GetImagesByAccountId(1);
        //    ViewData["picture"] = pictures;
        //    return View();
        //}
    }
}

