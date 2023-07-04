namespace Wfm.Domain.Features.FileManager.GetFiles;

public record GetFilesQuery(int LocationIndex, string RelativePath, string? OrderBy, bool OrderDesc);
