using Project_PicturesGalleryPlatform.Models;

namespace Project_PicturesGalleryPlatform.Services.ImageService
{
    public interface IImageService
    {
        List<ImageDetails> GetRandomImages();
        List<ImageDetails> GetImagesByKeyword(string keyword);
        List<ImageDetails> GetImagesByPage(int page, int pageSize);
        List<ImageDetails> GetAccountsById(int id);
        public List<ImageDetails> GetImagesByIds(List<int> ids);
        List<ImageDetails> GetAccountsByTag(string tag);
    }
}
