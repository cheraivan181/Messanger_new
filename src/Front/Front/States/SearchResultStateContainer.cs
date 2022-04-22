using Front.ClientsDomain.Responses.Search;

namespace Front.States
{
    public class SearchResultStateContainer
    {
        private List<SearchUserModel> _searchUserResponse = new List<SearchUserModel>();

        public List<SearchUserModel> SearchUserResponses
        {
            get => _searchUserResponse;
            set
            {
                _searchUserResponse = value;
                NotifyStateChanged();
            }
        }

        public void AddResponse(SearchUserModel model)
        {
            SearchUserResponses.Add(model);
            NotifyStateChanged();
        }

        public event Action? OnChange;
        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}
