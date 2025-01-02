using Project_PicturesGalleryPlatform.Models;
using Project_PicturesGalleryPlatform.Repositories.ImageRepository;


namespace Project_PicturesGalleryPlatform.Services.ImageService
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private static List<ImageDetails> _currentImageResults;

        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        public List<ImageDetails> GetRandomImages()
        {
            _currentImageResults = _imageRepository.GetRandomImages();
            return _currentImageResults;
        }
        public List<ImageDetails> SearchImagesByKeyword(string keyword)
        {
            _currentImageResults = _imageRepository.SearchImagesByKeyword(keyword);
            return _currentImageResults;
        }

        public List<ImageDetails> GetImagesByPageNumber(int page, int pageSize)
        {
            return _currentImageResults?.Skip(page * pageSize).Take(pageSize).ToList() ?? new List<ImageDetails>();
        }

        public List<ImageDetails> GetImagesByAccountId(int id)
        {
            _currentImageResults = _imageRepository.GetImagesByAccountId(id);
            return _currentImageResults;
        }
        public List<ImageDetails> GetImagesByIds(List<int> ids)
        {
            _currentImageResults = _imageRepository.GetImagesByIds(ids);
            return _currentImageResults;
        }

        public List<ImageDetails> GetAccountsByTag(string tag)
        {
            _currentImageResults = _imageRepository.GetImagesByTag(tag);
            return _currentImageResults;
        }
    }
}
