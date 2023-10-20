using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HaApi.Controllers;

[Route("api/app-info")]
[ApiController]
public class AppInfoController : ControllerBase
{
    private readonly IConfiguration config;
    public AppInfoController(IConfiguration config)
    {
        this.config = config;
    }

    [HttpGet("db-info")]
    public ActionResult<object> DbInfo()
    {
        var dsrc = int.Parse(config["DataSource"]);

        return new
        {
            DataSource = dsrc == 1 ? "InMemory" : "MySql",
            Details = dsrc == 1 ? null : $"MySQL connection string: {GenerateConnectionString(config)}"
        };
    }

    private static string GenerateConnectionString(IConfiguration config)
    {
        var server = config["MySqlDb:Server"];
        var dbName = config["MySqlDb:DbName"];
        var userId = config["MySqlDb:UserId"];
        var password = config["MySqlDb:Password"];

        return $"Server={server};Database={dbName};User={userId};Password={password};";
    }
}
