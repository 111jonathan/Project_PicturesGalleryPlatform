using Project_PicturesGalleryPlatform.Models;

namespace Project_PicturesGalleryPlatform.Repositories.ImageRepository
{
    public interface IImageRepository
    {
        List<ImageDetails> GetRandomImages();
        List<ImageDetails> SearchImagesByKeyword(string keyword);
        List<ImageDetails> GetRelatedImagesById(int id);
        List<ImageDetails> GetImagesByIds(List<int> ids);
        List<ImageDetails> GetImagesByTag(string tag);
        List<ImageDetails> GetImagesById(int id);
    }
}
