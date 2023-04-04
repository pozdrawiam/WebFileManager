using Wfm.Domain.Services;

namespace Wfm.Domain.Features.FileManager.GetThumbnail;

public class GetThumbnailHandler
{
    private readonly IImageService _imageService;

    public GetThumbnailHandler(IImageService imageService)
    {
        _imageService = imageService;
    }

    public GetThumbnailResult Handle(GetThumbnailQuery query)
    {
        string thumbnailDirPath = Path.Join(Path.GetDirectoryName(query.ImagePath), ".thumbnails");
        string thumbnailPath = Path.Join(thumbnailDirPath, Path.GetFileName(query.ImagePath));

        if (!Directory.Exists(thumbnailDirPath))
        {
            Directory.CreateDirectory(thumbnailDirPath);
        }

        _imageService.CreateThumbnail(query.ImagePath, thumbnailPath, 120, 120);

        return new GetThumbnailResult(thumbnailPath);
    }
}
