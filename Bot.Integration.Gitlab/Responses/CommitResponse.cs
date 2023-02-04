namespace Bot.Integration.Gitlab.Responses;

public class CommitResponse
{
    public bool IsSuccess { get; }
    //TODO: Success response
    //public ErrorResponseMessage? ErrorMessage { get; }


    private CommitResponse()
    {
        IsSuccess = true;
    }

    private CommitResponse(bool success)
    {
        IsSuccess = success;
    }

    public static CommitResponse Success() => new CommitResponse();
    //TODO: refactor this
    public static CommitResponse Error(string json) => new CommitResponse(false);

}
