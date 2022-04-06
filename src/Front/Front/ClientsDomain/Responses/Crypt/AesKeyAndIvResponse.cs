using System.Text.Json.Serialization;

namespace Front.ClientsDomain.Responses.Crypt
{
    public class AesKeyAndIvResponse
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("iv")]
        public string Iv { get; set; }
    }
}
