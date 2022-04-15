using Front.Clients.Interfaces;
using Front.ClientsDomain.Requests.Dialog;
using Front.ClientsDomain.Responses.Dialog;
using Front.Domain.Responses.Base;

namespace Front.Clients.Implementations
{
    public class DialogClient : IDialogClient
    {
        private readonly IRestClient _restClient;

        public DialogClient(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<RestClientResponse<GetDialogResponse>> GetDialogsAsync()
        {
            var response = await _restClient.MakeHttpRequestAsync<GetDialogResponse>("Dialog/getdialogs", HttpMethod.Get);
            return response;
        }

        public async Task<RestClientResponse<CreateDialogResponse>> CreateDialogAsync(Guid requestUserId)
        {
            var requestModel = new CreateDialogRequest()
            {
                RequestUserId = requestUserId
            };

            var result = await _restClient.MakeHttpRequestAsync<CreateDialogResponse>("Dialog/createDialog", HttpMethod.Post, data: requestModel);
            return result;
        }
    }
}
