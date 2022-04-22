using Microsoft.IO;

namespace Core.BinarySerializer;

public static class MessangerBinarySerializer
{
    public static string ToBinaryMessage(this ISerializableMessage message)
    {
        using (var writerStream = new MemoryStream())
        {
            using (var responseStream = new MemoryStream())
            {
                using (var binaryWriter = new BinaryWriter(writerStream))
                {
                    using (var binarySerializer = new BinaryMessangerSerializer(binaryWriter))
                    {
                        message.Serialize(binarySerializer);

                        binarySerializer.WriteResult(responseStream);
                        Span<byte> buffer = responseStream.GetBuffer();

                        return Convert.ToBase64String(buffer);
                    }
                }
            }
        }
    }

    public static T FromBinaryMessage<T>(this string data) where T : class, ISerializableMessage, new()
    {
        var buffer = Convert.FromBase64String(data);
        return Deserialize<T>(buffer);
    }
    
    public static T Deserialize<T>(this byte[] buffer) where T: class, ISerializableMessage, new()
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