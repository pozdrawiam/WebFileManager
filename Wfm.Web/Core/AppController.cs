using Microsoft.AspNetCore.Mvc;
using Wfm.Domain.Services.Settings;

namespace Wfm.Web.Core;

public class AppController : Controller
{
    protected readonly Lazy<ISettingService> SettingService;

    public AppController(Lazy<ISettingService> settingService)
    {
        SettingService = settingService;
    }
}
