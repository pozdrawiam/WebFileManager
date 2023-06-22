using NUnit.Framework;
using Wfm.Domain.Utils;

namespace Wfm.Domain.Tests.Utils;

[TestFixture]
public class FileSystemHelperTests
{
    #region ConvertPathToList Tests

    [Test]
    public void ConvertPathToList_WithNullPath_ReturnsEmptyList()
    {
        const string? path = null;

        var result = FileSystemHelper.ConvertPathToList(path!);

        Assert.IsEmpty(result);
    }

    [Test]
    public void ConvertPathToList_WithEmptyPath_ReturnsEmptyList()
    {
        const string path = "";

        var result = FileSystemHelper.ConvertPathToList(path);

        Assert.IsEmpty(result);
    }

    [Test]
    public void ConvertPathToList_WithRootPath_ReturnsSingleItemInList()
    {
        const string path = "folder1";

        var result = FileSystemHelper.ConvertPathToList(path);

        Assert.AreEqual(1, result.Count);

        Assert.AreEqual("folder1", result[0].Key);
        Assert.AreEqual("folder1", result[0].Value);
    }

    [Test]
    public void ConvertPathToList_WithValidPath_ReturnsCorrectList()
    {
        const string path = "folder1/folder2/folder3";

        var result = FileSystemHelper.ConvertPathToList(path);

        Assert.AreEqual(3, result.Count);

        Assert.AreEqual("folder1", result[0].Key);
        Assert.AreEqual("folder1", result[0].Value);

        Assert.AreEqual("folder2", result[1].Key);
        Assert.AreEqual("folder1/folder2", result[1].Value);

        Assert.AreEqual("folder3", result[2].Key);
        Assert.AreEqual("folder1/folder2/folder3", result[2].Value);
    }

    #endregion

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
    public void FormatBytes_ReturnsCorrectValue(long bytes, decimal value, string unit)
    {
        string expected = $"{value} {unit}";

        string result = FileSystemHelper.FormatBytes(bytes);

        Assert.AreEqual(expected, result);
    }
}
