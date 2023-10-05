using HaApi.Models;
using HaApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HaApi.Controllers;

[Route("api/call")]
[ApiController]
public class CallController : ControllerBase
{
    private readonly Logger logger;
    private readonly ILogger<CallController> consoleLogger;
    public CallController(Logger logger, ILogger<CallController> consoleLogger)
    {
        this.logger = logger;
        this.consoleLogger = consoleLogger;
    }

    [HttpGet("call-now")]
    public ActionResult<PostResponse> Call()
    {
        var logEntry = $"Called on {DateTime.Now}";
        logger.Called(logEntry);
        consoleLogger.LogInformation(logEntry);

        return new PostResponse
        {
            IsSuccess = true,
            Message = logEntry
        };
    }
}
