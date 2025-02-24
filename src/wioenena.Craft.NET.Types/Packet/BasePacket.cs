namespace wioenena.Craft.NET.Types.Packet;

/// <summary>
/// Represents a base class for packets with a packet id.
/// </summary>
/// <remarks>
/// This class is intended to be inherited by other packet types.
/// </remarks>
/// <param name="id">The packet id</param>
public abstract class BasePacket(int id) {
    public int Id { get; } = id;
}
