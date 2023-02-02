using Bot.Core.Options;
using Bot.Integration.Gitlab.Abstractions;
using Microsoft.Extensions.Options;

namespace Bot.Integration.Gitlab;

public class GitlabClient : IGitlabClient
{
    private const string BASE_URL = "https://gitlab.com/api/v4";
    private readonly HttpClient _httpClient;

    public GitlabClient()
    {
        _httpClient = new HttpClient();
        
    }

    public async Task SendAsync(IGitlabRequest request, CancellationToken cancellationToken)
    {
        
        var requestMessage = new HttpRequestMessage
        {
            RequestUri = new Uri(BASE_URL+request.Url),
            Method = request.Method,
            Content = request.Content
        };        
        if (request.AccessToken != null)
            requestMessage.Headers.Add("PRIVATE-TOKEN", request.AccessToken);
        var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
    }
}
