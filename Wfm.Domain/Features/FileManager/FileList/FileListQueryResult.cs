using Wfm.Domain.Core.FileSystem;

namespace Wfm.Domain.Features.FileManager.FileList;

public record FileListQueryResult(IEnumerable<FileSystemEntry> Entries);
