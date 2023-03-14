using Moq;
using NUnit.Framework;
using Wfm.Domain.Features.FileManager.GetFiles;
using Wfm.Domain.Services.FileSystem;
using Wfm.Domain.Services.Settings;

namespace Wfm.Domain.Tests.FileManager;

public class GetFilesHandlerTests
{
    private GetFilesHandler? _sut;

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

        _sut = new GetFilesHandler(settingServiceMock.Object, fileSystemServiceMock.Object);
    }

    [Test]
    public void Handle()
    {
        var query = new GetFilesQuery(0, "");

        var result = _sut!.Handle(query);

        Assert.IsNotNull(result);
    }
}
