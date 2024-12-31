using Project_PicturesGalleryPlatform.Models;
namespace Project_PicturesGalleryPlatform.Services.ImageAnalysisService
{
    public interface IImageAnalysisService
    {
        public List<ImageDetails> FindSimilarImagesByImage();
        public List<ImageDetails> SearchImagesByText();
    }
}
