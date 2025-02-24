using wioenena.Craft.NET.IO;

namespace wioenena.Craft.NET.Types.Packet.Client;

/// <summary>
/// Sent by the client to initiate a connection to the server.
/// </summary>
/// <param name="protocolVersion">The protocol version of the client.</param>
/// <param name="serverAddress">The address of the server.</param>
/// <param name="serverPort">The port of the server.</param>
/// <param name="nextState">The next state the client wants to switch to.</param>
public class HandshakePacket(int protocolVersion, string serverAddress, ushort serverPort, State nextState) : BasePacket(0x00), IDeserializablePacket<HandshakePacket> {
    public int ProtocolVersion { get; } = protocolVersion;
    public string ServerAddress { get; } = serverAddress;
    public ushort ServerPort { get; } = serverPort;
    public State NextState { get; } = nextState;

    public static HandshakePacket FromReader(CraftReader reader) =>
        new(reader.ReadVarInt(), reader.ReadVarString(), reader.ReadUShort(), (State)reader.ReadVarInt());
}
