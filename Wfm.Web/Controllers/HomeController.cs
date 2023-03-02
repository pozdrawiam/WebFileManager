using Microsoft.AspNetCore.Mvc;
using Wfm.Domain.Features.FileManager.GetFiles;

namespace Wfm.Web.Controllers;

public class HomeController : Controller
{
    private readonly GetFilesHandler _getFilesHandler;

    public HomeController(GetFilesHandler getFilesHandler)
    {
        _getFilesHandler = getFilesHandler;
    }

    public IActionResult Index()
    {
        return View();
    }

    public object Test()
    {
        return _getFilesHandler.Handle(new GetFilesQuery(0, "New folder"));
    }

    public IActionResult Error()
    {
        return Content("Error!");
    }
}
