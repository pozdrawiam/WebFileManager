using Wfm.Domain.Core.FileSystem;

namespace Wfm.Domain.Features.FileManager.GetFiles;

public record GetFilesResult(IEnumerable<FileSystemEntry> Entries);
