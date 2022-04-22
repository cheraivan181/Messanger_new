using System.Runtime.CompilerServices;
using System.Text;

namespace Core.BinarySerializer;

public class BinaryMessangerSerializer : BinaryBase, IDisposable
{
    private const int InitialStreamCapacity = 1024;
    private const int MaxStackLimit = 1024;
    private static Encoding s_utf8NoBom = new UTF8Encoding(false);
    private static int s_intSize = sizeof(int);
    private readonly BinaryWriter _binaryWriter;
    private bool _disposed;

    public BinaryMessangerSerializer(BinaryWriter binaryWriter) : this(binaryWriter, InitialStreamCapacity)
    {
    }
    
    public BinaryMessangerSerializer(BinaryWriter binaryWriter, int initialStreamCapacity) :
        base(binaryWriter.BaseStream, initialStreamCapacity)
    {
        _binaryWriter = binaryWriter;
    }

    public int StreamCapacity
    {
        get
        {
            if (_binaryWriter.BaseStream is MemoryStream memoryStream)
                return memoryStream.Capacity;
            return 0;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)] 
    public BinaryMessangerSerializer Write(bool value)
    {
        _binaryWriter.Write(value);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BinaryMessangerSerializer Write(int value)
    {
        _binaryWriter.Write((uint) value);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BinaryMessangerSerializer Write(uint value)
    {
        _binaryWriter.Write(value);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BinaryMessangerSerializer Write(long value)
    {
        _binaryWriter.Write(value);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BinaryMessangerSerializer Write(ulong value)
    {
        _binaryWriter.Write(value);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BinaryMessangerSerializer Write(float value)
    {
        _binaryWriter.Write(value);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BinaryMessangerSerializer Write(double value)
    {
        _binaryWriter.Write(value);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BinaryMessangerSerializer Write(short value)
    {
        _binaryWriter.Write(value);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BinaryMessangerSerializer Write(ushort value)
    {
        _binaryWriter.Write(value);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BinaryMessangerSerializer Write(string value)
    {
        WriteString(value);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BinaryMessangerSerializer Write(Guid value)
    {
        Span<byte> buffer = value.ToByteArray();
        WriteWithHeader(buffer, buffer.Length);

        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BinaryMessangerSerializer Write(DateTime dateTime)
    {
        string stringDateTime = dateTime.ToString();
        WriteString(stringDateTime);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BinaryMessangerSerializer Write(byte[] array)
    {
        if (array == null || array.Length == 0)
        {
            Write(EmptyArrayHeader);
        }
        else
        {
            Write(array.Length);
            _binaryWriter.Write(array);
        }

        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void WriteWithHeader(Span<byte> buffer, int lenght)
    {
        _binaryWriter.Write(lenght);
        _binaryWriter.Write(buffer);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void WriteString(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Write(EmptyArrayHeader);
            return;
        }

        Span<byte> buffer= value.Length <= MaxStackLimit
            ? stackalloc byte[value.Length]
            : new byte[value.Length];

        var lenght = s_utf8NoBom.GetBytes(value, buffer);
        WriteWithHeader(buffer, lenght);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteResult(Buffer buffer)
    {
        if (_binaryWriter.BaseStream.Length == 0)
            return;

        var position = (int)_binaryWriter.BaseStream.Position;
        
        _binaryWriter.BaseStream.Flush();
        _binaryWriter.BaseStream.Position = 0;

        if (buffer.Capacity < position)
        {
            buffer.Resize(position);
        }

        _binaryWriter.BaseStream.Read(buffer.Data, 0, position);
        buffer.SetSize(position);
        _binaryWriter.BaseStream.Position = position;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteResult(Stream outStream)
    {
        if (_binaryWriter.BaseStream.Length == 0)
        {
            return;
        }

        var position = _binaryWriter.BaseStream.Position;
        _binaryWriter.BaseStream.Flush();
        _binaryWriter.BaseStream.Position = 0;
        _binaryWriter.BaseStream.CopyTo(outStream);
        outStream.Position = 0;
        _binaryWriter.BaseStream.Position = position;
    }
    
    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;
        _binaryWriter?.Dispose();
        GC.SuppressFinalize(this);
    }
    
    ~BinaryMessangerSerializer()
    {
        Dispose();
    }
}