using Microsoft.Extensions.Logging;

namespace MedicalRecordsSystem.Utils;

public class FileLogger(string filePath) : ILogger
{
    private readonly string _filePath = filePath;
    private static readonly object _lock = new();

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        ArgumentNullException.ThrowIfNull(formatter);

        var logRecord = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {formatter(state, exception)}{Environment.NewLine}";

        lock (_lock)
        {
            File.AppendAllText(_filePath, logRecord);
        }
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }
}

public class FileLoggerProvider : ILoggerProvider
{
    private readonly string _filePath;

    public FileLoggerProvider()
    {
        _filePath = "logs.txt";
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new FileLogger(_filePath);
    }

    public void Dispose()
    {
    }
}
