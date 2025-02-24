using System.Net.Sockets;
using wioenena.Craft.NET.Logging;

namespace wioenena.Craft.NET.Server;

/// <summary>
/// Represents a server that can be connected to by clients.
/// </summary>
/// <param name="options">The options for the server.</param>
public class CraftServer(CraftServerOptions options) : IDisposable {
    private readonly TcpListener listener = new(options.Address, options.Port);
    private bool disposed;
    public bool IsListening => this.listener.Server.IsBound;
    public CraftServerOptions Options { get; } = options;
    internal const int protocolVersion = 769;
    private readonly Logger logger = new(nameof(CraftServer), options.LogLevel);


    /// <summary>
    /// Start server and listen for incoming connections.
    /// </summary>
    public void Start() {
        this.listener.Start();
        if (this.IsListening)
            this.logger.Info($"Server started on {this.Options.Address}:{this.Options.Port}");

        while (this.IsListening) {
            var session = new Session(this, this.listener.AcceptTcpClient());
            this.logger.Debug("Client connected");
            session.Handle();
        }
    }

    protected virtual void Dispose(bool disposing) {
        if (!this.disposed) {
            if (disposing) {
                this.listener.Stop();
                this.listener.Dispose();
            }

            this.disposed = true;
        }
    }

    public void Dispose() {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }
}
