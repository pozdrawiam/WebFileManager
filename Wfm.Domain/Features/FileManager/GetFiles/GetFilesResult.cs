using Wfm.Domain.Services.FileSystem;

namespace Wfm.Domain.Features.FileManager.GetFiles;

public record GetFilesResult(
    int LocationIndex,
    string RelativePath,
    string? OrderBy,
    bool OrderDesc,
    int Page,
    int TotalPages,
    int TotalEntries,
    IEnumerable<FileSystemEntry> Entries);
