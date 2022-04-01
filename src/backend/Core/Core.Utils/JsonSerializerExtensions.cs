using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Utils.Json;
using StackExchange.Redis;

namespace Core.Utils
{
    public static class JsonSerializerExtensions
    {
        public static JsonSerializerOptions options = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            Converters =
            {
                new RsaParametersConverter()
            }
        };
        public static string ToJson(this object obj) =>
            JsonSerializer.Serialize(obj, options);
        
        public static T FromJson<T>(this string json) =>
            JsonSerializer.Deserialize<T>(json, options);

        public static T FromJson<T>(this RedisValue value) =>
            JsonSerializer.Deserialize<T>(value.ToString());
    }
}
