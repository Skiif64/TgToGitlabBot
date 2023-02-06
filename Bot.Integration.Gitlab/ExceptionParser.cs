﻿using Bot.Integration.Gitlab.Abstractions;
using Bot.Integration.Gitlab.Exceptions;
using System.Net;
using System.Text.Json;

namespace Bot.Integration.Gitlab;

public class ExceptionParser : IExceptionParser
{
    //TODO: add descriptors
    public async Task<Exception> ParseAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var statusCode = response.StatusCode;
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return statusCode switch
        {
            HttpStatusCode.BadRequest => ParseValidationException(json),
            HttpStatusCode.NotFound => ParseNotFoundException(json),
            HttpStatusCode.Unauthorized => ParseAuthentificationException(json),
            _ => new Exception("WTF?")
        };
        
    }

    private Exception ParseValidationException(string json)
    {
        var document = JsonDocument.Parse(json);
        var message = document.RootElement.GetProperty("message").GetString();
        return new ValidationException(message);
    }

    private Exception ParseNotFoundException(string json)
    {
        var document = JsonDocument.Parse(json);
        var message = document.RootElement.GetProperty("message").GetString();
        return new NotFoundException(message);
    }

    private Exception ParseAuthentificationException(string json)
    {
        var document = JsonDocument.Parse(json);
        var message = document.RootElement.GetProperty("error").GetString();
        var description = document.RootElement.GetProperty("error_description").GetString();
        return new AuthentificationException(message, description);
    }
}
