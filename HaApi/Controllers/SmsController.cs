using HaApi.Models;
using HaApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HaApi.Controllers;

[Route("api/sms")]
[ApiController]
public class SmsController : ControllerBase
{
    private readonly Logger logger;
    private readonly ILogger<SmsController> consoleLogger;
    private readonly IDb db;

    public SmsController(Logger logger, ILogger<SmsController> consoleLogger, IDb db)
    {
        this.logger = logger;
        this.consoleLogger = consoleLogger;
        this.db = db;
    }

    [HttpGet("get")]
    public async Task<ActionResult<PostResponse>> GetSms([FromQuery] string smsId)
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
