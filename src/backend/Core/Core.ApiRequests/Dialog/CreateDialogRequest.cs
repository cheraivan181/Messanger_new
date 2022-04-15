using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Core.ApiRequests.Dialog;

public class CreateDialogRequest
{
    [Required]
    [JsonPropertyName("requestUserId")]
    public Guid RequestUserId { get; set; }
}