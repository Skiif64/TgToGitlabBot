using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bot.Integration.Gitlab.Contracts;

public class GitlabResponse
{
    public bool Success { get; }
    
    public string? ErrorMessage { get; }
    

    private GitlabResponse()
    {
        Success = true;
    }

    private GitlabResponse(string errorMessage)
    {
        Success = false;
        ErrorMessage = errorMessage;
    }

    public static GitlabResponse CreateSuccess() => new GitlabResponse();

    public static GitlabResponse CreateError(string json)
    {
        var response = JsonSerializer.Deserialize<ErrorResponseMessage>(json);
        return new GitlabResponse(response.Message);

    }
}
