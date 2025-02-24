using wioenena.Craft.NET.IO;

namespace wioenena.Craft.NET.Types.Packet.Client;

public sealed class PingRequestPacket(long timestamp) : IDeserializablePacket<PingRequestPacket> {
    public const int Id = 0x01;

    public long Timestamp { get; } = timestamp;

    public static PingRequestPacket FromReader(CraftReader reader) => new(reader.ReadVarLong());
}
