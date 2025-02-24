using wioenena.Craft.NET.Logging;

namespace wioenena.Craft.NET.Server;

public class Logger(string tag, LogLevel level) : Logging.Logger(level) {
    public string Tag { get; } = tag;


    public override void Log(LogLevel level, string message) => base.Log(level, $"[{this.Tag}] {message}");
}
