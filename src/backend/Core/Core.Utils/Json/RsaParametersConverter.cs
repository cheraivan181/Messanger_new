using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.Utils.Json;

public class RsaParametersConverter : JsonConverter<RSAParameters>
{
    public override RSAParameters Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new Exception("Incorrect rsa param object");
        
        var rsaParameter = new RSAParameters();
        
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                return rsaParameter;

            var property = reader.GetString();
            reader.Read();
            
            switch (property)
            {
                case nameof(rsaParameter.D):
                    rsaParameter.D = ReadBytes(ref reader);
                    break;
                case nameof(rsaParameter.DP):
                    rsaParameter.DP = ReadBytes(ref reader);
                    break;
                case nameof(rsaParameter.DQ):
                    rsaParameter.DQ = ReadBytes(ref reader);
                    break;
                case nameof(rsaParameter.Exponent):
                    rsaParameter.Exponent = ReadBytes(ref reader);
                    break;
                case nameof(rsaParameter.InverseQ):
                    rsaParameter.InverseQ = ReadBytes(ref reader);
                    break;
                case nameof(rsaParameter.Modulus):
                    rsaParameter.Modulus = ReadBytes(ref reader);
                    break;
                case nameof(rsaParameter.P):
                    rsaParameter.P = ReadBytes(ref reader);
                    break;
                case nameof(rsaParameter.Q):
                    rsaParameter.Q = ReadBytes(ref reader);
                    break;
            }
        }

        return rsaParameter;
    }

    public override void Write(Utf8JsonWriter writer, RSAParameters value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteBase64String(nameof(value.D), value.D);
        writer.WriteBase64String(nameof(value.DP), value.DP);
        writer.WriteBase64String(nameof(value.DQ), value.DQ);
        writer.WriteBase64String(nameof(value.Exponent), value.Exponent);
        writer.WriteBase64String(nameof(value.InverseQ), value.InverseQ);
        writer.WriteBase64String(nameof(value.Modulus), value.Modulus);
        writer.WriteBase64String(nameof(value.P), value.P);
        writer.WriteBase64String(nameof(value.Q), value.Q);
        writer.WriteEndObject();
    }

    private byte[] ReadBytes(ref Utf8JsonReader reader)
    {
        var result = reader.GetBytesFromBase64();
        return result.Length > 0
            ? result
            : null;
    }
}