using Project_PicturesGalleryPlatform.Models;

namespace Project_PicturesGalleryPlatform.Services.ImageService
{
    public interface IImageService
    {
        List<ImageDetails> GetRandomImages();
        List<ImageDetails> SearchImagesByKeyword(string keyword);
        List<ImageDetails> GetImagesByPageNumber(int page, int pageSize);
        List<ImageDetails> GetImagesByAccountId(int id);
        public List<ImageDetails> GetImagesByIds(List<int> ids);
        List<ImageDetails> GetAccountsByTag(string tag);
    }
}
