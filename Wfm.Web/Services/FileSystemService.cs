using Wfm.Domain.Core.FileSystem;
using Wfm.Domain.Services;

namespace Wfm.Web.Services;

public class FileSystemService : IFileSystemService
{
    public IEnumerable<FileSystemEntry> GetEntries(string directoryPath)
    {
        string[] fileSystemEntries = Directory.GetFileSystemEntries(directoryPath);

        foreach (string fileSystemEntry in fileSystemEntries)
        {
            bool isDirectory = (File.GetAttributes(fileSystemEntry) & FileAttributes.Directory) == FileAttributes.Directory;
            var entryType = isDirectory ? FileSystemEntryType.Directory : FileSystemEntryType.File;

            yield return new FileSystemEntry(entryType, Path.GetFileName(fileSystemEntry));
        }
    }
}
