using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Front.Domain.Requests;

public class SignUpRequest
{
    [Required]
    [MinLength(6)]
    [JsonPropertyName("userName")]
    public string UserName { get; set; }

    [Required]
    [MinLength(6)]
    [JsonPropertyName("password")]
    public string Password { get; set; }

    [Required]
    [Phone]
    [JsonPropertyName("phone")]
    public string Phone { get; set; }

    [Required]
    [EmailAddress]
    [JsonPropertyName("email")]
    public string Email { get; set; }
}