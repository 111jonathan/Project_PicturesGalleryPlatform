using Microsoft.AspNetCore.Mvc;
using Project_PicturesGalleryPlatform.Services.ImageService;

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
            var pictures = _imageService.GetImagesByAccountId(id);
            ViewData["picture"] = pictures;
            return View();
        }
        //public IActionResult SinglePic()
        //{
        //    var pictures = _imageService.GetImagesByAccountId(1);
        //    ViewData["picture"] = pictures;
        //    return View();
        //}
    }
}

