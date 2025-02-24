using wioenena.Craft.NET.IO;

namespace wioenena.Craft.NET.Types.Packet;

public interface IDeserializablePacket<TPacket> {
    /// <summary>
    /// Deserializes the packet data from the provided reader.
    /// </summary>
    /// <param name="reader">The <see cref="CraftReader"/> used to read the packet data.</param>
    public static abstract TPacket FromReader(CraftReader reader);
}
