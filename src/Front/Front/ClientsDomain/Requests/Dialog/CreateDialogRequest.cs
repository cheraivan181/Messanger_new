using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Front.ClientsDomain.Requests.Dialog
{
    public class CreateDialogRequest
    {
        [Required]
        [JsonPropertyName("requestUserId")]
        public Guid RequestUserId { get; set; }
    }
}
