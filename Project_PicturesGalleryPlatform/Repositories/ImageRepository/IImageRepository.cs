using Project_PicturesGalleryPlatform.Models;

namespace Project_PicturesGalleryPlatform.Repositories.ImageRepository
{
    public interface IImageRepository
    {
        List<ImageDetails> GetImagesByKeyword(string keyword);
        List<ImageDetails> GetRelatedImages(int id);
        List<ImageDetails> GetImagesByIds(List<int> ids);
        List<ImageDetails> GetImagesByTag(string tag);
        List<ImageDetails> GetAccountsById(int id);
    }
}
