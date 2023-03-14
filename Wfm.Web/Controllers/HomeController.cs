using Microsoft.AspNetCore.Mvc;
using Wfm.Domain.Services.Settings;

namespace Wfm.Web.Controllers;

public class HomeController : Controller
{
    private readonly ISettingService _settingService;

    public HomeController(ISettingService settingService)
    {
        _settingService = settingService;
    }

    public IActionResult Index()
    {
        LocationOptions[] locations = _settingService.StorageOptions.Locations;

        if (locations.Length == 1)
            return RedirectToAction(nameof(FilesController.Index), "Files", new { LocationIndex = 0 });

        Dictionary<int, string> locationInfos = locations.Select((obj, index) => new { Key = index, Value = obj.Name! })
            .ToDictionary(x => x.Key, x => x.Value);

        return View(locationInfos);
    }

    public IActionResult Error()
    {
        return Content("Error!");
    }
}
