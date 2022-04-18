using System.Text.Json.Serialization;
using Front.Domain.Dialogs;

namespace Front.ClientsDomain.Responses.Dialog
{
    public class GetDialogResponse
    {
        [JsonPropertyName("dialogs")]
        public List<DialogDomainModel> Dialogs { get; set; }
        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
}
