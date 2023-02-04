using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bot.Integration.Gitlab.Contracts;

public class CreateFileResponse
{
    public bool IsSuccess { get; }
    //TODO: Success response
    //public ErrorResponseMessage? ErrorMessage { get; }


    private CreateFileResponse()
    {
        IsSuccess = true;
    }

    private CreateFileResponse(bool success)
    {
        IsSuccess = success;        
    }

    public static CreateFileResponse Success() => new CreateFileResponse();
    //TODO: refactor this
    public static CreateFileResponse Error(string json) => new CreateFileResponse(false);
    
}
