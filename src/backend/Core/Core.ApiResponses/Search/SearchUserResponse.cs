using System.Text.Json.Serialization;
using Core.SearchServices.Domain;

namespace Core.ApiResponses.Search;

public class SearchUserResponse
{
    [JsonPropertyName("isFoundUsers")]
    public bool IsFoundUsers { get; set; }
    [JsonPropertyName("searchUserResults")]
    public List<SearchUserModel> SearchUserResults { get; set; }

    [JsonPropertyName("count")]
    public int Count
    {
        get
        {
            if (SearchUserResults != null)
                return SearchUserResults.Count;
            return 0;
        }
    }
}