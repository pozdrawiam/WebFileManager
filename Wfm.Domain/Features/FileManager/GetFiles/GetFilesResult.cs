using Wfm.Domain.Services.FileSystem;

namespace Wfm.Domain.Features.FileManager.GetFiles;

public record GetFilesResult(int LocationIndex, string RelativePath, IEnumerable<FileSystemEntry> Entries);
