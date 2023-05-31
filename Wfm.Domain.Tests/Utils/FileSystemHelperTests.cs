using NUnit.Framework;
using Wfm.Domain.Utils;

namespace Wfm.Domain.Tests.Utils;

[TestFixture]
public class FileSystemHelperTests
{
    [Test]
    [TestCase(0, 0, "B")]
    [TestCase(1, 1, "B")]
    [TestCase(500, 500, "B")]
    [TestCase(1024, 1, "KB")]
    [TestCase(2048, 2, "KB")]
    [TestCase(1048576, 1, "MB")]
    [TestCase(1572864, 1.5, "MB")]
    [TestCase(1073741824, 1, "GB")]
    [TestCase(1099511627776, 1, "TB")]
    [TestCase(1125899906842624, 1, "PB")]
    [TestCase(1152921504606846976, 1, "EB")]
    public void FormatBytes_ShouldReturnCorrectValue(long bytes, decimal value, string unit)
    {
        string expected = $"{value} {unit}";

        string result = FileSystemHelper.FormatBytes(bytes);

        Assert.AreEqual(expected, result);
    }
}
