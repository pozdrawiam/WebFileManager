using Wfm.Domain.Services.FileSystem;
using Wfm.Domain.Services.Settings;

namespace Wfm.Domain.Features.FileManager.DownloadFile;

public class DownloadFileHandler
{
    private readonly ISettingService _settingService;
    private readonly IFileSystemService _fileSystemService;

    public DownloadFileHandler(ISettingService settingService, IFileSystemService fileSystemService)
    {
        _settingService = settingService;
        _fileSystemService = fileSystemService;
    }

    public DownloadFileResult Handle(DownloadFileQuery query)
    {
        LocationOptions? location = _settingService.GetLocationByIndex(query.LocationIndex);

        if (string.IsNullOrWhiteSpace(location?.Path))
            return new ("");

        string filePath = Path.Join(location.Path, query.RelativeFilePath);

        if (_fileSystemService.IsFileExists(filePath))
            return new (filePath);

        return new ("");
    }
}
