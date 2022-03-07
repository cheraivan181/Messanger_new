using Core.Converters;
using System.Text.Json;

namespace Core.JsonSerializerSettings
{
    public class CoreJsonSerializer
    {
        private static JsonSerializerOptions options = new JsonSerializerOptions()
        {
            Converters =
            {
                new IntPtrConverter()
            }
        };

        public static string Serialize(object obj)
        {
            return JsonSerializer.Serialize(obj, options);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConv
        }
    }
}