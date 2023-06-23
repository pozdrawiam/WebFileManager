using Moq;
using NUnit.Framework;
using Wfm.Domain.Services.FileSystem;
using Wfm.Domain.Services.Settings;
using Wfm.Domain.Features.FileManager.GetFiles;
using System;

namespace Wfm.Domain.Tests
{
    [TestFixture]
    public class GetFilesHandlerTests
    {
        private GetFilesHandler? _handler;
        private Mock<IFileSystemService>? _fileSystemServiceMock;
        private Mock<ISettingService>? _settingServiceMock;

        [SetUp]
        public void Setup()
        {
            _settingServiceMock = new Mock<ISettingService>();
            _fileSystemServiceMock = new Mock<IFileSystemService>();
            _handler = new GetFilesHandler(_settingServiceMock.Object, _fileSystemServiceMock.Object);
        }

        [Test]
        public void Handle_WithInvalidLocationIndex_ThrowsException()
        {
            _settingServiceMock!.SetupGet(s => s.StorageOptions).Returns(new StorageOptions { Locations = new LocationOptions[0]});
            var query = new GetFilesQuery(1, "");

            Assert.Throws<Exception>(() => _handler!.Handle(query));
        }

        [Test]
        public void Handle_WithValidLocationIndex_ReturnsExpectedResult()
        {
            var storegeOptions = new StorageOptions 
            {
                Locations = new [] { new LocationOptions { Path = "/path1" }, new LocationOptions { Path = "/path2" }}
            };
            _settingServiceMock!.SetupGet(s => s.StorageOptions).Returns(storegeOptions);

            var fileSystemEntries = new [] 
            {
                 new FileSystemEntry(FileSystemEntryType.Directory, "entry1", "", 0, new DateTime(), ""), 
                 new FileSystemEntry(FileSystemEntryType.File, "entry2.txt", "", 0, new DateTime(), "txt"), 
            };
            _fileSystemServiceMock!.Setup(f => f.GetEntries(It.IsAny<string>())).Returns(fileSystemEntries);

            var query = new GetFilesQuery(1, "");

            GetFilesResult result = _handler!.Handle(query);

            Assert.AreEqual(query.LocationIndex, result.LocationIndex);
            Assert.AreEqual(query.RelativePath, result.RelativePath);
            CollectionAssert.AreEquivalent(fileSystemEntries, result.Entries);
        }
    }
}
