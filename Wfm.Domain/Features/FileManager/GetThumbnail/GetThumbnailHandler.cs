using Wfm.Domain.Services;

namespace Wfm.Domain.Features.FileManager.GetThumbnail;

//todo refactor, lock
public class GetThumbnailHandler
{
    private readonly IImageService _imageService;

    public GetThumbnailHandler(IImageService imageService)
    {
        _imageService = imageService;
    }

    public GetThumbnailResult Handle(GetThumbnailQuery query)
    {
        const string thumbnailsDir = ".thumbnails";

        if (string.IsNullOrWhiteSpace(query?.ImagePath) || query.ImagePath.Contains(thumbnailsDir))
            return new ("");

        string thumbnailDirPath = Path.Join(Path.GetDirectoryName(query.ImagePath), thumbnailsDir);
        string thumbnailPath = Path.Join(thumbnailDirPath, Path.GetFileName(query.ImagePath));

        if (!Directory.Exists(thumbnailDirPath))
            Directory.CreateDirectory(thumbnailDirPath);

        if (!File.Exists(query.ImagePath))
            _imageService.CreateThumbnail(query.ImagePath, thumbnailPath, 120, 120);

        return new GetThumbnailResult(thumbnailPath);
    }
}
