using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Front.Clients.Interfaces;
using Front.Domain.Requests;
using Front.Domain.Responses;
using Front.Domain.Responses.Base;

namespace Front.Clients.Implementations;

public class RestClient : IRestClient
{
    private readonly HttpClient _httpClient;
    
    private readonly ILocalStorageService _localStorageService;
   
    private static SucessResponse<AuthOptions> _authOptions;
    private static ILogger _logger;

    public RestClient(IHttpClientFactory httpClientFactory,
        ILocalStorageService localStorageService,
        ILoggerFactory loggerFactory)
    {
        _httpClient = httpClientFactory.CreateClient("CoreClient");
        _localStorageService = localStorageService;
        _logger = loggerFactory.CreateLogger<RestClient>();
    }

    public RestClient(IHttpClientFactory httpClientFactory,
        ILoggerFactory loggerFactory,
        string clientName)
    {
        _httpClient = httpClientFactory.CreateClient(clientName);
        _logger = loggerFactory.CreateLogger<RestClient>();
    }

    public async Task<RestClientResponse<T>> MakeHttpRequestAsync<T>(string uri, HttpMethod httpMethod, 
        HttpStatusCode exceptedStatusCode = HttpStatusCode.OK, object data = null) where T:class
    {
        var response = new RestClientResponse<T>();
        
        await SetHttpClientHeaders();
        
        var request = new HttpRequestMessage();
        request.RequestUri = new Uri($"{_httpClient.BaseAddress}/{uri}");
        request.Method = httpMethod;

        if (data != null)
        {
            request.Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        }

        var responseMessage = await _httpClient.SendAsync(request);
        response.StatusCode = response.StatusCode;
        
        var responseStream = await responseMessage.Content.ReadAsStreamAsync();
        if (responseMessage.StatusCode == exceptedStatusCode)
        {
            var responseModel = await JsonSerializer.DeserializeAsync<SucessResponse<T>>(responseStream);
            response.IsSucess = true;
            response.SucessResponse = responseModel;

            return response;
        }
        
        try
        {
            var errorResponse = await JsonSerializer.DeserializeAsync<ErrorResponse>(responseStream);
            response.IsSucess = false;
            response.IsCanHandleError = true;
            response.ErrorResponse = errorResponse;

            return response;
        }   
        catch (Exception ex)
        {
            response.IsSucess = false;
            response.IsCanHandleError = false;
        }

        return response;
    }

    public async Task<RestClientResponseWithoutResult> MakeHttpRequestWithoutResponseAsync(string uri, HttpMethod httpMethod,
        HttpStatusCode exceptedStatusCode = HttpStatusCode.OK, object data = null)
    {
        var result = new RestClientResponseWithoutResult();
        
        await SetHttpClientHeaders();
        
        var request = new HttpRequestMessage();
        request.RequestUri = new Uri(uri);
        request.Method = HttpMethod.Post;

        if (data != null)
        {
            request.Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        }

        var responseMessage = await _httpClient.SendAsync(request);
        result.StatusCode = responseMessage.StatusCode;

        if (result.StatusCode == exceptedStatusCode)
        {
            result.IsSucess = true;
        }

        var responseStream = await responseMessage.Content.ReadAsStreamAsync();
        try
        {
            var errorResponse = await JsonSerializer.DeserializeAsync<ErrorResponse>(responseStream);
            result.IsSucess = false;
            result.IsCanHandleError = true;
            result.ErrorResponse = errorResponse;
        }
        catch
        {
            result.IsSucess = false;
            result.IsCanHandleError = false;
        }

        return result;
    }
    
    private async Task SetHttpClientHeaders()
    {
        await SetAuthOptions();
        
        var acessToken = await _localStorageService.GetItemAsStringAsync(Constants.AcessTokenName);
        
        if (!DateTime.TryParse(await _localStorageService.GetItemAsStringAsync(Constants.TokenUpdateTime),
            out DateTime timeToGetAcessToken))
        {
            timeToGetAcessToken = DateTime.Now;
        }

        bool isNeedUpdateAcessToken = string.IsNullOrEmpty(acessToken)
            || timeToGetAcessToken.AddMinutes(_authOptions.Response.AcessTokenMinutes - 1) <= DateTime.Now;
        
        if (isNeedUpdateAcessToken)
        {
            var refreshToken = await _localStorageService.GetItemAsStringAsync(Constants.RefreshTokenName);
            if (string.IsNullOrEmpty(refreshToken))
                return;
            
            var updateTokenRefreshModel = new UpdateRefreshTokenRequest()
            {
                RefreshToken = refreshToken
            };

            var updateTokenRequestMessage = new HttpRequestMessage();
            updateTokenRequestMessage.Content = new StringContent(JsonSerializer.Serialize(updateTokenRefreshModel),
                Encoding.UTF8, "application/json");
            updateTokenRequestMessage.Method = HttpMethod.Post;
            updateTokenRequestMessage.RequestUri = new Uri($"{_httpClient.BaseAddress}/Account/updatetoken");
            
            var updateTokenResponseMessage = await _httpClient.SendAsync(updateTokenRequestMessage);
            if (updateTokenResponseMessage.StatusCode != HttpStatusCode.OK)
                return;

            var responseStream = await updateTokenResponseMessage.Content.ReadAsStreamAsync();
            var responseModel = await JsonSerializer.DeserializeAsync<SucessResponse<UpdateRefreshTokenResponse>>
                (responseStream);

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", responseModel.Response.AcessToken);

            await _localStorageService.SetItemAsStringAsync(Constants.TokenUpdateTime, DateTime.Now.ToString());
            await _localStorageService.SetItemAsStringAsync(Constants.AcessTokenName, 
                responseModel.Response.AcessToken);
            await _localStorageService.SetItemAsStringAsync(Constants.RefreshTokenName,
                responseModel.Response.RefreshToken);
        }
        else
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", acessToken);
        }
    }

    private async Task SetAuthOptions()
    {
        if (_authOptions != null)
            return;

        var request = new HttpRequestMessage();

        request.Method = HttpMethod.Get;
        request.RequestUri = new Uri($"{_httpClient.BaseAddress}/Account/getTokenLifeTimeOptions");

        var responseMessage = await _httpClient.SendAsync(request);
        responseMessage.EnsureSuccessStatusCode();
        
        var content = await responseMessage.Content.ReadAsStringAsync();
        _logger.LogDebug(content);

        var responseStream = await responseMessage.Content.ReadAsStreamAsync();
        _authOptions = await JsonSerializer.DeserializeAsync<SucessResponse<AuthOptions>>(responseStream);
    }
}