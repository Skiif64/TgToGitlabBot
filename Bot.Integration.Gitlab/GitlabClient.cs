using Bot.Integration.Gitlab.Abstractions;

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

    public async Task<bool> SendAsync(IGitlabRequest request, CancellationToken cancellationToken)
    {        
        var requestMessage = new HttpRequestMessage
        {
            RequestUri = new Uri(request.Url, UriKind.Relative),
            Method = request.Method,
            Content = request.ToHttpContent()
        };
        var requestContent = await requestMessage.Content.ReadAsStringAsync();
        if (!string.IsNullOrEmpty(request.AccessToken))
            requestMessage.Headers.Add("PRIVATE-TOKEN", request.AccessToken);
        var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        //TODO: add response parsing
        return response.IsSuccessStatusCode;
    }
}
