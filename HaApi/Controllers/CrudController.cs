using HaApi.Models;
using HaApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HaApi.Controllers;

[Route("api/crud")]
[ApiController]
public class CrudController : ControllerBase
{
    private readonly Logger logger;
    private readonly ILogger<CrudController> consoleLogger;
    public CrudController(Logger logger, ILogger<CrudController> consoleLogger)
    {
        this.logger = logger;
        this.consoleLogger = consoleLogger;
    }

    [HttpGet("get")]
    public ActionResult<PostResponse> Get()
    {
        var logEntry = $"Get method called on {DateTime.Now}";
        logger.Called(logEntry);
        consoleLogger.LogInformation(logEntry);

        return new PostResponse
        {
            IsSuccess = true,
            Message = logEntry
        };
    }
}
