using System.Buffers.Binary;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using wioenena.Craft.NET.IO;
using wioenena.Craft.NET.Logging;
using wioenena.Craft.NET.Types;
using wioenena.Craft.NET.Types.Packet.Client;
using wioenena.Craft.NET.Types.Packet.Server;
using wioenena.Craft.NET.Types.Packet.Server.StatusResponse;

namespace wioenena.Craft.NET.Server;

public class Session : IDisposable {
    private readonly CraftServer server;
    private readonly TcpClient client;
    private readonly CraftReader reader;
    private readonly CraftWriter writer;
    private readonly Logger logger;
    private bool disposed;
    private State state = State.Handshake;
    private bool IsConnected => this.client.Connected;

    public Session(CraftServer server, TcpClient client) {
        this.server = server;
        this.client = client;
        this.logger = new(nameof(Session), server.Options.LogLevel);
        var stream = this.client.GetStream();
        this.reader = new CraftReader(stream);
        this.writer = new CraftWriter(stream);
    }

    public void Handle() {
        try {
            while (this.IsConnected) {
                var packetLength = this.reader.ReadVarInt();
                var packetId = this.reader.ReadVarInt();
                if (this.state == State.Handshake && packetId == 0x00 /* Client handshake request */) {
                    this.logger.Debug("Client handshake request");
                    this.handleHandshake();
                } else if (this.state == State.Status && packetId == 0x00 /* Client status request */) {
                    this.logger.Debug("Client status request");
                    this.handleStatusRequest();
                } else if (this.state == State.Status && packetId == 0x01 /* Client ping request */) {
                    this.logger.Debug("Client ping request");
                    this.handlePingRequest();
                    this.Close();
                }
            }
        } catch (Exception e) {
            this.logger.Error(e.Message);
        }
    }

    private void handleHandshake() {
        var packet = HandshakePacket.FromReader(this.reader);
        if (
            packet.ProtocolVersion != CraftServer.protocolVersion ||
            packet.ServerAddress != this.server.Options.Address.ToString() ||
            packet.ServerPort != this.server.Options.Port
        ) {
            this.logger.Warn("Client mismatched handshake packet");
            // TODO Send a disconnect packet
            return;
        }

        this.state = packet.NextState switch {
            State.Status => State.Status,
            State.Login => State.Login,
            _ => throw new InvalidOperationException("Invalid next state") // TODO: Send a disconnect packet
        };
    }

    private void handleStatusRequest() {
        var statusResponsePacket = new StatusResponsePacket(new("1.21.4", 769), new(this.server.Options.maxPlayer, 0), new(this.server.Options.description));
        this.writer.WritePacket(StatusResponsePacket.Id, statusResponsePacket.SerializeToBytes());
    }

    private void handlePingRequest() {
        var pongResponsePacket = new PongResponsePacket();
        this.writer.WritePacket(PongResponsePacket.Id, pongResponsePacket.SerializeToBytes());
    }

    private void Close() {
        this.logger.Debug("Closing session");
        this.client.Close();
        this.Dispose();
    }


    protected virtual void Dispose(bool disposing) {
        if (!this.disposed) {
            if (disposing) {
                this.client.Close();
                this.client.Dispose();
                this.reader.Dispose();
                this.writer.Dispose();
            }

            this.disposed = true;
        }
    }

    public void Dispose() {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }
}
