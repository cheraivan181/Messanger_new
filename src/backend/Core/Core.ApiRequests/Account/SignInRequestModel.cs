using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Core.ApiRequests.Account
{
    public class SignInRequestModel
    {
        [Required]
        [MinLength(6)]
        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [Required]
        [MinLength(6)]
        [JsonPropertyName("password")]
        public string Password { get; set; }
        
        [JsonPropertyName("sessionId")]
        public long? SessionId { get; set; }
    }
}
