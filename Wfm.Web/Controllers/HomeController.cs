using Microsoft.AspNetCore.Mvc;
using Wfm.Domain.Services.Settings;
using Wfm.Web.Core;

namespace Wfm.Web.Controllers;

public class HomeController : AppController
{
    public HomeController(Lazy<ISettingService> settingService)
        : base(settingService)
    {
    }

    public IActionResult Index()
    {
        LocationOptions[] locations = SettingService.Value.StorageOptions.Locations;

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
