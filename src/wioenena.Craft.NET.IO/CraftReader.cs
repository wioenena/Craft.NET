using System.Net.Sockets;
using System.Text;

namespace wioenena.Craft.NET.IO;

/// <summary>
/// This class is responsible for processing incoming data from the network,
/// which is in the Minecraft Java Protocol format, and converting it into a
/// format suitable for use in C#.
/// </summary>
/// <remarks>
/// This class acts as an intermediary, ensuring the correct handling and
/// transformation of data between the Minecraft client/server and the C#
/// application. It handles all necessary transformations to make the data
/// compatible with the C# environment, enabling smooth communication between
/// the Minecraft server and your application.
/// </remarks>
/// <param name="stream">The network stream to read data from.</param>
public class CraftReader(NetworkStream stream) : IDisposable {
    private bool disposed;
    private readonly NetworkStream stream = stream;

    /// <summary>
    /// Reads a variable-length integer (VarInt) from the network stream.
    /// </summary>
    /// <returns>
    /// The decoded VarInt value as an integer.
    /// </returns>
    /// <remarks>
    /// A VarInt is a variable-length encoding for integers, commonly used in the
    /// Minecraft protocol to efficiently store integers. This method reads the
    /// incoming data from the network stream and decodes it into an integer.
    /// </remarks>
    /// <exception cref="IOException"/>
    public int ReadVarInt() {
        var value = 0;
        var position = 0;
        byte currentByte;

        while (true) {
            currentByte = (byte)this.stream.ReadByte();
            value |= (currentByte & Constants.SEGMENT_BITS) << position;

            if ((currentByte & Constants.CONTINUE_BIT) == 0)
                break;

            position += 7;

            if (position >= 32)
                throw new IOException("VarInt is too large");
        }

        return value;
    }

    /// <summary>
    /// Reads a variable-length long (VarLong) from the network stream.
    /// </summary>
    /// <returns>
    /// The decoded VarLong value as a long integer.
    /// </returns>
    /// <remarks>
    /// A VarLong is a variable-length encoding for long integers, commonly used in
    /// protocols like Minecraft for efficient storage of large numbers. This method
    /// reads the incoming data from the network stream and decodes it into a long.
    /// </remarks>
    /// <exception cref="IOException"/>
    public long ReadVarLong() {
        long value = 0;
        var position = 0;
        byte currentByte;

        while (true) {
            currentByte = (byte)this.stream.ReadByte();
            value |= (long)(currentByte & Constants.SEGMENT_BITS) << position;

            if ((currentByte & Constants.CONTINUE_BIT) == 0)
                break;

            position += 7;

            if (position >= 64)
                throw new IOException("VarLong is too big");
        }

        return value;
    }

    /// <summary>
    /// Reads a string from the network stream.
    /// </summary>
    /// <returns>
    /// The decoded string value as a <see cref="string"/>.
    /// </returns>
    /// <remarks>
    /// A VarString is a variable-length encoding for strings, commonly used in
    /// protocols like Minecraft for efficient storage of text data. This method
    /// reads the incoming data from the network stream, decodes it, and returns
    /// the result as a string.
    /// </remarks>
    /// <exception cref="IOException"/>
    public string ReadVarString() {
        var length = this.ReadVarInt(); // Read first VarInt to get the length of the string.
        var buffer = new byte[length];
        _ = this.stream.Read(buffer, 0, length);
        return Encoding.UTF8.GetString(buffer);
    }


    /// <summary>
    /// Reads an unsigned 16-bit short (ushort) from the stream.
    /// </summary>
    /// <remarks>
    /// This method reads two bytes from the stream, interprets them as a 16-bit
    /// unsigned integer (ushort), and returns the resulting value. The bytes are
    /// read in big-endian order.
    /// </remarks>
    /// <returns>The 16-bit unsigned short (ushort) value read from the stream.</returns>
    public ushort ReadUShort() {
        var buffer = new byte[2];
        _ = this.stream.Read(buffer, 0, 2);
        return (ushort)((buffer[0] << 8) | buffer[1]);
    }

    protected virtual void Dispose(bool disposing) {
        if (!this.disposed) {
            if (disposing) {
                this.stream?.Dispose();
            }

            this.disposed = true;
        }
    }

    public void Dispose() {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }
}
