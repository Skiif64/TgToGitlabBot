using Bot.Integration.Gitlab.Requests.Base;
using Bot.Integration.Gitlab.Responses;
using System.Net.Http.Json;

namespace Bot.Integration.Gitlab.Requests
{
    internal class GetFileRequest : RequestBase<GetFileResponse>
    {  
        public override HttpMethod Method { get; } = HttpMethod.Get;
        public override string Url { get; } = "/api/v4/projects/{0}/repository/files/{1}?ref={2}";        
        public GetFileRequest(string filePath, GitLabOptions options) : base(options)
        {           
            var project = options.Project.Replace("/", "%2F");
            filePath = filePath.Replace("/", "%2F").Replace(".", "%2E");
            Url = string.Format(Url, project, filePath, options.BranchName);
            Headers.Add("--head", null);
        }

        public override HttpContent? ToHttpContent() => null;
    }
}
