using System.Text.Json.Serialization;
using Core.DialogServices.Domain;

namespace Core.ApiResponses.Dialog;

public class GetDialogResponse
{
    [JsonPropertyName("dialogs")]
    public List<DialogDomainModel> Dialogs { get; set; }
    [JsonPropertyName("count")]
    public int Count { get; set; }
}