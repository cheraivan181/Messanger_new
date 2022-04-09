using System.Text.Json.Serialization;

namespace Front.ClientsDomain.Responses.Search
{
    public class SearchUserResponse
    {
        [JsonPropertyName("isFoundUsers")]
        public bool IsFoundUsers { get; set; }
        [JsonPropertyName("searchUserResults")]
        public List<SearchUserModel> SearchUserResults { get; set; }

        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
    public class SearchUserModel
    {
        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("isHadDialog")]
        public bool IsHadDialog { get; set; }
    }
}
