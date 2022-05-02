using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
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
    public BinaryMessangerSerializer Write<T>(List<T> models) where T : class, ISerializableMessage
    {
        if (models.Count == 0)
        {
            _binaryWriter.Write(EmptyArrayHeader);
            return this;
        }
        
        _binaryWriter.Write(models.Count);
        foreach (var model in models)
            WriteObject(model);
        
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BinaryMessangerSerializer Write(int value)
    {
        _binaryWriter.Write((uint) value);
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BinaryMessangerSerializer Write(decimal value)
    {
        _binaryWriter.Write(value);
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
    public BinaryMessangerSerializer Write(Guid? value)
    {
        if (value.HasValue)
            return Write(value.Value);
        
        Write(EmptyArrayHeader);
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
    public BinaryMessangerSerializer WriteObject<T>(T obj) where T:class, ISerializableMessage
    {
        if (obj == null)
        {
            Write(ObjectStatus.Null);
            return this;
        }

        Write(ObjectStatus.Existed);
        obj.Serialize(this);
        return this;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public BinaryMessangerSerializer Write(Enum o)
    {
        Write(Convert.ToInt32(o));
        return this;
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
    public void WriteResult(ref Span<byte> buffer)
    {
        if (_binaryWriter.BaseStream.Length == 0)
            return;
        
        var position = (int)_binaryWriter.BaseStream.Position;
        
        _binaryWriter.BaseStream.Flush();
        _binaryWriter.BaseStream.Position = 0;

        using var memoryStream = _binaryWriter.BaseStream as MemoryStream;
        buffer = memoryStream.GetBuffer();
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