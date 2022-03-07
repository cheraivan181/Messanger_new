using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.Utils
{
    public static class JsonSerializerExtensions
    {
        public static JsonSerializerOptions options = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
        public static string ToJson(this object obj) =>
            JsonSerializer.Serialize(obj, options);
        
        public static T FromJson<T>(this string json) =>
            JsonSerializer.Deserialize<T>(json, options);
    }
}
