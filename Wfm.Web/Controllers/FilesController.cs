using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Wfm.Domain.Features.FileManager.DownloadFile;
using Wfm.Domain.Features.FileManager.GetFiles;
using Wfm.Domain.Services.Settings;

namespace Wfm.Web.Controllers;

public class FilesController : Controller
{
    private readonly GetFilesHandler _getFilesHandler;
    private readonly DownloadFileHandler _downloadFileHandler;
    private readonly ISettingService _settingService;

    public FilesController(
        ISettingService settingService, 
        GetFilesHandler getFilesHandler,
        DownloadFileHandler downloadFileHandler)
    {
        _settingService = settingService;
        _getFilesHandler = getFilesHandler;
        _downloadFileHandler = downloadFileHandler;
    }

    public IActionResult Index(GetFilesQuery query)
    {
        ViewBag.LocationName = GetLocationName(query.LocationIndex);

        GetFilesResult result = _getFilesHandler.Handle(query);

        return View(result);
    }

    public IActionResult Download(DownloadFileQuery query)
    {
        DownloadFileResult result = _downloadFileHandler.Handle(query);

        if (string.IsNullOrWhiteSpace(result.FilePath))
            return NotFound();

        string fileName = Path.GetFileName(result.FilePath);

        return GetFileResult(result.FilePath, fileName);
    }

    public IActionResult Preview(DownloadFileQuery query)
    {
        DownloadFileResult result = _downloadFileHandler.Handle(query);

        if (string.IsNullOrWhiteSpace(result.FilePath))
            return NotFound();

        return GetFileResult(result.FilePath, "");
    }

    private FileStreamResult GetFileResult(string filePath, string targetFileName)
    {
        string mimeType = GetMimeTypeForFileExtension(filePath);
        var ms = new MemoryStream();

        using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            fs.CopyTo(ms);

        ms.Position = 0;

        return File(ms, mimeType, targetFileName);
    }

    private string? GetLocationName(int index)
    {
        return _settingService.StorageOptions.Locations?.ElementAt(index)?.Name;
    }

    private static string GetMimeTypeForFileExtension(string filePath)
    {
        const string defaultContentType = "application/octet-stream";

        var provider = new FileExtensionContentTypeProvider();

        if (!provider.TryGetContentType(filePath, out string? contentType))
        {
            contentType = defaultContentType;
        }

        return contentType;
    }
}
