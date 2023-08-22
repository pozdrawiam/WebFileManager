using Wfm.Domain.Consts;
using Wfm.Domain.Services.FileSystem;
using Wfm.Domain.Services.Settings;

namespace Wfm.Domain.Features.FileManager.GetFiles;

public class GetFilesHandler
{
    private readonly ISettingService _settingService;
    private readonly IFileSystemService _fileSystemService;

    public GetFilesHandler(ISettingService settingService, IFileSystemService fileSystemService)
    {
        _settingService = settingService;
        _fileSystemService = fileSystemService;
    }

    public GetFilesResult Handle(GetFilesQuery query)
    {
        LocationOptions[] locations = _settingService.StorageOptions.Locations;

        if (query.LocationIndex > locations.Length)
            throw new("Invalid locationIndex");

        string locationPath = locations[query.LocationIndex].Path;
        string path = Path.Join(locationPath, query.RelativePath);

        IEnumerable<FileSystemEntry> entries = _fileSystemService.GetEntries(path)
            .Where(x => x.RelativePath != ThumbnailConsts.DirName)
            .Select(x => x with { RelativePath = Path.Join(query.RelativePath, x.RelativePath) });

        entries = !string.IsNullOrWhiteSpace(query.OrderBy) ? 
            OrderEntries(entries, query.OrderBy, query.OrderDesc) : 
            entries.OrderByDescending(x => x.Type).ThenBy(x => x.Name);

        entries = entries.ToArray();

        int pageSize = _settingService.StorageOptions.ListPageSize;
        int totalEntries = entries.Count();
        int totalPages = (int)Math.Ceiling(totalEntries / (double)pageSize);

        entries = entries.Skip((query.Page - 1) * pageSize).Take(pageSize);

        return new(query.LocationIndex, query.RelativePath, query.OrderBy, query.OrderDesc, query.Page, totalPages, totalEntries, entries);
    }

    private static IEnumerable<FileSystemEntry> OrderEntries(IEnumerable<FileSystemEntry> entries, string orderBy, bool orderDesc)
    {
        var orderByMapping = new Dictionary<string, Func<FileSystemEntry, object>>()
        {
            { nameof(FileSystemEntry.Name), x => x.Name },
            { nameof(FileSystemEntry.Size), x => x.Size },
            { nameof(FileSystemEntry.ModifiedDate), x => x.ModifiedDate },
            { nameof(FileSystemEntry.Extension), x => x.Extension }
        };

        if (orderByMapping.TryGetValue(orderBy, out Func<FileSystemEntry, object>? orderByExpression))
            entries = orderDesc ? entries.OrderByDescending(orderByExpression) : entries.OrderBy(orderByExpression);

        return entries;
    }
}
