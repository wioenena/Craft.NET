using System.Text.Json;

namespace wioenena.Craft.NET.Types.Packet.Server.StatusResponse;

public class StatusResponsePacket(
    StatusResponseVersion version,
    StatusResponsePlayers players,
    StatusResponseDescription description
) : ISerializablePacket {
    public const int Id = 0x00;
    public StatusResponseVersion version { get; } = version;
    public StatusResponsePlayers players { get; } = players;
    public StatusResponseDescription description { get; } = description;

    public byte[] SerializeToBytes() => JsonSerializer.SerializeToUtf8Bytes(this);
}

public record StatusResponseVersion(string name, int protocol);
public record StatusResponsePlayers(int max, int online); // TODO: add sample field.
public record StatusResponseDescription(string text);
