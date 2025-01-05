using Microsoft.AspNetCore.Mvc;
using Project_PicturesGalleryPlatform.Models;
using Project_PicturesGalleryPlatform.Repositories.ImageRepository;
using Project_PicturesGalleryPlatform.Repositories.MyFavoritesRepository;
using Project_PicturesGalleryPlatform.Services.ImageService;

namespace Project_PicturesGalleryPlatform.Services.MyFavoritesService
{

    public class MyFavoritesService : IMyFavoritesService
    {
        private readonly IMyFavoritesRepository _myFavoritesRepository;
        private readonly IImageService _imageService;
        public MyFavoritesService(IMyFavoritesRepository myFavoritesRepository, IImageService imageService)
        {
            _myFavoritesRepository = myFavoritesRepository;
            _imageService = imageService;
        }
        public void AddFavorite(string userAccount, int pictureId)
        {
            _myFavoritesRepository.AddFavorite(userAccount, pictureId);
        }
        public void RemoveFavorite(string userAccount, int pictureId)
        {
            _myFavoritesRepository.RemoveFavorite(userAccount, pictureId);
        }
        public List<ImageDetails> UpdateFavoritedStatusForImages(List<ImageDetails> images, string userAccount)
        {
            foreach (var image in images)
                image.isFavorited = _myFavoritesRepository.IsPictureInFavorites(userAccount, image.id);
            return images;
        }
        public List<ImageDetails> GetImagesByUserAccount(string userAccount, int page, int pagesize)
        {
            var ids = _myFavoritesRepository.GetUserFavoritePictureIds(userAccount);
            return _imageService.GetImagesByIds(ids);
        }
    }
}
