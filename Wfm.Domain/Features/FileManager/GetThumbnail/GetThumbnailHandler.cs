using Wfm.Domain.Services;
using Wfm.Domain.Services.FileSystem;

namespace Wfm.Domain.Features.FileManager.GetThumbnail;

public class GetThumbnailHandler
{
    private const int ThumbnailMaxHeight = 120;
    private const int ThumbnailMaxWidth = 120;
    private const string ThumbnailsDir = ".thumbnails";

    private static readonly object CreateThumbnailLock = new();

    private readonly IFileSystemService _fileSystemService;
    private readonly IImageService _imageService;

    public GetThumbnailHandler(IFileSystemService fileSystemService, IImageService imageService)
    {
        _fileSystemService = fileSystemService;
        _imageService = imageService;
    }

    public GetThumbnailResult Handle(GetThumbnailQuery query)
    {
        if (string.IsNullOrWhiteSpace(query?.ImagePath) || query.ImagePath.Contains(ThumbnailsDir))
            return new ("");

        string thumbnailDirPath = Path.Join(Path.GetDirectoryName(query.ImagePath), ThumbnailsDir);
        string thumbnailPath = Path.Join(thumbnailDirPath, Path.GetFileName(query.ImagePath));

        if (!_fileSystemService.IsDirExists(thumbnailDirPath))
            _fileSystemService.CreateDir(thumbnailDirPath);

        if (!_fileSystemService.IsFileExists(thumbnailPath))
        {
            lock (CreateThumbnailLock)
            {
                if (!_fileSystemService.IsFileExists(thumbnailPath))
                    _imageService.CreateThumbnail(query.ImagePath, thumbnailPath, ThumbnailMaxWidth, ThumbnailMaxHeight);
            }
        }

        return new GetThumbnailResult(thumbnailPath);
    }
}