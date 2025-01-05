using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_PicturesGalleryPlatform.Models;
using Project_PicturesGalleryPlatform.Services.ImageService;
using Project_PicturesGalleryPlatform.Services.MyFavoritesService;
using System.Drawing.Printing;

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
        private List<ImageDetails> UpdateFavoritedStatusForImages(List<ImageDetails> images)
        {
            String? userAccount = Request.Cookies["UserAccount"];
            if (string.IsNullOrEmpty(userAccount))
                return images;
            return _myFavoritesService.UpdateFavoritedStatusForImages(images, userAccount);
        }
        [HttpGet("SearchImagesByKeyword")]
        public IActionResult SearchImagesByKeyword([FromQuery] String keyword, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var images = _imageService.SearchImagesByKeyword(keyword, page, pageSize);
            return Ok(UpdateFavoritedStatusForImages(images));
        }
        [HttpGet("GetImagesByTag")]

        public IActionResult GetImagesByTag([FromQuery] String tag, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var images = _imageService.GetImagesByTag(tag, page, pageSize);
            return Ok(UpdateFavoritedStatusForImages(images));
        }
        [HttpGet("GetImagesByUserAccount")]
        public IActionResult GetImagesByUserAccount([FromQuery] String userAccount, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var images = _myFavoritesService.GetImagesByUserAccount(userAccount, page, pageSize);
            return Ok(UpdateFavoritedStatusForImages(images));
        }
    }
}
