namespace Wfm.Domain.Services.FileSystem;

public interface IFileSystemService
{
    IEnumerable<FileSystemEntry> GetEntries(string directoryPath);
    bool IsFileExists(string filePath);
}
