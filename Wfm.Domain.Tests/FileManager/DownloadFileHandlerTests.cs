using Moq;
using NUnit.Framework;
using Wfm.Domain.Services.FileSystem;
using Wfm.Domain.Services.Settings;
using Wfm.Domain.Features.FileManager.DownloadFile;
using System.IO;

namespace Wfm.Domain.Tests.Features.FileManager.DownloadFile
{
    [TestFixture]
    public class DownloadFileHandlerTests
    {
        private DownloadFileHandler? _handler;
        private Mock<ISettingService>? _mockSettingService;
        private Mock<IFileSystemService>? _mockFileSystemService;

        [SetUp]
        public void SetUp()
        {
            _mockSettingService = new Mock<ISettingService>();
            _mockFileSystemService = new Mock<IFileSystemService>();
            _handler = new DownloadFileHandler(_mockSettingService.Object, _mockFileSystemService.Object);
        }

        [Test]
        public void Handle_LocationPathEmpty_ReturnsEmptyResult()
        {
            var query = new DownloadFileQuery(1, "file.txt");
            _mockSettingService!.Setup(s => s.GetLocationByIndex(It.IsAny<int>())).Returns(new LocationOptions { Path = "" });

            DownloadFileResult result = _handler!.Handle(query);

            Assert.AreEqual("", result.FilePath);
        }

        [Test]
        public void Handle_FileExists_ReturnsFilePath()
        {
            var query = new DownloadFileQuery(1, "file.txt");
            const string locationPath = "C:\\Location";

            _mockSettingService!.Setup(s => s.GetLocationByIndex(It.IsAny<int>())).Returns(new LocationOptions { Path = locationPath });
            _mockFileSystemService!.Setup(f => f.IsFileExists(It.IsAny<string>())).Returns(true);

            DownloadFileResult result = _handler!.Handle(query);

            var expectedFilePath = Path.Join(locationPath, query.RelativeFilePath);
            Assert.AreEqual(expectedFilePath, result.FilePath);
        }

        [Test]
        public void Handle_FileDoesNotExist_ReturnsEmptyResult()
        {
            var query = new DownloadFileQuery(1, "file.txt");
            const string locationPath = "C:\\Location";

            _mockSettingService!.Setup(s => s.GetLocationByIndex(It.IsAny<int>())).Returns(new LocationOptions { Path = locationPath });
            _mockFileSystemService!.Setup(f => f.IsFileExists(It.IsAny<string>())).Returns(false);

            DownloadFileResult result = _handler!.Handle(query);

            Assert.AreEqual("", result.FilePath);
        }
    }
}
