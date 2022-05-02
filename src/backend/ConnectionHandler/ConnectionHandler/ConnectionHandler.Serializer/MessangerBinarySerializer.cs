using System.Runtime.CompilerServices;

namespace Core.BinarySerializer;

public static class MessangerBinarySerializer
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToBinaryMessage(this ISerializableMessage message) 
    {
        using (var writerStream = new MemoryStream())
        {
            using (var binaryWriter = new BinaryWriter(writerStream))
            {
                using (var binarySerializer = new BinaryMessangerSerializer(binaryWriter))
                {
                    message.Serialize(binarySerializer);
                    Span<byte> buffer = binarySerializer.StreamCapacity > 1024
                        ? stackalloc byte[binarySerializer.StreamCapacity]
                        : new byte[binarySerializer.StreamCapacity];
                    
                    binarySerializer.WriteResult(ref buffer);
                    
                    return Convert.ToBase64String(buffer);
                }
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T FromBinaryMessage<T>(this string data) where T : class, ISerializableMessage, new()
    {
        var buffer = Convert.FromBase64String(data);
        return FromBinaryMessage<T>(buffer);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T FromBinaryMessage<T>(this byte[] buffer) where T: class, ISerializableMessage, new()
    {
        using (var stream = new MemoryStream())
        {
            stream.Write(buffer, 0, buffer.Length);
            stream.Position = 0;
            
            using (var binaryReader = new BinaryReader(stream))
            {
                using (var binaryDeserializer = new BinaryMessangerDeserializer(binaryReader))
                {
                    var result = binaryDeserializer.Deserialize<T>();
                    return result;
                }
            }
        }
    }
}