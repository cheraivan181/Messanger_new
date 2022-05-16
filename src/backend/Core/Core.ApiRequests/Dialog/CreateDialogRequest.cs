using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Core.ApiRequests.Dialog;

public class CreateDialogRequest
{
    [JsonPropertyName("requestUserId")]
    public Guid RequestUserId { get; set; }
}