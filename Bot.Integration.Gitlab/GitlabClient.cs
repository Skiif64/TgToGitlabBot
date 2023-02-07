using Bot.Integration.Gitlab.Abstractions;
using Bot.Integration.Gitlab.Requests.Base;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Bot.Integration.Gitlab;

public class GitlabClient : IGitlabClient
{
    private const string BASE_URL = "https://gitlab.com";
    private readonly IExceptionParser _exceptionParser;
    private readonly HttpClient _httpClient;
    private readonly GitLabOptions _gitLabOptions;

    public GitlabClient(IExceptionParser exceptionParser, HttpClient httpClient, IOptionsSnapshot<GitLabOptions> gitLabOptions)
    {
        _exceptionParser = exceptionParser;
        _httpClient = httpClient;
        _gitLabOptions = gitLabOptions.Value;
    }

    public async Task<TResponse> SendAsync<TResponse>(IGitlabRequest<TResponse> request, CancellationToken cancellationToken)
    {
        using var requestMessage = new HttpRequestMessage
        {            
            RequestUri = new Uri(new Uri(_gitLabOptions.BaseUrl), request.Url),
            Method = request.Method,
            Content = request.ToHttpContent()
        };
        if (request.Headers is not null && request.Headers.Count != 0)
            foreach (var (key, value) in request.Headers)
                requestMessage.Headers.Add(key, value);         
        using HttpResponseMessage response = await _httpClient.SendAsync(requestMessage, cancellationToken);        

        if (!response.IsSuccessStatusCode)
            throw await _exceptionParser.ParseAsync(response, cancellationToken);       
        using var responseContent = await response.Content.ReadAsStreamAsync(cancellationToken);        
        return await JsonSerializer.DeserializeAsync<TResponse>(responseContent, cancellationToken: cancellationToken)
            ?? throw new HttpRequestException("Error occured while deserializer json", null, response.StatusCode); // TODO: ???
    }
}
