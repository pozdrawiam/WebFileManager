using Moq;
using NUnit.Framework;
using Wfm.Domain.Features.FileManager.FileList;
using Wfm.Domain.Services;
using Wfm.Domain.Settings;

namespace Wfm.Domain.Tests.FileManager;

public class FileListQueryHandlerTests
{
    private FileListQueryHandler? _sut;
    
    [SetUp]
    public void Setup()
    {
        var settingServiceMock = new Mock<ISettingService>();
        settingServiceMock.Setup(x => x.StorageOptions).Returns(new StorageOptions 
        {
            Locations = new LocationOptions[] 
            { 
                new LocationOptions { Name = "test", Path = "test/path" } 
            } 
        });

        var fileSystemServiceMock = new Mock<IFileSystemService>();

        _sut = new FileListQueryHandler(settingServiceMock.Object, fileSystemServiceMock.Object);
    }

    [Test]
    public void Handle()
    {
        var query = new FileListQuery(0, "");

        var result = _sut.Handle(query);

        Assert.IsNotNull(result);
    }
}
