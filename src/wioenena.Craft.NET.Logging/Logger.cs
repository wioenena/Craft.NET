namespace wioenena.Craft.NET.Logging;

public class Logger(LogLevel level) {
    public LogLevel Level { get; } = level;

    public virtual void Log(LogLevel level, string message) {
        if (level >= this.Level) {
            Console.WriteLine(FormatMessage(level, message));
        }
    }

    public void Debug(string message) => this.Log(LogLevel.Debug, message);
    public void Info(string message) => this.Log(LogLevel.Info, message);
    public void Warn(string message) => this.Log(LogLevel.Warn, message);
    public void Error(string message) => this.Log(LogLevel.Error, message);
    public void Fatal(string message) => this.Log(LogLevel.Fatal, message);

    private static string FormatMessage(LogLevel level, string message) => $"[{DateTime.Now:dd-MM-yyyy HH:mm:ss}] [{level}] {message}";
}

public enum LogLevel {
    Debug,
    Info,
    Warn,
    Error,
    Fatal
}
