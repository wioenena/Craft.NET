using wioenena.Craft.NET.IO;

namespace wioenena.Craft.NET.Types.Packet.Client;

/// <summary>
/// Sent by the client to initiate a connection to the server.
/// </summary>
/// <param name="protocolVersion">The protocol version of the client.</param>
/// <param name="serverAddress">The address of the server.</param>
/// <param name="serverPort">The port of the server.</param>
/// <param name="nextState">The next state the client wants to switch to.</param>
public sealed class HandshakePacket(int protocolVersion, string serverAddress, ushort serverPort, State nextState) : IDeserializablePacket<HandshakePacket> {
    public const int Id = 0x00;
    public int ProtocolVersion { get; set; } = protocolVersion;
    public string ServerAddress { get; set; } = serverAddress;
    public ushort ServerPort { get; set; } = serverPort;
    public State NextState { get; set; } = nextState;

    public static HandshakePacket FromReader(CraftReader reader)
        => new(reader.ReadVarInt(), reader.ReadVarString(), reader.ReadUShort(), (State)reader.ReadVarInt());
}
