using System.Buffers.Binary;

namespace wioenena.Craft.NET.Types.Packet.Server;

public sealed class PongResponsePacket : ISerializablePacket {
    public const int Id = 0x01;
    public long Timestamp { get; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    public byte[] SerializeToBytes() {
        var buffer = new byte[8];
        BinaryPrimitives.WriteInt64BigEndian(buffer, this.Timestamp);
        return buffer;
    }
}
