using NLog.Config;
using NLog;
using System.Runtime.CompilerServices;

namespace HaApi.Services;

public class Logger
{
    private readonly bool enable;
    private readonly NLog.Logger debugLogger = LogManager.GetLogger("Debug");
    private readonly NLog.Logger dbErrorLogger = LogManager.GetLogger("DbError");
    private readonly NLog.Logger ioErrorLogger = LogManager.GetLogger("IOError");
    private readonly NLog.Logger reqInSpeedLogger = LogManager.GetLogger("ReqInSpeed");
    private readonly NLog.Logger calledLogger = LogManager.GetLogger("Called");

    public LoggingConfiguration LoggingConfiguration { get; }

    public Logger(IConfiguration cfg)
    {
        enable = bool.Parse(cfg["NLogConfig:Enable"]);

        if (enable)
        {
            var path = cfg["NLogConfig:LogPath"];
            var logList = LoggersToAdd();
            var ext = cfg["NLogConfig:LogFileType"];

            NLogConfig config = new(path);
            foreach (var log in logList)
                config.AddLog(log, $"{log}{ext}");
            LoggingConfiguration = config.FinalizeConfig();
        }
    }

    private static List<string> LoggersToAdd()
    {
        return new()
        {
            "Debug",
            "DbError",
            "IOError",
            "ReqInSpeed",
            "Called"
        };
    }

    public void DbError(Exception ex, bool detailed, [CallerMemberName] string caller = null, [CallerLineNumber] int? line = null)
    {
        if (enable)
        {
            var methodName = ex.TargetSite == null ? "Unknown method" : $"{caller}, Ln. {line}";

            if (detailed)
                dbErrorLogger.Info($"{methodName} | {ex} | Inner exception message: {ex.InnerException?.Message}");
            //dbErrorLogger.Info($"{methodName} | {ex.Message} | {ex.InnerException?.Message}");
            else
                dbErrorLogger.Info($"{methodName} | {ex.Message}");
        }
    }

    public void Debug(string txt)
    {
        if (enable)
        {
            debugLogger.Info(txt);
        }
    }

    public void IOError(Exception ex, [CallerMemberName] string caller = null, [CallerLineNumber] int? line = null)
    {
        if (enable)
        {
            var methodName = ex.TargetSite == null ? "Unknown method" : $"{caller}, Ln. {line}";
            ioErrorLogger.Info($"{methodName} | {ex.Message}");
        }
    }

    public void ReqInSpeed(string str)
    {
        if (enable)
        {
            reqInSpeedLogger.Info($"{str}");
        }
    }

    public void Called(string str)
    {
        if (enable)
        {
            calledLogger.Info($"{str}");
        }
    }
}

