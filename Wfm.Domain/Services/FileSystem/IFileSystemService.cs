namespace Wfm.Domain.Services.FileSystem;

public interface IFileSystemService
{
    void CreateDir(string dirPath);
    IEnumerable<FileSystemEntry> GetEntries(string directoryPath);
    bool IsDirExists(string dirPath);
    bool IsFileExists(string filePath);
}
