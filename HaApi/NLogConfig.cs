using NLog.Config;
using NLog.Targets;
using NLog;
using LogLevel = NLog.LogLevel;

namespace HaApi;

public class NLogConfig
{
    private const string logLayout = "${longdate} | ${logger} || ${message}";
    private const string dateFormat = "${date:format=yyyyMMdd}";
    private const string hourFormat = "${date:format=HH}";
    private const string minuteFormat = "${date:format=mm}";
    private readonly string headPath;
    private readonly LoggingConfiguration logConfig;

    /// <summary>
    /// Initialize new NLogConfig class with fill base directory location
    /// </summary>
    /// <param name="fullBaseDir">Full base directory including app name</param>
    public NLogConfig(string fullBaseDir)
    {
        var separator = Path.DirectorySeparatorChar;
        headPath = Path.Combine(fullBaseDir, dateFormat, $"{hourFormat}00{separator}");

        logConfig = new LoggingConfiguration();
    }

    public void AddLog(string logName, string textfileName)
    {
        var fileName = Path.Combine(headPath, textfileName);

        var target = new FileTarget(logName)
        {
            FileName = fileName,
            Layout = logLayout
        };

        logConfig.AddTarget(target);
        logConfig.AddRule(LogLevel.Debug, LogLevel.Error, target, logName);
    }

    public LoggingConfiguration FinalizeConfig() => LogManager.Configuration = logConfig;
}
