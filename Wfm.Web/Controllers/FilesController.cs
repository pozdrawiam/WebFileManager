using Microsoft.AspNetCore.Mvc;
using Wfm.Domain.Features.FileManager.GetFiles;

namespace Wfm.Web.Controllers;

public class FilesController : Controller
{
    private readonly GetFilesHandler _getFilesHandler;

    public FilesController(GetFilesHandler getFilesHandler)
    {
        _getFilesHandler = getFilesHandler;
    }
    
    public IActionResult Index()
    {
        var result = _getFilesHandler.Handle(new GetFilesQuery(0, "New folder"));

        return View(result);
    }
}
