using System.Net;
using wioenena.Craft.NET.Logging;

namespace wioenena.Craft.NET.Server;

public sealed record CraftServerOptions(
    IPAddress Address,
    ushort Port,
    int maxPlayer,
    string description,
    LogLevel LogLevel = LogLevel.Debug
);
