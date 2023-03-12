using Wfm.Domain.Core.FileSystem;
using Wfm.Domain.Services;

namespace Wfm.Web.Services;

public class FileSystemService : IFileSystemService
{
    public IEnumerable<FileSystemEntry> GetEntries(string directoryPath)
    {
        foreach (string fileSystemEntry in Directory.GetFileSystemEntries(directoryPath))
        {
            bool isDirectory = (File.GetAttributes(fileSystemEntry) & FileAttributes.Directory) == FileAttributes.Directory;
            var entryType = isDirectory ? FileSystemEntryType.Directory : FileSystemEntryType.File;

            yield return new FileSystemEntry(entryType, Path.GetFileName(fileSystemEntry), Path.GetRelativePath(directoryPath, fileSystemEntry));
        }
    }

    public bool IsFileExists(string filePath)
    {
        return File.Exists(filePath);
    }
}
