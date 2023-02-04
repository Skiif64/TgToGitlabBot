using Bot.Integration.Gitlab.Abstractions;
using Bot.Integration.Gitlab.Requests.Base;
using System.Text.Json;

namespace Bot.Integration.Gitlab;

public class GitlabClient : IGitlabClient
{
    private const string BASE_URL = "https://gitlab.com";
    private readonly HttpClient _httpClient;

    public GitlabClient()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(BASE_URL);
    }

    public async Task<TResponse> SendAsync<TResponse>(IGitlabRequest<TResponse> request, CancellationToken cancellationToken)
    {        
        var requestMessage = new HttpRequestMessage
        {
            RequestUri = new Uri(request.Url, UriKind.Relative),
            Method = request.Method,
            Content = request.ToHttpContent()
        };        
        if (!string.IsNullOrEmpty(request.AccessToken))
            requestMessage.Headers.Add("PRIVATE-TOKEN", request.AccessToken);
        var response = await _httpClient.SendAsync(requestMessage, cancellationToken);        
        if (!response.IsSuccessStatusCode)
            return default; //TODO: throw

        using var responseContent = await response.Content.ReadAsStreamAsync(cancellationToken);
        return await JsonSerializer.DeserializeAsync<TResponse>(responseContent, cancellationToken: cancellationToken)
            ?? throw new HttpRequestException("Error occured", null, response.StatusCode); // TODO: ???
    }
}
