using Microsoft.AspNetCore.Mvc;

namespace HaApi.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[Route("/")]
[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet]
    public ActionResult Home() => Redirect("/swagger/index.html");
}
