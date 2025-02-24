
using System.Net.Sockets;
using System.Text;

namespace wioenena.Craft.NET.IO;

public class CraftWriter(NetworkStream stream) : IDisposable {
    private bool disposed;
    private readonly NetworkStream stream = stream;

    /// <summary>
    /// Writes a variable-length integer (VarInt) to the network stream.
    /// </summary>
    /// <param name="value">
    /// The integer value to be encoded as a VarInt and written to the stream.
    /// </param>
    /// <remarks>
    /// A VarInt is a variable-length encoding for integers, commonly used in
    /// protocols like Minecraft for efficient storage of integers. This method
    /// encodes the given integer into the VarInt format and writes it to the
    /// network stream.
    /// </remarks>
    public void WriteVarInt(int value) {
        while (true) {
            if ((value & ~Constants.SEGMENT_BITS) == 0) {
                this.stream.WriteByte((byte)value);
                return;
            }

            this.stream.WriteByte((byte)((value & Constants.SEGMENT_BITS) | Constants.CONTINUE_BIT));
            value >>>= 7;
        }
    }

    /// <summary>
    /// Writes a variable-length long (VarLong) to the network stream.
    /// </summary>
    /// <param name="value">
    /// The long value to be encoded as a VarLong and written to the stream.
    /// </param>
    /// <remarks>
    /// A VarLong is a variable-length encoding for long integers, commonly used in
    /// protocols like Minecraft for efficient storage of large numbers. This method
    /// encodes the given long integer into the VarLong format and writes it to the
    /// network stream.
    /// </remarks>
    public void WriteVarLong(long value) {
        while (true) {
            if ((value & ~Constants.SEGMENT_BITS) == 0) {
                this.stream.WriteByte((byte)value);
                return;
            }

            this.stream.WriteByte((byte)((value & Constants.SEGMENT_BITS) | Constants.CONTINUE_BIT));

            value >>>= 7;
        }
    }

    /// <summary>
    /// Writes a variable-length string to the network stream.
    /// </summary>
    /// <param name="value">
    /// The string value to be encoded as a VarString and written to the stream.
    /// </param>
    /// <remarks>
    /// A VarString is a variable-length encoding for strings, commonly used in
    /// protocols like Minecraft for efficient storage of text data. This method
    /// encodes the given string into the VarString format and writes it to the
    /// network stream.
    /// </remarks>
    public void WriteString(string value) {
        var bytes = Encoding.UTF8.GetBytes(value);
        this.WriteVarInt(bytes.Length); // Write first the length of the string
        this.stream.Write(bytes, 0, bytes.Length);
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
