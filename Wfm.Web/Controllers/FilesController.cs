using Microsoft.AspNetCore.Mvc;
using Wfm.Domain.Features.FileManager.GetFiles;
using Wfm.Domain.Services;

namespace Wfm.Web.Controllers;

public class FilesController : Controller
{
    private readonly GetFilesHandler _getFilesHandler;
    private readonly ISettingService _settingService;

    public FilesController(ISettingService settingService, GetFilesHandler getFilesHandler)
    {
        _settingService = settingService;
        _getFilesHandler = getFilesHandler;
    }

    public IActionResult Index(GetFilesQuery query)
    {
        ViewBag.LocationName = GetLocationName(query.LocationIndex);
        GetFilesResult result = _getFilesHandler.Handle(query);

        return View(result);
    }

    private string? GetLocationName(int index)
    {
        return _settingService.StorageOptions.Locations?.ElementAt(index)?.Name;
    }
}
