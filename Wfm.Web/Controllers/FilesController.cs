using Microsoft.AspNetCore.Mvc;
using Wfm.Domain.Features.FileManager.DownloadFile;
using Wfm.Domain.Features.FileManager.GetFiles;
using Wfm.Domain.Services;

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
        ViewBag.BackPath = Path.GetDirectoryName(query.RelativePath);

        GetFilesResult result = _getFilesHandler.Handle(query);

        return View(result);
    }

    public IActionResult Download(DownloadFileQuery query)
    {
        DownloadFileResult result = _downloadFileHandler.Handle(query);

        if (string.IsNullOrWhiteSpace(result.FilePath))
            return NotFound();

        return GetFileResult(result.FilePath);
    }

    private FileStreamResult GetFileResult(string filePath)
    {
        string fileName = Path.GetFileName(filePath);
        var ms = new MemoryStream();

        using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            fs.CopyTo(ms);

        ms.Position = 0;

        return File(ms, "APPLICATION/octet-stream", fileName);
    }

    private string? GetLocationName(int index)
    {
        return _settingService.StorageOptions.Locations?.ElementAt(index)?.Name;
    }
}
