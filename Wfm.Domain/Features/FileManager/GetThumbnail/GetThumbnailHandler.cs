using Wfm.Domain.Consts;
using Wfm.Domain.Services;
using Wfm.Domain.Services.FileSystem;

namespace Wfm.Domain.Features.FileManager.GetThumbnail;

public class GetThumbnailHandler
{
    private readonly IFileSystemService _fileSystemService;
    private readonly IImageService _imageService;

    public GetThumbnailHandler(IFileSystemService fileSystemService, IImageService imageService)
    {
        _fileSystemService = fileSystemService;
        _imageService = imageService;
    }

    public GetThumbnailResult Handle(GetThumbnailQuery query)
    {
        if (string.IsNullOrWhiteSpace(query?.ImagePath) ||
            query.ImagePath.Contains(ThumbnailConsts.DirName) ||
            !ThumbnailConsts.Extensions.Contains(Path.GetExtension(query.ImagePath).ToLower().Replace(".", "")))
        {
            return new ("");
        }

        string thumbnailDirPath = Path.Join(Path.GetDirectoryName(query.ImagePath), ThumbnailConsts.DirName);
        string thumbnailPath = Path.Join(thumbnailDirPath, Path.GetFileName(query.ImagePath));

        if (!_fileSystemService.IsDirExists(thumbnailDirPath))
            _fileSystemService.CreateDir(thumbnailDirPath);

        if (!_fileSystemService.IsFileExists(thumbnailPath))
        {
            bool created = _imageService.CreateThumbnail(query.ImagePath, thumbnailPath, ThumbnailConsts.MaxWidth, ThumbnailConsts.MaxHeight);

            if (!created)
                return new ("");
        }

        return new GetThumbnailResult(thumbnailPath);
    }
}
