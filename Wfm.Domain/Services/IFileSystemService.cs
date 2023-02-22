using Wfm.Domain.Core.FileSystem;

namespace Wfm.Domain.Services;

public interface IFileSystemService
{
    IEnumerable<FileSystemEntry> GetEntries(string directoryPath);
}
