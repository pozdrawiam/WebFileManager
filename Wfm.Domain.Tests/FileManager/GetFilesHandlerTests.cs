using System;
using Wfm.Domain.Features.FileManager.GetFiles;
using Wfm.Domain.Services.FileSystem;
using Wfm.Domain.Services.Settings;

namespace Wfm.Domain.Tests.FileManager;

[TestFixture]
public class GetFilesHandlerTests
{
    private GetFilesHandler? _handler;
    private Mock<IFileSystemService>? _fileSystemServiceMock;
    private Mock<ISettingService>? _settingServiceMock;

    [SetUp]
    public void Setup()
    {
        _settingServiceMock = new();
        _fileSystemServiceMock = new();
        _handler = new(_settingServiceMock.Object, _fileSystemServiceMock.Object);
    }

    [Test]
    public void Handle_WithInvalidLocationIndex_ThrowsException()
    {
        _settingServiceMock!.SetupGet(s => s.StorageOptions).Returns(new StorageOptions { Locations = Array.Empty<LocationOptions>()});
        var query = new GetFilesQuery(1, "", null, false);

        Assert.Throws<Exception>(() => _handler!.Handle(query));
    }

    [Test]
    public void Handle_WithValidLocationIndex_ReturnsExpectedResult()
    {
        var storageOptions = new StorageOptions 
        {
            Locations = new [] { new LocationOptions { Path = "/path1" }, new LocationOptions { Path = "/path2" }}
        };
        _settingServiceMock!.SetupGet(s => s.StorageOptions).Returns(storageOptions);

        var fileSystemEntries = new [] 
        {
            new FileSystemEntry(FileSystemEntryType.Directory, "entry1", "", 0, new DateTime(), ""), 
            new FileSystemEntry(FileSystemEntryType.File, "entry2.txt", "", 0, new DateTime(), "txt"), 
        };
        _fileSystemServiceMock!.Setup(f => f.GetEntries(It.IsAny<string>())).Returns(fileSystemEntries);

        var query = new GetFilesQuery(1, "", null, false);

        GetFilesResult result = _handler!.Handle(query);

        Assert.AreEqual(query.LocationIndex, result.LocationIndex);
        Assert.AreEqual(query.RelativePath, result.RelativePath);
        CollectionAssert.AreEquivalent(fileSystemEntries, result.Entries);
    }
}
