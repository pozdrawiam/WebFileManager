using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Wfm.Domain.Features.FileManager.DownloadFile;
using Wfm.Domain.Features.FileManager.GetFiles;
using Wfm.Domain.Features.FileManager.GetThumbnail;
using Wfm.Domain.Services.Settings;
using Wfm.Web.Core;

namespace Wfm.Web.Controllers;

public class FilesController : AppController
{
    private readonly Lazy<GetFilesHandler> _getFilesHandler;
    private readonly Lazy<GetThumbnailHandler> _getThumbnailHandler;
    private readonly Lazy<DownloadFileHandler> _downloadFileHandler;

    public FilesController(
        Lazy<ISettingService> settingService,
        Lazy<GetFilesHandler> getFilesHandler,
        Lazy<GetThumbnailHandler> getThumbnailHandler,
        Lazy<DownloadFileHandler> downloadFileHandler
        ) : base(settingService)
    {
        _getFilesHandler = getFilesHandler;
        _getThumbnailHandler = getThumbnailHandler;
        _downloadFileHandler = downloadFileHandler;
    }

    public IActionResult Index(GetFilesQuery query)
    {
        ViewBag.LocationName = GetLocationName(query.LocationIndex) ?? string.Empty;

        GetFilesResult result = _getFilesHandler.Value.Handle(query);

        return View(result);
    }

    public IActionResult Download(DownloadFileQuery query)
    {
        DownloadFileResult result = _downloadFileHandler.Value.Handle(query);

        if (string.IsNullOrWhiteSpace(result.FilePath))
            return NotFound();

        string fileName = Path.GetFileName(result.FilePath);

        return GetFileResult(result.FilePath, fileName);
    }

    public IActionResult Preview(DownloadFileQuery query)
    {
        DownloadFileResult result = _downloadFileHandler.Value.Handle(query);

        if (string.IsNullOrWhiteSpace(result.FilePath))
            return NotFound();

        return GetFileResult(result.FilePath, "");
    }

    public IActionResult Thumbnail(DownloadFileQuery query)
    {
        DownloadFileResult downloadResult = _downloadFileHandler.Value.Handle(query);

        if (string.IsNullOrWhiteSpace(downloadResult.FilePath))
            return NotFound();

        var thumbnailQuery = new GetThumbnailQuery(downloadResult.FilePath);
        GetThumbnailResult thumbnailResult = _getThumbnailHandler.Value.Handle(thumbnailQuery);

        if (string.IsNullOrWhiteSpace(thumbnailResult.ImagePath))
            return NoContent();

        return GetFileResult(thumbnailResult.ImagePath, "");
    }

    private string? GetLocationName(int index)
    {
        return SettingService.Value.StorageOptions.Locations.ElementAt(index).Name;
    }

    private static PhysicalFileResult GetFileResult(string filePath, string targetFileName)
    {
        string mimeType = GetMimeTypeForFileExtension(filePath);

        return new(filePath, mimeType) { FileDownloadName = targetFileName };
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
