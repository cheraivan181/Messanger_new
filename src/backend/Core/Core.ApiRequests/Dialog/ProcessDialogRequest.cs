using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Core.ApiRequests.Dialog;

public class ProcessDialogRequest
{
    [Required]
    [JsonPropertyName("requestUserId")]
    public Guid RequestUserId { get; set; }
    
    [Required]
    [JsonPropertyName("isAccept")]
    public bool IsAccept { get; set; }
}