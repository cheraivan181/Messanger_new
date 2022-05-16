using System.Runtime.CompilerServices;
using System.Text;

namespace Core.BinarySerializer;

public class BinaryMessangerDeserializer : BinaryBase, IDisposable
{
    public long StreamLength => _binaryReader.BaseStream.Length;

    public int StreamCapacity
    {
        get
        {
            if (_binaryReader.BaseStream is MemoryStream memoryStream)
            {
                return memoryStream.Capacity;
            }

            return 0;
        }
    }

    private const int InitialStreamCapacity = 1024;
    private const int MaxStackLimit = 1024;
    private static Encoding s_utf8NoBom = new UTF8Encoding(false);
    private static int s_sizeOfInt = sizeof(int);
    private BinaryReader _binaryReader;
    private bool _disposed;

    public BinaryMessangerDeserializer(BinaryReader reader, int streamCapacity) : base(reader.BaseStream,
        streamCapacity)
    {
        _binaryReader = reader;
    }

    public BinaryMessangerDeserializer(BinaryReader reader) : this(reader, InitialStreamCapacity)
    {

    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool ReadBoolean()
    {
        return _binaryReader.ReadBoolean();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double ReadDouble()
    {
        return _binaryReader.ReadDouble();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float ReadSingle()
    {
        return _binaryReader.ReadSingle();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ReadInt64()
    {
        return _binaryReader.ReadInt64();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ulong ReadUInt64()
    {
        return _binaryReader.ReadUInt64();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ReadUInt32()
    {
        return _binaryReader.ReadUInt32();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadInt32()
    {
        return _binaryReader.ReadInt32();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public decimal ReadDecimal()
    {
        return _binaryReader.ReadDecimal();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public short ReadInt16()
    {
        return (short) ReadUInt16();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DateTime ReadDateTime()
    {
        var date = ReadString();
        return DateTime.Parse(date);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ushort ReadUInt16()
    {
        return _binaryReader.ReadUInt16();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int[] ReadArrayOfInt32()
    {
        var capacity = ReadInt32();
        if (capacity == EmptyArrayHeader)
        {
            return null;
        }

        var result = new int[capacity];
        GetIntArrayElements(capacity, result);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte[] ReadArrayOfBytes()
    {
        var length = _binaryReader.ReadInt32();
        if (length == EmptyArrayHeader)
        {
            return null;
        }

        return _binaryReader.ReadBytes(length);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GetIntArrayElements(int capacity, int[] result)
    {
        var bufferSize = capacity * s_sizeOfInt;

        if (bufferSize <= MaxStackLimit)
        {
            for (int i = 0; i < capacity; i++)
            {
                result[i] = _binaryReader.ReadInt32();
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string[] ReadArrayOfString()
    {
        int capacity = ReadInt32();
        if (capacity == EmptyArrayHeader)
        {
            return null;
        }

        return ReadStringArrayElements(capacity);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public List<T> ReadListOfObjects<T>() where T : class, ISerializableMessage, new()
    {
        var length = _binaryReader.ReadInt32();
        if (length == 0)
        {
            return null;
        }

        var result = new List<T>();

        for (var i = 0; i < length; i++)
        {
            var obj = Deserialize<T>();
            result.Add(obj);
        }

        return result;
    }

    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public List<Guid> ReadListOfGuids()
    {
        var lenght = _binaryReader.ReadInt32();
        if (lenght == 0)
        {
            return null;
        }

        var result = new List<Guid>(lenght);
        for (var i = 0; i < lenght; i++)
        {
            result.Add(ReadGuid());
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ReadString()
    {
        int strLength = ReadInt32();
        if (strLength == EmptyArrayHeader)
        {
            return null;
        }
        else if (strLength == 0)
        {
            return string.Empty;
        }

        Span<byte> buffer = strLength <= MaxStackLimit ? stackalloc byte[strLength] : new byte[strLength];
        ReadBytes(buffer);
        return s_utf8NoBom.GetString(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ReadBytes(Span<byte> buffer) => _binaryReader.BaseStream.Read(buffer);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private string[] ReadStringArrayElements(int capacity)
    {
        var result = new string[capacity];
        for (int i = 0; i < capacity; i++)
        {
            result[i] = ReadString();
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Deserialize<T>() where T : class, ISerializableMessage, new()
    {
        if (_binaryReader.BaseStream.Position != 0)
        {
            var objectStatus = TryReadEnum<ObjectStatus>();
            if (objectStatus == ObjectStatus.Null)
            {
                return null;
            }
        }

        var o = new T();
        o.Deserialize(this);
        return o;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Dictionary<ushort, uint> ReadUshortUintDictionary()
    {
        var length = _binaryReader.ReadInt32();
        if (length == EmptyArrayHeader)
        {
            return null;
        }

        var result = new Dictionary<ushort, uint>();
        for (int i = 0; i < length; i++)
        {
            var k = _binaryReader.ReadUInt16();
            var v = _binaryReader.ReadUInt32();
            result.Add(k, v);
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe Guid ReadGuid()
    {
        var lenght = _binaryReader.ReadInt32();
        if (lenght == 0)
            return default;

        const int guidSize = 16;
        Span<byte> buffer = _binaryReader.ReadBytes(guidSize);
 
        var result = new Guid(buffer);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe Guid? ReadNullableGuid()
    {
        var lenght = _binaryReader.ReadInt32();
        if (lenght == EmptyArrayHeader)
            return null;

        const int guidSize = 16;
        Span<byte> buffer = _binaryReader.ReadBytes(guidSize);
        
        var result = new Guid(buffer);
        return result;
    }

    public T Deserialzie<T>() where T : class, ISerializableMessage, new()
    {
        if (_binaryReader.BaseStream.Position != 0)
        {
            var objStatus = TryReadEnum<ObjectStatus>();
            if (objStatus == null)
            {
                return null;
            }
        }

        var o = new T();
        o.Deserialize(this);
        return o;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private TEnum TryReadEnum<TEnum>() where TEnum : struct, Enum
    {
        var status = ReadInt32();
        return (TEnum) Enum.ToObject(typeof(TEnum), status);
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        _binaryReader?.Dispose();
        GC.SuppressFinalize(this);
    }
    
    ~BinaryMessangerDeserializer()
    {
        Dispose();
    }
}
public enum ObjectStatus
{
    Null = -1,
    Existed = 1
}
