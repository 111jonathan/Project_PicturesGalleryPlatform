using Microsoft.AspNetCore.Mvc;
using Project_PicturesGalleryPlatform.Models;
using Project_PicturesGalleryPlatform.Services.ImageService;
using Project_PicturesGalleryPlatform.Services.MyFavoritesService;

namespace Project_PicturesGalleryPlatform.Controllers
{
    [Route("api/Images")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly IMyFavoritesService _myFavoritesService;

        public ImagesController(IImageService imageService, IMyFavoritesService myFavoritesService)
        {
            _imageService = imageService;
            _myFavoritesService = myFavoritesService;
        }

        // 更新圖片的收藏狀態
        private List<ImageDetails> UpdateFavoritedStatusForImages(List<ImageDetails> images)
        {
            var userAccount = Request.Cookies["UserAccount"];
            if (string.IsNullOrEmpty(userAccount)) return images;
            return _myFavoritesService.UpdateFavoritedStatusForImages(images, userAccount);
        }

        // 依關鍵字搜尋圖片並更新收藏狀態
        [HttpGet("Search")]
        public IActionResult SearchImagesByKeyword([FromQuery] string keyword, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var images = _imageService.SearchImagesByKeyword(keyword, page, pageSize);
            return Ok(UpdateFavoritedStatusForImages(images));
        }

        // 依標籤搜尋圖片並更新收藏狀態
        [HttpGet("SearchByTag")]
        public IActionResult GetImagesByTag([FromQuery] string tag, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var images = _imageService.GetImagesByTag(tag, page, pageSize);
            return Ok(UpdateFavoritedStatusForImages(images));
        }

        // 根據用戶帳號獲取圖片並更新收藏狀態
        [HttpGet("Favorites")]
        public IActionResult GetImagesByUserAccount([FromQuery] string userAccount, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var images = _myFavoritesService.GetImagesByUserAccount(userAccount, page, pageSize);
            return Ok(UpdateFavoritedStatusForImages(images));
        }
    }
}
