using Project_PicturesGalleryPlatform.Models;
using Project_PicturesGalleryPlatform.Repositories.ImageRepository;
using System.Drawing.Printing;


namespace Project_PicturesGalleryPlatform.Services.ImageService
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;

        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        public List<ImageDetails> GetRandomImages()
        {
            var images = _imageRepository.GetRandomImages();
            return images;
        }
        public List<ImageDetails> SearchImagesByKeyword(string keyword, int page, int pageSize)
        {
            var images = _imageRepository.SearchImagesByKeyword(keyword);
            return images?.Skip(page * pageSize).Take(pageSize).ToList() ?? new List<ImageDetails>();
        }
        public List<ImageDetails> GetImagesByAccountId(int id)
        {
            var images = _imageRepository.GetImagesByAccountId(id);
            return images;
        }
        public List<ImageDetails> GetImagesByIds(List<int> ids)
        {
            var images = _imageRepository.GetImagesByIds(ids);
            return images;
        }
        public List<ImageDetails> GetImagesByTag(string tag, int page, int pageSize)
        {
            var images = _imageRepository.GetImagesByTag(tag);
            return images?.Skip(page * pageSize).Take(pageSize).ToList() ?? new List<ImageDetails>();
        }
    }
}
