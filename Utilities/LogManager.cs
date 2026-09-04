using Serilog;

namespace BddTraining.Utilities;

public static class LogManager
{
    private static bool _isInitialised;

    public static string? CurrentLogFile { get; private set; }

    public static void Initialise()
    {
        if (_isInitialised)
        {
            return;
        }

        var projectRoot = Path.GetFullPath(
            Path.Combine(
                AppContext.BaseDirectory,
                "..",
                "..",
                ".."));

        var logDirectory = Path.Combine(
            projectRoot,
            "artifacts",
            "logs");

        Directory.CreateDirectory(logDirectory);

        var timestamp =
            DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

        CurrentLogFile = Path.Combine(
            logDirectory,
            $"bdd-test_{timestamp}.log");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File(
                CurrentLogFile,
                outputTemplate:
                "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} " +
                "[{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        _isInitialised = true;

        Log.Information("BDD test logging started");
        Log.Information("Log file: {LogFile}", CurrentLogFile);
    }

    public static void Information(
        string message,
        params object[] values)
    {
        Log.Information(message, values);
    }

    public static void Warning(
        string message,
        params object[] values)
    {
        Log.Warning(message, values);
    }

    public static void Error(
        Exception exception,
        string message,
        params object[] values)
    {
        Log.Error(exception, message, values);
    }

    public static void Close()
    {
        if (!_isInitialised)
        {
            return;
        }

        Log.Information("BDD test logging finished");

        Log.CloseAndFlush();

        _isInitialised = false;
    }
}