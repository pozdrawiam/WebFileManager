using Wfm.Domain.Services.FileSystem;

namespace Wfm.Web.Services;

public class FileSystemService : IFileSystemService
{
    public IEnumerable<FileSystemEntry> GetEntries(string directoryPath)
    {
        foreach (string fileSystemEntry in Directory.GetFileSystemEntries(directoryPath))
        {
            bool isDirectory = (File.GetAttributes(fileSystemEntry) & FileAttributes.Directory) == FileAttributes.Directory;
            var entryType = isDirectory ? FileSystemEntryType.Directory : FileSystemEntryType.File;
            long size = 0;
            DateTime modifiedDate = File.GetLastWriteTime(fileSystemEntry);

            if (!isDirectory)
            {
                var fileInfo = new FileInfo(fileSystemEntry);

                size = fileInfo.Length;
            }

            yield return new FileSystemEntry(
                entryType,
                Path.GetFileName(fileSystemEntry),
                Path.GetRelativePath(directoryPath, fileSystemEntry),
                size,
                modifiedDate);
        }
    }

    public bool IsFileExists(string filePath)
    {
        return File.Exists(filePath);
    }
}
