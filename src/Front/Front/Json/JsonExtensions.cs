using System.Text.Json;
using System.Text.Json.Serialization;

namespace Front.Json
{
    public static class JsonExtensions
    {
        public static JsonSerializerOptions options = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            Converters =
            {
                new RsaParameterConverter()
            }
        };

        public static string ToJson(this object obj) =>
            JsonSerializer.Serialize(obj, options);
        public static T FromJson<T>(this string json) =>
            JsonSerializer.Deserialize<T>(json);

        public static ValueTask<T> FromJson<T>(this Stream stream) =>
            JsonSerializer.DeserializeAsync<T>(stream);
    }
}
