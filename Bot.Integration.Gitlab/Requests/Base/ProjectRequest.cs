﻿using System.Net.Http.Json;

namespace Bot.Integration.Gitlab.Requests.Base;

internal abstract class ProjectRequest : IGitlabRequest
{
    public abstract HttpMethod Method { get; }
    public abstract string Url { get; }
    public string? AccessToken { get; }

    public ProjectRequest(GitLabOptions options)
    {        
        AccessToken = options.AccessToken;
    }

    public abstract HttpContent? ToHttpContent();

}
