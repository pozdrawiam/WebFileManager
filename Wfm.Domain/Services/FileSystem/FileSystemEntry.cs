namespace Wfm.Domain.Services.FileSystem;

public record FileSystemEntry(FileSystemEntryType Type, string Name, string RelativePath, long Size, DateTime ModifiedDate);
