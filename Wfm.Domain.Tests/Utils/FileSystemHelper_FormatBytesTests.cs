using NUnit.Framework;
using Wfm.Domain.Utils;

namespace Wfm.Domain.Tests.Utils;

[TestFixture]
public class FileSystemHelper_FormatBytesTests
{
    [Test]
    public void FormatBytes_ShouldReturnCorrectValue_For500B()
    {
        long bytes = 500;

        string result = FileSystemHelper.FormatBytes(bytes);

        Assert.AreEqual("500 B", result);
    }

    [Test]
    public void FormatBytes_ShouldReturnCorrectValue_For2KB()
    {
        long bytes = 2048;

        string result = FileSystemHelper.FormatBytes(bytes);

        Assert.AreEqual("2 KB", result);
    }

    [Test]
    public void FormatBytes_ShouldReturnCorrectValue_For1x5MB()
    {
        long bytes = 1572864;

        string result = FileSystemHelper.FormatBytes(bytes);

        Assert.AreEqual($"{1.5} MB", result);
    }

    [Test]
    public void FormatBytes_ShouldReturnCorrectValue_For1GB()
    {
        long bytes = 1073741824;

        string result = FileSystemHelper.FormatBytes(bytes);

        Assert.AreEqual("1 GB", result);
    }

    [Test]
    public void FormatBytes_ShouldReturnCorrectValue_For1TB()
    {
        long bytes = 1099511627776;

        string result = FileSystemHelper.FormatBytes(bytes);

        Assert.AreEqual("1 TB", result);
    }

    [Test]
    public void FormatBytes_ShouldReturnCorrectValue_For1PB()
    {
        long bytes = 1125899906842624;

        string result = FileSystemHelper.FormatBytes(bytes);

        Assert.AreEqual("1 PB", result);
    }

    [Test]
    public void FormatBytes_ShouldReturnCorrectValue_For1EB()
    {
        long bytes = 1152921504606846976;

        string result = FileSystemHelper.FormatBytes(bytes);

        Assert.AreEqual("1 EB", result);
    }
}
