using System.Linq;
using Wfm.Domain.Services;

namespace Wfm.Domain.Features.FileManager.FileList;

public class FileListQueryHandler
{
    private readonly ISettingService _settingService;
    private readonly IFileSystemService _fileSystemService;
    
    public FileListQueryHandler(ISettingService settingService, IFileSystemService fileSystemService)
    {
        _settingService = settingService;
        _fileSystemService = fileSystemService;
    }
    
    public FileListQueryResult Handle(FileListQuery query)
    {
        var locations = _settingService.StorageOptions.Locations;

        if (query.LocationIndex > locations.Length)
            throw new Exception("Invalid locationIndex");

        string locationPath = locations[query.LocationIndex].Path;
        string path = Path.Join(locationPath, query.RelativePath);
        
        var entries = _fileSystemService.GetEntries(path);

        return new FileListQueryResult(entries);
    }
}
