using System.Text.Json.Serialization;

namespace Front.ClientsDomain.Responses.Search
{
    public class SearchUserResponse
    {
        [JsonPropertyName("isFoundUsers")]
        public bool IsFoundUsers { get; set; }
        [JsonPropertyName("searchUserResults")]
        public List<SearchUserModel> SearchUserResults { get; set; }  = new List<SearchUserModel>();

        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
    public class SearchUserModel
    {
        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }
        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("isHadDialog")]
        public bool IsHadDialog { get; set; }
    }
}
