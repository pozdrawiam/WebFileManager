using System.IO;
using Wfm.Domain.Consts;
using Wfm.Domain.Features.FileManager.GetThumbnail;
using Wfm.Domain.Services;
using Wfm.Domain.Services.FileSystem;

namespace Wfm.Domain.Tests.FileManager;

[TestFixture]
public class GetThumbnailHandlerTests
{
    private GetThumbnailHandler? _handler;
    private Mock<IFileSystemService>? _fileSystemServiceMock;
    private Mock<IImageService>? _imageServiceMock;

    [SetUp]
    public void SetUp()
    {
        _fileSystemServiceMock = new();
        _imageServiceMock = new();
        _handler = new(_fileSystemServiceMock.Object, _imageServiceMock.Object);
    }

    [Test]
    public void Handle_InvalidImagePath_ReturnsEmptyResult()
    {
        var query = new GetThumbnailQuery("invalid_image_path");

        GetThumbnailResult result = _handler!.Handle(query);

        Assert.AreEqual("", result.ImagePath);
    }

    [Test]
    public void Handle_ValidImage_CreatesThumbnailAndReturnsPath()
    {
        var query = new GetThumbnailQuery("valid_image.jpg");
        var thumbnailDirPath = Path.Join(Path.GetDirectoryName(query.ImagePath), ThumbnailConsts.DirName);
        var thumbnailPath = Path.Join(thumbnailDirPath, Path.GetFileName(query.ImagePath));

        _fileSystemServiceMock!.Setup(f => f.IsDirExists(It.IsAny<string>())).Returns(false);
        _fileSystemServiceMock.Setup(f => f.IsFileExists(It.IsAny<string>())).Returns(false);
        _imageServiceMock!.Setup(i => i.CreateThumbnail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);

        GetThumbnailResult result = _handler!.Handle(query);

        Assert.AreEqual(thumbnailPath, result.ImagePath);
    }

    [Test]
    public void Handle_ThumbnailAlreadyExists_ReturnsPath()
    {
        var query = new GetThumbnailQuery("valid_image.jpg");
        var thumbnailDirPath = Path.Join(Path.GetDirectoryName(query.ImagePath), ThumbnailConsts.DirName);
        var thumbnailPath = Path.Join(thumbnailDirPath, Path.GetFileName(query.ImagePath));

        _fileSystemServiceMock!.Setup(f => f.IsDirExists(It.IsAny<string>())).Returns(true);
        _fileSystemServiceMock.Setup(f => f.IsFileExists(It.IsAny<string>())).Returns(true);

        GetThumbnailResult result = _handler!.Handle(query);

        Assert.AreEqual(thumbnailPath, result.ImagePath);
    }

    [Test]
    public void Handle_ThumbnailCreationFails_ReturnsEmptyResult()
    {
        var query = new GetThumbnailQuery("valid_image.jpg");

        _fileSystemServiceMock!.Setup(f => f.IsDirExists(It.IsAny<string>())).Returns(false);
        _fileSystemServiceMock.Setup(f => f.IsFileExists(It.IsAny<string>())).Returns(false);
        _imageServiceMock!.Setup(i => i.CreateThumbnail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(false);

        GetThumbnailResult result = _handler!.Handle(query);

        Assert.AreEqual("", result.ImagePath);
    }
}
