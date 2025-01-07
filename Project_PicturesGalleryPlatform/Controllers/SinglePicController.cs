using Microsoft.AspNetCore.Mvc;
using Project_PicturesGalleryPlatform.Models.AIPicturesModels;
using Project_PicturesGalleryPlatform.Services.ImageService;
using Project_PicturesGalleryPlatform.Models;

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
            var pictures = _imageService.GetImagesById(id);
            ViewData["picture"] = pictures;
            return View();
        }
        //public IActionResult SinglePic()
        //{
        //    var pictures = _imageService.GetImagesByAccountId(1);
        //    ViewData["picture"] = pictures;
        //    return View();
        //}

        public IActionResult SinglePic_AI(AIPicData aiPicData, string keyword)
        {
            Console.WriteLine("Single//Single_AI 接收參數: {0}&{1}", aiPicData, keyword);
            // 處理數據
            DataTransformer dataTransformer = new DataTransformer(aiPicData);
            ImageDetails imageDetail = dataTransformer.Transform(keyword);
            //ViewData["imageDetail"] = imageDetail;
            return View(imageDetail);
        }
    }
}

