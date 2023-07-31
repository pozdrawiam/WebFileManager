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
            throw new Exception("Invalid locationIndex");

        string locationPath = locations[query.LocationIndex].Path;
        string path = Path.Join(locationPath, query.RelativePath);

        IEnumerable<FileSystemEntry> entries = _fileSystemService.GetEntries(path)
            .Where(x => x.RelativePath != ThumbnailConsts.DirName)
            .Select(x => x with { RelativePath = Path.Join(query.RelativePath, x.RelativePath) });

        if (!string.IsNullOrWhiteSpace(query.OrderBy))
            entries = OrderEntries(entries, query.OrderBy, query.OrderDesc);
        else
            entries = entries.OrderByDescending(x => x.Type).ThenBy(x => x.Name);

        entries = entries.ToArray();

        int pageSize = _settingService.StorageOptions.ListPageSize;
        int totalEntries = entries.Count();
        int totalPages = (int)Math.Ceiling(totalEntries / (double)pageSize);

        entries = entries.Skip((query.Page - 1) * pageSize).Take(pageSize);

        return new GetFilesResult(query.LocationIndex, query.RelativePath, query.OrderBy, query.OrderDesc, query.Page, totalPages, totalEntries, entries);
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

        if (orderByMapping.ContainsKey(orderBy))
        {
            Func<FileSystemEntry, object> orderByExpression = orderByMapping[orderBy];

            if (orderDesc)
                entries = entries.OrderByDescending(orderByExpression);
            else
                entries = entries.OrderBy(orderByExpression);
        }

        return entries;
    }
}
