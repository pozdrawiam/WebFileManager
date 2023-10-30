using System.IO;
using Wfm.Domain.Features.FileManager.DownloadFile;
using Wfm.Domain.Services.FileSystem;
using Wfm.Domain.Services.Settings;

namespace Wfm.Domain.Tests.FileManager;

[TestFixture]
public class DownloadFileHandlerTests
{
    private DownloadFileHandler? _handler;
    private Mock<ISettingService>? _settingServiceMock;
    private Mock<IFileSystemService>? _fileSystemServiceMock;

    [SetUp]
    public void SetUp()
    {
        _settingServiceMock = new();
        _fileSystemServiceMock = new();
        _handler = new(_settingServiceMock.Object, _fileSystemServiceMock.Object);
    }

    [Test]
    public void Handle_LocationPathEmpty_ReturnsEmptyResult()
    {
        var query = new DownloadFileQuery(1, "file.txt");
        _settingServiceMock!.Setup(s => s.GetLocationByIndex(It.IsAny<int>())).Returns(new LocationOptions { Path = "" });

        DownloadFileResult result = _handler!.Handle(query);

        Assert.AreEqual("", result.FilePath);
    }

    [Test]
    public void Handle_FileExists_ReturnsFilePath()
    {
        var query = new DownloadFileQuery(1, "file.txt");
        const string locationPath = "C:\\Location";

        _settingServiceMock!.Setup(s => s.GetLocationByIndex(It.IsAny<int>())).Returns(new LocationOptions { Path = locationPath });
        _fileSystemServiceMock!.Setup(f => f.IsFileExists(It.IsAny<string>())).Returns(true);

        DownloadFileResult result = _handler!.Handle(query);

        var expectedFilePath = Path.Join(locationPath, query.RelativeFilePath);
        Assert.AreEqual(expectedFilePath, result.FilePath);
    }

    [Test]
    public void Handle_FileDoesNotExist_ReturnsEmptyResult()
    {
        var query = new DownloadFileQuery(1, "file.txt");
        const string locationPath = "C:\\Location";

        _settingServiceMock!.Setup(s => s.GetLocationByIndex(It.IsAny<int>())).Returns(new LocationOptions { Path = locationPath });
        _fileSystemServiceMock!.Setup(f => f.IsFileExists(It.IsAny<string>())).Returns(false);

        DownloadFileResult result = _handler!.Handle(query);

        Assert.AreEqual("", result.FilePath);
    }
}
