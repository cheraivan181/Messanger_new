using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.Converters
{
    public class IntPtrConverter : JsonConverter<IntPtr>
    {
        public override IntPtr Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var result = IntPtr.Parse(reader.GetString());
            return result;
        }

        public override void Write(Utf8JsonWriter writer, IntPtr value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
