namespace wioenena.Craft.NET.Types.Packet;

public interface ISerializablePacket {
    /// <summary>
    /// Serializes the packet data into a byte array.
    /// </summary>
    /// <returns>A byte array representing the serialized packet data.</returns>
    public abstract byte[] SerializeToBytes();
}
