﻿using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using System.Reflection.Metadata.Ecma335;

namespace Bot.Integration.Git.GitCommands;

internal class PushCommand : IGitCommand
{
    private readonly GitOptionsSection _optionsSection;
    private readonly CredentialsHandler _credentials;

    public PushCommand(GitOptionsSection optionsSection, CredentialsHandler credentials)
    {
        _optionsSection = optionsSection;
        _credentials = credentials;
    }
    public bool Execute(IRepository repository)
    {
        var remote = repository.Network.Remotes["origin"];
        if (remote == null)
            return false;

        repository.Network.Push(remote, $@"refs/heads/{_optionsSection.Branch}", new PushOptions
        {
            CredentialsProvider = _credentials
        });
        return true;
    }
}