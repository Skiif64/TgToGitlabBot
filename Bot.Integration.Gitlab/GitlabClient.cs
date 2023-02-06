using Bot.Integration.Gitlab.Abstractions;
using Bot.Integration.Gitlab.Requests.Base;
using System.Text.Json;

namespace Bot.Integration.Gitlab;

public class GitlabClient : IGitlabClient
{
    private const string BASE_URL = "https://gitlab.com";
    private readonly HttpClient _httpClient;
    private readonly IExceptionParser _exceptionParser;

    public GitlabClient(IExceptionParser exceptionParser)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(BASE_URL);
        _exceptionParser = exceptionParser;
    }

    public async Task<TResponse> SendAsync<TResponse>(IGitlabRequest<TResponse> request, CancellationToken cancellationToken)
    {        
        var requestMessage = new HttpRequestMessage
        {
            RequestUri = new Uri(request.Url, UriKind.Relative),
            Method = request.Method,
            Content = request.ToHttpContent()
        }; 
        if(request.Headers is not null && request.Headers.Count != 0)
            foreach(var (key, value) in request.Headers)
                requestMessage.Headers.Add(key, value);

        var response = await _httpClient.SendAsync(requestMessage, cancellationToken);
        if (!response.IsSuccessStatusCode)
            throw _exceptionParser.Parse(await response.Content.ReadAsStringAsync(cancellationToken));

        using var responseContent = await response.Content.ReadAsStreamAsync(cancellationToken);
        return await JsonSerializer.DeserializeAsync<TResponse>(responseContent, cancellationToken: cancellationToken)
            ?? throw new HttpRequestException("Error occured", null, response.StatusCode); // TODO: ???
    }
}
